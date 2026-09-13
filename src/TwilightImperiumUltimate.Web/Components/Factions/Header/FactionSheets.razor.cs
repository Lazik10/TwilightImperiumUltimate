namespace TwilightImperiumUltimate.Web.Components.Factions.Header;

public partial class FactionSheets
{
    private FactionName _factionName = FactionName.None;
    private bool _showImageOverlay;
    private string? _imagePath;

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    protected override void OnInitialized()
    {
        _factionName = FactionProvider.CurrentFactionName;
    }

    private void ShowOverlayImage(bool isFront, bool isObsidian)
    {
        var factionName = _factionName.ToString();
        if (_factionName == FactionName.TheFirmamentTheObsidian)
        {
            if (isObsidian)
                factionName = "TheObsidian";
            else
                factionName = "TheFirmament";
        }

        _imagePath = PathProvider.GetFactionSheetPath(factionName, isFront);
        _showImageOverlay = true;
    }

    private void Close()
    {
        _showImageOverlay = false;
    }
}
