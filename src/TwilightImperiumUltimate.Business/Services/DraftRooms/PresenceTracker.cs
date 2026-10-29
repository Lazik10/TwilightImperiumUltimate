using TwilightImperiumUltimate.Contracts.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Tracks room-scoped participant connections and delays offline transitions for brief reconnects.
/// </summary>
public sealed class PresenceTracker : IPresenceTracker, IDisposable
{
    private readonly object _sync = new();
    private readonly Dictionary<ParticipantKey, ParticipantPresence> _participants = [];
    private readonly Dictionary<string, ParticipantKey> _connectionKeys = new(StringComparer.Ordinal);
    private readonly TimeProvider _timeProvider;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PresenceTracker"/> class.
    /// </summary>
    /// <param name="timeProvider">The time source used for reconnect grace timers.</param>
    public PresenceTracker(TimeProvider timeProvider)
    {
        this._timeProvider = timeProvider
            ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    /// <inheritdoc />
    public event Action<DraftRoomPresenceChange>? ParticipantOnline;

    /// <inheritdoc />
    public event Action<DraftRoomPresenceChange>? ParticipantOffline;

    /// <inheritdoc />
    public void AddConnection(Guid roomId, Guid participantId, string connectionId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionId);

        if (roomId == Guid.Empty)
        {
            throw new ArgumentException("A room identifier is required.", nameof(roomId));
        }

        if (participantId == Guid.Empty)
        {
            throw new ArgumentException("A participant identifier is required.", nameof(participantId));
        }

        var key = new ParticipantKey(roomId, participantId);
        DraftRoomPresenceChange? onlineChange = null;

        lock (this._sync)
        {
            ObjectDisposedException.ThrowIf(this._disposed, this);

            if (this._connectionKeys.TryGetValue(connectionId, out var existingKey))
            {
                if (existingKey != key)
                {
                    throw new InvalidOperationException("A connection cannot represent multiple room participants.");
                }

                return;
            }

            if (!this._participants.TryGetValue(key, out var presence))
            {
                presence = new ParticipantPresence();
                this._participants.Add(key, presence);
                onlineChange = new DraftRoomPresenceChange(roomId, participantId);
            }
            else
            {
                this.CancelOfflineTimer(presence);
            }

            presence.ConnectionIds.Add(connectionId);
            this._connectionKeys.Add(connectionId, key);
        }

        if (onlineChange is not null)
        {
            this.ParticipantOnline?.Invoke(onlineChange);
        }
    }

    /// <inheritdoc />
    public void RemoveConnection(string connectionId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionId);

        lock (this._sync)
        {
            ObjectDisposedException.ThrowIf(this._disposed, this);

            if (!this._connectionKeys.Remove(connectionId, out var key)
                || !this._participants.TryGetValue(key, out var presence))
            {
                return;
            }

            presence.ConnectionIds.Remove(connectionId);
            if (presence.ConnectionIds.Count == 0)
            {
                this.ScheduleOffline(key, presence);
            }
        }
    }

    /// <inheritdoc />
    public bool IsOnline(Guid roomId, Guid participantId)
    {
        lock (this._sync)
        {
            ObjectDisposedException.ThrowIf(this._disposed, this);

            return this._participants.ContainsKey(new ParticipantKey(roomId, participantId));
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        lock (this._sync)
        {
            if (this._disposed)
            {
                return;
            }

            this._disposed = true;
            foreach (var presence in this._participants.Values)
            {
                this.CancelOfflineTimer(presence);
            }

            this._participants.Clear();
            this._connectionKeys.Clear();
        }
    }

    private void ScheduleOffline(ParticipantKey key, ParticipantPresence presence)
    {
        var generation = ++presence.OfflineGeneration;
        presence.OfflineTimer = this._timeProvider.CreateTimer(
            static state =>
            {
                var timerState = (OfflineTimerState)state!;
                timerState.Tracker.MarkOffline(timerState.Key, timerState.Presence, timerState.Generation);
            },
            new OfflineTimerState(this, key, presence, generation),
            DraftRoomLimits.PresenceGracePeriod,
            Timeout.InfiniteTimeSpan);
    }

    private void MarkOffline(ParticipantKey key, ParticipantPresence expectedPresence, long generation)
    {
        DraftRoomPresenceChange? offlineChange = null;

        lock (this._sync)
        {
            if (this._disposed
                || !this._participants.TryGetValue(key, out var presence)
                || !ReferenceEquals(presence, expectedPresence)
                || presence.ConnectionIds.Count > 0
                || presence.OfflineGeneration != generation)
            {
                return;
            }

            this._participants.Remove(key);
            presence.OfflineTimer?.Dispose();
            presence.OfflineTimer = null;
            offlineChange = new DraftRoomPresenceChange(key.RoomId, key.ParticipantId);
        }

        this.ParticipantOffline?.Invoke(offlineChange);
    }

    private void CancelOfflineTimer(ParticipantPresence presence)
    {
        presence.OfflineGeneration++;
        presence.OfflineTimer?.Dispose();
        presence.OfflineTimer = null;
    }

    private readonly record struct ParticipantKey(Guid RoomId, Guid ParticipantId);

    private sealed class ParticipantPresence
    {
        public HashSet<string> ConnectionIds { get; } = new(StringComparer.Ordinal);

        public ITimer? OfflineTimer { get; set; }

        public long OfflineGeneration { get; set; }
    }

    private sealed record OfflineTimerState(
        PresenceTracker Tracker,
        ParticipantKey Key,
        ParticipantPresence Presence,
        long Generation);
}
