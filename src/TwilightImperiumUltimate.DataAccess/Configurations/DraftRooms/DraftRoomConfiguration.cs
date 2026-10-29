using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class DraftRoomConfiguration : IEntityTypeConfiguration<DraftRoom>
{
    public void Configure(EntityTypeBuilder<DraftRoom> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.DraftRooms, Schema.Draft);

        builder.HasKey(room => room.Id);

        builder.HasIndex(room => room.Code)
            .IsUnique();

        builder.Property(room => room.Code)
            .IsRequired()
            .HasColumnType("varchar(8)")
            .HasMaxLength(8);

        builder.Property(room => room.Name)
            .IsRequired()
            .HasColumnType("nvarchar(60)")
            .HasMaxLength(60);

        builder.Property(room => room.PasswordHash)
            .IsRequired()
            .HasColumnType("varchar(256)")
            .HasMaxLength(256);

        builder.Property(room => room.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnType("varchar(16)")
            .HasMaxLength(16);

        builder.Property(room => room.DraftType)
            .IsRequired()
            .HasDefaultValue(nameof(DraftTypeId.FactionPick))
            .HasColumnType("varchar(64)")
            .HasMaxLength(64);

        builder.Property(room => room.IsLocked)
            .IsRequired();

        builder.Property(room => room.Version)
            .IsRequired()
            .IsConcurrencyToken();

        builder.Property(room => room.CreatedAtUtc)
            .IsRequired();

        builder.Property(room => room.LastActivityUtc)
            .IsRequired();

        builder.Property(room => room.ResultId);

        builder.HasMany(room => room.Participants)
            .WithOne(participant => participant.Room)
            .HasForeignKey(participant => participant.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(room => room.ChatMessages)
            .WithOne(message => message.Room)
            .HasForeignKey(message => message.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(room => room.LogEntries)
            .WithOne(entry => entry.Room)
            .HasForeignKey(entry => entry.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
