using TwilightImperiumUltimate.Contracts.ApiContracts;
using TwilightImperiumUltimate.Contracts.DTOs.Website;
using TwilightImperiumUltimate.Core.Entities.Website;

namespace TwilightImperiumUltimate.Business.Logic.Websites;

public class CreateWebsiteCommandHandler(
    IWebsiteRepository websiteRepository,
    IMapper mapper)
    : IRequestHandler<CreateWebsiteCommand, ApiResponse<WebsiteDto>>
{
    private readonly IWebsiteRepository _websiteRepository = websiteRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<WebsiteDto>> Handle(CreateWebsiteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var website = new Website
        {
            Title = request.Title,
            Description = request.Description,
            WebsitePath = request.WebsitePath,
            ImageData = request.ImageData,
            ImageContentType = request.ImageContentType,
        };

        var created = await _websiteRepository.CreateWebsite(website, cancellationToken);
        var dto = _mapper.Map<WebsiteDto>(created);

        return new ApiResponse<WebsiteDto>() { Success = true, Data = dto };
    }
}
