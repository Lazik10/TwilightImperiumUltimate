namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>Controls inactive room cleanup; permanent results are never expired.</summary>
public sealed class DraftRoomRetentionOptions
{
    public int InactiveRoomRetentionDays { get; set; } = 30;
}
