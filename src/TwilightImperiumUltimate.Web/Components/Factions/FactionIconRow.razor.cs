using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionIconRow : TwilightImperiumBaseComponenet
{
    private List<FactionModel>? _factions = new List<FactionModel>();

    [Parameter]
    public EventCallback<FactionModel> OnFactionClickGetFaction { get; set; }

    [Parameter]
    public EventCallback<IReadOnlyCollection<FactionModel>> OnInitializeGetFactions { get; set; }

    [Parameter]
    public bool EnableBanMode { get; set; } = false;

    [Parameter]
    public bool BanAllFactions { get; set; } = false;

    [Parameter]
    public bool ShowDiscordantStars { get; set; } = false;

    [Parameter]
    public bool ShowBaseGame { get; set; } = true;

    [Parameter]
    public List<FactionModel> ProvidedFactions { get; set; } = new List<FactionModel>();

    public IReadOnlyCollection<FactionModel>? Factions => _factions;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    public void RefreshFactions()
    {
        _factions = MapGeneratorSettingsService.FactionsForMapGenerator;
        StateHasChanged();
    }

    public void SetAllFactionsBanStatus(bool banStatus)
    {
        _factions?.ForEach(x => x.Banned = banStatus);
    }

    protected override async Task OnInitializedAsync()
    {
        // This is a hack so I can use this component in the map generator, unfortunatelly the componenet is initialized every time
        if (ProvidedFactions.Count != 0)
        {
            _factions = ProvidedFactions;

            await OnInitializeGetFactions.InvokeAsync(Factions);
            MapGeneratorSettingsService.FactionsForMapGenerator = ProvidedFactions;
            return;
        }

        await InitializeFactions();

        if (Factions is not null && Factions.Count != 0)
        {
            if (!ShowBaseGame && ShowDiscordantStars)
            {
                await OnFactionClickGetFaction.InvokeAsync(Factions.Single(x => x.FactionName == FactionName.TheAugursOfIlyxum));
            }
            else
            {
                await OnFactionClickGetFaction.InvokeAsync(Factions.Single(x => x.FactionName == FactionName.TheArborec));
            }
        }
    }

    private void FactionClicked(FactionModel selectedFaction)
    {
        if (EnableBanMode)
            selectedFaction.Banned = !selectedFaction.Banned;

        OnFactionClickGetFaction.InvokeAsync(selectedFaction);
    }

    private async Task InitializeFactions()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<FactionDto>>>(Paths.ApiPath_Factions);
        if (statusCode == HttpStatusCode.OK)
        {
            _factions = Mapper.Map<List<FactionModel>>(response!.Data!.Items);

            await OnInitializeGetFactions.InvokeAsync(Factions);

            if (BanAllFactions)
                SetAllFactionsBanStatus(true);
        }
    }

    private List<FactionModel> GetBaseGameFactions()
    {
        return _factions?.Where(x => x.GameVersion != GameVersion.DiscordantStars).ToList() ?? new List<FactionModel>();
    }

    private List<FactionModel> GetDiscordantStarsFactions()
    {
        return _factions?.Where(x => x.GameVersion == GameVersion.DiscordantStars).ToList() ?? new List<FactionModel>();
    }
}
