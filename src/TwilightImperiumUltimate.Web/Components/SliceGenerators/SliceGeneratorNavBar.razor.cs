namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceGeneratorNavBar
{
    [Parameter]
    public EventCallback<SliceGeneratorMenuItem> OnMenuItemClick { get; set; }
}