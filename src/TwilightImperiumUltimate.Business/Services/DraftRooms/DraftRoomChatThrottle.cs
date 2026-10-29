using TwilightImperiumUltimate.Contracts.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Applies a rolling per-participant limit to draft-room chat messages.
/// </summary>
public sealed class DraftRoomChatThrottle(TimeProvider timeProvider)
    : IDraftRoomChatThrottle
{
    private readonly object _sync = new();

    private readonly Dictionary<Guid, Queue<DateTimeOffset>> _messageTimes = [];

    private readonly TimeProvider _timeProvider = timeProvider
        ?? throw new ArgumentNullException(nameof(timeProvider));

    private DateTimeOffset _nextCleanupAtUtc = DateTimeOffset.MinValue;

    /// <inheritdoc />
    public bool TryAcquire(Guid participantId)
    {
        if (participantId == Guid.Empty)
        {
            throw new ArgumentException("Participant identifier cannot be empty.", nameof(participantId));
        }

        lock (this._sync)
        {
            var now = this._timeProvider.GetUtcNow();
            if (now >= this._nextCleanupAtUtc)
            {
                this.RemoveExpiredEntries(now);
                this._nextCleanupAtUtc = now + DraftRoomLimits.ChatMessageWindow;
            }

            if (!this._messageTimes.TryGetValue(participantId, out var messageTimes))
            {
                messageTimes = new Queue<DateTimeOffset>();
                this._messageTimes.Add(participantId, messageTimes);
            }

            RemoveExpiredMessages(messageTimes, now);
            if (messageTimes.Count >= DraftRoomLimits.MaxChatMessagesPerWindow)
            {
                return false;
            }

            messageTimes.Enqueue(now);

            return true;
        }
    }

    private static void RemoveExpiredMessages(Queue<DateTimeOffset> messageTimes, DateTimeOffset now)
    {
        var cutoff = now - DraftRoomLimits.ChatMessageWindow;
        while (messageTimes.TryPeek(out var messageTime) && messageTime <= cutoff)
        {
            messageTimes.Dequeue();
        }
    }

    private void RemoveExpiredEntries(DateTimeOffset now)
    {
        foreach (var participantId in this._messageTimes.Keys.ToArray())
        {
            var messageTimes = this._messageTimes[participantId];
            RemoveExpiredMessages(messageTimes, now);
            if (messageTimes.Count == 0)
            {
                this._messageTimes.Remove(participantId);
            }
        }
    }
}
