using TwilightImperiumUltimate.Core.Entities.Statistics;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Statistics;

internal class GameStatisticsConfiguration : IEntityTypeConfiguration<GameStatistics>
{
    public void Configure(EntityTypeBuilder<GameStatistics> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.GameStatistics, Schema.Statistics);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnType("integer")
            .HasColumnOrder(0);

        builder.Property(e => e.GameId)
            .IsRequired()
            .HasColumnName(nameof(GameStatistics.GameId))
            .HasColumnType("varchar(100)")
            .HasColumnOrder(1);

        builder.Property(e => e.MaxPoints)
            .IsRequired()
            .HasColumnName(nameof(GameStatistics.MaxPoints))
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(e => e.NumberOfPlayers)
            .IsRequired()
            .HasColumnName(nameof(GameStatistics.NumberOfPlayers))
            .HasColumnType("integer")
            .HasColumnOrder(3);

        builder.Property(e => e.Round)
            .IsRequired()
            .HasColumnName(nameof(GameStatistics.Round))
            .HasColumnType("integer")
            .HasColumnOrder(4);

        builder.Property(x => x.Time)
            .IsRequired()
            .HasColumnName(nameof(GameStatistics.Time))
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(5);

        builder.Property(x => x.Winner)
            .IsRequired()
            .HasColumnName(nameof(GameStatistics.Winner))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(6);
    }
}
