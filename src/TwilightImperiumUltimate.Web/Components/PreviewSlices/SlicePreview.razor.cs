namespace TwilightImperiumUltimate.Web.Components.PreviewSlices;

public partial class SlicePreview
{
    [Parameter]
    public SliceModel SliceModel { get; set; } = new SliceModel();

    [Parameter]
    public bool EditNameMode { get; set; }
}