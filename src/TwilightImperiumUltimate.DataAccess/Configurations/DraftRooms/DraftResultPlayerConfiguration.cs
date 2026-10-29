using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class DraftResultPlayerConfiguration : IEntityTypeConfiguration<DraftResultPlayer>
{
    public void Configure(EntityTypeBuilder<DraftResultPlayer> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable(TableName.DraftResultPlayers, Schema.Draft);
        builder.HasKey(player => new { player.ResultId, player.SeatIndex });
        builder.Property(player => player.DisplayName).IsRequired().HasMaxLength(DraftRoomLimits.MaxStoredDisplayNameLength);
        builder.Property(player => player.UserId).HasMaxLength(450);
        builder.HasIndex(player => player.UserId);
        builder.HasOne<TwilightImperiumUser>().WithMany().HasForeignKey(player => player.UserId).OnDelete(DeleteBehavior.SetNull);
    }
}
