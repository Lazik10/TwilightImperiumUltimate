using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.Enums;

/// <summary>
/// Determines how the initial seat order is chosen.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<FactionPickOrderMode>))]
public enum FactionPickOrderMode
{
    Random,
    HostDefined,
}
