namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleSearchbar
{
    [Parameter]
    public EventCallback<string> OnSearchChange { get; set; }

    private string SearchTerm { get; set; } = string.Empty;

    private string SearchPlaceholder { get; set; } = "Search...";

    private async Task SearchRules(string? text)
    {
        await OnSearchChange.InvokeAsync(text ?? string.Empty);
    }
}
