using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Rules;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleGrid
{
    private List<Rule> _rules = new();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected async override Task OnInitializedAsync()
    {
        _rules = await HttpClient.GetAsync<List<Rule>>(Paths.ApiPath_Rules);
    }
}
