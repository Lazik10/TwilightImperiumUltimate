using System.ComponentModel.DataAnnotations;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;

/// <summary>
/// Request to update a draft room's name and join lock.
/// </summary>
public sealed record UpdateRoomSettingsRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRoomSettingsRequest"/> class.
    /// </summary>
    /// <param name="name">The room's new display name.</param>
    /// <param name="isLocked">Whether new participants are prevented from joining.</param>
    public UpdateRoomSettingsRequest(string name, bool isLocked)
    {
        this.Name = name;
        this.IsLocked = isLocked;
    }

    /// <summary>
    /// Gets the room's new display name.
    /// </summary>
    [Required]
    [StringLength(
        DraftRoomLimits.MaxRoomNameLength,
        MinimumLength = DraftRoomLimits.MinRoomNameLength)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets whether new participants are prevented from joining.
    /// </summary>
    public bool IsLocked { get; init; }

    /// <summary>Gets the optional draft-type change, applied atomically with the room settings.</summary>
    public DraftTypeId? DraftType { get; init; }

    /// <summary>
    /// Deconstructs this request into its constructor values.
    /// </summary>
    /// <param name="name">The room's new display name.</param>
    /// <param name="isLocked">Whether new participants are prevented from joining.</param>
    public void Deconstruct(out string name, out bool isLocked)
    {
        name = this.Name;
        isLocked = this.IsLocked;
    }
}
