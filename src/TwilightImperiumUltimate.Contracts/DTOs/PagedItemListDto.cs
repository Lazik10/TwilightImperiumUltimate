namespace TwilightImperiumUltimate.Contracts.DTOs;

public record PagedItemListDto<T>(IReadOnlyCollection<T> Items, int TotalCount)
    where T : class;
