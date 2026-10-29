using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class UpdateAvailable : IAsyncDisposable
{
    private bool _newVersionAvailable;
    private DotNetObjectReference<UpdateAvailable>? _dotNetReference;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [JSInvokable(nameof(OnUpdateAvailable))]
    public Task OnUpdateAvailable()
    {
        _newVersionAvailable = true;

        StateHasChanged();

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        _dotNetReference?.Dispose();
        _dotNetReference = null;
        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }

    protected override async Task OnInitializedAsync()
    {
        await RegisterForUpdateAvailableNotification();
    }

    private async Task RegisterForUpdateAvailableNotification()
    {
        _dotNetReference ??= DotNetObjectReference.Create(this);

        await JSRuntime.InvokeAsync<object>(
            identifier: "registerForUpdateAvailableNotification",
            _dotNetReference,
            nameof(OnUpdateAvailable));
    }

    private async Task ReloadPage(MouseEventArgs args)
    {
        await JSRuntime.InvokeVoidAsync("reloadForUpdate");
    }
}
