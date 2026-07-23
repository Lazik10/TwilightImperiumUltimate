using TwilightImperiumUltimate.Contracts.ApiContracts;
using TwilightImperiumUltimate.Contracts.ApiContracts.Website;

namespace TwilightImperiumUltimate.Business.Logic.Websites;

public class DeleteWebsiteCommand(int id) : IRequest<ApiResponse<DeleteWebsiteResponse>>
{
    public int Id { get; } = id;
}
