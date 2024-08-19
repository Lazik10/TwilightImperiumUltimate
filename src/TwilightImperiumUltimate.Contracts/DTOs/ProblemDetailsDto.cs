using System.Text.Json.Serialization;

namespace TwilightImperiumUltimate.Contracts.DTOs;

public record ProblemDetailsDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("detail")]
    public string Detail { get; set; } = string.Empty;

    [JsonPropertyName("instance")]
    public string Instance { get; set; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
}
