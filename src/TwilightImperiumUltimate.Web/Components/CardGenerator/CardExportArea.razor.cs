using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Components.CardGenerator;

public partial class CardExportArea
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string CardTitle { get; set; } = string.Empty;

    private async Task ExportCard()
    {
        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/CardGenerator/CardExportArea.razor.js");
        await module.InvokeVoidAsync("saveCardAsImage", "cardArea", $"{CardTitle}.png");
    }
}
