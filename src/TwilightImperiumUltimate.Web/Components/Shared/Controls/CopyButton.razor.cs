using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class CopyButton
{
    private bool _showCopiedMessage;

    [Parameter]
    [EditorRequired]
    public string TextToCopy { get; set; } = string.Empty;

    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public string AriaLabel { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private string ButtonLabel => _showCopiedMessage ? Strings.ButtonText_Copied : DisplayLabel;

    private string DisplayLabel => string.IsNullOrWhiteSpace(Label) ? Strings.ButtonText_Copy : Label;

    private Dictionary<string, object> ButtonAttributes => string.IsNullOrWhiteSpace(AriaLabel)
        ? []
        : new Dictionary<string, object> { ["aria-label"] = AriaLabel };

    private async Task CopyAsync()
    {
        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Shared/Controls/CopyButton.razor.js");
        await module.InvokeVoidAsync("copyToClipboard", TextToCopy);

        _showCopiedMessage = true;
        StateHasChanged();

        await Task.Delay(1500);

        _showCopiedMessage = false;
        StateHasChanged();
    }
}
