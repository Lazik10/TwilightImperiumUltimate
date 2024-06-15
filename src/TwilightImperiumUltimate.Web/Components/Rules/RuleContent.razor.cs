using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Rules;

namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleContent
{
    [Parameter]
    public TransformedRule TransformedRule { get; set; } = default!;

    private MarkupString TransformedRuleContent { get; set; }

    private MarkupString TransformedRuleTitle { get; set; }

    protected override void OnParametersSet()
    {
        TransformedRuleContent = new MarkupString(TransformedRule.Content);
        TransformedRuleTitle = new MarkupString(TransformedRule.RuleTitle);
    }

    private int GetTransformedRuleId() => (int)TransformedRule.RuleCategory;
}
