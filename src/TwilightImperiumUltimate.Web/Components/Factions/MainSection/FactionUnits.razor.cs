namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionUnits
{
    [Parameter]
    public FactionName FactionName { get; set; }

    [Parameter]
    [EditorRequired]
    public IReadOnlyList<FlagshipCardModel> FlagshipCards { get; set; } = [];

    [Parameter]
    [EditorRequired]
    public IReadOnlyList<SpecialComponentCardModel> SpecialComponentCards { get; set; } = [];

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private IReadOnlyList<string> UnitImagePaths => CreateUnitImagePaths();

    private IReadOnlyList<string> CreateUnitImagePaths()
    {
        var imagePaths = new List<string>();

        if (FactionName == FactionName.TheFirmamentTheObsidian)
        {
            imagePaths.Add(PathProvider.GetFactionComponenetTypeImagePath("TheFirmament", ComponentType.Mech));
            imagePaths.Add(PathProvider.GetFactionComponenetTypeImagePath("TheObsidian", ComponentType.Mech));
        }
        else
        {
            imagePaths.Add(PathProvider.GetFactionComponenetTypeImagePath(FactionName.ToString(), ComponentType.Mech));
        }

        imagePaths.AddRange(FlagshipCards.Select(flagship => PathProvider.GetFlagshipImagePath(flagship.FlagshipName)));
        imagePaths.AddRange(SpecialComponentCards
            .Where(x => x.SpecialType == SpecialComponentType.FactionUnit)
            .Select(x => PathProvider.GetSpecialComponentImagePath(x.SpecialComponentName)));

        return imagePaths;
    }
}
