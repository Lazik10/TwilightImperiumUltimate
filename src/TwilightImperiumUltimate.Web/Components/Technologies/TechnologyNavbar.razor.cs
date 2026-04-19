namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class TechnologyNavbar
{
    [Parameter]
    public TechnologyType SelectedTechnologyType { get; set; } = TechnologyType.Biotic;

    [Parameter]
    public EventCallback<TechnologyType> OnTechnologyTypeChange { get; set; }

    private void OnTechnologyTypeClick(TechnologyType technologyType) => OnTechnologyTypeChange.InvokeAsync(technologyType);
}
