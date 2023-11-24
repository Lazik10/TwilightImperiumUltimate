using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Rules;

namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleContent
{
    [Parameter]
    public Rule Rule { get; set; } = default!;

    private MarkupString MarkupString { get; set; }

    protected override void OnParametersSet()
    {
        MarkupString = new MarkupString(Rule.Content);
    }
}
