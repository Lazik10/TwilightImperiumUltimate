using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Models.DraftRooms;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Sends create and join requests to the draft-room API.
/// </summary>
public interface IDraftRoomApiService
{
    /// <summary>Reads a public result without sending a room's guest credentials.</summary>
    Task<DraftRoomApiResult<DraftResultDto>> GetResultAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>Reads a finished result using the room's verified identity.</summary>
    Task<DraftRoomApiResult<DraftResultDto>> GetRoomResultAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>Reads the account's finished drafts, twenty at a time.</summary>
    Task<DraftRoomApiResult<DraftResultHistoryDto>> GetMyResultsAsync(int page, CancellationToken cancellationToken = default);

    /// <summary>Deletes a result, including guest manager credentials only when a source room code is supplied.</summary>
    Task<DraftRoomApiCommandResult> DeleteResultAsync(Guid id, string? code = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a room for the supplied form values.
    /// </summary>
    Task<DraftRoomApiResult<DraftRoomEntryDto>> CreateAsync(
        CreateDraftRoomFormModel model,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Joins a room, reusing a stored guest identity when one exists for the room code.
    /// </summary>
    Task<DraftRoomApiResult<DraftRoomEntryDto>> JoinAsync(
        string code,
        string? password,
        string? displayName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Links the authenticated account to the guest identity stored for a room.
    /// </summary>
    Task<DraftRoomApiCommandResult> LinkAccountAsync(
        string code,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the authenticated account's active room memberships.
    /// </summary>
    Task<DraftRoomApiResult<IReadOnlyList<MyDraftRoomDto>>> GetMyRoomsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the personalized view for the caller in a room.
    /// </summary>
    Task<DraftRoomApiResult<DraftRoomViewDto>> GetViewAsync(
        string code,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a bounded page of chat messages for the caller in a room.
    /// </summary>
    Task<DraftRoomApiResult<IReadOnlyList<ChatMessageDto>>> GetChatHistoryAsync(
        string code,
        long? afterId,
        int limit,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a bounded page of activity entries for the caller in a room.
    /// </summary>
    Task<DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>> GetLogHistoryAsync(
        string code,
        long? afterId,
        int limit,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a chat message to a room.
    /// </summary>
    Task<DraftRoomApiResult<ChatMessageDto>> SendChatAsync(
        string code,
        string text,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a seat to a room.
    /// </summary>
    Task<DraftRoomApiCommandResult> AddSeatAsync(
        string code,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a seat from a room.
    /// </summary>
    Task<DraftRoomApiCommandResult> RemoveSeatAsync(
        string code,
        Guid seatId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Claims a seat in a room.
    /// </summary>
    Task<DraftRoomApiCommandResult> ClaimSeatAsync(
        string code,
        Guid seatId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Unclaims a seat in a room.
    /// </summary>
    Task<DraftRoomApiCommandResult> UnclaimSeatAsync(
        string code,
        Guid seatId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Leaves a room.
    /// </summary>
    Task<DraftRoomApiCommandResult> LeaveAsync(
        string code,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Kicks a participant from a room.
    /// </summary>
    Task<DraftRoomApiCommandResult> KickAsync(
        string code,
        Guid participantId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes a participant's role.
    /// </summary>
    Task<DraftRoomApiCommandResult> SetRoleAsync(
        string code,
        Guid participantId,
        DraftRoomRole role,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a room's settings.
    /// </summary>
    Task<DraftRoomApiCommandResult> UpdateSettingsAsync(
        string code,
        string name,
        bool isLocked,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes the password required for new room participants.
    /// </summary>
    Task<DraftRoomApiCommandResult> ChangePasswordAsync(
        string code,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes and deletes a room.
    /// </summary>
    Task<DraftRoomApiCommandResult> CloseAsync(
        string code,
        CancellationToken cancellationToken = default);

    /// <summary>Saves engine rules in the lobby.</summary>
    Task<DraftRoomApiCommandResult> UpdateDraftSettingsAsync(string code, DraftSettingsDto settings, CancellationToken cancellationToken = default);

    /// <summary>Starts a ready lobby.</summary>
    Task<DraftRoomApiCommandResult> StartDraftAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>Resets a running draft.</summary>
    Task<DraftRoomApiCommandResult> ResetDraftAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>Undoes the last locked pick while the draft is still running.</summary>
    Task<DraftRoomApiCommandResult> UndoLastPickAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>Assigns a participant to an eligible vacancy.</summary>
    Task<DraftRoomApiCommandResult> AssignSeatAsync(string code, Guid seatId, Guid participantId, CancellationToken cancellationToken = default);

    /// <summary>Applies an action using the current verified seat.</summary>
    Task<DraftRoomApiCommandResult> ApplyDraftActionAsync(string code, DraftActionDto action, CancellationToken cancellationToken = default);

    /// <summary>Discloses and replaces a seat's private decision as manager.</summary>
    Task<DraftRoomApiCommandResult> DecideForAsync(string code, DecideForRequest request, CancellationToken cancellationToken = default);
}
