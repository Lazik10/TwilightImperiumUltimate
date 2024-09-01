using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class GetMapByIdQuery(int id) : IRequest<ApiResponse<MapDto>>
{
    public int Id { get; set; } = id;
}
