using Microsoft.JSInterop;
using TwilightImperiumUltimate.Web.Pages.Tools;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.SliceGenerators;

namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceGeneratorGrid
{
    private SliceGeneratorMenuItem _selectedMenuItem = SliceGeneratorMenuItem.SliceGenerator;

    private SliceHexTileMenu? _sliceHexTileMenu;

    private List<SliceModel> _slices = new List<SliceModel>();

    public IReadOnlyCollection<SliceModel> Slices => _slices;

    [CascadingParameter(Name = "SliceGeneratorPage")]
    public SliceGenerator SliceGeneratorPage { get; set; } = default!;

    [CascadingParameter(Name = "TilesValue")]
    public string TilesImported { get; set; } = string.Empty;

    [Inject]
    private ISliceGeneratorService SliceGeneratorService { get; set; } = default!;

    [Inject]
    private ISliceGeneratorSettingsService SliceGeneratorSettingsService { get; set; } = default!;

    [Inject]
    private ISlicesDraftToStringConverter SlicesDraftToStringConverter { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    public void UpdateSliceHexTileMenu()
    {
        _sliceHexTileMenu?.RefreshSystemTilesMenu();
    }

    protected override async Task OnInitializedAsync()
    {
        await SlicesDraftToStringConverter.InitializeSystemTiles();

        if (!string.IsNullOrEmpty(TilesImported))
        {
            await SlicesDraftToStringConverter.GenerateSlicesFromShareLink(TilesImported);
            _slices = SlicesDraftToStringConverter.Slices.ToList();
            await SliceGeneratorService.SetImportedSlices(_slices);
            await SliceGeneratorSettingsService.SetNumberOfSlices(_slices.Count);
        }
        else
        {
            await SliceGeneratorService.GeneratePreviewSlices();
            _slices = GetUpdatedSlices();
        }
    }

    private List<SliceModel> GetUpdatedSlices()
    {
        return SliceGeneratorService.Slices.ToList();
    }

    private async Task GenerateSlices()
    {
        await SliceGeneratorService.GenerateSlices(false);
        _slices = GetUpdatedSlices();
        StateHasChanged();
    }

    private void OnMenuItemClick(SliceGeneratorMenuItem item)
    {
        _selectedMenuItem = item;
        StateHasChanged();
    }

    private Task UpdateSlices()
    {
        _slices = GetUpdatedSlices();
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
            var slicesString = await GetSlicesString();
            var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/SliceGenerators/SliceGeneratorGrid.razor.js");
            await module.InvokeVoidAsync("copyToClipboard", slicesString);
        }

        if (iconType == IconType.ImportMapString)
        {
            await SliceGeneratorService.SetImportedSlices(SlicesDraftToStringConverter.Slices.ToList());
            var newSliceCount = SliceGeneratorService.Slices.Count;
            await SliceGeneratorSettingsService.SetNumberOfSlices(newSliceCount);
            await UpdateSlices();
        }

        if (iconType == IconType.ShareMap)
        {
            var slices = SliceGeneratorService.Slices;
            var slicesString = await SlicesDraftToStringConverter.ConvertSlicesDraftToString(slices);
            var shareString = await SlicesDraftToStringConverter.CreateShareLink(slicesString);
            var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/SliceGenerators/SliceGeneratorGrid.razor.js");
            await module.InvokeVoidAsync("copyToClipboard", shareString);
        }

        if (iconType == IconType.Archive)
        {
            var sliceDraftString = await SlicesDraftToStringConverter.ConvertSlicesDraftToString(SliceGeneratorService.Slices);
            var map64BaseString = await SlicesDraftToStringConverter.ConvertSliceDraftStringToBase64String(sliceDraftString);
            NavigationManager.NavigateTo(NavigationManager.BaseUri + $"community/add-slice-draft-to-archive?tiles={map64BaseString}");
        }
    }

    private async Task DownloadSliceImage()
    {
        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/SliceGenerators/SliceGeneratorGrid.razor.js");
        await module.InvokeVoidAsync("saveMapAsImage", "sliceArea", $"TI4_Ultimate_Slices_{DateTime.Now.ToLocalTime().ToLongTimeString()}.png");
    }

    private async Task<string> GetSlicesString()
    {
        return await SlicesDraftToStringConverter.ConvertSlicesDraftToString(Slices);
    }
}