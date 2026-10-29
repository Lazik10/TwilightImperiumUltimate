using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.Enums;

/// <summary>
/// Identifies an available room draft engine.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<DraftTypeId>))]
public enum DraftTypeId
{
    FactionPick,
}
