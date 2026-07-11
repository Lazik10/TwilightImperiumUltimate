namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ButtonGroup
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public ButtonGroupStyle GroupStyle { get; set; } = ButtonGroupStyle.Row;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    private string ComputedCssClass => $"button-group {GetGroupStyleCssClass()} {CssClass}".Trim();

    private string GetGroupStyleCssClass() => GroupStyle switch
    {
        ButtonGroupStyle.Row => "button-group-row",
        ButtonGroupStyle.RowFill => "button-group-row-fill",
        ButtonGroupStyle.Column => "button-group-column",
        ButtonGroupStyle.ColumnFill => "button-group-column-fill",
        _ => "button-group-row",
    };
}
