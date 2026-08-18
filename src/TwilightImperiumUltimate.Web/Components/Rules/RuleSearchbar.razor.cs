namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleSearchbar : IDisposable
{
    private const int DebounceMilliseconds = 250;
    private readonly string InputId = $"rules-search-{Guid.NewGuid():N}";
    private CancellationTokenSource? _debounceCancellation;
    private bool _disposed;

    [Parameter]
    public EventCallback<string> OnSearchChange { get; set; }

    [Parameter]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public string Label { get; set; } = "Search";

    [Parameter]
    public string Placeholder { get; set; } = "Search...";

    [Parameter]
    public int? ResultCount { get; set; }

    private string SearchTerm { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {
        if (!string.Equals(SearchTerm, Value, StringComparison.Ordinal))
        {
            CancelDebounce();
            SearchTerm = Value;
        }
    }

    private async Task OnInput(ChangeEventArgs args)
    {
        SearchTerm = args.Value?.ToString() ?? string.Empty;
        CancelDebounce();

        var cancellation = new CancellationTokenSource();
        _debounceCancellation = cancellation;

        try
        {
            await Task.Delay(DebounceMilliseconds, cancellation.Token);
            if (!_disposed)
                await OnSearchChange.InvokeAsync(SearchTerm);
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
        }
        finally
        {
            if (ReferenceEquals(_debounceCancellation, cancellation))
                _debounceCancellation = null;

            cancellation.Dispose();
        }
    }

    private async Task ClearSearch()
    {
        CancelDebounce();
        SearchTerm = string.Empty;
        await OnSearchChange.InvokeAsync(string.Empty);
    }

    private async Task OnKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs args)
    {
        if (args.Key == "Escape" && !string.IsNullOrEmpty(SearchTerm))
            await ClearSearch();
    }

    private void CancelDebounce()
    {
        _debounceCancellation?.Cancel();
        _debounceCancellation = null;
    }

    public void Dispose()
    {
        _disposed = true;
        CancelDebounce();
    }
}
