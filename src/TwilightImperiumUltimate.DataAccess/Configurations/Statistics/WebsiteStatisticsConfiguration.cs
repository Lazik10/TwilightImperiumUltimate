using TwilightImperiumUltimate.Core.Entities.Statistics;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Statistics;

internal class WebsiteStatisticsConfiguration : IEntityTypeConfiguration<WebsiteStatistics>
{
    public void Configure(EntityTypeBuilder<WebsiteStatistics> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.WebsiteStatistics, Schema.Statistics);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnType("integer")
            .HasColumnOrder(0);

        builder.Property(e => e.Visitors)
            .IsRequired()
            .HasColumnName(nameof(WebsiteStatistics.Visitors))
            .HasColumnType("integer")
            .HasColumnOrder(1);

        builder.Property(e => e.MapsGenerated)
            .IsRequired()
            .HasColumnName(nameof(WebsiteStatistics.MapsGenerated))
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(e => e.SlicesGenerated)
            .IsRequired()
            .HasColumnName(nameof(WebsiteStatistics.SlicesGenerated))
            .HasColumnType("integer")
            .HasColumnOrder(3);

        builder.Property(e => e.MapsArchived)
            .IsRequired()
            .HasColumnName(nameof(WebsiteStatistics.MapsArchived))
            .HasColumnType("integer")
            .HasColumnOrder(4);

        builder.Property(e => e.SlicesArchived)
            .IsRequired()
            .HasColumnName(nameof(WebsiteStatistics.SlicesArchived))
            .HasColumnType("integer")
            .HasColumnOrder(5);

        builder.Property(e => e.GamesPlayed)
            .IsRequired()
            .HasColumnName(nameof(WebsiteStatistics.GamesPlayed))
            .HasColumnType("integer")
            .HasColumnOrder(6);

        builder.Property(e => e.FactionsDrafted)
            .IsRequired()
            .HasColumnName(nameof(WebsiteStatistics.FactionsDrafted))
            .HasColumnType("integer")
            .HasColumnOrder(7);

        builder.Property(e => e.ColorsDrafted)
            .IsRequired()
            .HasColumnName(nameof(WebsiteStatistics.ColorsDrafted))
            .HasColumnType("integer")
            .HasColumnOrder(8);
    }
}
