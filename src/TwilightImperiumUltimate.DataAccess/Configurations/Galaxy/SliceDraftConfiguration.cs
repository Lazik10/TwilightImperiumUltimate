namespace TwilightImperiumUltimate.DataAccess.Configurations.Galaxy;

internal class SliceDraftConfiguration : IEntityTypeConfiguration<SliceDraft>
{
    public void Configure(EntityTypeBuilder<SliceDraft> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.SliceDrafts, Schema.Galaxy);

        builder.HasKey(m => m.Id);

        builder.HasIndex(r => new { r.Name, r.EventName })
            .IsUnique();

        builder.Property(m => m.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnType("integer")
            .HasColumnOrder(0);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasColumnName(nameof(SliceDraft.Name))
            .HasConversion<string>()
            .HasColumnType("varchar(256)")
            .HasMaxLength(256)
            .HasColumnOrder(1);

        builder.Property(m => m.EventName)
            .IsRequired()
            .HasColumnName(nameof(SliceDraft.EventName))
            .HasConversion<string>()
            .HasColumnType("varchar(256)")
            .HasMaxLength(256)
            .HasColumnOrder(2);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasColumnName(nameof(SliceDraft.Description))
            .HasConversion<string>()
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(3);

        builder.Property(x => x.SliceDraftString)
            .IsRequired()
            .HasColumnName(nameof(SliceDraft.SliceDraftString))
            .HasConversion<string>()
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(5);

        builder.Property(x => x.SliceNames)
            .IsRequired()
            .HasColumnName(nameof(SliceDraft.SliceNames))
            .HasConversion<string>()
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(6);

        builder.Property(x => x.SliceCount)
            .IsRequired()
            .HasColumnName(nameof(SliceDraft.SliceCount))
            .HasConversion<int>()
            .HasColumnType("integer")
            .HasColumnOrder(7);

        builder.Property(x => x.SliceDraftGeneratorLink)
            .IsRequired()
            .HasColumnName(nameof(SliceDraft.SliceDraftGeneratorLink))
            .HasConversion<string>()
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(8);

        builder.Property(x => x.SliceDraftArchiveLink)
            .IsRequired()
            .HasColumnName(nameof(SliceDraft.SliceDraftArchiveLink))
            .HasConversion<string>()
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(9);

        builder.Property(m => m.UserName)
            .IsRequired()
            .HasColumnName(nameof(SliceDraft.UserName))
            .HasConversion<string>()
            .HasColumnType("varchar(256)")
            .HasColumnOrder(10);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName(nameof(SliceDraft.UserId))
            .HasColumnOrder(11);

        // One-to-Many: SliceDraft -> SliceDraftRatings
        builder.HasMany(m => m.SliceDraftRatings)
            .WithOne(r => r.SliceDraft)
            .HasForeignKey(r => r.SliceDraftId);
    }
}
