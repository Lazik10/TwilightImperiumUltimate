using TwilightImperiumUltimate.Web.Models.Users;

namespace TwilightImperiumUltimate.Web.Models.News;

public class NewsArticle
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public int? UserId { get; set; }

    public User? User { get; set; }
}
