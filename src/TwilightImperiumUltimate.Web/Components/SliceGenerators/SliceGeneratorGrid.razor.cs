using Microsoft.JSInterop;
using System.Text;
using TwilightImperiumUltimate.Web.Pages.Tools;
using TwilightImperiumUltimate.Web.Services.SliceGenerators;

namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceGeneratorGrid
{
    private SliceGeneratorMenuItem _selectedMenuItem = SliceGeneratorMenuItem.SliceGenerator;

    private SliceHexTileMenu? _sliceHexTileMenu;

    public List<SliceModel> Slices => GetUpdatedSlices();

    [CascadingParameter(Name = "SliceGeneratorPage")]
    public SliceGenerator SliceGeneratorPage { get; set; } = default!;

    [Inject]
    private ISliceGeneratorService SliceGeneratorService { get; set; } = default!;

    [Inject]
    private ISliceGeneratorSettingsService SliceGeneratorSettingsService { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    public void UpdateSliceHexTileMenu()
    {
        _sliceHexTileMenu?.RefreshSystemTilesMenu();
    }

    protected override async Task OnInitializedAsync()
    {
        await SliceGeneratorService.GeneratePreviewSlices();
    }

    private List<SliceModel> GetUpdatedSlices()
    {
        return SliceGeneratorService.Slices.ToList();
    }

    private async Task GenerateSlices()
    {
        await SliceGeneratorService.GenerateSlices(false);
        StateHasChanged();
    }

    private void OnMenuItemClick(SliceGeneratorMenuItem item)
    {
        _selectedMenuItem = item;
        StateHasChanged();
    }

    private Task UpdateSlices()
    {
        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task HandleQuickMenuClick(IconType iconType)
    {
        if (iconType == IconType.DownloadMapImage)
            await DownloadSliceImage();

        if (iconType == IconType.Hashtag)
        {
            if (SliceGeneratorSettingsService.SystemTileOverlay != SystemTileOverlay.Id)
                await SliceGeneratorSettingsService.UpdateSystemTileOverlay(SystemTileOverlay.Id);
            else
                await SliceGeneratorSettingsService.UpdateSystemTileOverlay(SystemTileOverlay.None);

            await UpdateSlices();

            _sliceHexTileMenu?.RefreshSystemTilesMenu();
            StateHasChanged();
        }

        if (iconType == IconType.CopyMapString)
        {
            var slicesString = GetSlicesString();
            var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/SliceGenerators/SliceGeneratorGrid.razor.js");
            await module.InvokeVoidAsync("copyToClipboard", slicesString);
        }
    }

    private async Task DownloadSliceImage()
    {
        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/SliceGenerators/SliceGeneratorGrid.razor.js");
        await module.InvokeVoidAsync("saveMapAsImage", "sliceArea", $"TI4_Ultimate_Slices_{DateTime.Now.ToLocalTime().ToLongTimeString()}.png");
    }

    private string GetSlicesString()
    {
        StringBuilder sliceString = new();
        foreach (var slice in Slices)
        {
            foreach (var systemTile in slice.SystemTiles.Skip(1))
            {
                sliceString.Append(systemTile.SystemTileCode);
                sliceString.Append(',');
            }

            sliceString.Append('\n');
        }

        return sliceString.ToString();
    }
}