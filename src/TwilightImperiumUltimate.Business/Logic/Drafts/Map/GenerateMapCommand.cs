namespace TwilightImperiumUltimate.Business.Logic.Drafts.Map;

public class GenerateMapCommand(GenerateMapRequest draftRequest)
    : IRequest<GeneratedMapLayoutDto>
{
    public GenerateMapRequest DraftRequest { get; } = draftRequest;
}
