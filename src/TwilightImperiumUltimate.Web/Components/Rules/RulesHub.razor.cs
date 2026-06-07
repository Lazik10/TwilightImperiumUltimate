namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RulesHub
{
    [Parameter]
    public string Section { get; set; } = "index";

    [Parameter]
    public string SearchWord { get; set; } = string.Empty;

    [Parameter]
    public string? Letter { get; set; }

    [Parameter]
    public string? Category { get; set; }

    [Parameter]
    public string? Source { get; set; }

    [Parameter]
    public string? Version { get; set; }

    [Parameter]
    public int? RuleId { get; set; }

    [Parameter]
    public string? ItemKey { get; set; }
}
