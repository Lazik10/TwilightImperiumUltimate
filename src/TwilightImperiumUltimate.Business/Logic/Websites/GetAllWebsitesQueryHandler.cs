using TwilightImperiumUltimate.Contracts.DTOs.Website;

namespace TwilightImperiumUltimate.Business.Logic.Websites;

public class GetAllWebsitesQueryHandler(
    IMapper mapper,
    IWebsiteRepository websiteRepository)
    : IRequestHandler<GetAllWebsitesQuery, ItemListDto<WebsiteDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IWebsiteRepository _websiteRepository = websiteRepository;

    public async Task<ItemListDto<WebsiteDto>> Handle(GetAllWebsitesQuery request, CancellationToken cancellationToken)
    {
        var websites = await _websiteRepository.GetAllWebsites(cancellationToken);

        var websiteDtos = _mapper.Map<List<WebsiteDto>>(websites);

        return new ItemListDto<WebsiteDto>(websiteDtos);
    }
}
