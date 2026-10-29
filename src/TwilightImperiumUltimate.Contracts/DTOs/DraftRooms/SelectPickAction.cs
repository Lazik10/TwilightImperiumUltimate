using System.Text.Json.Serialization;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Changes the active seat's private pending pick.</summary>
public sealed class SelectPickAction : DraftActionDto
{
    [JsonConverter(typeof(JsonStringEnumConverter<FactionName>))]
    public FactionName Faction { get; set; }
}
