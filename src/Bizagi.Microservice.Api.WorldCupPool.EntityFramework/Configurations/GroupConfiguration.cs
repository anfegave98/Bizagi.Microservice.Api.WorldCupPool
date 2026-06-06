using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad <see cref="Group"/>.
/// </summary>
public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups");

        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).ValueGeneratedOnAdd();

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(g => g.Description)
            .HasMaxLength(150);

        builder.Property(g => g.State)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(g => g.DateCreated)
            .IsRequired();

        builder.HasMany(g => g.Teams)
            .WithOne(t => t.Group)
            .HasForeignKey(t => t.IdGroup)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(g => g.Matches)
            .WithOne(m => m.Group)
            .HasForeignKey(m => m.IdGroup)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
