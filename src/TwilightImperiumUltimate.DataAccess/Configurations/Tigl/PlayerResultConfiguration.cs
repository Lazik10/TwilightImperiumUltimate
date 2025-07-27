using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class PlayerResultConfiguration : IEntityTypeConfiguration<PlayerResult>
{
    public void Configure(EntityTypeBuilder<PlayerResult> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.PlayerResults, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.TiglUser)
            .IsRequired()
            .HasColumnName(nameof(PlayerResult.TiglUser))
            .HasColumnType("integer")
            .HasColumnOrder(1);

        builder.Property(e => e.Score)
            .IsRequired()
            .HasColumnName(nameof(PlayerResult.Score))
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(e => e.Faction)
            .IsRequired()
            .HasColumnName(nameof(PlayerResult.Faction))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(3);

        builder.HasOne(x => x.MatchReport)
            .WithMany(m => m.PlayerResults)
            .HasForeignKey(x => x.MatchReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
