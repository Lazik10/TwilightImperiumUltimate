namespace TwilightImperiumUltimate.Web.Components.Shared.Bars;

public partial class SearchBar
{
    [Parameter]
    public EventCallback<string> OnSearchChange { get; set; }

    [Parameter]
    public string Text { get; set; } = Strings.SearchForKeyword;

    [Parameter]
    public int Width { get; set; } = 100;

    private string SearchTerm { get; set; } = string.Empty;

    private string SearchPlaceholder { get; set; } = "Search...";

    private async Task SearchKeyword(string? text)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            return;

        await OnSearchChange.InvokeAsync(text);
    }
}
