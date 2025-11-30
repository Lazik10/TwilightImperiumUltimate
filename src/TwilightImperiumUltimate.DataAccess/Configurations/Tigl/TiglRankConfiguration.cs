using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class TiglRankConfiguration : IEntityTypeConfiguration<TiglRank>
{
    public void Configure(EntityTypeBuilder<TiglRank> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Ranks, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.HasIndex(e => e.TiglUserId)
            .HasDatabaseName("IX_Ranks_TiglUserId");

        builder.HasIndex(e => new { e.Name, e.League })
            .HasDatabaseName("IX_Ranks_Name_League");

        builder.HasIndex(e => e.AchievedAt)
            .HasDatabaseName("IX_Ranks_AchievedAt");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.HasIndex(e => new { e.TiglUserId, e.League, e.AchievedAt })
            .HasDatabaseName("IX_Ranks_User_League_AchievedAt");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName(nameof(TiglRank.Name))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder.Property(x => x.AchievedAt)
            .IsRequired()
            .HasColumnName(nameof(TiglRank.AchievedAt))
            .HasConversion<long>()
            .HasColumnType("bigint")
            .HasColumnOrder(2);

        builder.Property(x => x.League)
            .IsRequired()
            .HasColumnName(nameof(TiglRank.League))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(3);

        builder.Property(x => x.TiglUserId)
            .IsRequired()
            .HasColumnName(nameof(TiglRank.TiglUserId))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(4);

        builder.HasOne(x => x.TiglUser)
            .WithMany(x => x.TiglRanks)
            .HasForeignKey(x => x.TiglUserId);
    }
}
