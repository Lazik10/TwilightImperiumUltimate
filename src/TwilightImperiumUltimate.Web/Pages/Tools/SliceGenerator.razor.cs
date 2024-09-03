namespace TwilightImperiumUltimate.Web.Pages.Tools;

public partial class SliceGenerator
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "tiles")]
    public string TilesImported { get; set; } = string.Empty;

    public Task Reload() => InvokeAsync(StateHasChanged);
}