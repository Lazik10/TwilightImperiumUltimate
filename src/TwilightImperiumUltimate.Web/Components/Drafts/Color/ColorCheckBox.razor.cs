using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Components.Drafts.Color;

public partial class ColorCheckBox
{
    [Parameter]
    public EventCallback<PlayerColor> OnClick { get; set; }

    [Parameter]
    public PlayerColor Color { get; set; }

    [Parameter]
    public bool IsChecked { get; set; } = false;

    private string GetUpperColorName() => Color.ToString().ToUpperInvariant();

    private string GetLowerColorName() => Color.ToString().ToLowerInvariant();

    private async Task HandleClick()
    {
        IsChecked = !IsChecked;
        await OnClick.InvokeAsync(Color);
    }
}
