namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionTemplateGrid
{
    private FactionInfoType _selectedInfoType;

    /// <summary>
    /// Gets or sets the info type resolved from the page's "info" route/query value. Applied to
    /// the internal selection whenever it changes (e.g. browser back/forward, faction navigation)
    /// without discarding in-component tab clicks that happen in between.
    /// </summary>
    [Parameter]
    public FactionInfoType InitialInfoType { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    public void Refresh()
    {
        StateHasChanged();
    }

    protected override void OnParametersSet()
    {
        _selectedInfoType = InitialInfoType;
    }

    private static string GetInfoQueryValue(FactionInfoType infoType) => infoType.ToString().ToLowerInvariant();

    private void OnInfoTypeChanged(FactionInfoType infoType)
    {
        _selectedInfoType = infoType;
        NavigationManager.NavigateTo(NavigationManager.GetUriWithQueryParameter("info", GetInfoQueryValue(infoType)));
    }
}
