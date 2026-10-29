using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using PageRoutes = TwilightImperiumUltimate.Web.Pages.Pages;

namespace TwilightImperiumUltimate.Web.Pages.Tools;

public partial class DraftRoom : IAsyncDisposable
{
    private bool _isLoading = true;
    private bool _roomNotFound;
    private bool _roomRemoved;
    private bool _roomClosed;
    private bool _showJoinForm;
    private bool _showLoadError;
    private bool _isSessionSubscribed;
    private bool _shouldFocusRoomShell;
    private bool _isAccountLinkDialogOpen;
    private bool _isLinkingAccount;
    private bool _accountLinkPromptDismissed;
    private bool _disposed;
    private int _unreadChatCount;
    private int _unreadLogCount;
    private ElementReference _roomShellElement;
    private DraftRoomEntryDto? _room;
    private GuestIdentity? _guestIdentity;
    private string? _accountLinkError;
    private DraftRoomMobileTab _activeMobileTab;

    private enum DraftRoomMobileTab
    {
        Seats,
        People,
        Settings,
        Chat,
        Log,
    }

    [Parameter]
    public string Code { get; set; } = string.Empty;

    [Inject]
    private ICurrentUserState CurrentUserState { get; set; } = default!;

    [Inject]
    private IDraftRoomApiService DraftRoomApiService { get; set; } = default!;

    [Inject]
    private IGuestIdentityStore GuestIdentityStore { get; set; } = default!;

    [Inject]
    private IDraftRoomSession DraftRoomSession { get; set; } = default!;

    private string NormalizedCode => Code.Trim().ToUpperInvariant();

    private bool HasActiveRoom => !_isLoading && !_roomNotFound && !_roomRemoved && !_roomClosed && !_showLoadError && _room is not null;

    private bool CanLinkAccount =>
        _room is { IsGuest: true }
        && CurrentUserState.User is not null
        && _guestIdentity is not null;

    private bool ShowGuestAccountBanner =>
        _room is { IsGuest: true }
        && CurrentUserState.User is null;

    private string LoginUrl =>
        $"{PageRoutes.Login}?returnUrl={Uri.EscapeDataString(PageRoutes.DraftRoomPrefix + NormalizedCode)}";

    private string RegisterUrl => PageRoutes.Register;

    private bool CanChangeRoomSettings => DraftRoomSession.View?.Me.CanChangeSettings == true;

    private string RoomDocumentTitle
    {
        get
        {
            if (_roomRemoved)
            {
                return Strings.DraftRooms_RoomRemovedHeading;
            }

            if (_roomClosed)
            {
                return Strings.DraftRooms_RoomClosedHeading;
            }

            return _room?.RoomName ?? Strings.DraftRooms_RoomPageTitle;
        }
    }

    private string ParticipantDescription
    {
        get
        {
            if (_room is null)
            {
                return string.Empty;
            }

            var roomView = DraftRoomSession.View;
            var participant = roomView is null
                ? null
                : roomView.Participants.FirstOrDefault(
                    item => item.Id == roomView.Me.ParticipantId);
            var displayName = participant?.DisplayName;
            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = _room.DisplayName;
            }

            return string.Format(
                CultureInfo.CurrentCulture,
                Strings.DraftRooms_YouAre,
                displayName,
                GetRoleLabel(_room.Role));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        this.UnsubscribeFromSessionChanges();
        await DraftRoomSession.DisposeAsync().ConfigureAwait(false);
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    protected override async Task OnParametersSetAsync()
    {
        await LoadRoomAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_shouldFocusRoomShell)
        {
            return;
        }

        _shouldFocusRoomShell = false;
        await _roomShellElement.FocusAsync();
    }

    private static string GetString(string resourceName)
    {
        return Strings.ResourceManager.GetString(resourceName, CultureInfo.CurrentUICulture)
            ?? throw new InvalidOperationException($"Missing draft-room resource '{resourceName}'.");
    }

    private static string GetUnreadCountLabel(string resourceName, int count)
    {
        return string.Format(CultureInfo.CurrentCulture, GetString(resourceName), count);
    }

    private static int IncrementUnreadCount(int count) => count == int.MaxValue ? count : count + 1;

    private string GetMobileExpanded(DraftRoomMobileTab tab) => IsMobileTabActive(tab) ? "true" : "false";

    private async Task LoadRoomAsync()
    {
        this.UnsubscribeFromSessionChanges();
        _isLoading = true;
        _roomNotFound = false;
        _roomRemoved = false;
        _roomClosed = false;
        _showJoinForm = false;
        _showLoadError = false;
        _shouldFocusRoomShell = false;
        _isAccountLinkDialogOpen = false;
        _isLinkingAccount = false;
        _accountLinkPromptDismissed = false;
        _accountLinkError = null;
        _guestIdentity = null;
        _activeMobileTab = DraftRoomMobileTab.Seats;
        _unreadChatCount = 0;
        _unreadLogCount = 0;
        _room = null;

        var code = NormalizedCode;
        if (!IsValidRoomCode(code))
        {
            _roomNotFound = true;
            _isLoading = false;
            _shouldFocusRoomShell = true;
            return;
        }

        try
        {
            await CurrentUserState.InitializeAsync();
            var storedIdentity = await GuestIdentityStore.GetAsync(code);
            _guestIdentity = storedIdentity;

            if (CurrentUserState.User is null && storedIdentity is null)
            {
                _showJoinForm = true;
                return;
            }

            var result = await DraftRoomApiService.JoinAsync(code, null, null);
            if (result.IsSuccess && result.Data is not null && IsValidRoomEntry(result.Data))
            {
                _room = result.Data;
                await this.InitializeRoomSessionAsync(code);
                return;
            }

            if (result.Kind == DraftRoomApiResultKind.Forbidden)
            {
                _roomRemoved = true;
                return;
            }

            if (result.Kind == DraftRoomApiResultKind.NotFoundOrWrongPassword)
            {
                if (storedIdentity is not null)
                {
                    await GuestIdentityStore.RemoveAsync(code);
                }

                _showJoinForm = true;
                return;
            }

            _showLoadError = true;
        }
        catch (JSException)
        {
            _showLoadError = true;
        }
        finally
        {
            _isLoading = false;
            _shouldFocusRoomShell = _roomNotFound || _roomRemoved || _roomClosed || _showLoadError;
        }
    }

    private async Task HandleJoinedAsync(DraftRoomEntryDto room)
    {
        if (!IsValidRoomEntry(room))
        {
            _showLoadError = true;
            return;
        }

        _room = room;
        _showJoinForm = false;
        _showLoadError = false;
        _guestIdentity = null;
        await this.InitializeRoomSessionAsync(NormalizedCode);
        _shouldFocusRoomShell = !_roomRemoved && !_roomClosed && !_showLoadError;
    }

    private void OpenAccountLinkDialog()
    {
        if (!CanLinkAccount || _isLinkingAccount)
        {
            return;
        }

        _accountLinkError = null;
        _isAccountLinkDialogOpen = true;
    }

    private Task CancelAccountLinkAsync()
    {
        _isAccountLinkDialogOpen = false;
        _accountLinkPromptDismissed = true;
        return Task.CompletedTask;
    }

    private async Task ConfirmAccountLinkAsync()
    {
        if (!CanLinkAccount || _isLinkingAccount)
        {
            _isAccountLinkDialogOpen = false;
            return;
        }

        _isLinkingAccount = true;
        _isAccountLinkDialogOpen = false;
        _accountLinkError = null;
        try
        {
            var result = await DraftRoomApiService.LinkAccountAsync(NormalizedCode);
            if (!result.IsSuccess)
            {
                _accountLinkError = result.Kind == DraftRoomApiResultKind.Conflict
                    ? Strings.DraftRooms_LinkAccountConflict
                    : Strings.DraftRooms_LinkAccountFailed;
                return;
            }

            _guestIdentity = null;
            await DraftRoomSession.RefreshAsync();
            this.ApplyRoomSessionState();
        }
        catch (Exception exception) when (
            exception is JSException or HttpRequestException)
        {
            _isAccountLinkDialogOpen = false;
            _accountLinkError = _guestIdentity is null
                ? Strings.DraftRooms_LinkAccountRefreshFailed
                : Strings.DraftRooms_LinkAccountFailed;
        }
        finally
        {
            _isLinkingAccount = false;
        }
    }

    private async Task InitializeRoomSessionAsync(string code)
    {
        _isLoading = true;
        try
        {
            DraftRoomSession.Changed += this.HandleDraftRoomSessionChanged;
            DraftRoomSession.ChatMessageReceived += this.HandleChatMessageReceivedAsync;
            DraftRoomSession.LogEntryAdded += this.HandleLogEntryAddedAsync;
            _isSessionSubscribed = true;
            DraftRoomSession.Initialize(code);
            await DraftRoomSession.ConnectAsync();
            this.ApplyRoomSessionState();
            if (!_showLoadError && CanLinkAccount && !_accountLinkPromptDismissed)
            {
                _isAccountLinkDialogOpen = true;
            }
        }
        catch (Exception exception) when (
            exception is HubException
                or HttpRequestException
                or IOException
                or InvalidOperationException
                or JSException
                or OperationCanceledException
                or TimeoutException)
        {
            _showLoadError = true;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void HandleDraftRoomSessionChanged()
    {
        _ = this.InvokeAsync(() =>
        {
            if (!CanChangeRoomSettings && _activeMobileTab == DraftRoomMobileTab.Settings)
            {
                _activeMobileTab = DraftRoomMobileTab.Seats;
            }

            this.ApplyRoomSessionState();
            if (_roomRemoved || _roomClosed || _showLoadError)
            {
                _shouldFocusRoomShell = true;
            }

            this.StateHasChanged();
        });
    }

    private void ApplyRoomSessionState()
    {
        _roomRemoved = DraftRoomSession.Error == DraftRoomApiResultKind.Forbidden;
        _roomClosed = DraftRoomSession.Error == DraftRoomApiResultKind.NotFound;
        _showLoadError = !_roomRemoved
            && !_roomClosed
            && (DraftRoomSession.Error is not null || DraftRoomSession.View is null);
    }

    private void UnsubscribeFromSessionChanges()
    {
        if (!_isSessionSubscribed)
        {
            return;
        }

        DraftRoomSession.Changed -= this.HandleDraftRoomSessionChanged;
        DraftRoomSession.ChatMessageReceived -= this.HandleChatMessageReceivedAsync;
        DraftRoomSession.LogEntryAdded -= this.HandleLogEntryAddedAsync;
        _isSessionSubscribed = false;
    }

    private bool IsValidRoomCode(string code) =>
        code.Length == DraftRoomLimits.CodeLength
        && code.All(DraftRoomLimits.CodeAlphabet.Contains);

    private bool IsValidRoomEntry(DraftRoomEntryDto room) =>
        room.RoomId != Guid.Empty
        && room.ParticipantId != Guid.Empty
        && !string.IsNullOrWhiteSpace(room.Code)
        && !string.IsNullOrWhiteSpace(room.RoomName);

    private string GetRoleLabel(DraftRoomRole role) => role switch
    {
        DraftRoomRole.Host => Strings.DraftRooms_RoleHost,
        DraftRoomRole.CoHost => Strings.DraftRooms_RoleCoHost,
        _ => Strings.DraftRooms_RoleMember,
    };

    private bool IsMobileTabActive(DraftRoomMobileTab tab) => _activeMobileTab == tab;

    private void SelectMobileTab(DraftRoomMobileTab tab)
    {
        _activeMobileTab = tab;
        if (tab == DraftRoomMobileTab.Chat)
        {
            _unreadChatCount = 0;
        }
        else if (tab == DraftRoomMobileTab.Log)
        {
            _unreadLogCount = 0;
        }
    }

    private string GetMobilePanelState(DraftRoomMobileTab tab) =>
        this.IsMobileTabActive(tab) ? "draft-room-lobby__panel--active" : string.Empty;

    private Task HandleChatMessageReceivedAsync(ChatMessageDto message)
    {
        if (this._disposed
            || this._activeMobileTab == DraftRoomMobileTab.Chat
            || message.ParticipantId == this._room?.ParticipantId)
        {
            return Task.CompletedTask;
        }

        this._unreadChatCount = IncrementUnreadCount(this._unreadChatCount);
        return this.InvokeAsync(this.StateHasChanged);
    }

    private Task HandleLogEntryAddedAsync(DraftLogEntryDto entry)
    {
        if (this._disposed
            || this._activeMobileTab == DraftRoomMobileTab.Log
            || entry.ActorParticipantId == this._room?.ParticipantId)
        {
            return Task.CompletedTask;
        }

        this._unreadLogCount = IncrementUnreadCount(this._unreadLogCount);
        return this.InvokeAsync(this.StateHasChanged);
    }
}
