namespace TwilightImperiumUltimate.Web.Models.News;

/// <summary>
/// Create/edit form model for the admin News article editor. When <see cref="Id"/> is null the
/// form is in "create" mode; otherwise it represents the article being edited.
/// </summary>
public class NewsArticleFormModel
{
    public int? Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}
