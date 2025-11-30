using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.API.Discord.DisordEntities;

public sealed class DiscordWebhookMessage
{
    public DiscordWebhookMessage(string content)
    {
        Content = content;
    }

    public DiscordWebhookMessage()
    {
    }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }

    [JsonPropertyName("embeds")]
    public List<DiscordEmbed>? Embeds { get; set; }

    [JsonPropertyName("tts")]
    public bool? Tts { get; set; }

    [JsonPropertyName("flags")]
    public int? Flags { get; set; }
}
