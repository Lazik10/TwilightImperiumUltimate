namespace TwilightImperiumUltimate.DataAccess.Configurations.Galaxy;

internal sealed class MapConfiguration : IEntityTypeConfiguration<Map>
{
    public void Configure(EntityTypeBuilder<Map> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Maps, Schema.Galaxy);

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
            .HasColumnName(nameof(Map.Name))
            .HasConversion<string>()
            .HasColumnType("varchar(256)")
            .HasMaxLength(256)
            .HasColumnOrder(1);

        builder.Property(m => m.EventName)
            .IsRequired()
            .HasColumnName(nameof(Map.EventName))
            .HasConversion<string>()
            .HasColumnType("varchar(256)")
            .HasMaxLength(256)
            .HasColumnOrder(2);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasColumnName(nameof(Map.Description))
            .HasConversion<string>()
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(3);

        builder.Property(x => x.MapTemplate)
            .IsRequired()
            .HasColumnName(nameof(Map.MapTemplate))
            .HasConversion<string>()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnOrder(4);

        builder.Property(x => x.TtsString)
            .IsRequired()
            .HasColumnName(nameof(Map.TtsString))
            .HasConversion<string>()
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(5);

        builder.Property(x => x.MapGeneratorLink)
            .IsRequired()
            .HasColumnName(nameof(Map.MapGeneratorLink))
            .HasConversion<string>()
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(6);

        builder.Property(x => x.MapArchiveLink)
            .IsRequired()
            .HasColumnName(nameof(Map.MapArchiveLink))
            .HasConversion<string>()
            .HasColumnType("varchar(MAX)")
            .HasColumnOrder(7);

        builder.Property(m => m.UserName)
            .IsRequired()
            .HasColumnName(nameof(Map.UserName))
            .HasConversion<string>()
            .HasColumnType("varchar(256)")
            .HasColumnOrder(8);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName(nameof(Map.UserId))
            .HasColumnOrder(9);

        // One-to-Many: Map -> MapRatings
        builder.HasMany(m => m.MapRatings)
               .WithOne(r => r.Map)
               .HasForeignKey(r => r.MapId);
    }
}
