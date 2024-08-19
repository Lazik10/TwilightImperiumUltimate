namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class FactionTechnologyTree
{
    [Parameter]
    public IReadOnlyCollection<TechnologyModel> Technologies { get; set; } = Array.Empty<TechnologyModel>();
}
