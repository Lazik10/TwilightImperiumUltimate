using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class TiglUserConfiguration : IEntityTypeConfiguration<TiglUser>
{
    public void Configure(EntityTypeBuilder<TiglUser> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.TiglUsers, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.HasIndex(x => x.DiscordId)
            .IsUnique()
            .HasDatabaseName("IX_TiglUser_DiscordId");

        builder.HasIndex(x => x.TiglUserName)
            .IsUnique()
            .HasDatabaseName("IX_TiglUser_TiglUserName");

        builder.Property(x => x.DiscordId)
            .IsRequired()
            .HasColumnName(nameof(TiglUser.DiscordId))
            .HasColumnType("bigint")
            .HasColumnOrder(1);

        builder.Property(e => e.TiglUserName)
            .IsRequired()
            .HasColumnName(nameof(TiglUser.TiglUserName))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(e => e.DiscordTag)
            .IsRequired()
            .HasColumnName(nameof(TiglUser.DiscordTag))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(3);

        builder.Property(e => e.TtsUserName)
            .IsRequired()
            .HasColumnName(nameof(TiglUser.TtsUserName))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(4);

        builder.Property(e => e.TtpgUserName)
            .IsRequired()
            .HasColumnName(nameof(TiglUser.TtpgUserName))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(5);

        builder.Property(e => e.BgaUserName)
            .IsRequired()
            .HasColumnName(nameof(TiglUser.BgaUserName))
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(6);

        builder.HasMany(a => a.AsyncStats)
            .WithOne(u => u.TiglUser)
            .HasForeignKey(p => p.TiglUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.GlickoStats)
            .WithOne(u => u.TiglUser)
            .HasForeignKey(p => p.TiglUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.TrueSkillStats)
            .WithOne(u => u.TiglUser)
            .HasForeignKey(p => p.TiglUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
