namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class TechnologyNavbar
{
    [Parameter]
    public EventCallback<TechnologyType> OnTechnologyTypeChange { get; set; }

    private void OnTechnologyTypeClick(TechnologyType technologyType) => OnTechnologyTypeChange.InvokeAsync(technologyType);
}
