namespace TwilightImperiumUltimate.DataAccess.Configurations.Relationships;

internal class SliceDraftRatingConfiguration : IEntityTypeConfiguration<SliceDraftRating>
{
    public void Configure(EntityTypeBuilder<SliceDraftRating> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(SliceDraftRating), Schema.Relationships);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Rating)
               .IsRequired();

        builder.Property(x => x.Id)
                .IsRequired()
                .UseIdentityColumn()
                .HasColumnType("integer")
                .HasColumnOrder(0);

        builder.Property(r => r.SliceDraftId)
                .IsRequired()
                .HasColumnOrder(1);

        builder.Property(r => r.UserId)
                .IsRequired()
                .HasColumnOrder(2);

        builder.Property(r => r.Rating)
                .IsRequired()
                .HasColumnOrder(3);

        builder.HasIndex(r => new { r.SliceDraftId, r.UserId })
               .IsUnique(); // Ensure a user can only rate a slice draft once

        builder.HasOne(r => r.SliceDraft)
            .WithMany(d => d.SliceDraftRatings)
            .HasForeignKey(r => r.SliceDraftId);

        builder.HasOne(r => r.User)
            .WithMany(u => u.SliceDraftRatings)
            .HasForeignKey(r => r.UserId);
    }
}
