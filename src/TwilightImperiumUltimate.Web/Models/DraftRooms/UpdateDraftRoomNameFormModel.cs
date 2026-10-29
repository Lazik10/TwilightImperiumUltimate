namespace TwilightImperiumUltimate.Web.Models.DraftRooms;

/// <summary>
/// Contains the updated room name entered by a room manager.
/// </summary>
public sealed class UpdateDraftRoomNameFormModel
{
    /// <summary>
    /// Gets or sets the room name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
