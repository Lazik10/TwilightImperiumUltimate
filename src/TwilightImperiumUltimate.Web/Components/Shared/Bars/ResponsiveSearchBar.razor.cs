namespace TwilightImperiumUltimate.Web.Components.Shared.Bars;

public partial class ResponsiveSearchBar
{
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> OnSearchChange { get; set; }

    [Parameter]
    public string Text { get; set; } = Strings.SearchForKeyword;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public string? AriaDescribedBy { get; set; }

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public bool HideText { get; set; }

    [Parameter]
    public bool EnableEmptySearch { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the font size token (xs/sm/base/md/lg/xl/2xl/3xl) applied to both the label
    /// (when shown) and the input, matching
    /// <see cref="TwilightImperiumUltimate.Web.Components.Shared.Controls.ResponsiveTextBox.FontSize"/>.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "md";

    private string SearchTerm { get; set; } = string.Empty;

    private string SearchPlaceholder { get; set; } = "Search...";

    public Task ResetSearchTerm()
    {
        SearchTerm = string.Empty;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task OnSearchTermChanged(string value)
    {
        SearchTerm = value;
        await SearchKeyword(value);
    }

    private async Task SearchKeyword(string? text)
    {
        if (!EnableEmptySearch && string.IsNullOrWhiteSpace(text))
            return;

        await OnSearchChange.InvokeAsync(text);
    }
}
