using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class FactionPickTurnEntryConfiguration : IEntityTypeConfiguration<FactionPickTurnEntry>
{
    public void Configure(EntityTypeBuilder<FactionPickTurnEntry> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable(TableName.FactionPickTurnOrder, Schema.Draft);
        builder.HasKey(entry => new { entry.RoomId, entry.Position });
        builder.HasIndex(entry => new { entry.RoomId, entry.SeatId }).IsUnique();
        builder.HasOne<DraftRoom>().WithMany(room => room.FactionPickTurnOrder)
            .HasForeignKey(entry => entry.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<DraftRoomSeat>().WithMany()
            .HasForeignKey(entry => new { entry.RoomId, entry.SeatId })
            .HasPrincipalKey(seat => new { seat.RoomId, seat.Id })
            .OnDelete(DeleteBehavior.NoAction);
    }
}
