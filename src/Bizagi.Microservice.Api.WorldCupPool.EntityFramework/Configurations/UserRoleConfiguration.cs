using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad <see cref="UserRole"/>.
/// </summary>
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(ur => ur.Id);
        builder.Property(ur => ur.Id).ValueGeneratedOnAdd();

        builder.Property(ur => ur.IdUser).IsRequired();
        builder.Property(ur => ur.IdRole).IsRequired();

        builder.Property(ur => ur.State)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(ur => ur.DateCreated)
            .IsRequired();
    }
}
