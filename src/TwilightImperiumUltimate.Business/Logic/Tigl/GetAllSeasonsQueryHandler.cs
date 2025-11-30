using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class GetAllSeasonsQueryHandler(
    ISeasonRepository seasonRepository)
    : IRequestHandler<GetAllSeasonsQuery, ItemListDto<SeasonDto>>
{
    public async Task<ItemListDto<SeasonDto>> Handle(GetAllSeasonsQuery request, CancellationToken cancellationToken)
    {
        var seasons = await seasonRepository.GetAllSeasons(cancellationToken);

        var list = seasons.Select(s => new SeasonDto
        {
            SeasonNumber = s.SeasonNumber,
            Name = s.Name,
            IsActive = s.IsActive,
            StartDate = s.StartDate,
            EndDate = s.EndDate,
        })
        .OrderByDescending(s => s.SeasonNumber)
        .ToList();

        return new ItemListDto<SeasonDto>(list);
    }
}
