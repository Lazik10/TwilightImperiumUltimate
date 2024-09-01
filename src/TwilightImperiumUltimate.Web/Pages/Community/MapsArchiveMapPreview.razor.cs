namespace TwilightImperiumUltimate.Web.Pages.Community;

public partial class MapsArchiveMapPreview
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "name")]
    public string MapNameQuery { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "event")]
    public string MapEventQuery { get; set; } = string.Empty;

    protected override Task OnParametersSetAsync()
    {
        return Task.CompletedTask;
    }
}