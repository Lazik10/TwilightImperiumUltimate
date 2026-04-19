namespace TwilightImperiumUltimate.Web.Components.Units;

public partial class UnitImage : TwilightImperiumBaseComponent
{
    [Parameter]
    public UnitName UnitName { get; set; }

    private string ImagePath => PathProvider.GetUnitImagePath(UnitName);
}
