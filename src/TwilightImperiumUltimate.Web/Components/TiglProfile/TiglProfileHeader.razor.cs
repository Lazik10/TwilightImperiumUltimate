using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglProfileHeader
{
    [Parameter, EditorRequired]
    public TiglPlayerProfileDto Profile { get; set; } = default!;

    [Parameter]
    public TiglProfileCategory CurrentCategory { get; set; }

    [Parameter]
    public EventCallback<TiglProfileCategory> CurrentCategoryChanged { get; set; }

    private static IReadOnlyList<KeyValuePair<TiglProfileCategory, string>> Options =>
    [
        new(TiglProfileCategory.ThundersEdge, "Standard"),
        new(TiglProfileCategory.Fractured, "Fractured"),
        new(TiglProfileCategory.ProphecyOfKings, "Legacy"),
    ];
}
