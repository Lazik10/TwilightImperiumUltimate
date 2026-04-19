namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class TechnologyCard : TwilightImperiumBaseComponent
{
    [Parameter]
    public TechnologyName TechnologyName { get; set; }

    private string GetImagePath()
    {
        return PathProvider.GetTechnologyImagePath(TechnologyName);
    }
}

