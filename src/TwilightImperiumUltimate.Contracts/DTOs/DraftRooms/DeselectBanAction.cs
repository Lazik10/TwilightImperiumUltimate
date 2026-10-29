using System.Text.Json.Serialization;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Removes a pending faction ban.
/// </summary>
public sealed class DeselectBanAction : DraftActionDto
{
    /// <summary>
    /// Gets or sets the faction to remove.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<FactionName>))]
    public FactionName Faction { get; set; }
}
