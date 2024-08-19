using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface IGenerateMapService
{
    Task<GeneratedMapLayout> GenerateMapLayout(GenerateMapRequest request, CancellationToken cancellationToken);
}