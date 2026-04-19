using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.DTOs.NewsArticle;
using TwilightImperiumUltimate.Web.Formatting;

namespace TwilightImperiumUltimate.Web.Components.News;

public partial class NewsArticleSegment
{
    private MarkupString? _markupString;

    [Parameter]
    public NewsArticleDto NewsArticle { get; set; } = default!;

    private string FormattedCreatedAt => NewsArticle.CreatedAt.ToString(DateFormats.IsoDate);

    protected override void OnParametersSet()
    {
        _markupString = new MarkupString(NewsArticle.Content);
    }
}
