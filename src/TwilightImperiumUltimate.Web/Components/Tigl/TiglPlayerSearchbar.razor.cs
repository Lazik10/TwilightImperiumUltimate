namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglPlayerSearchbar
{
    [Parameter]
    public EventCallback<string> OnSearchUpdate { get; set; }

    private string SearchText { get; set; } = string.Empty;

    public Task ResetSearchAsync()
    {
        SearchText = string.Empty;
        return InvokeAsync(StateHasChanged);
    }

    private async Task UpdatePlayerList(string searchText)
    {
        var wasActiveSearch = SearchText.Length >= 3;
        SearchText = searchText;

        var isActiveSearch = searchText.Length >= 3;
        if (!wasActiveSearch && !isActiveSearch)
            return;

        await OnSearchUpdate.InvokeAsync(searchText);
    }
}
