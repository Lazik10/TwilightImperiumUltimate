using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Async;

public class AsyncPlayerProfileConfiguration : IEntityTypeConfiguration<AsyncPlayerProfile>
{
    public void Configure(EntityTypeBuilder<AsyncPlayerProfile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.AsyncPlayerProfile, Schema.Statistics);

        builder.HasKey(p => p.Id);

        builder.HasIndex(x => x.DiscordUserId)
            .IsClustered(false)
            .IsUnique();

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.DiscordUserId)
            .IsRequired()
            .HasColumnName(nameof(AsyncPlayerProfile.DiscordUserId))
            .HasColumnType("bigint")
            .HasColumnOrder(1);

        builder.Property(e => e.DiscordUserName)
            .HasColumnName(nameof(AsyncPlayerProfile.DiscordUserName))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.HasMany(p => p.GameStatistics)
            .WithOne(pg => pg.AsyncPlayerProfile)
            .HasForeignKey(pg => pg.AsyncPlayerProfileId);

        builder.HasOne(p => p.ProfileSettings)
            .WithOne(s => s.AsyncPlayerProfile)
            .HasForeignKey<AsyncPlayerProfileSettings>(s => s.AsyncPlayerProfileId);
    }
}

