using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class TiglInfo
{
    private int _sectionFontSize = 32;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private void NavigateToRegister() => NavigationManager.NavigateTo(Pages.TiglRegister);

    private void NavigateToReportGame() => NavigationManager.NavigateTo(Pages.TiglReportGame);

    private async Task NavigateToDiscord() => await JSRuntime.InvokeVoidAsync("open", "https://discord.gg/hxGUwyBmcT", "_blank");

    private async Task NavigateToManifest() => await JSRuntime.InvokeVoidAsync("open", "https://docs.google.com/document/d/1WoFPiluIz5cw80x1-WxUeckADIdYszNFZFTb6d648tk", "_blank");
}
