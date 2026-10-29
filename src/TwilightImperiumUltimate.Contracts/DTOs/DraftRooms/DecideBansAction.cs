using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Supplies a manager's replacement bans, without reading the seat's private choices.</summary>
public sealed class DecideBansAction : DraftActionDto
{
    public FactionName[] Factions { get; set; } = [];
}
