namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.Opponents;

public record AsyncPlayerOpponentsSummaryDto
{
    public AsyncPlayerOpponentsSummaryDto()
    {
    }

    public AsyncPlayerOpponentsSummaryDto(
        IReadOnlyCollection<AsyncPlayerOpponentInfoDto> all,
        IReadOnlyCollection<AsyncPlayerOpponentInfoDto> tigl,
        IReadOnlyCollection<AsyncPlayerOpponentInfoDto> custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public IReadOnlyCollection<AsyncPlayerOpponentInfoDto> All { get; init; } = new List<AsyncPlayerOpponentInfoDto>();

    public IReadOnlyCollection<AsyncPlayerOpponentInfoDto> Tigl { get; init; } = new List<AsyncPlayerOpponentInfoDto>();

    public IReadOnlyCollection<AsyncPlayerOpponentInfoDto> Custom { get; init; } = new List<AsyncPlayerOpponentInfoDto>();
}
