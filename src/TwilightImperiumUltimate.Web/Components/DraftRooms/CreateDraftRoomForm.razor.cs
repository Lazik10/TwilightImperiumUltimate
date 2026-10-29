using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Components.Shared.Controls;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using PageRoutes = TwilightImperiumUltimate.Web.Pages.Pages;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class CreateDraftRoomForm : IDisposable
{
    private readonly CreateDraftRoomFormModel _model = new();
    private bool _disposed;
    private bool _isSubmitting;
    private bool _roomCreated;
    private bool _showOpenRoomLink;
    private string _errorMessage = string.Empty;
    private string _roomUrl = string.Empty;
    private ResponsiveTextBox _roomNameInput = default!;

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
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    internal Task FocusFirstFieldAsync() => _roomNameInput.FocusAsync();

    protected override async Task OnInitializedAsync()
    {
        await CurrentUserState.InitializeAsync();
        _model.IsGuest = CurrentUserState.User is null;
        CurrentUserState.UserChanged += HandleUserChanged;
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

    private async Task CreateRoomAsync()
    {
        if (_isSubmitting)
        {
            return;
        }

        _isSubmitting = true;
        _roomCreated = false;
        _showOpenRoomLink = false;
        _errorMessage = string.Empty;

        try
        {
            _model.IsGuest = CurrentUserState.User is null;
            var result = await DraftRoomApiService.CreateAsync(_model);
            if (!result.IsSuccess || result.Data is null)
            {
                _errorMessage = result.Kind switch
                {
                    DraftRoomApiResultKind.ValidationFailed => Strings.DraftRooms_CreateValidationFailed,
                    DraftRoomApiResultKind.Throttled => Strings.DraftRooms_CreateThrottled,
                    _ => Strings.DraftRooms_CreateFailed,
                };

                return;
            }

            var room = result.Data;
            if (room.RoomId == Guid.Empty
                || room.ParticipantId == Guid.Empty
                || string.IsNullOrWhiteSpace(room.Code))
            {
                _errorMessage = Strings.DraftRooms_CreateFailed;
                return;
            }

            var code = room.Code.Trim().ToUpperInvariant();
            if (room.IsGuest)
            {
                if (string.IsNullOrWhiteSpace(room.GuestSecret))
                {
                    _errorMessage = Strings.DraftRooms_CreateIdentityUnavailable;
                    return;
                }

                try
                {
                    await GuestIdentityStore.SaveAsync(new GuestIdentity(
                        room.RoomId,
                        code,
                        room.ParticipantId,
                        room.GuestSecret,
                        room.DisplayName,
                        DateTimeOffset.UtcNow)
                    {
                        RoomName = room.RoomName,
                    });
                }
                catch (JSException)
                {
                    _errorMessage = Strings.DraftRooms_CreateIdentityStorageFailed;
                    return;
                }
            }

            _roomUrl = $"{PageRoutes.DraftRoomPrefix}{Uri.EscapeDataString(code)}";
            _roomCreated = true;
            try
            {
                _showOpenRoomLink = !await RoomTabOpener.OpenAsync(_roomUrl);
            }
            catch (JSException)
            {
                _showOpenRoomLink = true;
            }
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private void HandleUserChanged()
    {
        _ = InvokeAsync(() =>
        {
            _model.IsGuest = CurrentUserState.User is null;
            StateHasChanged();
        });
    }
}
