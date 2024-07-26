using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface ISystemTilesForMapSetupProvider
{
    Task<SystemTilesForMapSetup> GetSystemTilesForMapSetup(CancellationToken cancellationToken);
}
