namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftNavBar
{
    [Parameter]
    public EventCallback<MiltyDraftMenuItem> OnMenuItemClick { get; set; }
}