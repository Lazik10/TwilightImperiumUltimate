using System.Text.Json.Serialization;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Replaces and locks the active seat's pick after publicly disclosing its pending choice.</summary>
public sealed class DecidePickAction : DraftActionDto
{
    [JsonConverter(typeof(JsonStringEnumConverter<FactionName>))]
    public FactionName Faction { get; set; }

    public int TurnIndex { get; set; }
}
