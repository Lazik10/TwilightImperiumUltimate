namespace TwilightImperiumUltimate.Web.Components.Shared.GameVersions;

public partial class GameVersionPicker : TwilightImperiumBaseComponenet
{
    private List<GameVersion> _versions = new List<GameVersion>();

    [Parameter]
    public EventCallback<GameVersion> OnVersionSelected { get; set; }

    protected override void OnInitialized()
    {
        _versions = Enum.GetValues<GameVersion>()
            .Except(
            [
                GameVersion.CodexOrdinian,
                GameVersion.CodexAffinity,
                GameVersion.CodexRecolo,
                GameVersion.CodexLiberation,
                GameVersion.UnchartedSpace,
                GameVersion.AscendantSun,
                GameVersion.TtsHyperlines,
                GameVersion.Custom,
                GameVersion.Deprecated,
            ])
            .ToList();
    }

    private async Task OnGameVersionClick(GameVersion version)
    {
        await OnVersionSelected.InvokeAsync(version);
    }
}