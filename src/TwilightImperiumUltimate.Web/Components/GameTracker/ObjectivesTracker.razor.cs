using TwilightImperiumUltimate.Contracts.DTOs.Card;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class ObjectivesTracker
{
    private List<ObjectiveCardName> _stageOneObjectiveCards = new();

    private List<ObjectiveCardName> _stageTwoObjectiveCards = new();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ItemListDto<ObjectiveCardDto>>(Paths.ApiPath_ObjectiveCards);
        if (statusCode == HttpStatusCode.OK)
        {
            _stageOneObjectiveCards = response.Items
                .Where(x => x.ObjectiveCardType == ObjectiveCardType.StageOne)
                .Select(x => x.ObjectiveCardName)
                .ToList();

            _stageTwoObjectiveCards = response.Items
                .Where(x => x.ObjectiveCardType == ObjectiveCardType.StageTwo)
                .Select(x => x.ObjectiveCardName)
                .ToList();
        }
    }
}