using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad <see cref="Prediction"/>.
/// </summary>
public class PredictionConfiguration : IEntityTypeConfiguration<Prediction>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Prediction> builder)
    {
        builder.ToTable("Predictions");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.IdUser).IsRequired();
        builder.Property(p => p.IdMatch).IsRequired();

        builder.Property(p => p.HomeGoals).IsRequired();
        builder.Property(p => p.AwayGoals).IsRequired();

        builder.Property(p => p.Points)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.IsCalculated)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CalculatedDate);

        builder.Property(p => p.State)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.IdUserCreator).IsRequired();
        builder.Property(p => p.DateCreated).IsRequired();
        builder.Property(p => p.DateModified);

        // Restricción única: un usuario solo puede tener una predicción activa por partido.
        builder.HasIndex(p => new { p.IdUser, p.IdMatch }).IsUnique();

        builder.HasMany(p => p.ScoreLogs)
            .WithOne(sl => sl.Prediction)
            .HasForeignKey(sl => sl.IdPrediction)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
