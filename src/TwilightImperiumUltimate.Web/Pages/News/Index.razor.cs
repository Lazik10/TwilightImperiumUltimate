using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.News;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Pages.News;

public partial class Index
{
    private List<NewsArticle> _newsArticles = new();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected async override Task OnInitializedAsync()
    {
        _newsArticles = await HttpClient.GetAsync<List<NewsArticle>>(Paths.ApiPath_News);
    }
}