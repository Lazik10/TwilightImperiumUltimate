using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.API.Discord.DisordEntities;

public sealed class DiscordEmbedField
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("inline")]
    public bool? Inline { get; set; }
}
