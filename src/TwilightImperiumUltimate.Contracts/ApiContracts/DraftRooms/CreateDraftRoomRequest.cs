using System.ComponentModel.DataAnnotations;
using TwilightImperiumUltimate.Contracts.DraftRooms;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;

/// <summary>
/// Request to create a draft room.
/// </summary>
public sealed record CreateDraftRoomRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDraftRoomRequest"/> class.
    /// </summary>
    /// <param name="name">The room's display name.</param>
    /// <param name="password">The password required for new participants.</param>
    /// <param name="displayName">The guest host's display name, or <see langword="null"/> for account hosts.</param>
    public CreateDraftRoomRequest(string name, string password, string? displayName)
    {
        this.Name = name;
        this.Password = password;
        this.DisplayName = displayName;
    }

    /// <summary>
    /// Gets the room's display name.
    /// </summary>
    [Required]
    [StringLength(
        DraftRoomLimits.MaxRoomNameLength,
        MinimumLength = DraftRoomLimits.MinRoomNameLength)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the password required for new participants.
    /// </summary>
    [Required]
    [StringLength(
        DraftRoomLimits.MaxPasswordLength,
        MinimumLength = DraftRoomLimits.MinPasswordLength)]
    public string Password { get; init; } = string.Empty;

    /// <summary>
    /// Gets the guest host's display name, or <see langword="null"/> for account hosts.
    /// </summary>
    [StringLength(
        DraftRoomLimits.MaxDisplayNameLength,
        MinimumLength = DraftRoomLimits.MinDisplayNameLength)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Deconstructs this request into its constructor values.
    /// </summary>
    /// <param name="name">The room's display name.</param>
    /// <param name="password">The password required for new participants.</param>
    /// <param name="displayName">The guest host's display name, or <see langword="null"/> for account hosts.</param>
    public void Deconstruct(out string name, out string password, out string? displayName)
    {
        name = this.Name;
        password = this.Password;
        displayName = this.DisplayName;
    }
}
