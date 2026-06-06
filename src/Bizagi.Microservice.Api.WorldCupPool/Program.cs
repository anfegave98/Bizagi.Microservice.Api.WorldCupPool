using System.Text;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;
using Bizagi.Microservice.Api.WorldCupPool.EntityFramework;
using Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Seeders;
using Bizagi.Microservice.Api.WorldCupPool.Logic.Services;
using Bizagi.Microservice.Api.WorldCupPool.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ─── Opciones tipadas ─────────────────────────────────────────────────────
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<SwaggerOptions>(
    builder.Configuration.GetSection(SwaggerOptions.SectionName));
builder.Services.Configure<CorsOptions>(
    builder.Configuration.GetSection(CorsOptions.SectionName));
builder.Services.Configure<EncryptionOptions>(
    builder.Configuration.GetSection(EncryptionOptions.SectionName));
builder.Services.Configure<RateLimitingOptions>(
    builder.Configuration.GetSection(RateLimitingOptions.SectionName));

// ─── Base de datos (PostgreSQL) ────────────────────────────────────────────
builder.Services.AddDbContext<WorldCupPoolDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ─── CORS ──────────────────────────────────────────────────────────────────
var corsConfig = builder.Configuration
    .GetSection(CorsOptions.SectionName)
    .Get<CorsOptions>() ?? new CorsOptions();

builder.Services.AddCors(options =>
{
    options.AddPolicy("WorldCupPoolCors", policy =>
    {
        if (corsConfig.AllowedOrigins.Length > 0)
            policy.WithOrigins(corsConfig.AllowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        else
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
    });
});

// ─── Autenticación JWT ─────────────────────────────────────────────────────
var jwtConfig = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>() ?? new JwtOptions();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ─── Repositorios ──────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IPredictionRepository, PredictionRepository>();
builder.Services.AddScoped<IMatchResultRepository, MatchResultRepository>();
builder.Services.AddScoped<IScoreLogRepository, ScoreLogRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
builder.Services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();

// ─── Servicios de lógica ───────────────────────────────────────────────────
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IPredictionService, PredictionService>();
builder.Services.AddScoped<IAdminMatchService, AdminMatchService>();
builder.Services.AddScoped<IScoreCalculationService, ScoreCalculationService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();

// ─── Seeder ────────────────────────────────────────────────────────────────
builder.Services.AddScoped<DatabaseSeeder>();

// ─── Controllers + Swagger ────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var swaggerEnabled = builder.Configuration
    .GetSection(SwaggerOptions.SectionName)
    .GetValue<bool>("Enabled");

if (swaggerEnabled)
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Polla Mundialista API",
            Version = "v1",
            Description = "API para la gestión de predicciones del Mundial de Fútbol. " +
                          "⚠️ En desarrollo (Encryption:Enabled=false) los endpoints críticos " +
                          "aceptan y devuelven JSON en texto plano — Swagger funciona con normalidad."
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Ingrese: Bearer {token}"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id   = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
        foreach (var xmlFile in xmlFiles)
            c.IncludeXmlComments(xmlFile);
    });
}

// ═══════════════════════════════════════════════════════════════════════════
var app = builder.Build();
// ═══════════════════════════════════════════════════════════════════════════

// ─── Pipeline de middlewares ───────────────────────────────────────────────
//
//  Orden crítico:
//
//  1. GlobalException          → captura cualquier error de todo el pipeline
//  2. EncryptionResponse       → envuelve el stream de respuesta ANTES de ejecutar
//                                el resto, para poder capturar y cifrar lo que
//                                escriba el controller
//  3. Decryption               → descifra el body del request entrante
//  4. RateLimiting             → evalúa límites
//  5. Swagger                  → solo en desarrollo
//  6. HttpsRedirection
//  7. CORS
//  8. Authentication           → valida JWT
//  9. Authorization            → valida roles
// 10. Controllers              → procesa la solicitud y escribe la respuesta
//                                (que será capturada y cifrada por el paso 2)

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<EncryptionResponseMiddleware>(); // ← cifra la respuesta
app.UseMiddleware<DecryptionMiddleware>();          // ← descifra el request
app.UseMiddleware<RateLimitingMiddleware>();

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Polla Mundialista API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("WorldCupPoolCors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ─── Seeder automático ────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

app.Run();
