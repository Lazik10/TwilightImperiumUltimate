using System.Diagnostics.CodeAnalysis;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

/// <summary>
/// Provides persistence operations for draft rooms and their participants.
/// </summary>
public interface IDraftRoomRepository
{
    /// <summary>Reads a bounded, oldest-first batch of inactive room codes for retention cleanup.</summary>
    Task<IReadOnlyList<string>> GetInactiveRoomCodesAsync(DateTime cutoffUtc, int limit, CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether a room code is already in use.
    /// </summary>
    /// <param name="code">The room code to check.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns><see langword="true"/> if the code exists; otherwise, <see langword="false"/>.</returns>
    Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken);

    /// <summary>
    /// Persists a room and its host participant as one unit.
    /// </summary>
    /// <param name="room">The room to create.</param>
    /// <param name="hostParticipant">The participant who hosts the room.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <param name="appendLog">A callback that adds public activity to the same save operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [SuppressMessage(
        "Design",
        "CA1068:Method should take CancellationToken as the last parameter",
        Justification = "Preserve the established public repository callback signature.")]
    Task CreateRoomWithHostAsync(
        DraftRoom room,
        DraftRoomParticipant hostParticipant,
        CancellationToken cancellationToken,
        Action<DraftRoomScope> appendLog);

    /// <summary>
    /// Finds a room by its public room code.
    /// </summary>
    /// <param name="code">The room code.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The room and its participants, or <see langword="null"/> if no room matches.</returns>
    Task<DraftRoom?> GetRoomByCodeAsync(string code, CancellationToken cancellationToken);

    /// <summary>
    /// Finds a room and its participants and seats for a personalized room view.
    /// </summary>
    /// <param name="code">The room code.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The room and its loaded participants and seats, or <see langword="null"/> if no room matches.</returns>
    Task<DraftRoom?> GetRoomForViewByCodeAsync(string code, CancellationToken cancellationToken);

    /// <summary>
    /// Gets chat messages for a room, returning the latest page when no cursor is supplied.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="afterId">An optional exclusive message cursor.</param>
    /// <param name="limit">The maximum number of messages to return.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The selected messages in ascending identifier order.</returns>
    Task<IReadOnlyList<DraftRoomChatMessage>> GetChatHistoryAsync(
        Guid roomId,
        long? afterId,
        int limit,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets activity entries for a room, returning the latest page when no cursor is supplied.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="afterId">An optional exclusive entry cursor.</param>
    /// <param name="limit">The maximum number of entries to return.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The selected entries in ascending identifier order.</returns>
    Task<IReadOnlyList<DraftRoomLogEntry>> GetLogHistoryAsync(
        Guid roomId,
        long? afterId,
        int limit,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a bounded, newest-first list of active room memberships for an account.
    /// </summary>
    /// <param name="userId">The account identifier.</param>
    /// <param name="limit">The maximum number of active memberships to return.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The matching participant records with their rooms and seats loaded.</returns>
    Task<IReadOnlyList<DraftRoomParticipant>> GetActiveParticipantsForUserAsync(
        string userId,
        int limit,
        CancellationToken cancellationToken);

    /// <summary>
    /// Finds a participant associated with an account in a room.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="userId">The account identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The matching participant, or <see langword="null"/> if none exists.</returns>
    Task<DraftRoomParticipant?> FindParticipantByUserAsync(
        Guid roomId,
        string userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Finds a participant by identifier within the specified room.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="participantId">The participant identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The matching participant, or <see langword="null"/> if none exists.</returns>
    Task<DraftRoomParticipant?> FindParticipantByIdAsync(
        Guid roomId,
        Guid participantId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persists a participant in an existing room.
    /// </summary>
    /// <param name="participant">The participant to add.</param>
    /// <param name="activityAtUtc">The UTC time the participant joined.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <param name="appendLog">A callback that adds public activity to the same transaction.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [SuppressMessage(
        "Design",
        "CA1068:Method should take CancellationToken as the last parameter",
        Justification = "Preserve the established public repository callback signature.")]
    Task AddParticipantAsync(
        DraftRoomParticipant participant,
        DateTime activityAtUtc,
        CancellationToken cancellationToken,
        Action<DraftRoomScope> appendLog);

    /// <summary>
    /// Reactivates a known participant and records room activity atomically.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="participantId">The participant identifier.</param>
    /// <param name="activityAtUtc">The UTC time of re-entry.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <param name="appendLog">A callback that adds public activity to the same transaction.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [SuppressMessage(
        "Design",
        "CA1068:Method should take CancellationToken as the last parameter",
        Justification = "Preserve the established public repository callback signature.")]
    Task<bool> ReactivateParticipantAsync(
        Guid roomId,
        Guid participantId,
        DateTime activityAtUtc,
        CancellationToken cancellationToken,
        Action<DraftRoomScope> appendLog);

    /// <summary>
    /// Counts rooms that are not finished and are hosted by the specified account.
    /// </summary>
    /// <param name="userId">The host account identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The number of open rooms hosted by the account.</returns>
    Task<int> CountOpenRoomsForUserAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Counts participants whose status is active in a room.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The number of active participants.</returns>
    Task<int> CountActiveParticipantsAsync(Guid roomId, CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether an active participant already uses a display name key in a room.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="displayNameKey">The normalized display name key.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns><see langword="true"/> if the key is taken; otherwise, <see langword="false"/>.</returns>
    Task<bool> IsDisplayNameTakenAsync(
        Guid roomId,
        string displayNameKey,
        CancellationToken cancellationToken);
}
