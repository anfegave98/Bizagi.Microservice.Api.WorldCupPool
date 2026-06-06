using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad <see cref="Match"/>.
/// </summary>
public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.Property(m => m.IdGroup).IsRequired();
        builder.Property(m => m.IdHomeTeam).IsRequired();
        builder.Property(m => m.IdAwayTeam).IsRequired();
        builder.Property(m => m.MatchDate).IsRequired();

        builder.Property(m => m.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(MatchStatus.Scheduled);

        builder.Property(m => m.RoundName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.HomeTeamName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.AwayTeamName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.State)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(m => m.DateCreated).IsRequired();
        builder.Property(m => m.DateModified);

        builder.HasOne(m => m.HomeTeam)
            .WithMany()
            .HasForeignKey(m => m.IdHomeTeam)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.AwayTeam)
            .WithMany()
            .HasForeignKey(m => m.IdAwayTeam)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Predictions)
            .WithOne(p => p.Match)
            .HasForeignKey(p => p.IdMatch)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.MatchResult)
            .WithOne(mr => mr.Match)
            .HasForeignKey<MatchResult>(mr => mr.IdMatch)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
