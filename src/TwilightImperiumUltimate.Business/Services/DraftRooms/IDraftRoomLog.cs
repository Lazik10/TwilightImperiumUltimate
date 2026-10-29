using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Appends public activity entries to the current draft-room transaction.
/// </summary>
public interface IDraftRoomLog
{
    /// <summary>
    /// Adds a public activity entry to the persistence context associated with <paramref name="scope"/>.
    /// </summary>
    /// <param name="scope">The active draft-room transaction scope.</param>
    /// <param name="type">The kind of public activity.</param>
    /// <param name="actorParticipantId">The participant who performed the activity, if any.</param>
    /// <param name="data">The public, non-sensitive data for the activity.</param>
    /// <returns>The newly tracked activity entry.</returns>
    DraftRoomLogEntry Append(
        DraftRoomScope scope,
        DraftLogEntryType type,
        Guid? actorParticipantId,
        object data);
}
