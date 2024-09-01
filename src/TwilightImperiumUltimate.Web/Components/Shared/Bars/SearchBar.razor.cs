namespace TwilightImperiumUltimate.Web.Components.Shared.Bars;

public partial class SearchBar
{
    [Parameter]
    public EventCallback<string> OnSearchChange { get; set; }

    [Parameter]
    public string Text { get; set; } = Strings.SearchForKeyword;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public bool HideText { get; set; }

    [Parameter]
    public bool EnableEmptySearch { get; set; } = false;

    [Parameter]
    public int Height { get; set; } = 100;

    private string SearchTerm { get; set; } = string.Empty;

    private string SearchPlaceholder { get; set; } = "Search...";

    public Task ResetSearchTerm()
    {
        SearchTerm = string.Empty;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task SearchKeyword(string? text)
    {
        if (!EnableEmptySearch && (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)))
            return;

        await OnSearchChange.InvokeAsync(text);
    }

    private string GetHeight()
    {
        return $"width: 100%; height: {Height}% !important";
    }
}
