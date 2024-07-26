namespace TwilightImperiumUltimate.DataAccess.Configurations.News;

internal sealed class NewsArticleConfigruation : IEntityTypeConfiguration<NewsArticle>
{
    public void Configure(EntityTypeBuilder<NewsArticle> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableName.NewsArticles, Schema.News);

        builder.HasKey(e => e.Id)
            .IsClustered();

        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn()
            .HasColumnType("integer")
            .HasColumnOrder(0);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasColumnName(nameof(NewsArticle.Title))
            .HasColumnType("nvarchar(255)")
            .HasColumnOrder(1);

        builder.Property(e => e.Content)
            .IsRequired()
            .HasColumnName(nameof(NewsArticle.Content))
            .HasColumnType("nvarchar(MAX)")
            .HasColumnOrder(2);

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasColumnName(nameof(NewsArticle.CreatedAt))
            .HasColumnType("date")
            .HasColumnOrder(3);

        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasColumnName(nameof(NewsArticle.UpdatedAt))
            .HasColumnType("date")
            .HasColumnOrder(4);

        builder.Property(e => e.UserId)
            .IsRequired()
            .HasColumnName(nameof(NewsArticle.UserId))
            .HasColumnType("nvarchar(450)")
            .HasColumnOrder(5);

        builder.HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
