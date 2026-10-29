using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Pages.Tools;

public partial class DraftRooms
{
    private DraftRoomsLandingMode _landingMode;
    private bool _shouldFocusSelectedForm;
    private CreateDraftRoomForm _createRoomForm = default!;
    private JoinDraftRoomForm _joinRoomForm = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_shouldFocusSelectedForm)
        {
            return;
        }

        _shouldFocusSelectedForm = false;
        if (_landingMode == DraftRoomsLandingMode.Create)
        {
            await _createRoomForm.FocusFirstFieldAsync();
        }
        else if (_landingMode == DraftRoomsLandingMode.Join)
        {
            await _joinRoomForm.FocusFirstFieldAsync();
        }
    }

    private void SelectCreate()
    {
        _landingMode = DraftRoomsLandingMode.Create;
        _shouldFocusSelectedForm = true;
    }

    private void SelectJoin()
    {
        _landingMode = DraftRoomsLandingMode.Join;
        _shouldFocusSelectedForm = true;
    }

    private string GetPressedState(DraftRoomsLandingMode mode) =>
        _landingMode == mode ? "true" : "false";
}
