using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad <see cref="User"/>.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(u => u.UserName).IsUnique();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.PasswordSalt)
            .HasMaxLength(500);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.LastLoginDate);

        builder.Property(u => u.State)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.IdUserCreator)
            .IsRequired();

        builder.Property(u => u.DateCreated)
            .IsRequired();

        builder.Property(u => u.DateModified);

        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Predictions)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.IdUser)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
