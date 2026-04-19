using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class UpdateAvailable
{
    private bool _newVersionAvailable;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [JSInvokable(nameof(OnUpdateAvailable))]
    public Task OnUpdateAvailable()
    {
        _newVersionAvailable = true;

        StateHasChanged();

        return Task.CompletedTask;
    }

    protected override async Task OnInitializedAsync()
    {
        await RegisterForUpdateAvailableNotification();
    }

    private async Task RegisterForUpdateAvailableNotification()
    {
        await JSRuntime.InvokeAsync<object>(
            identifier: "registerForUpdateAvailableNotification",
            DotNetObjectReference.Create(this),
            nameof(OnUpdateAvailable));
    }

    private async Task ReloadPage(MouseEventArgs _)
    {
        await JSRuntime.InvokeVoidAsync("window.location.reload");
    }
}