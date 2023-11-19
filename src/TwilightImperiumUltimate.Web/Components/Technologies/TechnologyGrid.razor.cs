using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Technologies;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class TechnologyGrid
{
    private IReadOnlyCollection<Technology> _technologies = Array.Empty<Technology>();

    private TechnologyType _selectedTechnologyType = TechnologyType.Biotic;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _technologies = await HttpClient.GetAsync<List<Technology>>(Paths.ApiPath_Technologies);
    }

    private void TechnologyTypeChange(TechnologyType technologyType) => _selectedTechnologyType = technologyType;

    private List<Technology> TechnologiesToShow()
    {
        if (_selectedTechnologyType == TechnologyType.Faction)
        {
            return _technologies
                .Where(x => x.IsFactionTechnology)
                .ToList();
        }
        else if (_selectedTechnologyType == TechnologyType.UnitUpgrade)
        {
            return _technologies
                .Where(x => x.Type == TechnologyType.UnitUpgrade)
                .Where(x => !x.IsFactionTechnology)
                .OrderBy(x => x.TechnologyName)
                .ToList();
        }
        else
        {
            return _technologies
                .Where(x => x.Type == _selectedTechnologyType)
                .Where(x => !x.IsFactionTechnology)
                .ToList();
        }
    }
}
