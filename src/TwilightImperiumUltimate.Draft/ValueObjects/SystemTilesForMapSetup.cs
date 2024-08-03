using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Draft.ValueObjects;

public record SystemTilesForMapSetup(
    SystemTile MecatolRex,
    SystemTile EmptySystemPlaceholder,
    SystemTile EmptyHomeSystemPlaceholder,
    IReadOnlyCollection<SystemTile> HomeTiles,
    IReadOnlyCollection<SystemTile> BlueTiles,
    IReadOnlyCollection<SystemTile> RedTiles);
