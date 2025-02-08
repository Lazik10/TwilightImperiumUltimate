using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Async;

public class GameStats : IEntity
{
    public int Id { get; set; }

    public string AsyncGameID { get; set; } = string.Empty;

    public string AsyncFunGameName { get; set; } = string.Empty;

    public string Platform { get; set; } = string.Empty;

    public long Timestamp { get; set; }

    public long SetupTimestamp { get; set; }

    public long? EndedTimestamp { get; set; }

    public bool HasWinner { get; set; }

    public int NumberOfPlayers { get; set; }

    public int Round { get; set; }

    public string? Turn { get; set; } = string.Empty;

    public int Scoreboard { get; set; }

    public string MapString { get; set; } = string.Empty;

    public bool AbsolMode { get; set; }

    public bool DiscordantStarsMode { get; set; }

    public bool FrankenGame { get; set; }

    public bool Homebrew { get; set; }

    public bool IsPoK { get; set; }

    public bool IsTigl { get; set; }

    public ICollection<AsyncPlayerProfileGameStats> GameStatistics { get; set; } = new List<AsyncPlayerProfileGameStats>();

    public ICollection<PlayerStats> PlayerStatistics { get; set; } = new List<PlayerStats>();
}
