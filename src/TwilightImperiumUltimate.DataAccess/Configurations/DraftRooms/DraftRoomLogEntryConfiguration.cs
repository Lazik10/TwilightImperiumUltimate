using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class DraftRoomLogEntryConfiguration : IEntityTypeConfiguration<DraftRoomLogEntry>
{
    public void Configure(EntityTypeBuilder<DraftRoomLogEntry> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.DraftRoomLogEntries, Schema.Draft);

        builder.HasKey(entry => entry.Id);

        builder.HasIndex(entry => new { entry.RoomId, entry.Id });

        builder.Property(entry => entry.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnType("varchar(48)")
            .HasMaxLength(48);

        builder.Property(entry => entry.DataJson)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(entry => entry.CreatedAtUtc)
            .IsRequired();
    }
}
