namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Contains a bounded, one-based page of account result history.</summary>
public sealed record DraftResultHistoryDto(IReadOnlyList<DraftResultHistoryItemDto> Items, int Page, bool HasMore);
