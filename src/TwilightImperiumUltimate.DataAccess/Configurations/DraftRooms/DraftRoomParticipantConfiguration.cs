using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class DraftRoomParticipantConfiguration : IEntityTypeConfiguration<DraftRoomParticipant>
{
    public void Configure(EntityTypeBuilder<DraftRoomParticipant> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.DraftRoomParticipants, Schema.Draft);

        builder.HasKey(participant => participant.Id);

        builder.HasIndex(participant => new { participant.RoomId, participant.UserId })
            .IsUnique()
            .HasFilter("[UserId] IS NOT NULL");

        builder.HasIndex(participant => new { participant.RoomId, participant.DisplayNameKey })
            .IsUnique()
            .HasFilter("[Status] = 'Active'");

        builder.Property(participant => participant.UserId)
            .HasColumnType("nvarchar(450)")
            .HasMaxLength(450);

        builder.Property(participant => participant.SecretHash)
            .HasColumnType("varbinary(32)")
            .HasMaxLength(32);

        builder.Property(participant => participant.DisplayName)
            .IsRequired()
            .HasColumnType($"nvarchar({DraftRoomLimits.MaxStoredDisplayNameLength})")
            .HasMaxLength(DraftRoomLimits.MaxStoredDisplayNameLength);

        builder.Property(participant => participant.DisplayNameKey)
            .IsRequired()
            .HasColumnType($"nvarchar({DraftRoomLimits.MaxStoredDisplayNameLength})")
            .HasMaxLength(DraftRoomLimits.MaxStoredDisplayNameLength);

        builder.Property(participant => participant.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnType("varchar(16)")
            .HasMaxLength(16);

        builder.Property(participant => participant.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnType("varchar(16)")
            .HasMaxLength(16);

        builder.Property(participant => participant.JoinedAtUtc)
            .IsRequired();

        builder.Property(participant => participant.LastSeenUtc)
            .IsRequired();

        builder.HasOne<TwilightImperiumUser>()
            .WithMany()
            .HasForeignKey(participant => participant.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
