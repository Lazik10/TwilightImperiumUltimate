using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Loads faction identifiers before entering the pure engine.
/// </summary>
public interface IFactionPickPoolProvider
{
    /// <summary>
    /// Loads distinct faction identifiers for the enabled expansions.
    /// </summary>
    Task<IReadOnlyList<FactionName>> GetPoolAsync(
        IReadOnlyCollection<GameVersion> gameVersions,
        CancellationToken cancellationToken);
}
