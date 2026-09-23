namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ResponsiveCheckbox
{
    [Parameter]
    public bool IsChecked { get; set; }

    [Parameter]
    [EditorRequired]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<bool> IsCheckedChanged { get; set; }

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private async Task HandleChange(ChangeEventArgs args)
    {
        IsChecked = args.Value is bool value && value;
        await IsCheckedChanged.InvokeAsync(IsChecked);
    }
}
