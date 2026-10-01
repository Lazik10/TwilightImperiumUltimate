using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class TiglInfo
{
    private readonly MarkupString _overviewContent = (MarkupString)Strings.TiglInfo_OverviewContent;

    private readonly MarkupString _tiglContent = (MarkupString)Strings.TiglInfo_TiglContent;

    private readonly MarkupString _codeOfConductContent = (MarkupString)Strings.TiglInfo_CodeOfConductContent;

    private readonly MarkupString _gettingStartedContent = (MarkupString)Strings.TiglInfo_GettingStartedContent;

    private readonly MarkupString _allowedContent = (MarkupString)Strings.TiglInfo_AllowedContent;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private void NavigateToRegister() => NavigationManager.NavigateTo(Pages.TiglRegister);

    private void NavigateToReportGame() => NavigationManager.NavigateTo(Pages.TiglReportGame);

    private async Task NavigateToDiscord() => await JSRuntime.InvokeVoidAsync("open", "https://discord.gg/hxGUwyBmcT", "_blank");

    private async Task NavigateToManifest() => await JSRuntime.InvokeVoidAsync("open", "https://docs.google.com/document/d/1WoFPiluIz5cw80x1-WxUeckADIdYszNFZFTb6d648tk", "_blank");
}
