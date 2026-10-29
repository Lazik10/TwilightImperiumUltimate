using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Contains only the draft information projected for the requesting viewer.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(FactionPickViewDto), "faction-pick")]
public abstract class DraftViewDto
{
    /// <summary>Gets or sets whether the verified viewer has an outstanding action.</summary>
    public bool MyActionRequired { get; set; }
}
