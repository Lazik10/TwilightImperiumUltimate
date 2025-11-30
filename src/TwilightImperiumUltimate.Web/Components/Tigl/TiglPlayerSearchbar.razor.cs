namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglPlayerSearchbar
{
    [Parameter]
    public EventCallback<string> OnSearchUpdate { get; set; }

    private void UpdatePlayerList(string searchText) => OnSearchUpdate.InvokeAsync(searchText);
}
