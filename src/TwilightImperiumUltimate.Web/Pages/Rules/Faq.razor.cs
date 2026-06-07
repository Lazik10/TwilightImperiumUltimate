using TwilightImperiumUltimate.Contracts.ApiContracts.Faqs;
using TwilightImperiumUltimate.Web.Services.Rules;

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

    [Inject]
    private IApprovedFaqCache FaqCache { get; set; } = default!;

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
        await SetFaqStatus(faq, FaqStatus.Approved);
    }

    private async Task RejectFaq(FaqModel faq)
    {
        await SetFaqStatus(faq, FaqStatus.Rejected);
    }

    private async Task SetFaqStatus(FaqModel faq, FaqStatus status)
    {
        faq.FaqStatus = status;
        var request = new UpdateFaqRequest(Mapper.Map<FaqDto>(faq));
        var result = await HttpClient.PutAsync<UpdateFaqRequest, FaqDto>(
            Paths.ApiPath_Faq,
            request,
            default);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            faq.FaqStatus = FaqStatus.Submitted;
            return;
        }

        FaqCache.Invalidate();
        Faqs = Faqs.Where(x => x.FaqStatus == FaqStatus.Submitted).ToList();
    }

    private Task EditFaq(int id)
    {
        NavigationManager.NavigateTo($"/faq/edit-faq/{id}");
        return Task.CompletedTask;
    }
}
