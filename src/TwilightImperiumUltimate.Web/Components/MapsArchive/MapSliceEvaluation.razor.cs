using TwilightImperiumUltimate.Web.Models.MapGenerators.MapData;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapsArchive;

public partial class MapSliceEvaluation
{
    [Parameter]
    public IReadOnlyDictionary<int, SystemTileModel> Map { get; set; } = default!;

    [Parameter]
    public MapTemplate MapTemplate { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IMapDataProvider SliceDataProvider { get; set; } = default!;

    private IMapData SliceData { get; set; } = default!;

    private static float GetOptimalInfluenceValue(PlanetModel planetModel)
    {
        return planetModel.Influence switch
        {
            var influence when planetModel.Influence > planetModel.Resources => influence,
            var influence when influence == planetModel.Resources => influence / 2.0f,
            var influence when influence < planetModel.Resources => 0.0f,
            _ => 0.0f,
        };
    }

    private static float GetOptimalResourceValue(PlanetModel planetModel)
    {
        return planetModel.Resources switch
        {
            var resources when planetModel.Resources > planetModel.Influence => resources,
            var resources when resources == planetModel.Influence => resources / 2.0f,
            var resources when resources < planetModel.Influence => 0.0f,
            _ => 0.0f,
        };
    }

    private string GetPlanetTraitPath(PlanetTrait planetTrait)
    {
        return PathProvider.GetPlanetTraitPath(planetTrait);
    }

    private string GetTechnologyPath(TechnologyType technologyType)
    {
        return PathProvider.GetTechnologyIconPath(technologyType);
    }

    private List<SliceEvaluation> GetSliceEvaluations()
    {
        var sliceData = SliceDataProvider.GetMapData(MapTemplate);
        var sliceEvaluations = new List<SliceEvaluation>();

        foreach (var slice in sliceData.SlicePositions)
        {
            var sliceEvaluation = new SliceEvaluation
            {
                Id = slice.Key,
                TechnologySkips = new Dictionary<TechnologyType, int>()
                {
                    [TechnologyType.Propulsion] = 0,
                    [TechnologyType.Biotic] = 0,
                    [TechnologyType.Cybernetic] = 0,
                    [TechnologyType.Warfare] = 0,
                },
                PlanetTraits = new Dictionary<PlanetTrait, int>()
                {
                    [PlanetTrait.Hazardous] = 0,
                    [PlanetTrait.Cultural] = 0,
                    [PlanetTrait.Industrial] = 0,
                },
            };

            foreach (var position in slice.Value)
            {
                var systemTileModel = Map[position];
                if (systemTileModel is not null)
                {
                    sliceEvaluation.Influence += systemTileModel.Influence;
                    sliceEvaluation.Resources += systemTileModel.Resources;

                    sliceEvaluation.LegendariesCount += systemTileModel.Planets.Count(x => x.IsLegendary);
                    sliceEvaluation.AlphaWormholesCount += systemTileModel.Wormholes.Count(x => x.WormholeName == WormholeName.Alpha);
                    sliceEvaluation.BetaWormholesCount += systemTileModel.Wormholes.Count(x => x.WormholeName == WormholeName.Beta);
                    sliceEvaluation.GammaWormholesCount += systemTileModel.Wormholes.Count(x => x.WormholeName == WormholeName.Gamma);

                    sliceEvaluation.TechnologySkips[TechnologyType.Propulsion] += systemTileModel.Planets.Count(x => x.TechnologySkip == TechnologyType.Propulsion);
                    sliceEvaluation.TechnologySkips[TechnologyType.Biotic] += systemTileModel.Planets.Count(x => x.TechnologySkip == TechnologyType.Biotic);
                    sliceEvaluation.TechnologySkips[TechnologyType.Cybernetic] += systemTileModel.Planets.Count(x => x.TechnologySkip == TechnologyType.Cybernetic);
                    sliceEvaluation.TechnologySkips[TechnologyType.Warfare] += systemTileModel.Planets.Count(x => x.TechnologySkip == TechnologyType.Warfare);

                    sliceEvaluation.PlanetTraits[PlanetTrait.Hazardous] += systemTileModel.Planets.Count(x => x.PlanetTrait == PlanetTrait.Hazardous);
                    sliceEvaluation.PlanetTraits[PlanetTrait.Cultural] += systemTileModel.Planets.Count(x => x.PlanetTrait == PlanetTrait.Cultural);
                    sliceEvaluation.PlanetTraits[PlanetTrait.Industrial] += systemTileModel.Planets.Count(x => x.PlanetTrait == PlanetTrait.Industrial);

                    sliceEvaluation.OptimalResourcesSum += systemTileModel.Planets.Sum(x => GetOptimalResourceValue(x));
                    sliceEvaluation.OptimalInfluenceSum += systemTileModel.Planets.Sum(x => GetOptimalInfluenceValue(x));
                }
            }

            sliceEvaluations.Add(sliceEvaluation);
        }

        return sliceEvaluations;
    }
}