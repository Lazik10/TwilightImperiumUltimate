using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapZoomViewport : IAsyncDisposable
{
    private ElementReference _viewport;
    private IJSObjectReference? _module;
    private IJSObjectReference? _zoomController;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_zoomController is not null)
            {
                await _zoomController.InvokeVoidAsync("dispose");
                await _zoomController.DisposeAsync();
            }

            if (_module is not null)
                await _module.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/MapGenerator/MapGeneratorMainGrid.razor.js");
        _zoomController = await _module.InvokeAsync<IJSObjectReference>("initializeMapZoom", _viewport);
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs eventArgs)
    {
        if (_module is null)
        {
            return;
        }

        switch (eventArgs.Key)
        {
            case "+" or "=":
                await _module.InvokeVoidAsync("adjustMapZoom", _viewport, 1.2);
                break;
            case "-":
                await _module.InvokeVoidAsync("adjustMapZoom", _viewport, 1 / 1.2);
                break;
            case "0":
                await _module.InvokeVoidAsync("resetMapZoom", _viewport);
                break;
        }
    }
}