using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Async;

public class AsyncPlayerProfileSettingsConfiguration : IEntityTypeConfiguration<AsyncPlayerProfileSettings>
{
    public void Configure(EntityTypeBuilder<AsyncPlayerProfileSettings> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.AsyncPlayerProfileSettings, Schema.Statistics);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnOrder(0);

        builder.Property(e => e.ExcludeFromAsyncStats)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfileSettings.ExcludeFromAsyncStats))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(1);

        builder.Property(e => e.ShowWinRate)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfileSettings.ShowWinRate))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(2);

        builder.Property(e => e.ShowTurnStats)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfileSettings.ShowTurnStats))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(3);

        builder.Property(e => e.ShowCombatStats)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfileSettings.ShowCombatStats))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(4);

        builder.Property(e => e.ShowVpStats)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfileSettings.ShowVpStats))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(5);

        builder.Property(e => e.ShowFactionStats)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfileSettings.ShowFactionStats))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(6);

        builder.Property(e => e.ShowOpponents)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfileSettings.ShowOpponents))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(7);

        builder.Property(e => e.ShowGames)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfileSettings.ShowGames))
            .HasConversion(new BoolToStringConverter("false", "true"))
            .HasColumnType("varchar(5)")
            .HasColumnOrder(8);
    }
}
