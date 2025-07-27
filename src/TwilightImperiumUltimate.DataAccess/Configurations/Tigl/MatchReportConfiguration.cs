using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class MatchReportConfiguration : IEntityTypeConfiguration<MatchReport>
{
    public void Configure(EntityTypeBuilder<MatchReport> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.MatchReports, Schema.Tigl);

        builder.HasKey(p => p.Id);

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

        builder.Property(x => x.Timestamp)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.Timestamp))
            .HasColumnType("bigint")
            .HasColumnOrder(3);

        builder.Property(e => e.State)
            .IsRequired()
            .HasColumnName(nameof(MatchReport.State))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(4);

        builder.HasMany(m => m.PlayerResults)
          .WithOne(x => x.MatchReport)
          .HasForeignKey(x => x.MatchReportId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.PlayerMatchStats)
            .WithOne(x => x.Match)
            .HasForeignKey(x => x.MatchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
