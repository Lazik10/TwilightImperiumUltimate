namespace TwilightImperiumUltimate.Web.Models.DraftRooms;

/// <summary>
/// Contains a new room password and its confirmation.
/// </summary>
public sealed class ChangeDraftRoomPasswordFormModel
{
    /// <summary>
    /// Gets or sets the new room password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the repeated new room password.
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}
