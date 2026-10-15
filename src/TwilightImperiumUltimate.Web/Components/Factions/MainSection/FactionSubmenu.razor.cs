namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionSubmenu
{
    [Parameter]
    public FactionInfoType SelectedType { get; set; }

    [Parameter]
    public EventCallback<FactionInfoType> OnFactionInfoTypeChange { get; set; }

    private void OnInfoTypeClick(FactionInfoType factionInfoType) => OnFactionInfoTypeChange.InvokeAsync(factionInfoType);
}
