using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Components.DraftRooms.FactionPick;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using PageRoutes = TwilightImperiumUltimate.Web.Pages.Pages;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftRoomResultView
{
    private Dictionary<string, object> _parameters = [];

    [Parameter]
    [EditorRequired]
    public DraftResultDto Result { get; set; } = default!;

    [Inject]
    private IDraftTypeUiRegistry UiRegistry { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private string ShareUrl => Navigation.ToAbsoluteUri($"{PageRoutes.DraftRoomResultPrefix}{Uri.EscapeDataString(Result.ShareToken)}").AbsoluteUri;

    protected override void OnParametersSet() => _parameters = new()
    {
        [nameof(FactionPickResultView.Payload)] = Result.Payload,
    };

    private static string Text(string key) => DraftRoomText.Get(key);
}
