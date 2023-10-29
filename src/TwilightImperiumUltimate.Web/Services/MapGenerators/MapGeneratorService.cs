using System.Net.Http.Json;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.MapGenerators;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapGeneratorService : IMapGeneratorService
{
    private readonly IMapGeneratorSettingsService _mapGeneratorSettingsService;
    private readonly HttpClient _http = new();

    public MapGeneratorService(IMapGeneratorSettingsService mapGeneratorSettingsService)
    {
        _mapGeneratorSettingsService = mapGeneratorSettingsService;
    }

    public async Task<IReadOnlyDictionary<int, int>> GenerateMapAsync()
    {
        Uri uri = new(Paths.ApiPath_MapDraft);

        MapDraftRequest request = new()
        {
            Factions = new List<FactionName>(),
            MapTemplate = _mapGeneratorSettingsService.MapTemplate,
            NumberOfPlayers = _mapGeneratorSettingsService.NumberOfPlayers,
            PlacementStyle = _mapGeneratorSettingsService.PlacementStyle,
            SystemWeight = _mapGeneratorSettingsService.SystemWeight,
        };

        var response = await _http.PostAsJsonAsync(uri, request);

        return await response.Content.ReadFromJsonAsync<IReadOnlyDictionary<int, int>>() ?? new Dictionary<int, int>();
    }
}
