using TwilightImperiumUltimate.Core.Entities.Tigl.Ranks;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class PrestigeRankConfiguration : IEntityTypeConfiguration<PrestigeRank>
{
    public void Configure(EntityTypeBuilder<PrestigeRank> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.PrestigeRanks, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.HasIndex(e => new { e.Name, e.FactionName, e.League })
            .HasDatabaseName("IX_PrestigeRanks_Name_Faction_League");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName(nameof(PrestigeRank.Name))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder.Property(x => x.FactionName)
            .IsRequired()
            .HasColumnName(nameof(PrestigeRank.FactionName))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(x => x.League)
            .IsRequired()
            .HasColumnName(nameof(PrestigeRank.League))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(3);
    }
}
