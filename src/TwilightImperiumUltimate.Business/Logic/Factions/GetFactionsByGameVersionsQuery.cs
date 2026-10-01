namespace TwilightImperiumUltimate.Business.Logic.Factions;

public class GetFactionsByGameVersionsQuery(IReadOnlyCollection<GameVersion> gameVersions) : IRequest<ItemListDto<FactionDto>>
{
    public IReadOnlyCollection<GameVersion> GameVersions { get; set; } = gameVersions;
}
