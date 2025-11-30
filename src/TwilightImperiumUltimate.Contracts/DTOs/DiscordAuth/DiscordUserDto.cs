using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.DTOs.DiscordAuth;

public class DiscordUserDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("username")]
    public string Username { get; set; } = default!;

    [JsonPropertyName("global_name")]
    public string? GlobalName { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("discriminator")]
    public string? Discriminator { get; set; }

    [JsonPropertyName("public_flags")]
    public int? PublicFlags { get; set; }
}

