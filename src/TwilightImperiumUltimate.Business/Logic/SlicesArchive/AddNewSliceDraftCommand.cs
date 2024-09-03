namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class AddNewSliceDraftCommand(SliceDraftDto sliceDraft) : IRequest<bool>
{
    public SliceDraftDto SliceDraft { get; set; } = sliceDraft;
}
