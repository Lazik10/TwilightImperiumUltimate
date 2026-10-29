using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.Enums;

/// <summary>
/// Determines who bans factions before the picks phase.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<FactionPickBansMode>))]
public enum FactionPickBansMode
{
    None,
    HostBans,
    SimultaneousPlayerBans,
}
