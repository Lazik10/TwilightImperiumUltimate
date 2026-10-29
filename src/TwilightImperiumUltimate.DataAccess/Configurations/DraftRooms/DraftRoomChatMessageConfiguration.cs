using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class DraftRoomChatMessageConfiguration : IEntityTypeConfiguration<DraftRoomChatMessage>
{
    public void Configure(EntityTypeBuilder<DraftRoomChatMessage> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.DraftRoomChatMessages, Schema.Draft);

        builder.HasKey(message => message.Id);

        builder.HasIndex(message => new { message.RoomId, message.Id });

        builder.Property(message => message.Kind)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnType("varchar(16)")
            .HasMaxLength(16);

        builder.Property(message => message.Text)
            .IsRequired()
            .HasColumnType("nvarchar(300)")
            .HasMaxLength(300);

        builder.Property(message => message.CreatedAtUtc)
            .IsRequired();
    }
}
