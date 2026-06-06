using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad <see cref="Role"/>.
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(r => r.Name).IsUnique();

        builder.Property(r => r.Description)
            .HasMaxLength(200);

        builder.Property(r => r.State)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(r => r.DateCreated)
            .IsRequired();

        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.IdRole)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
