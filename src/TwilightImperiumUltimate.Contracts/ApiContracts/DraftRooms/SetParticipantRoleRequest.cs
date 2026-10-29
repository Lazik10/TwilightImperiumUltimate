using System.ComponentModel.DataAnnotations;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;

/// <summary>
/// Request to change a participant's role.
/// </summary>
public sealed record SetParticipantRoleRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetParticipantRoleRequest"/> class.
    /// </summary>
    /// <param name="role">The requested participant role.</param>
    public SetParticipantRoleRequest(DraftRoomRole role)
    {
        this.Role = role;
    }

    /// <summary>
    /// Gets the requested participant role.
    /// </summary>
    [EnumDataType(typeof(DraftRoomRole))]
    public DraftRoomRole Role { get; init; }

    /// <summary>
    /// Deconstructs this request into its constructor value.
    /// </summary>
    /// <param name="role">The requested participant role.</param>
    public void Deconstruct(out DraftRoomRole role)
    {
        role = this.Role;
    }
}
