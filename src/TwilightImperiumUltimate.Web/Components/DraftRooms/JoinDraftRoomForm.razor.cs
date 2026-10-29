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

public partial class JoinDraftRoomForm : IDisposable
{
    private readonly JoinDraftRoomFormModel _model = new();
    private bool _disposed;
    private bool _hasStoredIdentity;
    private bool _isGuest;
    private bool _isSubmitting;
    private bool _roomJoined;
    private bool _showOpenRoomLink;
    private string _checkedCode = string.Empty;
    private string _errorMessage = string.Empty;
    private string _roomUrl = string.Empty;
    private ResponsiveTextBox _roomCodeInput = default!;

    /// <summary>
    /// Gets or sets a room code supplied by the room page that cannot be edited in this form.
    /// </summary>
    [Parameter]
    public string ForcedCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether a successful join opens the room in a new browser tab.
    /// </summary>
    [Parameter]
    public bool OpenInNewTab { get; set; } = true;

    /// <summary>
    /// Gets or sets the callback invoked after joining when this form is embedded in the room page.
    /// </summary>
    [Parameter]
    public EventCallback<DraftRoomEntryDto> OnJoined { get; set; }

    [Inject]
    private ICurrentUserState CurrentUserState { get; set; } = default!;

    [Inject]
    private IDraftRoomApiService DraftRoomApiService { get; set; } = default!;

    [Inject]
    private IGuestIdentityStore GuestIdentityStore { get; set; } = default!;

    [Inject]
    private IRoomTabOpener RoomTabOpener { get; set; } = default!;

    private bool IsCodeForced => !string.IsNullOrWhiteSpace(ForcedCode);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    internal Task FocusFirstFieldAsync() =>
        _roomCodeInput.FocusAsync();

    protected override async Task OnInitializedAsync()
    {
        await CurrentUserState.InitializeAsync();
        _isGuest = CurrentUserState.User is null;
        UpdateCredentialRequirements();
        CurrentUserState.UserChanged += HandleUserChanged;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (IsCodeForced)
        {
            _model.Code = ForcedCode;
            await RefreshStoredIdentityAsync();
        }
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

    private async Task HandleCodeInputAsync(string value)
    {
        _model.Code = value;
        await RefreshStoredIdentityAsync();
    }

    private async Task JoinRoomAsync()
    {
        if (_isSubmitting)
        {
            return;
        }

        _isSubmitting = true;
        _roomJoined = false;
        _showOpenRoomLink = false;
        _errorMessage = string.Empty;

        try
        {
            var result = await DraftRoomApiService.JoinAsync(
                _model.Code,
                _model.RequiresCredentials ? _model.Password : null,
                _model.RequiresGuestDisplayName ? _model.DisplayName : null);
            if (!result.IsSuccess || result.Data is null)
            {
                await HandleJoinFailureAsync(result.Kind);
                return;
            }

            var room = result.Data;
            if (room.RoomId == Guid.Empty
                || room.ParticipantId == Guid.Empty
                || string.IsNullOrWhiteSpace(room.Code))
            {
                _errorMessage = Strings.DraftRooms_JoinFailed;
                return;
            }

            if (room.IsGuest)
            {
                if (!string.IsNullOrWhiteSpace(room.GuestSecret))
                {
                    try
                    {
                        await GuestIdentityStore.SaveAsync(new GuestIdentity(
                            room.RoomId,
                            room.Code.Trim().ToUpperInvariant(),
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
                        _errorMessage = Strings.DraftRooms_JoinIdentityStorageFailed;
                        return;
                    }
                }
                else if (!_hasStoredIdentity)
                {
                    _errorMessage = Strings.DraftRooms_JoinIdentityUnavailable;
                    return;
                }
            }

            _roomJoined = true;
            if (!OpenInNewTab)
            {
                await OnJoined.InvokeAsync(room);
                return;
            }

            _roomUrl = $"{PageRoutes.DraftRoomPrefix}{Uri.EscapeDataString(room.Code.Trim().ToUpperInvariant())}";
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

    private async Task HandleJoinFailureAsync(DraftRoomApiResultKind resultKind)
    {
        _errorMessage = resultKind switch
        {
            DraftRoomApiResultKind.NotFoundOrWrongPassword => Strings.DraftRooms_JoinNotFoundOrWrongPassword,
            DraftRoomApiResultKind.Throttled => Strings.DraftRooms_JoinThrottled,
            DraftRoomApiResultKind.ValidationFailed => Strings.DraftRooms_JoinValidationFailed,
            _ => Strings.DraftRooms_JoinFailed,
        };

        if (resultKind == DraftRoomApiResultKind.NotFoundOrWrongPassword && _hasStoredIdentity)
        {
            try
            {
                await GuestIdentityStore.RemoveAsync(_model.Code);
            }
            catch (JSException)
            {
                _errorMessage = Strings.DraftRooms_JoinFailed;
                return;
            }

            _hasStoredIdentity = false;
            UpdateCredentialRequirements();
        }
    }

    private async Task RefreshStoredIdentityAsync()
    {
        var code = _model.Code;
        if (code == _checkedCode)
        {
            return;
        }

        _checkedCode = code;
        _hasStoredIdentity = code.Length == DraftRoomLimits.CodeLength
            && code.All(DraftRoomLimits.CodeAlphabet.Contains)
            && await GuestIdentityStore.GetAsync(code) is not null;

        UpdateCredentialRequirements();
    }

    private void UpdateCredentialRequirements()
    {
        _model.RequiresCredentials = !_isGuest || !_hasStoredIdentity;
        _model.RequiresGuestDisplayName = _isGuest && !_hasStoredIdentity;
    }

    private void HandleUserChanged()
    {
        _ = InvokeAsync(() =>
        {
            _isGuest = CurrentUserState.User is null;
            UpdateCredentialRequirements();
            StateHasChanged();
        });
    }
}
