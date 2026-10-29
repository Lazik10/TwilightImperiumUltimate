using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;

namespace TwilightImperiumUltimate.DataAccess.Configurations.DraftRooms;

internal sealed class DraftResultConfiguration : IEntityTypeConfiguration<DraftResult>
{
    public void Configure(EntityTypeBuilder<DraftResult> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable(TableName.DraftResults, Schema.Draft);
        builder.HasKey(result => result.Id);
        builder.Property(result => result.ShareToken).IsRequired().IsUnicode(false).HasMaxLength(22)
            .UseCollation("Latin1_General_100_BIN2");
        builder.HasIndex(result => result.ShareToken).IsUnique();
        builder.HasIndex(result => result.SourceRoomId).IsUnique().HasFilter("[SourceRoomId] IS NOT NULL");
        builder.HasIndex(result => new { result.OwnerUserId, result.FinishedAtUtc });
        builder.Property(result => result.DraftType).HasConversion<string>().IsUnicode(false).HasMaxLength(32);
        builder.Property(result => result.RoomName).IsRequired().HasMaxLength(DraftRoomLimits.MaxRoomNameLength);
        builder.Property(result => result.PayloadJson).IsRequired();
        builder.Property(result => result.OwnerUserId).HasMaxLength(450);
        builder.HasOne<TwilightImperiumUser>().WithMany().HasForeignKey(result => result.OwnerUserId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(result => result.Players).WithOne().HasForeignKey(player => player.ResultId).OnDelete(DeleteBehavior.Cascade);
    }
}
