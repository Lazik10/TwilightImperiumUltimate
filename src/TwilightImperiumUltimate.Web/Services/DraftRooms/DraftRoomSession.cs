using System.Diagnostics.CodeAnalysis;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Holds the current view and request state for one open draft-room page.
/// </summary>
public sealed class DraftRoomSession : IDraftRoomSession
{
    [SuppressMessage(
        "Reliability",
        "CA2213:Disposable fields should be disposed",
        Justification = "Disposing the semaphore during an in-flight refresh can make its finally block throw on Release.")]
    private readonly SemaphoreSlim _refreshGate = new(1, 1);
    private readonly IDraftRoomApiService _draftRoomApiService;
    private readonly IDraftRoomConnection _draftRoomConnection;
    private readonly IGuestIdentityStore _guestIdentityStore;
    private string? _code;
    private int _latestObservedVersion = -1;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="DraftRoomSession"/> class.
    /// </summary>
    /// <param name="draftRoomApiService">The room API service.</param>
    /// <param name="draftRoomConnection">The live SignalR connection.</param>
    /// <param name="guestIdentityStore">The browser-held guest identity store.</param>
    public DraftRoomSession(
        IDraftRoomApiService draftRoomApiService,
        IDraftRoomConnection draftRoomConnection,
        IGuestIdentityStore guestIdentityStore)
    {
        this._draftRoomApiService = draftRoomApiService
            ?? throw new ArgumentNullException(nameof(draftRoomApiService));
        this._draftRoomConnection = draftRoomConnection
            ?? throw new ArgumentNullException(nameof(draftRoomConnection));
        this._guestIdentityStore = guestIdentityStore
            ?? throw new ArgumentNullException(nameof(guestIdentityStore));

        this._draftRoomConnection.RoomChanged += this.HandleRoomChangedAsync;
        this._draftRoomConnection.PresenceChanged += this.HandlePresenceChangedAsync;
        this._draftRoomConnection.ChatMessageReceived += this.HandleChatMessageReceivedAsync;
        this._draftRoomConnection.LogEntryAdded += this.HandleLogEntryAddedAsync;
        this._draftRoomConnection.Reconnected += this.HandleReconnectedAsync;
        this._draftRoomConnection.Closed += this.HandleRoomClosedAsync;
        this._draftRoomConnection.RemovedFromRoom += this.HandleRemovedFromRoomAsync;
        this._draftRoomConnection.StateChanged += this.HandleConnectionStateChanged;
    }

    /// <inheritdoc/>
    public event Action? Changed;

    /// <inheritdoc/>
    public event Action<DraftRoomConnectionState>? ConnectionStateChanged;

    /// <inheritdoc/>
    public event Action<DraftRoomPresenceUpdateDto>? PresenceChanged;

    /// <inheritdoc />
    public event Func<ChatMessageDto, Task>? ChatMessageReceived;

    /// <inheritdoc />
    public event Func<DraftLogEntryDto, Task>? LogEntryAdded;

    /// <inheritdoc/>
    public DraftRoomViewDto? View { get; private set; }

    /// <inheritdoc/>
    public bool IsLoading { get; private set; }

    /// <inheritdoc/>
    public DraftRoomApiResultKind? Error { get; private set; }

    /// <inheritdoc/>
    public DraftRoomConnectionState ConnectionState => this._draftRoomConnection.State;

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (this._disposed)
        {
            return;
        }

        this._disposed = true;
        this._draftRoomConnection.RoomChanged -= this.HandleRoomChangedAsync;
        this._draftRoomConnection.PresenceChanged -= this.HandlePresenceChangedAsync;
        this._draftRoomConnection.ChatMessageReceived -= this.HandleChatMessageReceivedAsync;
        this._draftRoomConnection.LogEntryAdded -= this.HandleLogEntryAddedAsync;
        this._draftRoomConnection.Reconnected -= this.HandleReconnectedAsync;
        this._draftRoomConnection.Closed -= this.HandleRoomClosedAsync;
        this._draftRoomConnection.RemovedFromRoom -= this.HandleRemovedFromRoomAsync;
        this._draftRoomConnection.StateChanged -= this.HandleConnectionStateChanged;
        await this._draftRoomConnection.DisposeAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Initialize(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        this._code = code.Trim().ToUpperInvariant();
        this.View = null;
        this.Error = null;
        Interlocked.Exchange(ref this._latestObservedVersion, -1);
    }

    /// <inheritdoc/>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        var code = this._code
            ?? throw new InvalidOperationException("The draft-room session has not been initialized.");
        var identity = await this._guestIdentityStore.GetAsync(code, cancellationToken).ConfigureAwait(false);
        var credentials = identity is null
            ? new DraftRoomCredentials()
            : new DraftRoomCredentials
            {
                GuestParticipantId = identity.ParticipantId,
                GuestSecret = identity.Secret,
            };

        var subscribedVersion = await this._draftRoomConnection
            .ConnectAsync(code, credentials, cancellationToken)
            .ConfigureAwait(false);
        this.ObserveVersion(subscribedVersion);
        await this.RefreshAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        var code = this._code
            ?? throw new InvalidOperationException("The draft-room session has not been initialized.");

        await this._refreshGate.WaitAsync(cancellationToken).ConfigureAwait(false);
        this.IsLoading = true;
        this.Error = null;

        try
        {
            var result = await this._draftRoomApiService
                .GetViewAsync(code, cancellationToken)
                .ConfigureAwait(false);
            this.View = result.IsSuccess ? result.Data : null;
            this.Error = result.IsSuccess ? null : result.Kind;
            if (this.View is not null)
            {
                this.ObserveVersion(this.View.Version);
            }
        }
        finally
        {
            this.IsLoading = false;
            this._refreshGate.Release();
            this.Changed?.Invoke();
        }
    }

    /// <inheritdoc />
    public async Task<DraftRoomApiResult<IReadOnlyList<ChatMessageDto>>> GetChatHistoryAsync(
        long? afterId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var code = this._code
            ?? throw new InvalidOperationException("The draft-room session has not been initialized.");

        return await this._draftRoomApiService
            .GetChatHistoryAsync(code, afterId, limit, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>> GetLogHistoryAsync(
        long? afterId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var code = this._code
            ?? throw new InvalidOperationException("The draft-room session has not been initialized.");

        return await this._draftRoomApiService
            .GetLogHistoryAsync(code, afterId, limit, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<DraftRoomApiResult<ChatMessageDto>> SendChatAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        var code = this._code
            ?? throw new InvalidOperationException("The draft-room session has not been initialized.");

        return await this._draftRoomApiService
            .SendChatAsync(code, text, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<DraftRoomApiCommandResult> ExecuteAsync(
        Func<Task<DraftRoomApiCommandResult>> command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = await command().ConfigureAwait(false);
        await this.RefreshAsync(cancellationToken).ConfigureAwait(false);

        return result;
    }

    private async Task HandleRoomChangedAsync(int version)
    {
        if (!this.TryObserveVersion(version))
        {
            return;
        }

        await this.RefreshAsync().ConfigureAwait(false);
    }

    private async Task HandlePresenceChangedAsync(DraftRoomPresenceUpdateDto presenceUpdate)
    {
        ArgumentNullException.ThrowIfNull(presenceUpdate);

        await this._refreshGate.WaitAsync().ConfigureAwait(false);
        var viewChanged = false;
        try
        {
            var participant = this.View?.Participants
                .FirstOrDefault(item => item.Id == presenceUpdate.ParticipantId);
            if (participant is not null && participant.IsOnline != presenceUpdate.IsOnline)
            {
                participant.IsOnline = presenceUpdate.IsOnline;
                viewChanged = true;
            }
        }
        finally
        {
            this._refreshGate.Release();
        }

        this.PresenceChanged?.Invoke(presenceUpdate);
        if (viewChanged)
        {
            this.Changed?.Invoke();
        }
    }

    private async Task HandleChatMessageReceivedAsync(ChatMessageDto message)
    {
        var handlers = this.ChatMessageReceived;
        if (handlers is null)
        {
            return;
        }

        foreach (var handler in handlers.GetInvocationList().Cast<Func<ChatMessageDto, Task>>())
        {
            await handler(message).ConfigureAwait(false);
        }
    }

    private async Task HandleLogEntryAddedAsync(DraftLogEntryDto entry)
    {
        var handlers = this.LogEntryAdded;
        if (handlers is null)
        {
            return;
        }

        foreach (var handler in handlers.GetInvocationList().Cast<Func<DraftLogEntryDto, Task>>())
        {
            await handler(entry).ConfigureAwait(false);
        }
    }

    private async Task HandleReconnectedAsync()
    {
        await this.RefreshAsync().ConfigureAwait(false);
    }

    private async Task HandleRoomClosedAsync()
    {
        await this.RefreshAsync().ConfigureAwait(false);
    }

    private async Task HandleRemovedFromRoomAsync()
    {
        await this.RefreshAsync().ConfigureAwait(false);
    }

    private void HandleConnectionStateChanged(DraftRoomConnectionState state)
    {
        this.ConnectionStateChanged?.Invoke(state);
    }

    private bool TryObserveVersion(int version)
    {
        while (true)
        {
            var currentVersion = Volatile.Read(ref this._latestObservedVersion);
            if (version <= currentVersion)
            {
                return false;
            }

            if (Interlocked.CompareExchange(ref this._latestObservedVersion, version, currentVersion) == currentVersion)
            {
                return true;
            }
        }
    }

    private void ObserveVersion(int version)
    {
        _ = this.TryObserveVersion(version);
    }
}
