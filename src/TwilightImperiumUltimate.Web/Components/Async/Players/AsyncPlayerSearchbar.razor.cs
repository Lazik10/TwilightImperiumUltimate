using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Players;

public partial class AsyncPlayerSearchbar
{
    [Parameter]
    public EventCallback<string> OnSearchUpdate { get; set; }

    private string SearchText { get; set; } = string.Empty;

    private void UpdatePlayerList(string searchText) => OnSearchUpdate.InvokeAsync(searchText);
}