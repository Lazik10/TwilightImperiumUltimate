using System.Text.Json;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Contains a shareable result without account, participant, source-room or credential data.</summary>
public sealed record DraftResultDto(
    Guid Id,
    string ShareToken,
    DraftTypeId DraftType,
    string RoomName,
    DateTime FinishedAtUtc,
    int PayloadVersion,
    JsonElement Payload,
    string Text);
