using TwilightImperiumUltimate.Contracts.ApiContracts;
using TwilightImperiumUltimate.Contracts.ApiContracts.Website;

namespace TwilightImperiumUltimate.Business.Logic.Websites;

public class DeleteWebsiteCommandHandler(
    IWebsiteRepository websiteRepository)
    : IRequestHandler<DeleteWebsiteCommand, ApiResponse<DeleteWebsiteResponse>>
{
    private readonly IWebsiteRepository _websiteRepository = websiteRepository;

    public async Task<ApiResponse<DeleteWebsiteResponse>> Handle(DeleteWebsiteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deleted = await _websiteRepository.DeleteWebsite(request.Id, cancellationToken);

        return new ApiResponse<DeleteWebsiteResponse>() { Success = deleted, Data = new DeleteWebsiteResponse() { Success = deleted } };
    }
}
