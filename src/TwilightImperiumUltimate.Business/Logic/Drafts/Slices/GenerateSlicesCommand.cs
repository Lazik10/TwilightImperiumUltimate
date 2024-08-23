using TwilightImperiumUltimate.Contracts.DTOs.SliceGeneration;

namespace TwilightImperiumUltimate.Business.Logic.Drafts.Slices;

public class GenerateSlicesCommand(
    SliceDraftRequest request)
    : IRequest<ItemListDto<SliceDto>>
{
    public SliceDraftRequest SliceDraftRequest { get; set; } = request;
}
