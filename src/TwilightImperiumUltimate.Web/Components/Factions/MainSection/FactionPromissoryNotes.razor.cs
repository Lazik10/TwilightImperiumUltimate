namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionPromissoryNotes
{
    [Parameter]
    public FactionName FactionName { get; set; }

    [Parameter]
    [EditorRequired]
    public IReadOnlyList<PromissoryNoteCardModel> PromissoryNotes { get; set; } = [];

    private int DesktopColumns => Math.Clamp(PromissoryNotes.Count, 1, 4);

    private int TouchColumns => PromissoryNotes.Count <= 2 ? Math.Max(PromissoryNotes.Count, 1) : 1;
}
