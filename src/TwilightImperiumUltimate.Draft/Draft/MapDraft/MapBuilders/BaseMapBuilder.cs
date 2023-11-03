using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders;

internal abstract class BaseMapBuilder : IMapBuilder
{
    protected BaseMapBuilder(ISystemTileRepository systemTileRepository)
    {
        SystemTileRepository = systemTileRepository;
    }

    protected ISystemTileRepository SystemTileRepository { get; set; }

    protected abstract IEnumerable<int> MapPositions { get; set; }

    public abstract Task<Dictionary<int, SystemTile>> GenerateGalaxy(bool previewMap);
}
