using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class FactionPickStateConfiguration : IEntityTypeConfiguration<FactionPickState>
{
    public void Configure(EntityTypeBuilder<FactionPickState> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable(TableName.FactionPickStates, Schema.Draft);
        builder.HasKey(state => state.RoomId);
        builder.HasOne<DraftRoom>().WithOne(room => room.FactionPickState)
            .HasForeignKey<FactionPickState>(state => state.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(state => state.Phase).HasConversion<string>().HasMaxLength(16).IsUnicode(false);
        builder.PrimitiveCollection(state => state.LockedBanSeatIds).IsRequired();
    }
}
