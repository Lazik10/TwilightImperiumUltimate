using System.ComponentModel.DataAnnotations;
using TwilightImperiumUltimate.Contracts.DraftRooms;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;

/// <summary>
/// Request to replace the password required for new room participants.
/// </summary>
public sealed record ChangeRoomPasswordRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeRoomPasswordRequest"/> class.
    /// </summary>
    /// <param name="password">The new room password.</param>
    public ChangeRoomPasswordRequest(string password)
    {
        this.Password = password;
    }

    /// <summary>
    /// Gets the new room password.
    /// </summary>
    [Required]
    [StringLength(
        DraftRoomLimits.MaxPasswordLength,
        MinimumLength = DraftRoomLimits.MinPasswordLength)]
    public string Password { get; init; } = string.Empty;

    /// <summary>
    /// Deconstructs this request into its constructor value.
    /// </summary>
    /// <param name="password">The new room password.</param>
    public void Deconstruct(out string password)
    {
        password = this.Password;
    }
}
