using TwilightImperiumUltimate.Contracts.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Limits failed draft-room join attempts using a rolling time window per client and room code.
/// </summary>
public sealed class JoinAttemptThrottle(TimeProvider timeProvider)
    : IJoinAttemptThrottle
{
    private readonly object _sync = new();
    private readonly Dictionary<ThrottleKey, List<DateTimeOffset>> _failures = [];
    private readonly TimeProvider _timeProvider = timeProvider
        ?? throw new ArgumentNullException(nameof(timeProvider));

    /// <inheritdoc />
    public bool IsBlocked(string ipAddress, string roomCode)
    {
        ArgumentNullException.ThrowIfNull(ipAddress);
        ArgumentNullException.ThrowIfNull(roomCode);

        lock (this._sync)
        {
            var now = this._timeProvider.GetUtcNow();
            this.RemoveExpiredFailures(now);

            return this._failures.TryGetValue(new ThrottleKey(ipAddress, roomCode), out var failures)
                && failures.Count >= DraftRoomLimits.MaxJoinFailuresPerIpAndCode;
        }
    }

    /// <inheritdoc />
    public void RegisterFailure(string ipAddress, string roomCode)
    {
        ArgumentNullException.ThrowIfNull(ipAddress);
        ArgumentNullException.ThrowIfNull(roomCode);

        lock (this._sync)
        {
            var now = this._timeProvider.GetUtcNow();
            this.RemoveExpiredFailures(now);

            var key = new ThrottleKey(ipAddress, roomCode);

            if (!this._failures.TryGetValue(key, out var failures))
            {
                failures = [];
                this._failures.Add(key, failures);
            }

            failures.Add(now);
        }
    }

    /// <inheritdoc />
    public void RegisterSuccess(string ipAddress, string roomCode)
    {
        ArgumentNullException.ThrowIfNull(ipAddress);
        ArgumentNullException.ThrowIfNull(roomCode);

        lock (this._sync)
        {
            var now = this._timeProvider.GetUtcNow();
            this.RemoveExpiredFailures(now);
            this._failures.Remove(new ThrottleKey(ipAddress, roomCode));
        }
    }

    private void RemoveExpiredFailures(DateTimeOffset now)
    {
        var cutoff = now - DraftRoomLimits.JoinFailureWindow;

        foreach (var key in this._failures.Keys.ToArray())
        {
            var failures = this._failures[key];
            failures.RemoveAll(failure => failure <= cutoff);
            if (failures.Count == 0)
            {
                this._failures.Remove(key);
            }
        }
    }

    private readonly record struct ThrottleKey(string IpAddress, string RoomCode);
}
