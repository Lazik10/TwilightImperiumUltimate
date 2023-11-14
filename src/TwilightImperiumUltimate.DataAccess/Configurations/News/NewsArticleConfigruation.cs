using TwilightImperiumUltimate.Core.Entities.News;
using TwilightImperiumUltimate.DataAccess.Tables.News;

namespace TwilightImperiumUltimate.DataAccess.Configurations.News;

internal class NewsArticleConfigruation : IEntityTypeConfiguration<NewsArticle>
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
            .HasColumnType("integer");

        builder.Property(e => e.Title)
            .IsRequired()
            .HasColumnName(nameof(NewsArticle.Title))
            .HasColumnType("nvarchar(255)");

        builder.Property(e => e.Content)
            .IsRequired()
            .HasColumnName(nameof(NewsArticle.Content))
            .HasColumnType("nvarchar(MAX)");

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasColumnName(nameof(NewsArticle.CreatedAt))
            .HasColumnType("date");

        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasColumnName(nameof(NewsArticle.UpdatedAt))
            .HasColumnType("date");

        builder.Property(e => e.UserId)
            .IsRequired()
            .HasColumnName(nameof(NewsArticle.UserId))
            .HasColumnType("integer");
    }
}
