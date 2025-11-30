using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.API.Discord.DisordEntities;

public sealed class DiscordEmbedImage
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }
}
