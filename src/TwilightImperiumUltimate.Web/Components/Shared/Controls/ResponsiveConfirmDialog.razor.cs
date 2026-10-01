using Microsoft.AspNetCore.Components.Web;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

/// <summary>
/// Small reusable confirmation popup (e.g. "Delete this article?") rendered as an accessible
/// modal alert dialog. The caller owns the open/closed state and both the confirm and cancel
/// actions; this component only renders the dialog shell, moves focus into it when it opens, and
/// closes on Escape or backdrop click (invoking <see cref="OnCancel"/>).
/// </summary>
/// <remarks>
/// Focus is moved to the dialog itself on open. Returning focus to whatever element triggered
/// the dialog (e.g. a specific row's delete button) is the caller's responsibility, since this
/// component has no knowledge of which element opened it -- callers should capture an
/// <see cref="ElementReference"/> to their trigger and call <c>FocusAsync()</c> on it from their
/// own <see cref="OnConfirm"/>/<see cref="OnCancel"/> handlers after closing.
/// </remarks>
public partial class ResponsiveConfirmDialog
{
    private readonly string _titleId = $"confirm-dialog-title-{Guid.NewGuid():N}";
    private readonly string _messageId = $"confirm-dialog-message-{Guid.NewGuid():N}";
    private ElementReference _dialogRef;
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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsOpen && !_wasOpen)
        {
            await _dialogRef.FocusAsync();
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
