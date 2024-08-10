namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapEvaluationService(
    IMapGeneratorService mapGeneratorService,
    IMapGeneratorSettingsService mapGeneratorSettingsService,
    ILogger<MapEvaluationService> logger)
    : IMapEvaluationService
{
    private readonly IMapGeneratorService _mapGeneratorService = mapGeneratorService;
    private readonly IMapGeneratorSettingsService _mapGeneratorSettingsService = mapGeneratorSettingsService;
    private readonly ILogger<MapEvaluationService> _logger = logger;

    public Task<MapEvaluations> GetMapEvaluation()
    {
        var resourceResults = GetPossibleResourceValuesForMap();
        var influenceResults = GetPossibleInfluenceValuesForMap();
        var planetResults = GetPossiblePlanetValuesForMap();
        var industiralPlanetsResults = GetPossiblePlanetTraitValuesForMap(PlanetTrait.Industrial);
        var culturalPlanetsResults = GetPossiblePlanetTraitValuesForMap(PlanetTrait.Cultural);
        var hazardousPlanetsResults = GetPossiblePlanetTraitValuesForMap(PlanetTrait.Hazardous);
        var frontierResults = GetPossibleFrontierValuesForMap();
        var legendaryResults = GetPossibleLegendaryValuesForMap();
        var wormholeResults = GetPossibleWormholeValuesForMap();

        var mapEvaluation = new MapEvaluations
        {
            Resources = resourceResults.Current,
            Influence = influenceResults.Current,
            PlanetsCount = planetResults.Current,
            TotalResourcesMin = resourceResults.Min,
            TotalInfluenceMin = influenceResults.Min,
            TotalPlanetsCountMin = planetResults.Min,
            TotalResourcesMax = resourceResults.Max,
            TotalInfluenceMax = influenceResults.Max,
            TotalPlanetsCountMax = planetResults.Max,
            IndustrialPlanets = industiralPlanetsResults.Current,
            IndustrialPlanetsMin = industiralPlanetsResults.Min,
            IndustrialPlanetsMax = industiralPlanetsResults.Max,
            CulturalPlanets = culturalPlanetsResults.Current,
            CulturalPlanetsMin = culturalPlanetsResults.Min,
            CulturalPlanetsMax = culturalPlanetsResults.Max,
            HazardousPlanets = hazardousPlanetsResults.Current,
            HazardousPlanetsMin = hazardousPlanetsResults.Min,
            HazardousPlanetsMax = hazardousPlanetsResults.Max,
            FrontierTokens = frontierResults.Current,
            FrontierTokensMin = frontierResults.Min,
            FrontierTokensMax = frontierResults.Max,
            LegendaryPlanets = legendaryResults.Current,
            LegendaryPlanetsMin = legendaryResults.Min,
            LegendaryPlanetsMax = legendaryResults.Max,
            Wormholes = wormholeResults.Current,
            WormholesMin = wormholeResults.Min,
            WormholesMax = wormholeResults.Max,
        };

        return Task.FromResult(mapEvaluation);
    }

    private (int Min, int Current, int Max) GetPossibleInfluenceValuesForMap()
    {
        var systemTiles = _mapGeneratorService.GeneratedPositionsWithSystemTiles.Values.ToList();

        var mecatolRex = _mapGeneratorService.AllSystemTiles.Single(x => x.SystemTileName == SystemTileName.Tile18);

        var allSystemTiles = _mapGeneratorService.AllSystemTiles
            .Where(x => _mapGeneratorSettingsService.GameVersions.Contains(x.GameVersion)
                && x.SystemTileName != SystemTileName.Tile18
                && x.FactionName == FactionName.None)
            .ToList();

        var maxRedTiles = systemTiles.Count(x => x.SystemTileCategory == SystemTileCategory.Red);
        var maxBlueTiles = systemTiles.Count(x => x.SystemTileCategory == SystemTileCategory.Blue);

        var bestRedSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Red)
            .OrderByDescending(x => x.Influence)
            .Take(maxRedTiles)
            .ToList();

        var bestBlueSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Blue)
            .OrderByDescending(x => x.Influence)
            .Take(maxBlueTiles)
            .ToList();

        var worstRedSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Red)
            .OrderBy(x => x.Influence)
            .Take(maxRedTiles)
            .ToList();

        var worstBlueSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Blue)
            .OrderBy(x => x.Influence)
            .Take(maxBlueTiles)
            .ToList();

        var maxInfluence = bestRedSystemTiles.Sum(x => x.Influence) + bestBlueSystemTiles.Sum(x => x.Influence);
        var minInfluence = worstRedSystemTiles.Sum(x => x.Influence) + worstBlueSystemTiles.Sum(x => x.Influence);
        var actualInfluence = systemTiles.Sum(x => x.Influence);

        _logger.LogInformation("Max Red Tiles: {MaxRedTiles}, Max Blue Tiles: {MaxBlueTiles}", maxRedTiles, maxBlueTiles);

        _logger.LogInformation(
            "Influence: Best red system tiles count: {BestRedCount} and combined value: {BestRedValue}\nBest blue system tiles count: {BestBlueCount} and combined value: {BestBlueValue}",
            bestRedSystemTiles.Count,
            bestRedSystemTiles.Sum(x => x.Resources),
            bestBlueSystemTiles.Count,
            bestBlueSystemTiles.Sum(x => x.Resources));

        _logger.LogInformation(
            "Influence: Worst red system tiles count: {WorstRedCount} and combined value: {WorstRedValue}\nWorst blue system tiles count: {WorstBlueCount} and combined value: {WorstBlueValue}",
            worstRedSystemTiles.Count,
            worstRedSystemTiles.Sum(x => x.Resources),
            worstBlueSystemTiles.Count,
            worstBlueSystemTiles.Sum(x => x.Resources));

        _logger.LogInformation(
            "Influence: Min Influence: {MinInfluence}, Actual Influence: {ActualInfluence} Max Influence: {MaxInfleunce}",
            minInfluence + mecatolRex.Influence,
            actualInfluence,
            maxInfluence + mecatolRex.Influence);

        return (minInfluence + mecatolRex.Influence, actualInfluence, maxInfluence + mecatolRex.Influence);
    }

    private (int Min, int Current, int Max) GetPossibleResourceValuesForMap()
    {
        var systemTiles = _mapGeneratorService.GeneratedPositionsWithSystemTiles.Values.ToList();

        var mecatolRex = _mapGeneratorService.AllSystemTiles.Single(x => x.SystemTileName == SystemTileName.Tile18);

        var allSystemTiles = _mapGeneratorService.AllSystemTiles
            .Where(x => _mapGeneratorSettingsService.GameVersions.Contains(x.GameVersion)
                && x.FactionName == FactionName.None
                && x.SystemTileCategory != SystemTileCategory.Green
                && x.SystemTileCategory != SystemTileCategory.MecatolRex
                && x.SystemTileCategory != SystemTileCategory.ExternalMapTile
                && x.SystemTileCategory != SystemTileCategory.Hyperlane)
            .ToList();

        var maxRedTiles = systemTiles.Count(x => x.SystemTileCategory == SystemTileCategory.Red);
        var maxBlueTiles = systemTiles.Count(x => x.SystemTileCategory == SystemTileCategory.Blue);

        var bestRedSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Red)
            .OrderByDescending(x => x.Resources)
            .Take(maxRedTiles)
            .ToList();

        var bestBlueSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Blue)
            .OrderByDescending(x => x.Resources)
            .Take(maxBlueTiles)
            .ToList();

        var worstRedSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Red)
            .OrderBy(x => x.Resources)
            .Take(maxRedTiles)
            .ToList();

        var worstBlueSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Blue)
            .OrderBy(x => x.Resources)
            .Take(maxBlueTiles)
            .ToList();

        var maxResources = bestRedSystemTiles.Sum(x => x.Resources) + bestBlueSystemTiles.Sum(x => x.Resources);
        var minResources = worstRedSystemTiles.Sum(x => x.Resources) + worstBlueSystemTiles.Sum(x => x.Resources);
        var actualResources = systemTiles.Sum(x => x.Resources);

        _logger.LogInformation("Max Red Tiles: {MaxRedTiles}, Max Blue Tiles: {MaxBlueTiles}", maxRedTiles, maxBlueTiles);

        _logger.LogInformation(
            "Resources: Best red system tiles count: {BestRedCount} and combined value: {BestRedValue}\nBest blue system tiles count: {BestBlueCount} and combined value: {BestBlueValue}",
            bestRedSystemTiles.Count,
            bestRedSystemTiles.Sum(x => x.Resources),
            bestBlueSystemTiles.Count,
            bestBlueSystemTiles.Sum(x => x.Resources));

        _logger.LogInformation(
            "Resources: Worst red system tiles count: {WorstRedCount} and combined value: {WorstRedValue}\nWorst blue system tiles count: {WorstBlueCount} and combined value: {WorstBlueValue}",
            worstRedSystemTiles.Count,
            worstRedSystemTiles.Sum(x => x.Resources),
            worstBlueSystemTiles.Count,
            worstBlueSystemTiles.Sum(x => x.Resources));

        _logger.LogInformation(
            "Resources: Min Resources: {MinResources}, Actual Resources: {ActualResources} Max Resources: {MaxResources}",
            minResources + mecatolRex.Resources,
            actualResources,
            maxResources + mecatolRex.Resources);

        return (minResources + mecatolRex.Resources, actualResources, maxResources + mecatolRex.Resources);
    }

    private (int Min, int Current, int Max) GetPossiblePlanetValuesForMap()
    {
        var systemTiles = _mapGeneratorService.GeneratedPositionsWithSystemTiles.Values.ToList();

        var allSystemTiles = _mapGeneratorService.AllSystemTiles
            .Where(x => _mapGeneratorSettingsService.GameVersions.Contains(x.GameVersion)
                && x.FactionName == FactionName.None
                && x.SystemTileCategory != SystemTileCategory.Green
                && x.SystemTileCategory != SystemTileCategory.MecatolRex
                && x.SystemTileCategory != SystemTileCategory.ExternalMapTile
                && x.SystemTileCategory != SystemTileCategory.Hyperlane)
            .ToList();

        var maxRedTiles = systemTiles.Count(x => x.SystemTileCategory == SystemTileCategory.Red);
        var maxBlueTiles = systemTiles.Count(x => x.SystemTileCategory == SystemTileCategory.Blue);

        var bestRedSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Red)
            .OrderByDescending(x => x.Planets.Count)
            .Take(maxRedTiles)
            .ToList();

        var bestBlueSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Blue)
            .OrderByDescending(x => x.Planets.Count)
            .Take(maxBlueTiles)
            .ToList();

        var worstRedSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Red)
            .OrderBy(x => x.Planets.Count)
            .Take(maxRedTiles)
            .ToList();

        var worstBlueSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Blue)
            .OrderBy(x => x.Planets.Count)
            .Take(maxBlueTiles)
            .ToList();

        var maxPlanets = bestRedSystemTiles.Sum(x => x.Planets.Count) + bestBlueSystemTiles.Sum(x => x.Planets.Count);
        var minPlanets = worstRedSystemTiles.Sum(x => x.Planets.Count) + worstBlueSystemTiles.Sum(x => x.Planets.Count);
        var actualPlanets = systemTiles.Sum(x => x.Planets.Count);

        _logger.LogInformation(
            "Planets: Best red system tiles count: {BestRedCount} and combined planets count: {BestRedValue}\nBest blue system tiles count: {BestBlueCount} and combined planets count: {BestBlueValue}",
            bestRedSystemTiles.Count,
            bestRedSystemTiles.Sum(x => x.Planets.Count),
            bestBlueSystemTiles.Count,
            bestBlueSystemTiles.Sum(x => x.Planets.Count));

        _logger.LogInformation(
            "Planets: Worst red system tiles count: {WorstRedCount} and combined planets count: {WorstRedValue}\nWorst blue system tiles count: {WorstBlueCount} and combined planets count: {WorstBlueValue}",
            worstRedSystemTiles.Count,
            worstRedSystemTiles.Sum(x => x.Planets.Count),
            worstBlueSystemTiles.Count,
            worstBlueSystemTiles.Sum(x => x.Planets.Count));

        _logger.LogInformation("Min Planets: {MinPlanets}, Actual Planets: {ActualPlanets} Max Planets: {MaxPlanets}, ", minPlanets + 1, actualPlanets, maxPlanets + 1);

        // Add one for mecatol rex
        return (minPlanets + 1, actualPlanets, maxPlanets + 1);
    }

    private (int Min, int Current, int Max) GetPossiblePlanetTraitValuesForMap(PlanetTrait planetTrait)
    {
        var systemTiles = _mapGeneratorService.GeneratedPositionsWithSystemTiles.Values.ToList();

        var allSystemTiles = _mapGeneratorService.AllSystemTiles
            .Where(x => _mapGeneratorSettingsService.GameVersions.Contains(x.GameVersion)
                && x.FactionName == FactionName.None
                && x.SystemTileCategory != SystemTileCategory.Green
                && x.SystemTileCategory != SystemTileCategory.MecatolRex
                && x.SystemTileCategory != SystemTileCategory.ExternalMapTile
                && x.SystemTileCategory != SystemTileCategory.Hyperlane)
            .ToList();

        var maxRedTiles = systemTiles.Count(x => x.SystemTileCategory == SystemTileCategory.Red);
        var maxBlueTiles = systemTiles.Count(x => x.SystemTileCategory == SystemTileCategory.Blue);

        var bestRedSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Red)
            .OrderByDescending(x => x.Planets.Count(x => x.PlanetTrait == planetTrait))
            .Take(maxRedTiles)
            .ToList();

        var bestBlueSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Blue)
            .OrderByDescending(x => x.Planets.Count(x => x.PlanetTrait == planetTrait))
            .Take(maxBlueTiles)
            .ToList();

        var worstRedSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Red)
            .OrderBy(x => x.Planets.Count(x => x.PlanetTrait == planetTrait))
            .Take(maxRedTiles)
            .ToList();

        var worstBlueSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Blue)
            .OrderBy(x => x.Planets.Count(x => x.PlanetTrait == planetTrait))
            .Take(maxBlueTiles)
            .ToList();

        var maxPlanets = bestRedSystemTiles.Sum(x => x.Planets.Count(x => x.PlanetTrait == planetTrait))
            + bestBlueSystemTiles.Sum(x => x.Planets.Count(x => x.PlanetTrait == planetTrait));

        var minPlanets = worstRedSystemTiles.Sum(x => x.Planets.Count(x => x.PlanetTrait == planetTrait))
            + worstBlueSystemTiles.Sum(x => x.Planets.Count(x => x.PlanetTrait == planetTrait));
        var actualPlanets = systemTiles.Sum(x => x.Planets.Count(x => x.PlanetTrait == planetTrait));

        _logger.LogInformation(
            "Planets: Best red system tiles count: {BestRedCount} and combined trait planets count: {BestRedValue}\nBest blue system tiles count: {BestBlueCount} and combined trait planets count: {BestBlueValue}",
            bestRedSystemTiles.Count,
            bestRedSystemTiles.Sum(x => x.Planets.Count(x => x.PlanetTrait == planetTrait)),
            bestBlueSystemTiles.Count,
            bestBlueSystemTiles.Sum(x => x.Planets.Count(x => x.PlanetTrait == planetTrait)));

        _logger.LogInformation(
            "Planets: Worst red system tiles count: {WorstRedCount} and combined trait planets count: {WorstRedValue}\nWorst blue system tiles count: {WorstBlueCount} and combined trait planets count: {WorstBlueValue}",
            worstRedSystemTiles.Count,
            worstRedSystemTiles.Sum(x => x.Planets.Count(x => x.PlanetTrait == planetTrait)),
            worstBlueSystemTiles.Count,
            worstBlueSystemTiles.Sum(x => x.Planets.Count(x => x.PlanetTrait == planetTrait)));

        _logger.LogInformation("Min trait Planets: {MinPlanets}, Actual trait Planets: {ActualPlanets} Max trait Planets: {MaxPlanets}, ", minPlanets, actualPlanets, maxPlanets);

        return (minPlanets, actualPlanets, maxPlanets);
    }

    private (int Min, int Current, int Max) GetPossibleFrontierValuesForMap()
    {
        var systemTiles = _mapGeneratorService.GeneratedPositionsWithSystemTiles.Values.ToList();

        var allSystemTiles = _mapGeneratorService.AllSystemTiles
            .Where(x => _mapGeneratorSettingsService.GameVersions.Contains(x.GameVersion)
                && x.FactionName == FactionName.None
                && x.SystemTileCategory != SystemTileCategory.Green
                && x.SystemTileCategory != SystemTileCategory.MecatolRex
                && x.SystemTileCategory != SystemTileCategory.ExternalMapTile
                && x.SystemTileCategory != SystemTileCategory.Hyperlane)
            .ToList();

        var maxRedTiles = systemTiles.Count(x => x.SystemTileCategory == SystemTileCategory.Red);
        var maxBlueTiles = systemTiles.Count(x => x.SystemTileCategory == SystemTileCategory.Blue);

        var bestRedSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Red)
            .OrderByDescending(x => !x.HasPlanets)
            .Take(maxRedTiles)
            .ToList();

        var bestBlueSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Blue)
            .OrderByDescending(x => !x.HasPlanets)
            .Take(maxBlueTiles)
            .ToList();

        var worstRedSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Red)
            .OrderBy(x => !x.HasPlanets)
            .Take(maxRedTiles)
            .ToList();

        var worstBlueSystemTiles = allSystemTiles
            .Where(x => x.SystemTileCategory == SystemTileCategory.Blue)
            .OrderBy(x => !x.HasPlanets)
            .Take(maxBlueTiles)
            .ToList();

        var maxFrontiers = bestRedSystemTiles.Count(x => !x.HasPlanets)
            + bestBlueSystemTiles.Count(x => !x.HasPlanets);

        var minFrontiers = worstRedSystemTiles.Count(x => !x.HasPlanets)
            + worstBlueSystemTiles.Count(x => !x.HasPlanets);

        var actualFrontiers = systemTiles.Count(x => !x.HasPlanets && (x.SystemTileCategory == SystemTileCategory.Blue || x.SystemTileCategory == SystemTileCategory.Red));

        _logger.LogInformation(
            "Planets: Best red system tiles count: {BestRedCount} and combined frontier count: {BestRedValue}\nBest blue system tiles count: {BestBlueCount} and combined frontier count: {BestBlueValue}",
            bestRedSystemTiles.Count,
            bestRedSystemTiles.Count(x => !x.HasPlanets),
            bestBlueSystemTiles.Count,
            bestBlueSystemTiles.Count(x => !x.HasPlanets));

        _logger.LogInformation(
            "Planets: Worst red system tiles count: {WorstRedCount} and combined frontier count: {WorstRedValue}\nWorst blue system tiles count: {WorstBlueCount} and combined frontier count: {WorstBlueValue}",
            worstRedSystemTiles.Count,
            worstRedSystemTiles.Count(x => !x.HasPlanets),
            worstBlueSystemTiles.Count,
            worstBlueSystemTiles.Count(x => !x.HasPlanets));

        _logger.LogInformation("Min frontier: {MinPlanets}, Actual frontier: {ActualPlanets} Max frontier: {MaxPlanets}, ", minFrontiers, actualFrontiers, maxFrontiers);

        return (minFrontiers, actualFrontiers, maxFrontiers);
    }

    private (int Min, int Current, int Max) GetPossibleLegendaryValuesForMap()
    {
        var systemTiles = _mapGeneratorService.GeneratedPositionsWithSystemTiles.Values.ToList();

        var allSystemTiles = _mapGeneratorService.AllSystemTiles
            .Where(x => _mapGeneratorSettingsService.GameVersions.Contains(x.GameVersion)
                && x.FactionName == FactionName.None
                && x.SystemTileCategory != SystemTileCategory.Green
                && x.SystemTileCategory != SystemTileCategory.MecatolRex
                && x.SystemTileCategory != SystemTileCategory.ExternalMapTile
                && x.SystemTileCategory != SystemTileCategory.Hyperlane)
            .ToList();

        var currentLegendaryCount = systemTiles.SelectMany(x => x.Planets).Count(x => x.IsLegendary);
        var maxPossibleLegendaryCount = allSystemTiles.SelectMany(x => x.Planets).Count(x => x.IsLegendary);

        return (0, currentLegendaryCount, maxPossibleLegendaryCount);
    }

    private (int Min, int Current, int Max) GetPossibleWormholeValuesForMap()
    {
        var systemTiles = _mapGeneratorService.GeneratedPositionsWithSystemTiles.Values.ToList();

        var allSystemTiles = _mapGeneratorService.AllSystemTiles
            .Where(x => _mapGeneratorSettingsService.GameVersions.Contains(x.GameVersion)
                && x.FactionName == FactionName.None
                && x.SystemTileCategory != SystemTileCategory.Green
                && x.SystemTileCategory != SystemTileCategory.MecatolRex
                && x.SystemTileCategory != SystemTileCategory.ExternalMapTile
                && x.SystemTileCategory != SystemTileCategory.Hyperlane)
            .ToList();

        var currenWormholeCount = systemTiles.Sum(x => x.Wormholes.Count);
        var maxPossibleWormholesCount = allSystemTiles.Sum(x => x.Wormholes.Count);

        return (0, currenWormholeCount, maxPossibleWormholesCount);
    }
}
