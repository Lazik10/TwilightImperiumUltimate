using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Components.Shared.Pagination;

/// <summary>
/// Invisible sentinel element that raises <see cref="OnIntersect"/> once when it scrolls into
/// the viewport, using the browser IntersectionObserver API. Used to implement "load more on
/// scroll" (infinite scroll) behavior without polling scroll events.
/// </summary>
public partial class InfiniteScrollSentinel : IAsyncDisposable
{
    private ElementReference _sentinelElement;
    private IJSObjectReference? _jsModule;
    private DotNetObjectReference<InfiniteScrollSentinel>? _dotNetRef;
    private bool _isBusy;
    private bool _disposed;

    /// <summary>
    /// Gets or sets the callback invoked when the sentinel becomes visible in the viewport.
    /// </summary>
    [Parameter]
    public EventCallback OnIntersect { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Invoked from JavaScript when the sentinel element intersects the viewport. Guards against
    /// overlapping invocations, since the underlying IntersectionObserver keeps observing (and can
    /// fire again) while a previous load is still in flight.
    /// </summary>
    [JSInvokable]
    public async Task HandleIntersectAsync()
    {
        if (_isBusy)
            return;

        _isBusy = true;

        try
        {
            await OnIntersect.InvokeAsync();
        }
        finally
        {
            _isBusy = false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("unobserve", _sentinelElement);
                await _jsModule.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Circuit already gone; nothing left to clean up.
            }
        }

        _dotNetRef?.Dispose();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./Components/Shared/Pagination/InfiniteScrollSentinel.razor.js");
        _dotNetRef = DotNetObjectReference.Create(this);

        await _jsModule.InvokeVoidAsync("observe", _sentinelElement, _dotNetRef);
    }
}
