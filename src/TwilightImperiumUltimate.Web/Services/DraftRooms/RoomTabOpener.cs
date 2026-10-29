using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Uses an isolated JavaScript module to open draft rooms in a separate browser tab.
/// </summary>
public sealed class RoomTabOpener(IJSRuntime jsRuntime) : IRoomTabOpener, IAsyncDisposable
{
    private const string ModulePath = "./js/draft-room-tab.js";

    private IJSObjectReference? _module;

    public async Task<bool> OpenAsync(string url)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        _module ??= await jsRuntime
            .InvokeAsync<IJSObjectReference>("import", ModulePath)
            .ConfigureAwait(false);

        return await _module
            .InvokeAsync<bool>("openDraftRoom", url)
            .ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync().ConfigureAwait(false);
            _module = null;
        }
    }
}
