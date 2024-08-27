using Microsoft.JSInterop;
using System.Reflection.Emit;
using TwilightImperiumUltimate.Web.Components.MiltyDraft.MapGrids;
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
        builder.OpenComponent(0, AssignMapType());
        var generatedPositionsWithSystemTiles = MiltyDraftService.GeneratedPositionsWithSystemTiles;
        builder.AddAttribute(1, "GeneratedPositionsWithSystemTiles", generatedPositionsWithSystemTiles);
        builder.CloseComponent();
    };

    private Type AssignMapType()
    {
        var mapTemplate = MiltyDraftService.CurrentDraftMapTemplate;

        var mapTemplateType = mapTemplate switch
        {
            MapTemplate.FivePlayersMediumHyperlineMap => typeof(MiltyDraftFivePlayersMediumHyperlineMap),
            MapTemplate.SixPlayersMediumMap => typeof(MiltyDraftSixPlayersMediumMap),
            MapTemplate.SevenPlayersLargeWarpMap => typeof(MiltyDraftSevenPlayersLargeWarpMap),
            MapTemplate.EightPlayersLargeWarpMap => typeof(MiltyDraftEightPlayersLargeWarpMap),
            _ => typeof(MiltyDraftSixPlayersMediumMap),
        };

        return mapTemplateType;
    }

    private bool IsSupportedMapPreview()
    {
        var supportedMapTemplates = new List<MapTemplate>()
        {
            MapTemplate.FivePlayersMediumHyperlineMap,
            MapTemplate.SixPlayersMediumMap,
            MapTemplate.SevenPlayersLargeWarpMap,
            MapTemplate.EightPlayersLargeWarpMap,
        };

        return supportedMapTemplates.Contains(MiltyDraftService.CurrentDraftMapTemplate);
    }

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