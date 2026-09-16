using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwilightImperiumUltimate.Core.Entities.Async;
using TwilightImperiumUltimate.DataAccess.Tables;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Async;

public sealed class AsyncStatisticsSnapshotConfiguration : IEntityTypeConfiguration<AsyncStatisticsSnapshot>
{
    public void Configure(EntityTypeBuilder<AsyncStatisticsSnapshot> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.AsyncStatisticsSnapshots, Schema.Statistics);
        builder.HasKey(snapshot => snapshot.Id);
        builder.Property(snapshot => snapshot.GeneratedAtUtc).IsRequired();
        builder.Property(snapshot => snapshot.SnapshotVersion).IsRequired();
        builder.Property(snapshot => snapshot.SourceDataVersion).HasMaxLength(128);
        builder.Property(snapshot => snapshot.Payload).IsRequired();
        builder.HasIndex(snapshot => new { snapshot.IsPublished, snapshot.SnapshotVersion });
        builder.HasIndex(snapshot => snapshot.IsPublished)
            .IsUnique()
            .HasFilter("[IsPublished] = 1");
    }
}
