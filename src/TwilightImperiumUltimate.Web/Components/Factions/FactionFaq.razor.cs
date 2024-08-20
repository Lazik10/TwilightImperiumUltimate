namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionFaq : FactionInfoComponentBase
{
    private bool _showFaq;

    private bool _showNotes = true;

    private MarkupString FactionNotes => (MarkupString)GetFactionNotes();

    private List<FaqModel> Faqs { get; set; } = new List<FaqModel>();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<FaqDto>>>(Paths.ApiPath_Faq, default);

        if (statusCode == HttpStatusCode.OK)
        {
            var faqs = Mapper.Map<List<FaqModel>>(response!.Data!.Items);
            Faqs = faqs.Where(f => f.ComponentName == Faction.FactionName.ToString() && f.FaqStatus == FaqStatus.Approved)
                .ToList();
        }
    }

    private string GetFactionNotes()
    {
        return Faction.FactionName.GetFactionUIText(FactionResourceType.Notes);
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

    private void AddFaq()
    {
        NavigationManager.NavigateTo($"/faq/create-new-faq/{Faction.FactionName}");
    }
}
