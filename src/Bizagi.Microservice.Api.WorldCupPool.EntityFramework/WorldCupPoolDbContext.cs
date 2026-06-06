using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework;

/// <summary>
/// Contexto principal de Entity Framework para el sistema Polla Mundialista.
/// </summary>
public class WorldCupPoolDbContext : DbContext
{
    /// <summary>
    /// Inicializa una nueva instancia del contexto con las opciones especificadas.
    /// </summary>
    /// <param name="options">Opciones de configuración del contexto.</param>
    public WorldCupPoolDbContext(DbContextOptions<WorldCupPoolDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Conjunto de usuarios del sistema.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Conjunto de roles del sistema.
    /// </summary>
    public DbSet<Role> Roles { get; set; } = null!;

    /// <summary>
    /// Relación entre usuarios y roles.
    /// </summary>
    public DbSet<UserRole> UserRoles { get; set; } = null!;

    /// <summary>
    /// Grupos mundialistas.
    /// </summary>
    public DbSet<Group> Groups { get; set; } = null!;

    /// <summary>
    /// Equipos participantes.
    /// </summary>
    public DbSet<Team> Teams { get; set; } = null!;

    /// <summary>
    /// Partidos precargados.
    /// </summary>
    public DbSet<Match> Matches { get; set; } = null!;

    /// <summary>
    /// Predicciones de los usuarios.
    /// </summary>
    public DbSet<Prediction> Predictions { get; set; } = null!;

    /// <summary>
    /// Resultados reales registrados por el administrador.
    /// </summary>
    public DbSet<MatchResult> MatchResults { get; set; } = null!;

    /// <summary>
    /// Registros de trazabilidad del cálculo de puntajes.
    /// </summary>
    public DbSet<ScoreLog> ScoreLogs { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorldCupPoolDbContext).Assembly);
    }
}
