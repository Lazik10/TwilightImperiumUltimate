using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class TechnologyNavItem : TwilightImperiumBaseComponenet
{
    [Parameter]
    public TechnologyType TechnologyType { get; set; } = TechnologyType.None;

    [Parameter]
    public string ColorClass { get; set; } = string.Empty;

    [Parameter]
    public EventCallback OnClick { get; set; }

    private string TechnologyName => TechnologyType.GetDisplayName();

    private string ImagePath => PathProvider.GetTechnologyIconPath(TechnologyType);
}