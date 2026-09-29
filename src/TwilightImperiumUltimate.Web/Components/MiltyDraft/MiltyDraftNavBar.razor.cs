namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftNavBar
{
    [Parameter]
    public MiltyDraftMenuItem SelectedMenuItem { get; set; }

    [Parameter]
    public EventCallback<MiltyDraftMenuItem> OnMenuItemClick { get; set; }
}
