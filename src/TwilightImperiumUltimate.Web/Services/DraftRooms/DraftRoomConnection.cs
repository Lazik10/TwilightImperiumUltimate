using Blazored.LocalStorage;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Options.Api;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Implements the live SignalR connection for a draft room.
/// </summary>
public sealed class DraftRoomConnection(
    IOptions<TwilightImperiumApiOptions> apiOptions,
    ILocalStorageService localStorage,
    ILogger<DraftRoomConnection> logger)
    : IDraftRoomConnection
{
    private static readonly TimeSpan[] ReconnectDelays =
    [
        TimeSpan.Zero,
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(10),
        TimeSpan.FromSeconds(30),
    ];

    private readonly IOptions<TwilightImperiumApiOptions> _apiOptions = apiOptions
        ?? throw new ArgumentNullException(nameof(apiOptions));

    private readonly ILocalStorageService _localStorage = localStorage
        ?? throw new ArgumentNullException(nameof(localStorage));

    private readonly ILogger<DraftRoomConnection> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    private HubConnection? _hubConnection;
    private string? _code;
    private DraftRoomCredentials? _credentials;
    private DraftRoomConnectionState _state = DraftRoomConnectionState.Offline;
    private bool _disposed;

    /// <inheritdoc />
    public event Func<int, Task>? RoomChanged;

    /// <inheritdoc />
    public event Func<DraftRoomPresenceUpdateDto, Task>? PresenceChanged;

    /// <inheritdoc />
    public event Func<ChatMessageDto, Task>? ChatMessageReceived;

    /// <inheritdoc />
    public event Func<DraftLogEntryDto, Task>? LogEntryAdded;

    /// <inheritdoc />
    public event Func<Task>? Reconnected;

    /// <inheritdoc />
    public event Func<Task>? Closed;

    /// <inheritdoc />
    public event Func<Task>? RemovedFromRoom;

    /// <inheritdoc />
    public event Action<DraftRoomConnectionState>? StateChanged;

    /// <inheritdoc />
    public DraftRoomConnectionState State => this._state;

    /// <inheritdoc />
    public async Task<int> ConnectAsync(
        string code,
        DraftRoomCredentials credentials,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentNullException.ThrowIfNull(credentials);
        ObjectDisposedException.ThrowIf(this._disposed, this);

        await this.DisposeConnectionAsync().ConfigureAwait(false);
        this._code = code.Trim().ToUpperInvariant();
        this._credentials = credentials;

        var connection = new HubConnectionBuilder()
            .WithUrl(
                new Uri(this._apiOptions.Value.BaseUrl, "/hubs/draft-rooms"),
                options => options.AccessTokenProvider = this.GetAccessTokenAsync)
            .WithAutomaticReconnect(ReconnectDelays)
            .Build();
        this._hubConnection = connection;
        this.RegisterHandlers(connection);
        this.SetState(DraftRoomConnectionState.Reconnecting);

        try
        {
            await connection.StartAsync(cancellationToken).ConfigureAwait(false);
            var version = await connection
                .InvokeAsync<int>("Subscribe", this._code, this._credentials, cancellationToken)
                .ConfigureAwait(false);
            this.SetState(DraftRoomConnectionState.Connected);

            return version;
        }
        catch
        {
            await this.DisposeConnectionAsync().ConfigureAwait(false);
            this.SetState(DraftRoomConnectionState.Offline);
            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (this._disposed)
        {
            return;
        }

        this._disposed = true;
        await this.DisposeConnectionAsync().ConfigureAwait(false);
        this.SetState(DraftRoomConnectionState.Offline);
    }

    private static bool IsConnectionException(Exception exception)
    {
        return exception is HubException
            or HttpRequestException
            or IOException
            or InvalidOperationException
            or JSException
            or OperationCanceledException
            or TimeoutException;
    }

    private void RegisterHandlers(HubConnection connection)
    {
        connection.On<int>("RoomChanged", this.HandleRoomChangedAsync);
        connection.On<DraftRoomPresenceUpdateDto>("PresenceChanged", this.HandlePresenceChangedAsync);
        connection.On<ChatMessageDto>("ChatMessageReceived", this.HandleChatMessageReceivedAsync);
        connection.On<DraftLogEntryDto>("LogEntryAdded", this.HandleLogEntryAddedAsync);
        connection.On("RoomClosed", this.HandleClosedAsync);
        connection.On("RemovedFromRoom", this.HandleRemovedFromRoomAsync);
        connection.Reconnecting += this.HandleReconnectingAsync;
        connection.Reconnected += this.HandleReconnectedAsync;
        connection.Closed += this.HandleConnectionClosedAsync;
    }

    private async Task HandleRoomChangedAsync(int version)
    {
        if (this.RoomChanged is not null)
        {
            await this.RoomChanged.Invoke(version).ConfigureAwait(false);
        }
    }

    private async Task HandlePresenceChangedAsync(DraftRoomPresenceUpdateDto presenceUpdate)
    {
        if (this.PresenceChanged is not null)
        {
            await this.PresenceChanged.Invoke(presenceUpdate).ConfigureAwait(false);
        }
    }

    private async Task HandleChatMessageReceivedAsync(ChatMessageDto message)
    {
        if (this.ChatMessageReceived is not null)
        {
            await this.ChatMessageReceived.Invoke(message).ConfigureAwait(false);
        }
    }

    private async Task HandleLogEntryAddedAsync(DraftLogEntryDto entry)
    {
        if (this.LogEntryAdded is not null)
        {
            await this.LogEntryAdded.Invoke(entry).ConfigureAwait(false);
        }
    }

    private async Task HandleClosedAsync()
    {
        if (this.Closed is not null)
        {
            await this.Closed.Invoke().ConfigureAwait(false);
        }
    }

    private async Task HandleRemovedFromRoomAsync()
    {
        if (this.RemovedFromRoom is not null)
        {
            await this.RemovedFromRoom.Invoke().ConfigureAwait(false);
        }
    }

    private Task HandleReconnectingAsync(Exception? exception)
    {
        this.SetState(DraftRoomConnectionState.Reconnecting);
        if (exception is not null)
        {
            this._logger.LogWarning(exception, "Draft room SignalR connection is reconnecting.");
        }

        return Task.CompletedTask;
    }

    private async Task HandleReconnectedAsync(string? connectionId)
    {
        if (this._hubConnection is null || this._code is null || this._credentials is null)
        {
            return;
        }

        try
        {
            await this._hubConnection
                .InvokeAsync<int>("Subscribe", this._code, this._credentials)
                .ConfigureAwait(false);
            this.SetState(DraftRoomConnectionState.Connected);
        }
        catch (Exception exception) when (IsConnectionException(exception))
        {
            this._logger.LogWarning(exception, "Draft room SignalR subscription failed after reconnect.");
            this.SetState(DraftRoomConnectionState.Offline);
        }

        if (this.Reconnected is not null)
        {
            await this.Reconnected.Invoke().ConfigureAwait(false);
        }
    }

    private Task HandleConnectionClosedAsync(Exception? exception)
    {
        this.SetState(DraftRoomConnectionState.Offline);
        if (exception is not null)
        {
            this._logger.LogWarning(exception, "Draft room SignalR connection closed.");
        }

        return Task.CompletedTask;
    }

    private void SetState(DraftRoomConnectionState state)
    {
        if (this._state == state)
        {
            return;
        }

        this._state = state;
        this.StateChanged?.Invoke(state);
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        var loginResponse = await this._localStorage
            .GetItemAsync<LoginResponse>("authentication")
            .ConfigureAwait(false);
        return string.IsNullOrWhiteSpace(loginResponse?.AccessToken)
            ? null
            : loginResponse.AccessToken;
    }

    private async Task DisposeConnectionAsync()
    {
        var connection = this._hubConnection;
        this._hubConnection = null;
        if (connection is not null)
        {
            await connection.DisposeAsync().ConfigureAwait(false);
        }
    }
}
