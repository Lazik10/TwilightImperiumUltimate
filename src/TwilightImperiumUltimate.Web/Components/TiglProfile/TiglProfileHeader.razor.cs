using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglProfileHeader
{
    [Parameter, EditorRequired]
    public TiglPlayerProfileDto Profile { get; set; } = default!;

}
