using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Describes a public activity entry in a draft room.
/// </summary>
public sealed class DraftLogEntryDto
{
    /// <summary>
    /// Gets or sets the persisted entry identifier.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the participant who caused the activity, if any.
    /// </summary>
    public Guid? ActorParticipantId { get; set; }

    /// <summary>
    /// Gets or sets the activity type.
    /// </summary>
    public DraftLogEntryType Type { get; set; }

    /// <summary>
    /// Gets or sets the JSON-serialized public activity data.
    /// </summary>
    public string DataJson { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC time the activity occurred.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }
}
