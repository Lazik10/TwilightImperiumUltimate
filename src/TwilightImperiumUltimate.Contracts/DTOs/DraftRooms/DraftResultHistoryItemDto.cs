using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Describes an account's finished draft and effective deletion permission.</summary>
public sealed record DraftResultHistoryItemDto(
    Guid Id,
    string ShareToken,
    DraftTypeId DraftType,
    string RoomName,
    DateTime FinishedAtUtc,
    bool CanDelete);
