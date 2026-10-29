namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Contains the outcome of a draft-room command that has no response data.
/// </summary>
public sealed record DraftRoomApiCommandResult(DraftRoomApiResultKind Kind)
{
    /// <summary>
    /// Gets a value indicating whether the request succeeded.
    /// </summary>
    public bool IsSuccess => Kind == DraftRoomApiResultKind.Success;
}
