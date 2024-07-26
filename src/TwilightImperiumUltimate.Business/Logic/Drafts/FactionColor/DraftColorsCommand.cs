namespace TwilightImperiumUltimate.Business.Logic.Drafts.FactionColor;

public class DraftColorsCommand(ColorDraftRequest draftRequest)
    : IRequest<FactionColorDraftResultDto>
{
    public ColorDraftRequest ColorDraftRequest { get; } = draftRequest;
}
