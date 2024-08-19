namespace TwilightImperiumUltimate.Business.Logic.Drafts.Faction;

public class DraftFactionsCommand(FactionDraftRequest draftRequest)
    : IRequest<FactionDraftResultDto>
{
    public FactionDraftRequest FactionDraftRequest { get; } = draftRequest;
}
