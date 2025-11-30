using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class PlayerResultConfiguration : IEntityTypeConfiguration<PlayerResult>
{
    public void Configure(EntityTypeBuilder<PlayerResult> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.PlayerResults, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.MatchReportId)
            .HasDatabaseName("IX_PlayerResults_MatchReportId");

        builder.HasIndex(p => p.TiglUserId)
            .HasDatabaseName("IX_PlayerResults_TiglUser");

        builder.HasIndex(p => new { p.Faction, p.Score })
            .HasDatabaseName("IX_PlayerResults_Faction_Score");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.TiglUserId)
            .IsRequired()
            .HasColumnName(nameof(PlayerResult.TiglUserId))
            .HasColumnType("integer")
            .HasColumnOrder(1);

        builder.Property(e => e.Score)
            .IsRequired()
            .HasColumnName(nameof(PlayerResult.Score))
            .HasColumnType("integer")
            .HasColumnOrder(2);

        builder.Property(e => e.IsWinner)
            .IsRequired()
            .HasColumnName(nameof(PlayerResult.IsWinner))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .HasColumnOrder(3);

        builder.Property(e => e.Faction)
            .IsRequired()
            .HasColumnName(nameof(PlayerResult.Faction))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(4);

        builder.HasOne(x => x.MatchReport)
            .WithMany(m => m.PlayerResults)
            .HasForeignKey(x => x.MatchReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
