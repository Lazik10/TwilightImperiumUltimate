namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleSearchBar
{
    [Parameter]
    public EventCallback<string> OnSearchChange { get; set; }

    private string SearchTerm { get; set; } = string.Empty;

    private string SearchPlaceholder { get; set; } = "Search...";

    private async Task SearchRules(string? text)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            return;

        await OnSearchChange.InvokeAsync(text);
    }
}
