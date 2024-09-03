using System.Globalization;
using TwilightImperiumUltimate.Web.Services.SliceGenerators;

namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceEvaluationGrid
{
    [Parameter]
    public IReadOnlyCollection<SliceModel> Slices { get; set; } = new List<SliceModel>();

    [Inject]
    private ISliceGeneratorService SliceGeneratorService { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

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

    private string GetDisplaySliceId(SliceEvaluation sliceEvaluation) => (sliceEvaluation.Id + 1).ToString(CultureInfo.InvariantCulture);

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
        List<SliceModel> slices;

        if (Slices.Count == 0)
            slices = SliceGeneratorService.Slices.ToList();
        else
            slices = Slices.ToList();

        var sliceEvaluations = new List<SliceEvaluation>();

        foreach (var slice in slices)
        {
            sliceEvaluations.Add(GetSliceEvaluation(slice));
        }

        return sliceEvaluations;
    }

    private SliceEvaluation GetSliceEvaluation(SliceModel slice)
    {
        var sliceEvaluation = new SliceEvaluation
        {
            Id = slice.Id,
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

        foreach (var systemTile in slice.SystemTiles)
        {
            if (systemTile is not null)
            {
                sliceEvaluation.Influence += systemTile.Influence;
                sliceEvaluation.Resources += systemTile.Resources;
                sliceEvaluation.LegendariesCount += systemTile.Planets.Count(x => x.IsLegendary);
                sliceEvaluation.AlphaWormholesCount += systemTile.Wormholes.Count(x => x.WormholeName == WormholeName.Alpha);
                sliceEvaluation.BetaWormholesCount += systemTile.Wormholes.Count(x => x.WormholeName == WormholeName.Beta);
                sliceEvaluation.GammaWormholesCount += systemTile.Wormholes.Count(x => x.WormholeName == WormholeName.Gamma);

                sliceEvaluation.TechnologySkips[TechnologyType.Propulsion] += systemTile.Planets.Count(x => x.TechnologySkip == TechnologyType.Propulsion);
                sliceEvaluation.TechnologySkips[TechnologyType.Biotic] += systemTile.Planets.Count(x => x.TechnologySkip == TechnologyType.Biotic);
                sliceEvaluation.TechnologySkips[TechnologyType.Cybernetic] += systemTile.Planets.Count(x => x.TechnologySkip == TechnologyType.Cybernetic);
                sliceEvaluation.TechnologySkips[TechnologyType.Warfare] += systemTile.Planets.Count(x => x.TechnologySkip == TechnologyType.Warfare);

                sliceEvaluation.PlanetTraits[PlanetTrait.Hazardous] += systemTile.Planets.Count(x => x.PlanetTrait == PlanetTrait.Hazardous);
                sliceEvaluation.PlanetTraits[PlanetTrait.Cultural] += systemTile.Planets.Count(x => x.PlanetTrait == PlanetTrait.Cultural);
                sliceEvaluation.PlanetTraits[PlanetTrait.Industrial] += systemTile.Planets.Count(x => x.PlanetTrait == PlanetTrait.Industrial);

                sliceEvaluation.OptimalResourcesSum += systemTile.Planets.Sum(x => GetOptimalResourceValue(x));
                sliceEvaluation.OptimalInfluenceSum += systemTile.Planets.Sum(x => GetOptimalInfluenceValue(x));
            }
        }

        return sliceEvaluation;
    }
}