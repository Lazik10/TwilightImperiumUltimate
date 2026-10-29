namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Confirms the current private pick, refusing a stale turn cursor.</summary>
public sealed class LockInPickAction : DraftActionDto
{
    public int TurnIndex { get; set; }
}
