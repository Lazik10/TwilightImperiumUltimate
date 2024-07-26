namespace TwilightImperiumUltimate.Web.Components.Units;

public partial class UnitImage : TwilightImperiumBaseComponenet
{
    [Parameter]
    public UnitName UnitName { get; set; }

    private string ImagePath => PathProvider.GetUnitImagePath(UnitName);
}