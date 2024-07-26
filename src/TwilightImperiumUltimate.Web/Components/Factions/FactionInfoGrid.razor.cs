namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionInfoGrid
{
    private FactionModel _selectedFaction = default!;

    private FactionInfoType _selectedFactionInfoType = FactionInfoType.Ability;

    private bool showBigImage;

    private string currentBigImageSrc = string.Empty;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, CreateInfoType());
        builder.AddAttribute(1, "Faction", _selectedFaction);
        builder.CloseComponent();
    };

    public void UpdateSelectedFaction(FactionModel faction)
    {
        _selectedFaction = faction;
        StateHasChanged();
    }

/*    protected async override Task OnInitializedAsync()
    {
        await InitializeSelectedFaction();
    }*/

    private Type CreateInfoType()
    {
        var infoType = _selectedFactionInfoType switch
        {
            FactionInfoType.Ability => typeof(FactionAbilities),
            FactionInfoType.Setup => typeof(FactionSetup),
            FactionInfoType.Componenets => typeof(FactionComponents),
            FactionInfoType.Leaders => typeof(FactionLeaders),
            FactionInfoType.History => typeof(FactionLore),
            FactionInfoType.Faq => typeof(FactionFaq),
            _ => throw new NotImplementedException(),
        };

        return infoType;
    }

    private void ShowBigImage(bool front)
    {
        if (_selectedFaction is not null)
        {
            currentBigImageSrc = PathProvider.GetFactionSheetPath(_selectedFaction.FactionName.ToString(), front);
            showBigImage = true;
        }
    }

    private void HideBigImage()
    {
        showBigImage = false;
    }

    private void SetFactionInfoType(FactionInfoType factionInfoType)
    {
        _selectedFactionInfoType = factionInfoType;
    }

/*    private async Task InitializeSelectedFaction()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<FactionDto>>>(Paths.ApiPath_FirstFaction);
        if (statusCode == HttpStatusCode.OK && response?.Data?.Items is not null)
            _selectedFaction = Mapper.Map<FactionModel>(response.Data?.Items?.First());

        StateHasChanged();
    }*/
}
