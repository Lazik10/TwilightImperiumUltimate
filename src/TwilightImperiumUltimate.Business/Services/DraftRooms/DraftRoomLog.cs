using System.Text.Json;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Serializes public room activity into the log table using the current room transaction.
/// </summary>
public sealed class DraftRoomLog : IDraftRoomLog
{
    /// <inheritdoc />
    public DraftRoomLogEntry Append(
        DraftRoomScope scope,
        DraftLogEntryType type,
        Guid? actorParticipantId,
        object data)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(data);

        var entry = new DraftRoomLogEntry
        {
            RoomId = scope.Room.Id,
            ActorParticipantId = actorParticipantId,
            Type = type,
            DataJson = JsonSerializer.Serialize(data, data.GetType()),
            CreatedAtUtc = scope.Now,
        };
        scope.Context.DraftRoomLogEntries.Add(entry);

        return entry;
    }
}
