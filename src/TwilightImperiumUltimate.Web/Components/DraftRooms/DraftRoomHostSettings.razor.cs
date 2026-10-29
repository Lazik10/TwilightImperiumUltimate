using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Components.Shared.Controls;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftRoomHostSettings : IAsyncDisposable
{
    private UpdateDraftRoomNameFormModel _nameModel = new();
    private ChangeDraftRoomPasswordFormModel _passwordModel = new();
    private ElementReference _passwordDialogRef;
    private IJSObjectReference? _jsModule;
    private bool _isNameDirty;
    private bool _isPasswordDialogOpen;
    private bool _wasPasswordDialogOpen;
    private bool _isProcessing;
    private bool _showActionError;
    private bool _disposed;

    [CascadingParameter]
    private IDraftRoomSession DraftRoomSession { get; set; } = default!;

    [Inject]
    private IDraftRoomApiService DraftRoomApiService { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private DraftRoomViewDto? Room => DraftRoomSession.View;

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            DraftRoomSession.Changed -= this.HandleSessionChanged;
            _disposed = true;
        }

        if (_jsModule is not null)
        {
            if (_wasPasswordDialogOpen)
            {
                await _jsModule.InvokeVoidAsync("closeModal", _passwordDialogRef);
            }

            await _jsModule.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }

    protected override void OnInitialized()
    {
        DraftRoomSession.Changed += this.HandleSessionChanged;
        this.UpdateNameFromRoom();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isPasswordDialogOpen != _wasPasswordDialogOpen)
        {
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./Components/Shared/Controls/ResponsiveConfirmDialog.razor.js");

            if (_isPasswordDialogOpen)
            {
                await _jsModule.InvokeVoidAsync("showModal", _passwordDialogRef);
            }
            else
            {
                await _jsModule.InvokeVoidAsync("closeModal", _passwordDialogRef);
            }
        }

        _wasPasswordDialogOpen = _isPasswordDialogOpen;
    }

    private void HandleSessionChanged()
    {
        if (!_isNameDirty)
        {
            this.UpdateNameFromRoom();
        }

        _ = this.InvokeAsync(this.StateHasChanged);
    }

    private void UpdateNameFromRoom()
    {
        if (Room is { } room)
        {
            _nameModel.Name = room.Name;
        }
    }

    private void HandleRoomNameChanged()
    {
        _isNameDirty = Room is not { } room
            || !string.Equals(_nameModel.Name.Trim(), room.Name, StringComparison.Ordinal);
    }

    private async Task SaveRoomNameAsync()
    {
        if (Room is not { } room)
        {
            return;
        }

        var roomName = _nameModel.Name.Trim();
        if (await this.ExecuteCommandAsync(
            () => DraftRoomApiService.UpdateSettingsAsync(room.Code, roomName, room.IsLocked)))
        {
            _nameModel.Name = roomName;
            _isNameDirty = false;
        }
    }

    private async Task HandleLockChangedAsync(bool isLocked)
    {
        if (Room is not { } room)
        {
            return;
        }

        await this.ExecuteCommandAsync(
            () => DraftRoomApiService.UpdateSettingsAsync(room.Code, room.Name, isLocked));
    }

    private void OpenPasswordDialog()
    {
        _passwordModel = new ChangeDraftRoomPasswordFormModel();
        _showActionError = false;
        _isPasswordDialogOpen = true;
    }

    private async Task ChangePasswordAsync()
    {
        if (Room is not { } room)
        {
            return;
        }

        var passwordChanged = await this.ExecuteCommandAsync(
            () => DraftRoomApiService.ChangePasswordAsync(room.Code, _passwordModel.Password));
        if (passwordChanged)
        {
            await this.ClosePasswordDialogAsync();
        }
    }

    private Task CancelPasswordChangeAsync() => this.ClosePasswordDialogAsync();

    private Task HandlePasswordDialogKeyDownAsync(KeyboardEventArgs args)
    {
        if (args.Key == "Escape" && !_isProcessing)
        {
            return this.ClosePasswordDialogAsync();
        }

        return Task.CompletedTask;
    }

    private Task ClosePasswordDialogAsync()
    {
        _isPasswordDialogOpen = false;
        _passwordModel = new ChangeDraftRoomPasswordFormModel();
        _showActionError = false;
        return Task.CompletedTask;
    }

    private async Task<bool> ExecuteCommandAsync(Func<Task<DraftRoomApiCommandResult>> command)
    {
        _isProcessing = true;
        _showActionError = false;
        try
        {
            var result = await DraftRoomSession.ExecuteAsync(command);
            _showActionError = !result.IsSuccess;
            return result.IsSuccess;
        }
        catch (JSException)
        {
            _showActionError = true;
            return false;
        }
        finally
        {
            _isProcessing = false;
        }
    }
}
