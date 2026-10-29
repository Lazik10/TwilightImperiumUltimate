using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class FactionPickSelectionConfiguration : IEntityTypeConfiguration<FactionPickSelection>
{
    public void Configure(EntityTypeBuilder<FactionPickSelection> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable(TableName.FactionPickSelections, Schema.Draft);
        builder.HasKey(selection => selection.Id);
        builder.HasIndex(selection => new { selection.RoomId, selection.SeatId, selection.Kind });
        builder.HasIndex(selection => new { selection.RoomId, selection.SeatId, selection.Kind, selection.Faction })
            .IsUnique().HasFilter(null);
        builder.HasOne<DraftRoom>().WithMany(room => room.FactionPickSelections)
            .HasForeignKey(selection => selection.RoomId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<DraftRoomSeat>().WithMany()
            .HasForeignKey(selection => new { selection.RoomId, selection.SeatId })
            .HasPrincipalKey(seat => new { seat.RoomId, seat.Id }).OnDelete(DeleteBehavior.NoAction);
        builder.Property(selection => selection.Kind).HasConversion<string>().HasMaxLength(16).IsUnicode(false);
        builder.Property(selection => selection.State).HasConversion<string>().HasMaxLength(16).IsUnicode(false);
        builder.Property(selection => selection.Faction).HasConversion<string>().HasMaxLength(128).IsUnicode(false);
    }
}
