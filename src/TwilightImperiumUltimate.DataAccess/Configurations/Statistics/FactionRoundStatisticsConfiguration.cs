using TwilightImperiumUltimate.Core.Entities.Statistics;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Statistics;

internal class FactionRoundStatisticsConfiguration : IEntityTypeConfiguration<FactionRoundStatistics>
{
    public void Configure(EntityTypeBuilder<FactionRoundStatistics> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.FactionRoundStatistics, Schema.Statistics);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnType("integer")
            .HasColumnOrder(0);

        builder.Property(e => e.GameId)
            .IsRequired()
            .HasColumnName(nameof(FactionRoundStatistics.GameId))
            .HasColumnType("varchar(100)")
            .HasColumnOrder(1);

        builder.Property(e => e.Round)
            .IsRequired()
            .HasColumnName(nameof(FactionRoundStatistics.Round))
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(e => e.FactionName)
            .IsRequired()
            .HasColumnName(nameof(FactionRoundStatistics.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(3);

        builder.Property(e => e.Score)
            .IsRequired()
            .HasColumnName(nameof(FactionRoundStatistics.Score))
            .HasColumnType("integer")
            .HasColumnOrder(4);
    }
}
