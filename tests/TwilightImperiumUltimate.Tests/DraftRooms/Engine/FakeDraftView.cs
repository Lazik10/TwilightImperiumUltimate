using System.Text.Json.Serialization;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Tests.DraftRooms.Engine;

internal sealed class FakeDraftView : DraftViewDto
{
    public string PublicValue { get; init; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PrivateValue { get; init; }
}
