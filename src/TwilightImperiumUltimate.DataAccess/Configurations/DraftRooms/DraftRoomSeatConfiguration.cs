using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class DraftRoomSeatConfiguration : IEntityTypeConfiguration<DraftRoomSeat>
{
    public void Configure(EntityTypeBuilder<DraftRoomSeat> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.DraftRoomSeats, Schema.Draft);

        builder.HasKey(seat => seat.Id);

        builder.HasIndex(seat => new { seat.RoomId, seat.Index })
            .IsUnique();

        builder.HasIndex(seat => new { seat.RoomId, seat.ParticipantId })
            .IsUnique()
            .HasFilter("[ParticipantId] IS NOT NULL");

        builder.Property(seat => seat.Index)
            .IsRequired();

        builder.Property(seat => seat.ClaimedAtUtc);

        builder.HasOne(seat => seat.Room)
            .WithMany(room => room.Seats)
            .HasForeignKey(seat => seat.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(seat => seat.Participant)
            .WithMany()
            .HasForeignKey(seat => seat.ParticipantId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
