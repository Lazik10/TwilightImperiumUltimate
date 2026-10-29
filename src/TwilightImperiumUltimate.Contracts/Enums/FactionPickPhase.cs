using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.Enums;

/// <summary>
/// Identifies the current phase of a Faction Pick draft.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<FactionPickPhase>))]
public enum FactionPickPhase
{
    Bans,
    Picks,
    Finished,
}
