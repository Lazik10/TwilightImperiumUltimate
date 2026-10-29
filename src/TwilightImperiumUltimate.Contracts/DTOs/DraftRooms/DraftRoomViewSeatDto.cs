namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Describes a seat in a draft-room view.
/// </summary>
public sealed class DraftRoomViewSeatDto
{
    /// <summary>
    /// Gets or sets the seat identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the one-based seat position.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the participant occupying the seat, or <see langword="null"/> if it is free.
    /// </summary>
    public Guid? ParticipantId { get; set; }

    /// <summary>Gets or sets whether this viewer may claim the vacancy.</summary>
    public bool CanClaim { get; set; }

    /// <summary>Gets or sets whether claiming restores this viewer's released draft seat.</summary>
    public bool CanReclaim { get; set; }

    /// <summary>Gets or sets whether this viewer may release the occupied seat.</summary>
    public bool CanUnclaim { get; set; }

    /// <summary>Gets or sets whether a manager can assign an eligible participant to this vacancy.</summary>
    public bool CanAssign { get; set; }

    /// <summary>Gets or sets the eligible unseated participants offered to managers for this vacancy.</summary>
    public IReadOnlyList<Guid> AssignableParticipantIds { get; set; } = [];
}
