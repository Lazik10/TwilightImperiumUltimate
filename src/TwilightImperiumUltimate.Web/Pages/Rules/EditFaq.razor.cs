using TwilightImperiumUltimate.Contracts.ApiContracts.Faqs;

namespace TwilightImperiumUltimate.Web.Pages.Rules;

public partial class EditFaq
{
    [Parameter]
    public int Id { get; set; }

    private FaqModel FaqModel { get; set; } = new FaqModel();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var result = await HttpClient.GetAsync<ApiResponse<FaqDto>>($"{Paths.ApiPath_Faq}/{Id}");
        var response = result.Response;
        var statusCode = result.StatusCode;

        if (statusCode == HttpStatusCode.OK)
        {
            FaqModel = Mapper.Map<FaqModel>(response!.Data!);
        }
    }

    private async Task Save()
    {
        var faqDto = Mapper.Map<FaqDto>(FaqModel);
        var result = await HttpClient.PutAsync<UpdateFaqRequest, ApiResponse<FaqDto>>(Paths.ApiPath_Faq, new UpdateFaqRequest(faqDto), default);
        var statusCode = result.StatusCode;

        if (statusCode == HttpStatusCode.OK)
        {
            NavigationManager.NavigateTo(Pages.Faq);
        }
    }
}