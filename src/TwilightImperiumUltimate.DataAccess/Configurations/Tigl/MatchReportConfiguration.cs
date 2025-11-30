using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class MatchReportConfiguration : IEntityTypeConfiguration<MatchReport>
{
    public void Configure(EntityTypeBuilder<MatchReport> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.MatchReports, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.HasIndex(e => new { e.GameId, e.Source })
            .IsUnique()
            .HasDatabaseName("IX_MatchReports_GameId_Source");

        builder.HasIndex(e => new { e.League, e.Season })
            .HasDatabaseName("IX_MatchReports_League_Season");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.GameId)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.GameId))
            .HasColumnType("varchar(100)")
            .HasColumnOrder(1);

        builder.Property(e => e.Source)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.Source))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(2);

        builder.Property(x => x.StartTimestamp)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.StartTimestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(3);

        builder.Property(x => x.EndTimestamp)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.EndTimestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(4);

        builder.Property(x => x.Score)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.Score))
            .HasColumnType("int")
            .HasColumnOrder(5);

        builder.Property(x => x.Round)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.Round))
            .HasColumnType("int")
            .HasColumnOrder(6);

        builder.Property(x => x.PlayerCount)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.PlayerCount))
            .HasColumnType("int")
            .HasColumnOrder(7);

        builder.Property(x => x.Season)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.Season))
            .HasColumnType("int")
            .HasColumnOrder(8);

        builder.Property(x => x.League)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.League))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(9);

        builder.Property(x => x.Events)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.Events))
            .HasColumnType("bigint")
            .HasColumnOrder(10);

        builder.Property(e => e.State)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.State))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(11);

        builder.HasMany(m => m.PlayerResults)
            .WithOne(x => x.MatchReport)
            .HasForeignKey(x => x.MatchReportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.PlayerMatchAsyncStats)
            .WithOne(x => x.Match)
            .HasForeignKey(x => x.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.PlayerMatchGlickoStats)
            .WithOne(x => x.Match)
            .HasForeignKey(x => x.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.PlayerMatchTrueSkillStats)
            .WithOne(x => x.Match)
            .HasForeignKey(x => x.MatchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
