using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.DTOs.NewsArticle;

namespace TwilightImperiumUltimate.Web.Components.News;

public partial class NewsArticleSegment
{
    private MarkupString? _markupString;

    [Parameter]
    public NewsArticleDto NewsArticle { get; set; } = default!;

    protected override void OnParametersSet()
    {
        _markupString = new MarkupString(NewsArticle.Content);
    }
}
