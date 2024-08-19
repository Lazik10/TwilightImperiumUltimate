using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface ISystemTilesForMapSetupProvider
{
    Task<SystemTilesForMapSetup> GetSystemTilesForMapSetup(IMapSettings mapSettings, GenerateMapRequest request, CancellationToken cancellationToken);
}
