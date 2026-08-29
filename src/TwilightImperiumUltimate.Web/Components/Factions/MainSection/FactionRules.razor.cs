namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionRules : FactionInfoComponentBase
{
    private MarkupString FactionNotes => (MarkupString)GetFactionNotes();

    private List<FaqModel> Faqs { get; set; } = [];

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<FaqDto>>>(Paths.ApiPath_Faq);
        var response = result.Response;
        var statusCode = result.StatusCode;

        if (statusCode == HttpStatusCode.OK)
        {
            var faqs = Mapper.Map<List<FaqModel>>(response!.Data!.Items);
            Faqs = faqs.Where(f => f.ComponentName == FactionName.ToString() && f.FaqStatus == FaqStatus.Approved)
                .ToList();
        }
    }

    private string GetFactionNotes()
    {
        return FactionName.GetFactionUIText(FactionResourceType.Notes);
    }

    private void AddFaq()
    {
        NavigationManager.NavigateTo($"/faq/create-new-faq/{FactionName}");
    }
}
