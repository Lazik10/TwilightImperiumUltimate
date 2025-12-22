using TwilightImperiumUltimate.Contracts.ApiContracts.Faqs;

namespace TwilightImperiumUltimate.Web.Pages.Rules;

public partial class CreateNewFaq
{
    private bool _submitSuccess;

    [Parameter]
    public string ComponentName { get; set; } = string.Empty;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    private FaqModel FaqModel { get; set; } = new FaqModel()
    {
        ComponentName = string.Empty,
        QuestionEnglish = "Q: ",
        AnswerEnglish = "A: ",
        QuestionCzech = "Q: ",
        AnswerCzech = "A: ",
        FaqStatus = FaqStatus.Submitted,
    };

    private async Task Submit()
    {
        FaqModel.ComponentName = ComponentName;
        var faqDto = Mapper.Map<FaqDto>(FaqModel);
        var result = await HttpClient.PutAsync<InsertFaqRequest, FaqDto>(Paths.ApiPath_Faq, new InsertFaqRequest(faqDto), default);
        var statusCode = result.StatusCode;

        if (statusCode == HttpStatusCode.OK)
        {
            _submitSuccess = true;
            FaqModel = new FaqModel()
            {
                ComponentName = string.Empty,
                QuestionEnglish = "Q: ",
                AnswerEnglish = "A: ",
                QuestionCzech = "Q: ",
                AnswerCzech = "A: ",
                FaqStatus = FaqStatus.Submitted,
            };
        }
    }
}