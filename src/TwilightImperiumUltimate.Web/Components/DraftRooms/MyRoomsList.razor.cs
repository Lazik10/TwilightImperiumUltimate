using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using PageRoutes = TwilightImperiumUltimate.Web.Pages.Pages;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class MyRoomsList : IDisposable
{
    private IReadOnlyList<MyDraftRoomDto> _accountRooms = [];
    private IReadOnlyList<GuestIdentity> _guestRooms = [];
    private bool _isLoading = true;
    private string? _loadErrorMessage;
    private string? _actionErrorMessage;
    private string? _roomNeedingFallback;
    private int _loadVersion;
    private bool _disposed;

    [Inject]
    private ICurrentUserState CurrentUserState { get; set; } = default!;

    [Inject]
    private IDraftRoomApiService DraftRoomApiService { get; set; } = default!;

    [Inject]
    private IGuestIdentityStore GuestIdentityStore { get; set; } = default!;

    [Inject]
    private IRoomTabOpener RoomTabOpener { get; set; } = default!;

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
            CurrentUserState.UserChanged -= HandleUserChanged;
        }

        _disposed = true;
    }

    protected override async Task OnInitializedAsync()
    {
        await CurrentUserState.InitializeAsync();
        CurrentUserState.UserChanged += HandleUserChanged;
        await LoadRoomsAsync();
    }

    private async Task LoadRoomsAsync()
    {
        var loadVersion = ++_loadVersion;
        var isAccount = CurrentUserState.User is not null;
        _isLoading = true;
        _loadErrorMessage = null;
        _actionErrorMessage = null;
        _roomNeedingFallback = null;

        try
        {
            if (isAccount)
            {
                var result = await DraftRoomApiService.GetMyRoomsAsync();
                if (loadVersion != _loadVersion)
                {
                    return;
                }

                if (!result.IsSuccess || result.Data is null)
                {
                    _loadErrorMessage = Strings.DraftRooms_MyRoomsLoadFailed;
                    return;
                }

                _accountRooms = result.Data;
                _guestRooms = [];
                return;
            }

            var guestRooms = await GuestIdentityStore.ListAsync();
            if (loadVersion != _loadVersion)
            {
                return;
            }

            _guestRooms = guestRooms;
            _accountRooms = [];
        }
        catch (JSException)
        {
            if (loadVersion == _loadVersion)
            {
                _loadErrorMessage = Strings.DraftRooms_MyRoomsLoadFailed;
            }
        }
        finally
        {
            if (loadVersion == _loadVersion)
            {
                _isLoading = false;
            }
        }
    }

    private async Task OpenRoomAsync(string code)
    {
        _roomNeedingFallback = null;
        var roomUrl = BuildRoomUrl(code);
        try
        {
            if (!await RoomTabOpener.OpenAsync(roomUrl))
            {
                _roomNeedingFallback = code;
            }
        }
        catch (JSException)
        {
            _roomNeedingFallback = code;
        }
    }

    private async Task ForgetRoomAsync(string code)
    {
        _actionErrorMessage = null;
        try
        {
            await GuestIdentityStore.RemoveAsync(code);
            _guestRooms = _guestRooms
                .Where(room => !string.Equals(room.Code, code, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (string.Equals(_roomNeedingFallback, code, StringComparison.OrdinalIgnoreCase))
            {
                _roomNeedingFallback = null;
            }
        }
        catch (JSException)
        {
            _actionErrorMessage = Strings.DraftRooms_MyRoomsForgetFailed;
        }
    }

    private void HandleUserChanged()
    {
        _ = InvokeAsync(async () =>
        {
            await LoadRoomsAsync();
            if (!_disposed)
            {
                StateHasChanged();
            }
        });
    }

    private string BuildRoomUrl(string code) =>
        $"{PageRoutes.DraftRoomPrefix}{Uri.EscapeDataString(code)}";

    private string Format(string format, object value) =>
        string.Format(CultureInfo.CurrentCulture, format, value);
}
