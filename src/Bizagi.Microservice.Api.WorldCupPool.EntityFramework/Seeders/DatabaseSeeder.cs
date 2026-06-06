using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Seeders;

/// <summary>
/// Seeder principal de la base de datos.
/// Carga roles, usuario administrador inicial, 2 grupos, 8 equipos y 12 partidos precargados.
/// </summary>
public class DatabaseSeeder
{
    private readonly WorldCupPoolDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<DatabaseSeeder> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del seeder.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    /// <param name="passwordService">Servicio de hash de contraseñas.</param>
    /// <param name="logger">Logger del seeder.</param>
    public DatabaseSeeder(
        WorldCupPoolDbContext context,
        IPasswordService passwordService,
        ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _passwordService = passwordService;
        _logger = logger;
    }

    /// <summary>
    /// Ejecuta la migración de la base de datos y carga los datos iniciales si no existen.
    /// </summary>
    public async Task SeedAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
            await SeedRolesAsync();
            await SeedAdminUserAsync();
            await SeedGroupsAndTeamsAsync();
            await SeedMatchesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar el seeder de la base de datos.");
            throw;
        }
    }

    private async Task SeedRolesAsync()
    {
        if (await _context.Roles.AnyAsync()) return;

        _logger.LogInformation("Cargando roles iniciales...");

        _context.Roles.AddRange(
            new Role
            {
                Name = "Admin",
                Description = "Administrador del sistema con acceso total.",
                State = true,
                DateCreated = DateTime.UtcNow
            },
            new Role
            {
                Name = "User",
                Description = "Usuario participante de la Polla Mundialista.",
                State = true,
                DateCreated = DateTime.UtcNow
            }
        );

        await _context.SaveChangesAsync();
        _logger.LogInformation("Roles cargados correctamente.");
    }

    private async Task SeedAdminUserAsync()
    {
        if (await _context.Users.AnyAsync(u => u.UserName == "admin")) return;

        _logger.LogInformation("Creando usuario administrador inicial...");

        var adminRole = await _context.Roles.FirstAsync(r => r.Name == "Admin");

        _passwordService.CreateHash("Admin@1234!", out var hash, out var salt);

        var adminUser = new User
        {
            UserName = "admin",
            FullName = "Administrador Sistema",
            Email = "admin@worldcuppool.com",
            PasswordHash = hash,
            PasswordSalt = salt,
            IsActive = true,
            State = true,
            IdUserCreator = 0,
            DateCreated = DateTime.UtcNow
        };

        _context.Users.Add(adminUser);
        await _context.SaveChangesAsync();

        _context.UserRoles.Add(new UserRole
        {
            IdUser = adminUser.Id,
            IdRole = adminRole.Id,
            State = true,
            DateCreated = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        _logger.LogInformation("Usuario administrador creado: admin / Admin@1234!");
    }

    private async Task SeedGroupsAndTeamsAsync()
    {
        if (await _context.Groups.AnyAsync()) return;

        _logger.LogInformation("Cargando grupos y equipos...");

        var groupA = new Group
        {
            Name = "Grupo A",
            Description = "Fase de grupos — Grupo A",
            State = true,
            DateCreated = DateTime.UtcNow
        };

        var groupB = new Group
        {
            Name = "Grupo B",
            Description = "Fase de grupos — Grupo B",
            State = true,
            DateCreated = DateTime.UtcNow
        };

        _context.Groups.AddRange(groupA, groupB);
        await _context.SaveChangesAsync();

        _context.Teams.AddRange(
            // Grupo A
            new Team { Name = "Colombia",  Code = "COL", IdGroup = groupA.Id, State = true, DateCreated = DateTime.UtcNow },
            new Team { Name = "Brasil",    Code = "BRA", IdGroup = groupA.Id, State = true, DateCreated = DateTime.UtcNow },
            new Team { Name = "Argentina", Code = "ARG", IdGroup = groupA.Id, State = true, DateCreated = DateTime.UtcNow },
            new Team { Name = "Uruguay",   Code = "URU", IdGroup = groupA.Id, State = true, DateCreated = DateTime.UtcNow },
            // Grupo B
            new Team { Name = "Francia",   Code = "FRA", IdGroup = groupB.Id, State = true, DateCreated = DateTime.UtcNow },
            new Team { Name = "Alemania",  Code = "GER", IdGroup = groupB.Id, State = true, DateCreated = DateTime.UtcNow },
            new Team { Name = "España",    Code = "ESP", IdGroup = groupB.Id, State = true, DateCreated = DateTime.UtcNow },
            new Team { Name = "Portugal",  Code = "POR", IdGroup = groupB.Id, State = true, DateCreated = DateTime.UtcNow }
        );

        await _context.SaveChangesAsync();
        _logger.LogInformation("Grupos y equipos cargados correctamente.");
    }

    private async Task SeedMatchesAsync()
    {
        if (await _context.Matches.AnyAsync()) return;

        _logger.LogInformation("Cargando 12 partidos precargados...");

        var groupA = await _context.Groups.FirstAsync(g => g.Name == "Grupo A");
        var groupB = await _context.Groups.FirstAsync(g => g.Name == "Grupo B");

        var col = await _context.Teams.FirstAsync(t => t.Code == "COL");
        var bra = await _context.Teams.FirstAsync(t => t.Code == "BRA");
        var arg = await _context.Teams.FirstAsync(t => t.Code == "ARG");
        var uru = await _context.Teams.FirstAsync(t => t.Code == "URU");
        var fra = await _context.Teams.FirstAsync(t => t.Code == "FRA");
        var ger = await _context.Teams.FirstAsync(t => t.Code == "GER");
        var esp = await _context.Teams.FirstAsync(t => t.Code == "ESP");
        var por = await _context.Teams.FirstAsync(t => t.Code == "POR");

        var baseDate = new DateTime(2026, 6, 10, 15, 0, 0, DateTimeKind.Utc);
        const string round = "Fase de grupos";

        _context.Matches.AddRange(
            // Grupo A — Jornada 1
            new Match { IdGroup = groupA.Id, IdHomeTeam = col.Id, IdAwayTeam = bra.Id, HomeTeamName = col.Name, AwayTeamName = bra.Name, MatchDate = baseDate,                    RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            new Match { IdGroup = groupA.Id, IdHomeTeam = arg.Id, IdAwayTeam = uru.Id, HomeTeamName = arg.Name, AwayTeamName = uru.Name, MatchDate = baseDate.AddHours(3),         RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            // Grupo A — Jornada 2
            new Match { IdGroup = groupA.Id, IdHomeTeam = col.Id, IdAwayTeam = arg.Id, HomeTeamName = col.Name, AwayTeamName = arg.Name, MatchDate = baseDate.AddDays(3),         RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            new Match { IdGroup = groupA.Id, IdHomeTeam = bra.Id, IdAwayTeam = uru.Id, HomeTeamName = bra.Name, AwayTeamName = uru.Name, MatchDate = baseDate.AddDays(3).AddHours(3), RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            // Grupo A — Jornada 3
            new Match { IdGroup = groupA.Id, IdHomeTeam = col.Id, IdAwayTeam = uru.Id, HomeTeamName = col.Name, AwayTeamName = uru.Name, MatchDate = baseDate.AddDays(6),         RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            new Match { IdGroup = groupA.Id, IdHomeTeam = bra.Id, IdAwayTeam = arg.Id, HomeTeamName = bra.Name, AwayTeamName = arg.Name, MatchDate = baseDate.AddDays(6).AddHours(3), RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            // Grupo B — Jornada 1
            new Match { IdGroup = groupB.Id, IdHomeTeam = fra.Id, IdAwayTeam = ger.Id, HomeTeamName = fra.Name, AwayTeamName = ger.Name, MatchDate = baseDate.AddDays(1),         RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            new Match { IdGroup = groupB.Id, IdHomeTeam = esp.Id, IdAwayTeam = por.Id, HomeTeamName = esp.Name, AwayTeamName = por.Name, MatchDate = baseDate.AddDays(1).AddHours(3), RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            // Grupo B — Jornada 2
            new Match { IdGroup = groupB.Id, IdHomeTeam = fra.Id, IdAwayTeam = esp.Id, HomeTeamName = fra.Name, AwayTeamName = esp.Name, MatchDate = baseDate.AddDays(4),         RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            new Match { IdGroup = groupB.Id, IdHomeTeam = ger.Id, IdAwayTeam = por.Id, HomeTeamName = ger.Name, AwayTeamName = por.Name, MatchDate = baseDate.AddDays(4).AddHours(3), RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            // Grupo B — Jornada 3
            new Match { IdGroup = groupB.Id, IdHomeTeam = fra.Id, IdAwayTeam = por.Id, HomeTeamName = fra.Name, AwayTeamName = por.Name, MatchDate = baseDate.AddDays(7),         RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow },
            new Match { IdGroup = groupB.Id, IdHomeTeam = ger.Id, IdAwayTeam = esp.Id, HomeTeamName = ger.Name, AwayTeamName = esp.Name, MatchDate = baseDate.AddDays(7).AddHours(3), RoundName = round, Status = MatchStatus.Scheduled, State = true, DateCreated = DateTime.UtcNow }
        );

        await _context.SaveChangesAsync();
        _logger.LogInformation("12 partidos precargados correctamente.");
    }
}
