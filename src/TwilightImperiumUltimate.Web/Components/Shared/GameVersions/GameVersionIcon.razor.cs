namespace TwilightImperiumUltimate.Web.Components.Shared.GameVersions;

public partial class GameVersionIcon : TwilightImperiumBaseComponenet
{
    [Parameter]
    public GameVersion Version { get; set; }

    [Parameter]
    public EventCallback<GameVersion> OnClick { get; set; }

    public string ImagePath => PathProvider.GetGameVersionIconPath(Version);

    private async Task OnIconClick()
    {
        await OnClick.InvokeAsync(Version);
    }
}