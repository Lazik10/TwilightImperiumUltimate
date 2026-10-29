namespace TwilightImperiumUltimate.Web.Models.DraftRooms;

/// <summary>
/// Contains the values entered when joining a draft room.
/// </summary>
public sealed class JoinDraftRoomFormModel
{
    private string _code = string.Empty;

    /// <summary>
    /// Gets or sets the room code, normalized to uppercase.
    /// </summary>
    public string Code
    {
        get => _code;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _code = value.Trim().ToUpperInvariant();
        }
    }

    /// <summary>
    /// Gets or sets the room password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name for a guest participant.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the caller needs to provide credentials for a new participation.
    /// </summary>
    public bool RequiresCredentials { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the caller needs to provide a guest display name.
    /// </summary>
    public bool RequiresGuestDisplayName { get; set; }
}
