# Arquitectura del Sistema — Polla Mundialista

## Diagrama de contexto (C4 Nivel 1)

```
┌─────────────────────────────────────────────────────────────────────┐
│                        SISTEMA POLLA MUNDIALISTA                    │
│                                                                     │
│   ┌──────────────┐      HTTP/HTTPS       ┌───────────────────────┐  │
│   │   Navegador  │ ◄──────────────────►  │  bizagi-worldcup-     │  │
│   │   (Usuario)  │                       │  pool-site            │  │
│   └──────────────┘                       │  [Angular 18 SPA]     │  │
│                                          └──────────┬────────────┘  │
│   ┌──────────────┐                                  │ REST + JWT    │
│   │   Navegador  │                                  ▼               │
│   │ (Admin)      │ ◄─────────────────►  ┌───────────────────────┐   │
│   └──────────────┘      HTTP/HTTPS      │  Bizagi.Microservice. │   │
│                                         │  Api.WorldCupPool     │   │
│                                         │  [.NET 8 Web API]     │   │
│                                         └──────────┬────────────┘   │
│                                                    │ EF Core        │
│                                                    ▼                │
│                                          ┌──────────────────────┐   │
│                                          │     PostgreSQL       │   │
│                                          │  [Base de datos]     │   │
│                                          └──────────────────────┘   │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Diagrama de contenedores backend (C4 Nivel 2)

```
Bizagi.Microservice.Api.WorldCupPool  (.NET 8 Solution)
╔═══════════════════════════════════════════════════════════════════╗
║                                                                   ║
║  ┌────────────────────────────────────────────────────────────┐   ║
║  │           Bizagi.Microservice.Api.WorldCupPool             │   ║
║  │                      [API Host]                            │   ║
║  │                                                            │   ║
║  │  Program.cs          ← DI, JWT, CORS, Swagger, Seeder      │   ║
║  │                                                            │   ║
║  │  Middlewares/                                              │   ║
║  │  ├── GlobalExceptionMiddleware   ← Errores controlados     │   ║
║  │  └── RateLimitingMiddleware      ← 5 políticas / JWT / IP  │   ║
║  │                                                            │   ║
║  │  Controllers/                                              │   ║
║  │  ├── AuthController              ← /api/v1/auth            │   ║
║  │  ├── MatchesController           ← /api/v1/matches         │   ║
║  │  ├── PredictionsController       ← /api/v1/predictions     │   ║
║  │  ├── AdminController             ← /api/v1/admin/matches   │   ║
║  │  ├── AdminDashboardController    ← /api/v1/admin/dashboard │   ║
║  │  ├── LeaderboardController       ← /api/v1/leaderboard     │   ║
║  │  └── HealthController            ← /api/v1/health          │   ║
║  └────────────────────────────────────────────────────────────┘   ║
║                            │ depende de                           ║
║  ┌─────────────────────────▼──────────────────────────────────┐   ║
║  │         Bizagi.Microservice.Api.WorldCupPool.Logic         │   ║
║  │                  [Lógica de negocio]                       │   ║
║  │                                                            │   ║
║  │  Services/                                                 │   ║
║  │  ├── AuthService          ← Registro y login               │   ║
║  │  ├── TokenService         ← Generación JWT                 │   ║
║  │  ├── PasswordService      ← HMACSHA512                     │   ║
║  │  ├── EncryptionService    ← AES-256-CBC                    │   ║
║  │  ├── MatchService         ← Consulta partidos              │   ║
║  │  ├── PredictionService    ← Crear/actualizar predicción    │   ║
║  │  ├── AdminMatchService    ← Registrar resultado real       │   ║
║  │  ├── ScoreCalculationService ← Cálculo de puntos           │   ║
║  │  ├── LeaderboardService   ← Ranking e historial            │   ║
║  │  └── AdminDashboardService ← Indicadores administrativos   │   ║
║  └────────────────────────────────────────────────────────────┘   ║
║                            │ depende de                           ║
║  ┌─────────────────────────▼──────────────────────────────────┐   ║
║  │      Bizagi.Microservice.Api.WorldCupPool.Abstractions     │   ║
║  │                    [Interfaces]                            │   ║
║  │                                                            │   ║
║  │  Repositories/   IUserRepository, IRoleRepository,         │   ║
║  │                  IMatchRepository, IPredictionRepository,  │   ║
║  │                  IMatchResultRepository, IScoreLogRepo,    │   ║
║  │                  ILeaderboardRepository,                   │   ║
║  │                  IAdminDashboardRepository                 │   ║
║  │                                                            │   ║
║  │  Services/       IAuthService, ITokenService,              │   ║
║  │                  IPasswordService, IEncryptionService,     │   ║
║  │                  IMatchService, IPredictionService,        │   ║
║  │                  IAdminMatchService,                       │   ║
║  │                  IScoreCalculationService,                 │   ║
║  │                  ILeaderboardService,                      │   ║
║  │                  IAdminDashboardService                    │   ║
║  └────────────────────────────────────────────────────────────┘   ║
║                            │ implementado por                     ║
║  ┌─────────────────────────▼──────────────────────────────────┐   ║
║  │    Bizagi.Microservice.Api.WorldCupPool.EntityFramework    │   ║
║  │                   [Persistencia EF Core]                   │   ║
║  │                                                            │   ║
║  │  WorldCupPoolDbContext                                     │   ║
║  │  WorldCupPoolDbContextFactory  ← CLI migrations            │   ║
║  │                                                            │   ║
║  │  Configurations/  (Fluent API por entidad)                 │   ║
║  │  Repositories/    (Implementaciones EF)                    │   ║
║  │  Seeders/         DatabaseSeeder                           │   ║
║  └────────────────────────────────────────────────────────────┘   ║
║                            │ usa                                  ║
║  ┌─────────────────────────▼──────────────────────────────────┐   ║
║  │         Bizagi.Microservice.Api.WorldCupPool.Entities      │   ║
║  │                  [Entidades de dominio]                    │   ║
║  │                                                            │   ║
║  │  BaseEntity, User, Role, UserRole, Group, Team,            │   ║
║  │  Match, MatchStatus, Prediction, MatchResult,              │   ║
║  │  ScoreLog, ScoreRule                                       │   ║
║  └────────────────────────────────────────────────────────────┘   ║
║                                                                   ║
║  ┌────────────────────────────────────────────────────────────┐   ║
║  │   Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object│   ║
║  │                     [DTOs y Options]                       │   ║
║  │                                                            │   ║
║  │  Auth/       RegisterUserDto, LoginRequestDto,             │   ║
║  │              LoginResponseDto, AuthUserDto                 │   ║
║  │  Match/      MatchDto                                      │   ║
║  │  Prediction/ PredictionCreateDto, PredictionDto            │   ║
║  │  Admin/      MatchResultCreateDto, MatchResultDto,         │   ║
║  │              AdminDashboardDto                             │   ║
║  │  Leaderboard/ LeaderboardItemDto,                          │   ║
║  │               UserPredictionHistoryDto                     │   ║
║  │  Options/    JwtOptions, EncryptionOptions,                │   ║
║  │              RateLimitingOptions, SwaggerOptions,          │   ║
║  │              CorsOptions                                   │   ║
║  └────────────────────────────────────────────────────────────┘   ║
╚═══════════════════════════════════════════════════════════════════╝
```

---

## Dependencias entre proyectos

```
API ──────────────────► Logic
API ──────────────────► EntityFramework
API ──────────────────► Abstractions
API ──────────────────► DTO

Logic ────────────────► Abstractions
Logic ────────────────► DTO
Logic ────────────────► Entities

EntityFramework ──────► Abstractions
EntityFramework ──────► Entities

Abstractions ─────────► DTO
Abstractions ─────────► Entities
```

---

## Flujo de datos — Registro de predicción (caso de uso principal)

```
[Angular 18]                [.NET 8 API]                [PostgreSQL]
     │                           │                           │
     │  POST /api/v1/predictions │                           │
     │  { idMatch, homeGoals,    │                           │
     │    awayGoals }            │                           │
     │  Authorization: Bearer <JWT>                          │
     │──────────────────────────►│                           │
     │                           │                           │
     │               ┌───────────┴──────────────┐            │
     │               │ GlobalExceptionMiddleware │           │
     │               │ RateLimitingMiddleware    │           │
     │               │  → verifica política      │           │
     │               │    PredictionEndpoints    │           │
     │               │  → partición: user:{sub}  │           │
     │               └───────────┬──────────────┘            │
     │                           │                           │
     │               ┌───────────┴──────────────┐            │
     │               │  JwtBearer Middleware     │           │
     │               │  → valida JWT             │           │
     │               │  → extrae claims          │           │
     │               └───────────┬──────────────┘            │
     │                           │                           │
     │               ┌───────────┴──────────────┐            │
     │               │  PredictionsController    │           │
     │               │  → GetCurrentUserId()     │           │
     │               │    lee claim "sub"        │           │
     │               └───────────┬──────────────┘            │
     │                           │                           │
     │               ┌───────────┴──────────────┐            │
     │               │  PredictionService        │           │
     │               │  → valida partido existe  │           │
     │               │  → valida status ≠        │           │
     │               │    Finished               │           │
     │               │  → busca predicción       │           │
     │               │    existente              │           │
     │               └───────────┬──────────────┘            │
     │                           │  SELECT / INSERT / UPDATE │
     │                           │──────────────────────────►│
     │                           │◄──────────────────────────│
     │                           │                           │
     │               ┌───────────┴──────────────┐            │
     │               │  Mapeo a PredictionDto    │           │
     │               └───────────┬──────────────┘            │
     │                           │                           │
     │  HTTP 200 OK              │                           │
     │  { PredictionDto }        │                           │
     │◄──────────────────────────│                           │
```

---

## Flujo de datos — Registro de resultado y cálculo de puntos

```
[Admin Browser]          [.NET 8 API]                     [PostgreSQL]
     │                        │                                │
     │  POST /api/v1/admin/   │                                │
     │  matches/{id}/result   │                                │
     │  { homeGoals,          │                                │
     │    awayGoals }         │                                │
     │  Authorization: Bearer <JWT Admin>                      │
     │───────────────────────►│                                │
     │                        │                                │
     │            ┌───────────┴────────────┐                   │
     │            │ Middlewares            │                   │
     │            │ JWT + rol Admin válido │                   │
     │            └───────────┬────────────┘                   │
     │                        │                                │
     │            ┌───────────┴────────────┐                   │
     │            │  AdminController       │                   │
     │            │  → AdminMatchService   │                   │
     │            └───────────┬────────────┘                   │
     │                        │                                │
     │            ┌───────────┴────────────────────────────┐   │
     │            │  AdminMatchService.RegisterResultAsync │   │
     │            │  1. Valida que partido existe          │   │
     │            │  2. Valida que status ≠ Finished       │   │
     │            │  3. Crea MatchResult en BD             │──►│
     │            │  4. Actualiza Match.Status = Finished  │──►│
     │            │  5. Llama ScoreCalculationService      │   │
     │            └───────────┬────────────────────────────┘   │
     │                        │                                │
     │            ┌───────────┴────────────────────────────┐   │
     │            │  ScoreCalculationService.CalculateAsync│   │
     │            │  Para cada predicción del partido:     │   │
     │            │  → predictedHome == realHome &&        │   │
     │            │    predictedAway == realAway           │   │
     │            │    ? 3 pts (ExactScore)                │   │
     │            │  → sign(pred) == sign(real)            │   │
     │            │    ? 1 pt (WinnerOrDraw)               │   │
     │            │  → else 0 pts (Failed)                 │   │
     │            │  → UPDATE Prediction.Points            │──►│
     │            │  → INSERT ScoreLog (trazabilidad)      │──►│
     │            └───────────┬────────────────────────────┘   │
     │                        │                                │
     │  HTTP 200 OK           │                                │
     │  { MatchResultDto }    │                                │
     │◄───────────────────────│                                │
```

---

## Pipeline de middlewares (orden de ejecución)

```
Solicitud entrante
       │
       ▼
┌──────────────────────────┐
│ GlobalExceptionMiddleware│  ← Captura cualquier excepción, retorna JSON controlado
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│  RateLimitingMiddleware  │  ← Verifica límite por política/usuario/IP
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│  Swagger (dev only)      │  ← Solo si Swagger:Enabled = true
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│  UseHttpsRedirection     │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│  UseCors                 │  ← Orígenes desde appsettings
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│  UseAuthentication       │  ← Valida JWT, carga claims
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│  UseAuthorization        │  ← Valida rol (User / Admin)
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│  MapControllers          │  ← Rutea al Controller correspondiente
└──────────────────────────┘
```

---

## Seguridad por capas

| Capa | Mecanismo |
|---|---|
| Red | HTTPS, CORS restringido por origen |
| Transporte | JWT Bearer (HS256), expiración configurable |
| Autorización | Roles `User` / `Admin` por atributo `[Authorize(Roles = "...")]` |
| Rate limiting | Ventana fija por política de endpoint, user o IP |
| Contraseñas | HMACSHA512 con salt individual, nunca en texto plano |
| Datos sensibles | AES-256-CBC configurable desde `appsettings.json` |
| Errores | Middleware global, sin stack trace en respuesta |
| Logs | Sin registro de tokens, contraseñas ni payloads cifrados |
| Swagger | Deshabilitado en producción |

---

**Desarrollado por:** Andres Felipe Galeano Velasco  
**Sprint:** 04 – 08 de junio de 2026
