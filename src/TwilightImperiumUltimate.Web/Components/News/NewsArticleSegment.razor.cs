using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.News;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Components.News;

public partial class NewsArticleSegment
{
    private MarkupString? _markupString;

    [Parameter]
    public NewsArticle NewsArticle { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    protected override void OnParametersSet()
    {
        _markupString = new MarkupString(NewsArticle.Content);
    }
}
