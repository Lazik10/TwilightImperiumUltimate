namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class HexTileMenu
{
    private bool _showUnusedOnly;

    private IEnumerable<int> HexTiles { get; set; } = Enumerable.Range(1, 81);

    private void UnusedOnly()
    {
        if (_showUnusedOnly)
            HexTiles = Enumerable.Range(1, 81);
        else
            HexTiles = Enumerable.Range(1, 81).Where(x => x % 15 == 0);

        _showUnusedOnly = !_showUnusedOnly;
    }
}
