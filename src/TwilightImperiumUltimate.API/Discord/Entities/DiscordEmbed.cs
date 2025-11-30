using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.API.Discord.DisordEntities;

public sealed class DiscordEmbed
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; } = "rich";

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTimeOffset? Timestamp { get; set; }

    [JsonPropertyName("color")]
    public int? Color { get; set; }

    [JsonPropertyName("footer")]
    public DiscordEmbedFooter? Footer { get; set; }

    [JsonPropertyName("image")]
    public DiscordEmbedImage? Image { get; set; }

    [JsonPropertyName("thumbnail")]
    public DiscordEmbedThumbnail? Thumbnail { get; set; }

    [JsonPropertyName("author")]
    public DiscordEmbedAuthor? Author { get; set; }

    [JsonPropertyName("fields")]
    public List<DiscordEmbedField>? Fields { get; set; }
}
