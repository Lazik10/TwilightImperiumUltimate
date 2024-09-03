namespace TwilightImperiumUltimate.DataAccess.Configurations.Users;

internal sealed class TwilightImperiumUserConfiguration : IEntityTypeConfiguration<TwilightImperiumUser>
{
    public void Configure(EntityTypeBuilder<TwilightImperiumUser> builder)
    {
        builder.HasIndex(x => x.UserName)
            .IsUnique();

        // One-to-Many: User -> Maps
        builder.HasMany(u => u.Maps)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // One-to-Many: User -> MapRatings
        builder.HasMany(u => u.MapRatings)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // One-to-Many: User -> SliceDrafts
        builder.HasMany(u => u.SliceDrafts)
            .WithOne(d => d.User)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // One-to-Many: User -> SliceDraftRatings
        builder.HasMany(u => u.SliceDraftRatings)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
