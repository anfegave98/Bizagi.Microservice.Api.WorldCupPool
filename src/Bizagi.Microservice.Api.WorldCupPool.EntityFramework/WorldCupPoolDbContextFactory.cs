using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework;

/// <summary>
/// Fábrica de contexto para herramientas de diseño de Entity Framework Core.
/// Permite ejecutar comandos de migración (dotnet ef migrations add / database update)
/// desde la CLI apuntando al proyecto EntityFramework sin necesidad de levantar la API.
/// </summary>
public class WorldCupPoolDbContextFactory : IDesignTimeDbContextFactory<WorldCupPoolDbContext>
{
    /// <inheritdoc />
    public WorldCupPoolDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),
                "../Bizagi.Microservice.Api.WorldCupPool"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<WorldCupPoolDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new WorldCupPoolDbContext(optionsBuilder.Options);
    }
}
