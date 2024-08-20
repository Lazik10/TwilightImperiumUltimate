using TwilightImperiumUltimate.Contracts.ApiContracts.Faqs;

namespace TwilightImperiumUltimate.Web.Pages.Rules;

public partial class Faq
{
    private List<FaqModel> Faqs { get; set; } = new List<FaqModel>();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<FaqDto>>>(Paths.ApiPath_Faq, default);

        if (statusCode == HttpStatusCode.OK)
        {
            var faqs = Mapper.Map<List<FaqModel>>(response!.Data!.Items);
            Faqs = faqs.Where(f => f.FaqStatus == FaqStatus.Submitted).ToList();
        }
    }

    private async Task ApproveFaq(FaqModel faq)
    {
        faq.FaqStatus = FaqStatus.Approved;
        var apporveFaqRequest = new UpdateFaqRequest(Mapper.Map<FaqDto>(faq));
        await HttpClient.PutAsync<UpdateFaqRequest, ApiResponse<FaqDto>>(Paths.ApiPath_Faq, apporveFaqRequest, default);
        Faqs = Faqs.Where(x => x.FaqStatus == FaqStatus.Submitted).ToList();
        StateHasChanged();
    }

    private async Task RejectFaq(FaqModel faq)
    {
        faq.FaqStatus = FaqStatus.Rejected;
        var rejectFaqRequest = new UpdateFaqRequest(Mapper.Map<FaqDto>(faq));
        await HttpClient.PutAsync<UpdateFaqRequest, ApiResponse<FaqDto>>(Paths.ApiPath_Faq, rejectFaqRequest, default);
        Faqs = Faqs.Where(x => x.FaqStatus == FaqStatus.Submitted).ToList();
        StateHasChanged();
    }

    private Task EditFaq(int id)
    {
        NavigationManager.NavigateTo($"/faq/edit-faq/{id}");
        return Task.CompletedTask;
    }
}