using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Contains only public configuration, numbered seats, revealed bans and locked picks.</summary>
public sealed class FactionPickResultDto
{
    public IReadOnlyList<GameVersion> GameVersions { get; set; } = [];

    public FactionPickBansMode BansMode { get; set; }

    public int BansPerPlayer { get; set; }

    public int HostBanCount { get; set; }

    public FactionPickOrderMode PickOrderMode { get; set; }

    public int PicksPerPlayer { get; set; }

    public bool Snake { get; set; }

    public IReadOnlyList<int> SeatOrder { get; set; } = [];

    public IReadOnlyList<FactionPickResultBanDto> Bans { get; set; } = [];

    public IReadOnlyList<FactionPickResultSeatDto> Seats { get; set; } = [];
}
