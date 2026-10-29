using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <inheritdoc />
public sealed class FactionPickPoolProvider(IFactionRepository factions) : IFactionPickPoolProvider
{
    private readonly IFactionRepository factions = factions ?? throw new ArgumentNullException(nameof(factions));

    /// <inheritdoc />
    public async Task<IReadOnlyList<FactionName>> GetPoolAsync(
        IReadOnlyCollection<GameVersion> gameVersions,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(gameVersions);
        var pool = await this.factions.GetFactionsByGameVersions(gameVersions, cancellationToken);
        return pool.Select(faction => faction.FactionName).Distinct().ToArray();
    }
}
