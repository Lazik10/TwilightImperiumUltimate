using TwilightImperiumUltimate.Web.Models.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.Shared.Shapes;

public partial class Rectangle
{
    [Parameter]
    public EventCallback OnClick { get; set; }

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public int Height { get; set; } = 100;

    [Parameter]
    public string Fill { get; set; } = "black";

    private async Task HandleClick()
    {
        await OnClick.InvokeAsync();
    }
}