namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionSubmenu
{
    [Parameter]
    public FactionInfoType SelectedType { get; set; }

    [Parameter]
    public EventCallback<FactionInfoType> OnFactionInfoTypeChange { get; set; }

    private string GetTabCssClass(FactionInfoType factionInfoType) =>
        factionInfoType == SelectedType ? "faction-info-tab-active" : string.Empty;

    private void OnInfoTypeClick(FactionInfoType factionInfoType) => OnFactionInfoTypeChange.InvokeAsync(factionInfoType);
}
