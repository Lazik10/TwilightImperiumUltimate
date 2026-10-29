using System.ComponentModel.DataAnnotations;
using TwilightImperiumUltimate.Contracts.DraftRooms;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;

/// <summary>
/// Request to join a draft room or re-enter as a known participant.
/// </summary>
public sealed record JoinDraftRoomRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JoinDraftRoomRequest"/> class.
    /// </summary>
    /// <param name="code">The room's public code.</param>
    /// <param name="password">The room password for a new identity.</param>
    /// <param name="displayName">The guest display name, or <see langword="null"/> for account participants.</param>
    public JoinDraftRoomRequest(string code, string? password, string? displayName)
    {
        this.Code = code;
        this.Password = password;
        this.DisplayName = displayName;
    }

    /// <summary>
    /// Gets the room's public code.
    /// </summary>
    [Required]
    [StringLength(
        DraftRoomLimits.CodeLength,
        MinimumLength = DraftRoomLimits.CodeLength)]
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Gets the room password for a new identity.
    /// </summary>
    [StringLength(
        DraftRoomLimits.MaxPasswordLength,
        MinimumLength = DraftRoomLimits.MinPasswordLength)]
    public string? Password { get; init; }

    /// <summary>
    /// Gets the guest display name, or <see langword="null"/> for account participants.
    /// </summary>
    [StringLength(
        DraftRoomLimits.MaxDisplayNameLength,
        MinimumLength = DraftRoomLimits.MinDisplayNameLength)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Deconstructs this request into its constructor values.
    /// </summary>
    /// <param name="code">The room's public code.</param>
    /// <param name="password">The room password for a new identity.</param>
    /// <param name="displayName">The guest display name, or <see langword="null"/> for account participants.</param>
    public void Deconstruct(out string code, out string? password, out string? displayName)
    {
        code = this.Code;
        password = this.Password;
        displayName = this.DisplayName;
    }
}
