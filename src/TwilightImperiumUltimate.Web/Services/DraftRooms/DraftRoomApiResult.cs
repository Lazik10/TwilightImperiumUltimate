namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Contains a draft-room API outcome and its successful response data, when available.
/// </summary>
/// <typeparam name="T">The response data type.</typeparam>
public sealed record DraftRoomApiResult<T>(DraftRoomApiResultKind Kind, T? Data)
    where T : class
{
    /// <summary>
    /// Gets a value indicating whether the request succeeded and returned data.
    /// </summary>
    public bool IsSuccess => Kind == DraftRoomApiResultKind.Success && Data is not null;
}
