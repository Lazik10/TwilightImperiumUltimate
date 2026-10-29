using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <inheritdoc />
public sealed class PageVisibility(IJSRuntime jsRuntime) : IPageVisibility
{
    private IJSObjectReference? _module;
    private IJSObjectReference? _observer;
    private DotNetObjectReference<PageVisibility>? _reference;
    private bool _disposed;

    /// <inheritdoc />
    public event Action? Changed;

    /// <inheritdoc />
    public bool IsHidden { get; private set; }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (_observer is not null)
        {
            return;
        }

        _module ??= await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/draft-room-visibility.js");
        _reference ??= DotNetObjectReference.Create(this);
        _observer = await _module.InvokeAsync<IJSObjectReference>("observe", _reference);
    }

    /// <summary>Receives only the document's hidden flag from the isolated browser module.</summary>
    [JSInvokable]
    public void SetHidden(bool hidden)
    {
        if (!_disposed && hidden != IsHidden)
        {
            IsHidden = hidden;
            Changed?.Invoke();
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        if (_observer is not null)
        {
            await _observer.InvokeVoidAsync("dispose");
            await _observer.DisposeAsync();
        }

        _reference?.Dispose();
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}
