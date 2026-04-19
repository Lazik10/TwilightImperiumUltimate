namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class TechnologyGrid : TwilightImperiumBaseComponent
{
    private IReadOnlyCollection<TechnologyModel> _technologies = new List<TechnologyModel>();

    [Parameter]
    public TechnologyType SelectedTechnologyType { get; set; } = TechnologyType.Biotic;

    protected override async Task OnInitializedAsync()
    {
        await InitializeTechnologies();
    }

    private List<TechnologyModel> TechnologiesToShow()
    {
        if (SelectedTechnologyType == TechnologyType.Faction)
        {
            return _technologies
                .Where(x => x.IsFactionTechnology)
                .OrderBy(x => x.FactionName)
                .ToList();
        }
        else if (SelectedTechnologyType == TechnologyType.UnitUpgrade)
        {
            return _technologies
                .Where(x => x.Type == TechnologyType.UnitUpgrade)
                .Where(x => !x.IsFactionTechnology)
                .OrderBy(x => x.Level)
                .ThenBy(x => x.TechnologyName)
                .ToList();
        }
        else
        {
            return _technologies
                .Where(x => x.Type == SelectedTechnologyType)
                .Where(x => !x.IsFactionTechnology)
                .OrderBy(x => x.Level)
                .ToList();
        }
    }

    private async Task InitializeTechnologies()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<TechnologyDto>>>(Paths.ApiPath_Technologies);
        if (statusCode == HttpStatusCode.OK)
        {
            var technologies = Mapper.Map<List<TechnologyModel>>(response!.Data!.Items);
            _technologies = technologies.Where(x => x.GameVersion != GameVersion.Deprecated).ToList();
        }
    }
}

