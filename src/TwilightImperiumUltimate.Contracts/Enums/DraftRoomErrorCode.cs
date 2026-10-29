namespace TwilightImperiumUltimate.Contracts.Enums;

/// <summary>
/// Identifies a failure during a draft-room operation.
/// </summary>
public enum DraftRoomErrorCode
{
    InvalidRoomName,
    InvalidPassword,
    InvalidGuestDisplayName,
    InvalidCaller,
    OpenRoomLimitReached,
    RoomCodeGenerationFailed,
    NotFoundOrWrongPassword,
    JoinAttemptsThrottled,
    ParticipantKicked,
    RoomFull,
    RoomLocked,
    DuplicateDisplayName,
    RoomChanged,
    Forbidden,
    SeatLimitReached,
    MinimumSeatCountReached,
    SeatNotFound,
    SeatOccupied,
    SeatNotOccupied,
    InvalidRole,
    ParticipantNotFound,
    InvalidChatMessage,
    ChatRateLimited,
    InvalidHistoryCursor,
    InvalidHistoryLimit,
    AccountAlreadyInRoom,
    InvalidDraftType,
    NotEnoughSeats,
    SeatsNotFull,
    SettingsInvalid,
    InvalidDraftAction,
    ResultNotFound,
}
