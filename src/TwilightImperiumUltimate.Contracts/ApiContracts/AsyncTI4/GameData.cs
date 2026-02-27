using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;

public class GameData
{
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("players")]
    public IReadOnlyCollection<PlayerData> Players { get; set; } = new List<PlayerData>();

    [JsonPropertyName("winners")]
    public IReadOnlyCollection<string> Winners { get; set; } = new List<string>();

    [JsonPropertyName("modes")]
    public IReadOnlyCollection<string> Modes { get; set; } = new List<string>();

    [JsonPropertyName("completed")]
    public bool Completed { get; set; }

    [JsonPropertyName("round")]
    public int Round { get; set; }

    [JsonPropertyName("absolMode")]
    public bool AbsolMode { get; set; }

    [JsonPropertyName("discordantStarsMode")]
    public bool DiscordantStarsMode { get; set; }

    [JsonPropertyName("frankenGame")]
    public bool FrankenGame { get; set; }

    [JsonPropertyName("scoreboard")]
    public int Scoreboard { get; set; }

    [JsonPropertyName("setupTimestamp")]
    public long SetupTimestamp { get; set; }

    [JsonPropertyName("endedTimestamp")]
    public long? EndedTimestamp { get; set; }

    [JsonPropertyName("turn")]
    public string Turn { get; set; } = string.Empty;

    [JsonPropertyName("asyncGameID")]
    public string AsyncGameID { get; set; } = string.Empty;

    [JsonPropertyName("asyncFunGameName")]
    public string AsyncFunGameName { get; set; } = string.Empty;

    [JsonPropertyName("platform")]
    public string Platform { get; set; } = string.Empty;

    [JsonPropertyName("homebrew")]
    public bool Homebrew { get; set; }

    [JsonPropertyName("allianceMode")]
    public bool AllianceMode { get; set; }

    [JsonPropertyName("mapString")]
    public string MapString { get; set; } = string.Empty;

    [JsonPropertyName("isPoK")]
    public bool IsPoK { get; set; }

    [JsonPropertyName("tiglGame")]
    public bool IsTigl { get; set; }

    [JsonPropertyName("creationEpochMilliseconds")]
    public long CreationEpochTimestamp { get; set; }

    [JsonPropertyName("endedEpochMilliseconds")]
    public long? EndedEpochTimestamp { get; set; }
}
