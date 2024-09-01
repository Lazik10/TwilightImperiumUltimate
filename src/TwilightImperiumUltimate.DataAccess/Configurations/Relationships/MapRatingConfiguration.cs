namespace TwilightImperiumUltimate.DataAccess.Configurations.Relationships;

internal sealed class MapRatingConfiguration : IEntityTypeConfiguration<MapRating>
{
    public void Configure(EntityTypeBuilder<MapRating> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(MapRating), Schema.Relationships);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Rating)
               .IsRequired();

        builder.Property(x => x.Id)
                .IsRequired()
                .UseIdentityColumn()
                .HasColumnType("integer")
                .HasColumnOrder(0);

        builder.Property(r => r.MapId)
                .IsRequired()
                .HasColumnOrder(1);

        builder.Property(r => r.UserId)
                .IsRequired()
                .HasColumnOrder(2);

        builder.Property(r => r.Rating)
                .IsRequired()
                .HasColumnOrder(3);

        // Configuring composite key (if needed)
        builder.HasIndex(r => new { r.MapId, r.UserId })
               .IsUnique(); // Ensure a user can only rate a map once
    }
}
