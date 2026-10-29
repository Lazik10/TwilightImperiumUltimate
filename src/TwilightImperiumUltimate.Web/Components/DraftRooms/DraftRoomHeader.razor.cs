using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.Shared.Controls;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using PageRoutes = TwilightImperiumUltimate.Web.Pages.Pages;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftRoomHeader : IDisposable
{
    private ResponsiveButton _leaveButton = default!;
    private ResponsiveDangerButton _closeButton = default!;
    private bool _isLeaveConfirmationOpen;
    private bool _isCloseConfirmationOpen;
    private bool _isProcessing;
    private bool _showActionError;
    private bool _disposed;

    [Parameter]
    public bool CanLinkAccount { get; set; }

    [Parameter]
    public bool IsLinkingAccount { get; set; }

    [Parameter]
    public EventCallback OnLinkAccount { get; set; }

    [CascadingParameter]
    private IDraftRoomSession DraftRoomSession { get; set; } = default!;

    [Inject]
    private IDraftRoomApiService DraftRoomApiService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private DraftRoomViewDto? Room => DraftRoomSession.View;

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            DraftRoomSession.Changed -= this.HandleSessionChanged;
            DraftRoomSession.ConnectionStateChanged -= this.HandleConnectionStateChanged;
        }

        _disposed = true;
    }

    protected override void OnInitialized()
    {
        DraftRoomSession.Changed += this.HandleSessionChanged;
        DraftRoomSession.ConnectionStateChanged += this.HandleConnectionStateChanged;
    }

    private void HandleSessionChanged()
    {
        _ = this.InvokeAsync(this.StateHasChanged);
    }

    private void HandleConnectionStateChanged(DraftRoomConnectionState state)
    {
        _ = this.InvokeAsync(this.StateHasChanged);
    }

    private string GetRoleLabel(DraftRoomRole role) => role switch
    {
        DraftRoomRole.Host => Strings.DraftRooms_RoleHost,
        DraftRoomRole.CoHost => Strings.DraftRooms_RoleCoHost,
        _ => Strings.DraftRooms_RoleMember,
    };

    private string GetRoomUrl(DraftRoomViewDto room)
    {
        var relativeUrl = $"{PageRoutes.DraftRoomPrefix}{Uri.EscapeDataString(room.Code)}";
        return NavigationManager.ToAbsoluteUri(relativeUrl).ToString();
    }

    private async Task RefreshAsync()
    {
        _showActionError = false;
        try
        {
            await DraftRoomSession.RefreshAsync();
        }
        catch (JSException)
        {
            _showActionError = true;
        }
    }

    private void RequestLeave()
    {
        _isLeaveConfirmationOpen = true;
    }

    private async Task ConfirmLeaveAsync()
    {
        if (Room is not { } room)
        {
            _isLeaveConfirmationOpen = false;
            return;
        }

        var result = await ExecuteCommandAsync(() => DraftRoomApiService.LeaveAsync(room.Code));
        _isLeaveConfirmationOpen = false;
        if (result.IsSuccess)
        {
            NavigationManager.NavigateTo(PageRoutes.DraftRooms);
            return;
        }

        await _leaveButton.FocusAsync();
    }

    private async Task CancelLeaveAsync()
    {
        _isLeaveConfirmationOpen = false;
        await _leaveButton.FocusAsync();
    }

    private void RequestClose()
    {
        _isCloseConfirmationOpen = true;
    }

    private async Task ConfirmCloseAsync()
    {
        if (Room is not { } room)
        {
            _isCloseConfirmationOpen = false;
            return;
        }

        var result = await ExecuteCommandAsync(() => DraftRoomApiService.CloseAsync(room.Code));
        _isCloseConfirmationOpen = false;
        if (!result.IsSuccess)
        {
            await _closeButton.FocusAsync();
        }
    }

    private async Task CancelCloseAsync()
    {
        _isCloseConfirmationOpen = false;
        await _closeButton.FocusAsync();
    }

    private async Task<DraftRoomApiCommandResult> ExecuteCommandAsync(
        Func<Task<DraftRoomApiCommandResult>> command)
    {
        _isProcessing = true;
        _showActionError = false;
        try
        {
            var result = await DraftRoomSession.ExecuteAsync(command);
            _showActionError = !result.IsSuccess;
            return result;
        }
        catch (JSException)
        {
            _showActionError = true;
            return new DraftRoomApiCommandResult(DraftRoomApiResultKind.Failed);
        }
        finally
        {
            _isProcessing = false;
        }
    }
}
