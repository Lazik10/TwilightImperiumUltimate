using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.API.Discord.DisordEntities;

public sealed class DiscordEmbedAuthor
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("icon_url")]
    public string? IconUrl { get; set; }
}
