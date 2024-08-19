namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class TechnologyCard : TwilightImperiumBaseComponenet
{
    [Parameter]
    public TechnologyName TechnologyName { get; set; }

    private string GetImagePath()
    {
        return PathProvider.GetTechnologyImagePath(TechnologyName);
    }
}
