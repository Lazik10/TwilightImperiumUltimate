namespace TwilightImperiumUltimate.Web.Components.Rules;

public partial class RuleContent
{
    [Parameter]
    public TransformedRule TransformedRule { get; set; } = default!;

    [Parameter]
    public bool ShowNotes { get; set; }

    private MarkupString TransformedRuleContent { get; set; }

    private MarkupString TransformedRuleTitle { get; set; }

    private MarkupString TransformedNoteContent { get; set; }

    protected override void OnParametersSet()
    {
        TransformedRuleContent = new MarkupString(TransformedRule.Content);
        TransformedRuleTitle = new MarkupString(TransformedRule.RuleTitle);
        TransformedNoteContent = new MarkupString(TransformedRule.NotesContent);
    }

    private int GetTransformedRuleId() => (int)TransformedRule.RuleCategory;
}
