namespace TwilightImperiumUltimate.Web.Components.TiglAdmin;

public partial class AdminMenu
{
    [Parameter]
    public EventCallback<AdminMenuItem> OnMenuItemClick { get; set; }
}
