namespace TwilightImperiumUltimate.Web.Models.DraftRooms;

/// <summary>
/// Contains the values entered when creating a draft room.
/// </summary>
public sealed class CreateDraftRoomFormModel
{
    /// <summary>
    /// Gets or sets the room name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the room password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name for a guest creator.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this creator is using a guest identity.
    /// </summary>
    public bool IsGuest { get; set; }
}
