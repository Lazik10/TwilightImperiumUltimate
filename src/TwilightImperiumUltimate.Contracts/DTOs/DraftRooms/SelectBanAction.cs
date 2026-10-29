using System.Text.Json.Serialization;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>
/// Selects a faction to ban, subject to server-side actor and phase validation.
/// </summary>
public sealed class SelectBanAction : DraftActionDto
{
    /// <summary>
    /// Gets or sets the selected faction.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<FactionName>))]
    public FactionName Faction { get; set; }
}
