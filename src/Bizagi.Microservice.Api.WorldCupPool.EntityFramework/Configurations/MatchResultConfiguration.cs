using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad <see cref="MatchResult"/>.
/// </summary>
public class MatchResultConfiguration : IEntityTypeConfiguration<MatchResult>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<MatchResult> builder)
    {
        builder.ToTable("MatchResults");

        builder.HasKey(mr => mr.Id);
        builder.Property(mr => mr.Id).ValueGeneratedOnAdd();

        builder.Property(mr => mr.IdMatch).IsRequired();
        builder.Property(mr => mr.HomeGoals).IsRequired();
        builder.Property(mr => mr.AwayGoals).IsRequired();
        builder.Property(mr => mr.RegisteredByUserId).IsRequired();
        builder.Property(mr => mr.RegisteredDate).IsRequired();

        builder.Property(mr => mr.State)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(mr => mr.DateCreated).IsRequired();
        builder.Property(mr => mr.DateModified);

        // Restricción única: solo puede existir un resultado activo por partido.
        builder.HasIndex(mr => mr.IdMatch).IsUnique();

        builder.HasMany(mr => mr.ScoreLogs)
            .WithOne(sl => sl.MatchResult)
            .HasForeignKey(sl => sl.IdMatchResult)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
