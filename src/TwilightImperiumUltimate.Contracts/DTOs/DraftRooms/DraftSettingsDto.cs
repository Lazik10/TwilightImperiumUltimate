using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Contains the settings of a supported draft type.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(FactionPickSettingsDto), "faction-pick")]
public abstract class DraftSettingsDto
{
}
