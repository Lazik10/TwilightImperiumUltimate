namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionExtrasVerticalCardGroup
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public IReadOnlyList<SpecialComponentCardModel> Components { get; set; } = [];
}
