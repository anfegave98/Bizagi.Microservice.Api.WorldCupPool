using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad <see cref="ScoreLog"/>.
/// </summary>
public class ScoreLogConfiguration : IEntityTypeConfiguration<ScoreLog>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ScoreLog> builder)
    {
        builder.ToTable("ScoreLogs");

        builder.HasKey(sl => sl.Id);
        builder.Property(sl => sl.Id).ValueGeneratedOnAdd();

        builder.Property(sl => sl.IdPrediction).IsRequired();
        builder.Property(sl => sl.IdMatchResult).IsRequired();
        builder.Property(sl => sl.PredictedHomeGoals).IsRequired();
        builder.Property(sl => sl.PredictedAwayGoals).IsRequired();
        builder.Property(sl => sl.RealHomeGoals).IsRequired();
        builder.Property(sl => sl.RealAwayGoals).IsRequired();
        builder.Property(sl => sl.PointsAssigned).IsRequired();

        builder.Property(sl => sl.RuleApplied)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(sl => sl.CalculationDate).IsRequired();

        builder.Property(sl => sl.State)
            .IsRequired()
            .HasDefaultValue(true);
    }
}
