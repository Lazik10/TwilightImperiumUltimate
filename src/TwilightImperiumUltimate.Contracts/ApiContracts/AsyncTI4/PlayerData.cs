using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;

public class PlayerData
{
    [JsonPropertyName("discordUserID")]
    public string DiscordUserID { get; set; } = string.Empty;

    [JsonPropertyName("discordUsername")]
    public string DiscordUserName { get; set; } = string.Empty;

    [JsonPropertyName("factionName")]
    public string FactionName { get; set; } = string.Empty;

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("colorActual")]
    public string ColorActual { get; set; } = string.Empty;

    [JsonPropertyName("totalNumberOfTurns")]
    public int TotalNumberOfTurns { get; set; }

    [JsonPropertyName("totalTurnTime")]
    public long TotalTurnTime { get; set; }

    [JsonPropertyName("expectedHits")]
    public float ExpectedHits { get; set; }

    [JsonPropertyName("actualHits")]
    public float ActualHits { get; set; }

    [JsonPropertyName("eliminated")]
    public bool Eliminated { get; set; }
}