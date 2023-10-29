using MediatR;
using TwilightImperiumUltimate.Draft.Draft.ColorDraft;

namespace TwilightImperiumUltimate.Business.Draft.FactionColorDraft;

internal class ColorsDraftCommandHandler : IRequestHandler<ColorsDraftCommand, IReadOnlyList<FactionColorDraftResult>>
{
    private readonly IColorDraftService _colorDraftService;

    public ColorsDraftCommandHandler(IColorDraftService colorDraftService)
    {
        _colorDraftService = colorDraftService;
    }

    public async Task<IReadOnlyList<FactionColorDraftResult>> Handle(ColorsDraftCommand request, CancellationToken cancellationToken)
    {
        return await _colorDraftService.DraftColorsAsync(request.ColorDraftRequest);
    }
}
