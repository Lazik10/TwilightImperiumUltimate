using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglTopOpponentsDesktopTable
{
    [Parameter]
    public IReadOnlyList<TiglTopOpponentDto> Opponents { get; set; } = [];

    [Parameter]
    public EventCallback<int> OnPlayerSelected { get; set; }
}
