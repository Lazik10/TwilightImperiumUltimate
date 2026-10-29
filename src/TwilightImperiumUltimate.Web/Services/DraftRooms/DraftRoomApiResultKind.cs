namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Describes the outcome of a draft-room API request.
/// </summary>
public enum DraftRoomApiResultKind
{
    Success,
    NotFoundOrWrongPassword,
    NotFound,
    Forbidden,
    Conflict,
    Throttled,
    ValidationFailed,
    Failed,
}
