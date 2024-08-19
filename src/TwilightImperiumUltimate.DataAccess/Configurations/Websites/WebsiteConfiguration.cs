namespace TwilightImperiumUltimate.DataAccess.Configurations.Websites;

internal class WebsiteConfiguration : IEntityTypeConfiguration<Website>
{
    public void Configure(EntityTypeBuilder<Website> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.Websites, Schema.Website);

        builder.HasKey(e => e.Id);

        builder.Property(x => x.Id)
            .HasColumnOrder(0);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)")
            .HasColumnName(nameof(Website.Title))
            .HasColumnOrder(1);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnType("varchar(500)")
            .HasColumnName(nameof(Website.Description))
            .HasColumnOrder(2);

        builder.Property(x => x.WebsitePath)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("varchar(255)")
            .HasColumnName(nameof(Website.WebsitePath))
            .HasColumnOrder(3);
    }
}
