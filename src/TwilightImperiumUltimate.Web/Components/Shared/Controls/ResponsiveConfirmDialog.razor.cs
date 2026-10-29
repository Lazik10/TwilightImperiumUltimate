using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

/// <summary>
/// Small reusable confirmation popup (e.g. "Delete this article?") rendered as a native modal
/// alert dialog. The caller owns the open/closed state and both the confirm and cancel actions.
/// </summary>
/// <remarks>
/// The browser manages modal focus containment and restores focus to the element that opened the
/// dialog. Escape invokes <see cref="OnCancel"/> without allowing the browser to close the dialog
/// outside the caller-owned state.
/// </remarks>
public partial class ResponsiveConfirmDialog : IAsyncDisposable
{
    private readonly string _titleId = $"confirm-dialog-title-{Guid.NewGuid():N}";
    private readonly string _messageId = $"confirm-dialog-message-{Guid.NewGuid():N}";
    private ElementReference _dialogRef;
    private IJSObjectReference? _jsModule;
    private bool _wasOpen;

    [Parameter]
    [EditorRequired]
    public bool IsOpen { get; set; }

    [Parameter]
    [EditorRequired]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public string Message { get; set; } = string.Empty;

    [Parameter]
    public string ConfirmText { get; set; } = string.Empty;

    [Parameter]
    public string CancelText { get; set; } = string.Empty;

    [Parameter]
    public EventCallback OnConfirm { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            if (_wasOpen)
            {
                await _jsModule.InvokeVoidAsync("closeModal", _dialogRef);
            }

            await _jsModule.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsOpen != _wasOpen)
        {
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./Components/Shared/Controls/ResponsiveConfirmDialog.razor.js");

            if (IsOpen)
            {
                await _jsModule.InvokeVoidAsync("showModal", _dialogRef);
            }
            else
            {
                await _jsModule.InvokeVoidAsync("closeModal", _dialogRef);
            }
        }

        _wasOpen = IsOpen;
    }

    private async Task HandleConfirmAsync()
    {
        if (OnConfirm.HasDelegate)
            await OnConfirm.InvokeAsync();
    }

    private async Task HandleCancelAsync()
    {
        if (OnCancel.HasDelegate)
            await OnCancel.InvokeAsync();
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs e)
    {
        if (e.Key == "Escape")
            await HandleCancelAsync();
    }
}
