namespace TwilightImperiumUltimate.Contracts.DTOs;

public record ItemListDto<T>(IReadOnlyCollection<T> Items)
    where T : class;
