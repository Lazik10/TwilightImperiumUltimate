using MediatR;
using TwilightImperiumUltimate.Draft.Draft.ColorDraft;

namespace TwilightImperiumUltimate.Business.Draft.FactionColorDraft;

internal class DraftColorsHandler : IRequestHandler<DraftColorsCommand, IReadOnlyList<FactionColorDraftResult>>
{
    private readonly IColorDraftService _colorDraftService;

    public DraftColorsHandler(IColorDraftService colorDraftService)
    {
        _colorDraftService = colorDraftService;
    }

    public async Task<IReadOnlyList<FactionColorDraftResult>> Handle(DraftColorsCommand request, CancellationToken cancellationToken)
    {
        return await _colorDraftService.DraftColorsAsync(request.ColorDraftRequest);
    }
}
