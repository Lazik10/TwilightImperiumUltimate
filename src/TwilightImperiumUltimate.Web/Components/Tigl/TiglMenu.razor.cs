namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglMenu
{
    [Parameter]
    public EventCallback<TiglMenuItem> OnMenuItemClick { get; set; }
}