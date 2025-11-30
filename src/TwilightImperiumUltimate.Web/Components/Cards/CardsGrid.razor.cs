using TwilightImperiumUltimate.Contracts.DTOs.Card;

namespace TwilightImperiumUltimate.Web.Components.Cards;

public partial class CardsGrid
{
    private bool _showNotes = true;
    private bool _showFaq;

    private IReadOnlyCollection<CardModel> _listOfCards = new List<CardModel>();

    private IReadOnlyCollection<CardModel> _listOfDeprecatedCards = new List<CardModel>();

    private CardModel _selectedCardModel = new CardModel();

    private bool showBigImage;

    private string currentBigImageSrc = string.Empty;

    private string currentBigImageCulture = string.Empty;

    private GameVersion? _currentGameVersion;

    [Parameter]
    public string TypeOfCard { get; set; } = Paths.ResourcePath_StrategyCard;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private List<FaqModel> Faqs { get; set; } = new List<FaqModel>();

    protected override async Task OnParametersSetAsync()
    {
        await InitializeCards();

        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<FaqDto>>>(Paths.ApiPath_Faq);
        var response = result.Response;
        var statusCode = result.StatusCode;
        if (statusCode == System.Net.HttpStatusCode.OK)
        {
            var faqs = Mapper.Map<List<FaqModel>>(response!.Data!.Items);
            Faqs = faqs.Where(f => f.FaqStatus == FaqStatus.Approved).ToList();
        }
    }

    private int GetNumberOfColumns() => TypeOfCard == Paths.ResourcePath_StrategyCard ? 4 : 5;

    private void ShowBigImage(CardModel card, string culture)
    {
        _selectedCardModel = card;
        currentBigImageSrc = PathProvider.GetCardImagePath(card.Name, TypeOfCard);
        currentBigImageCulture = culture;
        showBigImage = true;
    }

    private void HideBigImage()
    {
        showBigImage = false;
    }

    private string GetCultureIconPath(string culture)
    {
        return PathProvider.GetCultureIconPath(culture);
    }

    private void SetBigImageAddress(string culture)
    {
        currentBigImageSrc = currentBigImageSrc.Replace(currentBigImageCulture, culture);
        currentBigImageCulture = culture;
        StateHasChanged();
    }

    private string GetCorrectApiEndpoint()
    {
        return TypeOfCard switch
        {
            var s when s == Paths.ResourcePath_ActionCard => Paths.ApiPath_ActionCards,
            var s when s == Paths.ResourcePath_AgendaCard => Paths.ApiPath_AgendaCards,
            var s when s == Paths.ResourcePath_ExplorationCard => Paths.ApiPath_ExplorationCards,
            var s when s == Paths.ResourcePath_FrontierCard => Paths.ApiPath_FrontierCards,
            var s when s == Paths.ResourcePath_ObjectiveSecret => Paths.ApiPath_SecretObjectiveCards,
            var s when s == Paths.ResourcePath_ObjectiveStageOne => Paths.ApiPath_StageOneObjectiveCards,
            var s when s == Paths.ResourcePath_ObjectiveStageTwo => Paths.ApiPath_StageTwoObjectiveCards,
            var s when s == Paths.ResourcePath_PromissoryCard => Paths.ApiPath_PromissoryNoteCards,
            var s when s == Paths.ResourcePath_RelicCard => Paths.ApiPath_RelicCards,
            var s when s == Paths.ResourcePath_StrategyCard => Paths.ApiPath_StrategyCards,
            _ => string.Empty,
        };
    }

    private async Task InitializeCards()
    {
        var apiEndpoint = GetCorrectApiEndpoint();
        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<BaseCardDto>>>(apiEndpoint);
        var response = result.Response;
        var statusCode = result.StatusCode;

        if (statusCode == System.Net.HttpStatusCode.OK)
        {
            var cards = Mapper.Map<List<CardModel>>(response!.Data!.Items);
            _listOfCards = cards.Where(x => x.GameVersion != GameVersion.Deprecated).ToList();
            _listOfDeprecatedCards = cards.Where(x => x.GameVersion == GameVersion.Deprecated).ToList();
        }

        StateHasChanged();
    }

    private IEnumerable<IGrouping<GameVersion, CardModel>> GetSortedCards()
    {
        return _listOfCards
            .OrderBy(x => x.GameVersion)
            .ThenBy(x => x.Id)
            .GroupBy(x => x.GameVersion);
    }

    private void ShowFaq()
    {
        _showNotes = false;
        _showFaq = true;
    }

    private void ShowNotes()
    {
        _showFaq = false;
        _showNotes = true;
    }

    private List<FaqModel> GetSpecificCardFaqs()
    {
        return Faqs.Where(f => f.ComponentName == _selectedCardModel.Name && f.FaqStatus == FaqStatus.Approved).ToList();
    }

    private void AddFaq()
    {
        NavigationManager.NavigateTo($"/faq/create-new-faq/{_selectedCardModel.Name}");
    }

    private MarkupString GetSelectedCardNotes()
    {
        return (MarkupString)_selectedCardModel.Name.ToString().GetComponentNotesText();
    }
}
