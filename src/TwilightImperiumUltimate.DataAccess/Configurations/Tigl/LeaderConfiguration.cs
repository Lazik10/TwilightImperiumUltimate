using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Configurations.Tigl;

public class LeaderConfiguration : IEntityTypeConfiguration<Leader>
{
    public void Configure(EntityTypeBuilder<Leader> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Leaders, Schema.Tigl);

        builder.HasKey(p => p.Id);

        builder.HasIndex(l => new { l.Faction, l.League })
            .HasDatabaseName("IX_Leaders_Faction_League");

        builder.HasIndex(l => l.ShortestHolderId)
            .HasDatabaseName("IX_Leaders_ShortestHolderId");

        builder.HasIndex(l => l.LongestHolderId)
            .HasDatabaseName("IX_Leaders_LongestHolderId");

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName(nameof(Leader.Name))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder.Property(x => x.Faction)
            .IsRequired()
            .HasColumnName(nameof(Leader.Faction))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(x => x.FirstUpdate)
            .IsRequired(false)
            .HasColumnName(nameof(Leader.FirstUpdate))
            .HasColumnType("bigint")
            .HasColumnOrder(3);

        builder.Property(x => x.LastUpdate)
            .IsRequired(false)
            .HasColumnName(nameof(Leader.LastUpdate))
            .HasColumnType("bigint")
            .HasColumnOrder(4);

        builder.Property(x => x.PreviousUpdate)
            .IsRequired(false)
            .HasColumnName(nameof(Leader.PreviousUpdate))
            .HasColumnType("bigint")
            .HasColumnOrder(5);

        builder.Property(x => x.ShortestDuration)
            .IsRequired(false)
            .HasColumnName(nameof(Leader.ShortestDuration))
            .HasColumnType("bigint")
            .HasColumnOrder(6);

        builder.Property(x => x.LongestDuration)
            .IsRequired(false)
            .HasColumnName(nameof(Leader.LongestDuration))
            .HasColumnType("bigint")
            .HasColumnOrder(7);

        builder.Property(x => x.ChangeCount)
            .HasColumnName(nameof(Leader.ChangeCount))
            .HasColumnType("int")
            .HasColumnOrder(8);

        builder.Property(x => x.League)
            .IsRequired()
            .HasColumnName(nameof(Leader.League))
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .HasColumnOrder(9);

        builder.Property(x => x.PreviousOwnerId)
            .IsRequired(false)
            .HasColumnName(nameof(Leader.PreviousOwnerId))
            .HasColumnType("int")
            .HasColumnOrder(10);

        builder.Property(x => x.CurrentOwnerId)
            .IsRequired(false)
            .HasColumnName(nameof(Leader.CurrentOwnerId))
            .HasColumnType("int")
            .HasColumnOrder(11);

        builder.Property(x => x.ShortestHolderId)
            .IsRequired(false)
            .HasColumnName(nameof(Leader.ShortestHolderId))
            .HasColumnType("int")
            .HasColumnOrder(12);

        builder.Property(x => x.LongestHolderId)
            .IsRequired(false)
            .HasColumnName(nameof(Leader.LongestHolderId))
            .HasColumnType("int")
            .HasColumnOrder(13);

        // Current leader holder: keep SetNull when TiglUser is deleted
        builder.HasOne(l => l.CurrentOwner)
            .WithMany(u => u.Leaders)
            .HasForeignKey(l => l.CurrentOwnerId)
            .OnDelete(DeleteBehavior.SetNull);

        // Avoid multiple cascade paths by disabling cascading on these two
        builder.HasOne(l => l.ShortestHolder)
            .WithMany()
            .HasForeignKey(l => l.ShortestHolderId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Leaders_Users_ShortestHolderId");

        builder.HasOne(l => l.LongestHolder)
            .WithMany()
            .HasForeignKey(l => l.LongestHolderId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Leaders_Users_LongestHolderId");
    }
}
