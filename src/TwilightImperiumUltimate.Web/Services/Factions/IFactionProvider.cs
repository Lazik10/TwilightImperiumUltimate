namespace TwilightImperiumUltimate.Web.Services.Factions;

public interface IFactionProvider
{
    FactionName CurrentFactionName { get; }

    FactionSource? Source { get; }

    FactionDto? CurrentFaction { get; }

    void SetCurrentFactionName(FactionName factionName);

    void SetSource(FactionSource source);

    void ClearSource();

    FactionDto? GetFactionByName(FactionName factionName);

    Task<IReadOnlyCollection<FactionDto>> GetAllFactions(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<FactionDto>> GetFactionsByGameVersions(IReadOnlyCollection<GameVersion> gameVersions, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<FactionDto>> GetFactionsBySource(FactionSource source, CancellationToken cancellationToken = default);
}
