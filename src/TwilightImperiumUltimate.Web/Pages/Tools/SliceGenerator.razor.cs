namespace TwilightImperiumUltimate.Web.Pages.Tools;

public partial class SliceGenerator
{
    public Task Reload() => InvokeAsync(StateHasChanged);
}