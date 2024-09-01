using Microsoft.JSInterop;
using TwilightImperiumUltimate.Web.Helpers.Maps;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.MiltyDraft;

namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftMapPreview
{
    [Inject]
    private IMiltyDraftService MiltyDraftService { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private IMapToStringConverter MapToStringConverter { get; set; } = default!;

    private IReadOnlyCollection<MiltyDraftPlayerModel> Players => MiltyDraftService.Players;

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, MiltyDraftService.CurrentDraftMapTemplate.GetPreviewMapTypeFromMapTemplate());
        var generatedPositionsWithSystemTiles = MiltyDraftService.GeneratedPositionsWithSystemTiles;
        builder.AddAttribute(1, "GeneratedPositionsWithSystemTiles", generatedPositionsWithSystemTiles);
        builder.CloseComponent();
    };

    private bool IsSupportedPreviewMap() => MiltyDraftService.CurrentDraftMapTemplate.IsSupportedPreviewForMiltyDraftMap();

    private async Task HandleDownloadImage(IconType iconType)
    {
        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/MiltyDraft/MiltyDraftMapPreview.razor.js");
        await module.InvokeVoidAsync("saveMapAsImage", "draftpreviewArea", $"TI4_Ultimate_MiltyDraftPreview_{DateTime.Now.ToLocalTime().ToLongTimeString()}.png");
    }

    private async Task HandleCopyMapString()
    {
        var mapTemplate = MiltyDraftService.CurrentDraftMapTemplate;
        var mapString = await MapToStringConverter.ConvertMapToTtsString(mapTemplate, MiltyDraftService.GeneratedPositionsWithSystemTiles);

        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/MiltyDraft/MiltyDraftMapPreview.razor.js");
        await module.InvokeVoidAsync("copyToClipboard", mapString);
    }

    private string MapTtsString()
    {
        var mapTemplate = MiltyDraftService.CurrentDraftMapTemplate;
        return MapToStringConverter.ConvertMapToTtsString(mapTemplate, MiltyDraftService.GeneratedPositionsWithSystemTiles).Result;
    }
}