using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class FactionPickSettingsConfiguration : IEntityTypeConfiguration<FactionPickSettings>
{
    public void Configure(EntityTypeBuilder<FactionPickSettings> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable(TableName.FactionPickSettings, Schema.Draft);
        builder.HasKey(settings => settings.RoomId);
        builder.HasOne<DraftRoom>().WithOne(room => room.FactionPickSettings)
            .HasForeignKey<FactionPickSettings>(settings => settings.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.PrimitiveCollection(settings => settings.GameVersions).IsRequired()
            .ElementType().HasConversion<string>();
        builder.PrimitiveCollection(settings => settings.SeatOrder).IsRequired();
        builder.Property(settings => settings.BansMode).HasConversion<string>().HasMaxLength(32).IsUnicode(false);
        builder.Property(settings => settings.PickOrderMode).HasConversion<string>().HasMaxLength(16).IsUnicode(false);
    }
}
