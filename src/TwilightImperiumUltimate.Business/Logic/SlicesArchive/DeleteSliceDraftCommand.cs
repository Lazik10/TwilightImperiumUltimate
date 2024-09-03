namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class DeleteSliceDraftCommand : IRequest<bool>
{
    public int Id { get; set; }
}
