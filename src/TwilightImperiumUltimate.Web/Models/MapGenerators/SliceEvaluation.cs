namespace TwilightImperiumUltimate.Web.Models.MapGenerators;

public class SliceEvaluation
{
    public int Id { get; set; }

    public float Influence { get; set; }

    public float Resources { get; set; }

    public float OptimalResourcesSum { get; set; }

    public float OptimalInfluenceSum { get; set; }

    public float OptimalValueSum => OptimalResourcesSum + OptimalInfluenceSum;

    public Dictionary<TechnologyType, int> TechnologySkips { get; set; } = new Dictionary<TechnologyType, int>();

    public int LegendariesCount { get; set; }

    public Dictionary<PlanetTrait, int> PlanetTraits { get; set; } = new Dictionary<PlanetTrait, int>();
}
