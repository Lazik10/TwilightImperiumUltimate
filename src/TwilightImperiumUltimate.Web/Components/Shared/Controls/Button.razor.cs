using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class Button
{
    [Parameter]
    public string ButtonText { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; } = false;

    private async Task OnClickHandler(MouseEventArgs e)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }
    }
}
