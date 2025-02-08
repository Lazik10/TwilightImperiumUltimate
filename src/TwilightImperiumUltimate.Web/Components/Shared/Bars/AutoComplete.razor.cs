namespace TwilightImperiumUltimate.Web.Components.Shared.Bars;

public partial class AutoComplete
{
    [Parameter]
    public IEnumerable<string>? Data { get; set; }

    [Parameter]
    public string Placeholder { get; set; } = "Type to search...";

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public string Value { get; set; } = string.Empty;

    private IEnumerable<string>? Suggestions { get; set; }

    private void OnInput(ChangeEventArgs e)
    {
        Value = e.Value?.ToString() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(Value) && Data != null)
        {
            Suggestions = Data.Where(d => d.Contains(Value, StringComparison.OrdinalIgnoreCase)).Take(10);
        }
        else
        {
            Suggestions = Enumerable.Empty<string>();
        }
    }

    private async Task SelectSuggestion(string suggestion)
    {
        Value = suggestion;
        Suggestions = Enumerable.Empty<string>();
        await ValueChanged.InvokeAsync(suggestion);
    }

}