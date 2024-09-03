using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class GetSliceDraftByIdQuery(int id) : IRequest<ApiResponse<SliceDraftDto>>
{
    public int Id { get; set; } = id;
}
