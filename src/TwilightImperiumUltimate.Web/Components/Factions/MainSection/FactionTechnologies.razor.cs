namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionTechnologies
{
    [Parameter]
    [EditorRequired]
    public IReadOnlyList<TechnologyModel> Technologies { get; set; } = [];
}
