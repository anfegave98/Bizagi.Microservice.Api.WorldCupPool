using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad <see cref="Team"/>.
/// </summary>
public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(t => t.FlagUrl)
            .HasMaxLength(300);

        builder.Property(t => t.IdGroup).IsRequired();

        builder.Property(t => t.State)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(t => t.DateCreated)
            .IsRequired();
    }
}
