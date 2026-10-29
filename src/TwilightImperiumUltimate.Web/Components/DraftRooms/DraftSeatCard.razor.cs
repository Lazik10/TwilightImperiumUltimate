using System.Globalization;
using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftSeatCard
{
    /// <summary>
    /// Gets or sets the room view used to determine the current participant's permissions.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public DraftRoomViewDto Room { get; set; } = default!;

    /// <summary>
    /// Gets or sets the seat displayed by this card.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public DraftRoomViewSeatDto Seat { get; set; } = default!;

    /// <summary>
    /// Gets or sets the active participant occupying the seat.
    /// </summary>
    [Parameter]
    public DraftRoomViewParticipantDto? Occupant { get; set; }

    /// <summary>
    /// Gets or sets whether a room command is in progress.
    /// </summary>
    [Parameter]
    public bool IsProcessing { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the current participant claims this seat.
    /// </summary>
    [Parameter]
    public EventCallback<Guid> OnClaimSeat { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the current participant leaves this seat.
    /// </summary>
    [Parameter]
    public EventCallback<Guid> OnLeaveSeat { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a manager unclaims this seat.
    /// </summary>
    [Parameter]
    public EventCallback<Guid> OnUnclaimSeat { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a manager removes this seat.
    /// </summary>
    [Parameter]
    public EventCallback<Guid> OnRemoveSeat { get; set; }

    private string RemovalDisabledReasonId => $"draft-room-seat-{Seat.Id:N}-remove-reason";

    private string SeatHeading =>
        string.Format(CultureInfo.CurrentCulture, Strings.DraftRooms_SeatNumber, Seat.Index);

    private bool IsOccupied => Seat.ParticipantId is not null;

    private bool IsOwnSeat => Seat.ParticipantId == Room.Me.ParticipantId;

    private bool IsManager => Room.Me.Role is DraftRoomRole.Host or DraftRoomRole.CoHost;

    private bool CanClaim => Room.Status == DraftRoomStatus.Lobby ? Room.Me.CanClaimSeat : Seat.CanClaim;

    private bool CanUnclaim => Room.Status == DraftRoomStatus.Lobby ? IsOwnSeat || IsManager : Seat.CanUnclaim;

    private string ClaimLabel => Seat.CanReclaim ? DraftRoomText.Get("ReclaimSeat") : Strings.DraftRooms_ClaimSeat;

    private string LeaveLabel => Room.Status == DraftRoomStatus.InProgress ? DraftRoomText.Get("ReleaseSeat") : Strings.DraftRooms_LeaveSeat;

    private string UnclaimLabel => Room.Status == DraftRoomStatus.InProgress ? DraftRoomText.Get("ReleaseSeat") : Strings.DraftRooms_UnclaimSeat;

    private string OccupantName => Occupant?.DisplayName
        ?? (IsOccupied ? Strings.DraftRooms_UnknownSeatOccupant : Strings.DraftRooms_SeatFree);

    private string? RemovalDisabledReason
    {
        get
        {
            if (IsOccupied)
            {
                return Strings.DraftRooms_OccupiedSeatCannotBeRemoved;
            }

            return Room.Seats.Count <= DraftRoomLimits.MinSeatCount
                ? Strings.DraftRooms_MinSeatsReached
                : null;
        }
    }
}
