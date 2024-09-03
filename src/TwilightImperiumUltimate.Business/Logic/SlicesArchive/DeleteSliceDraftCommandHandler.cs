namespace TwilightImperiumUltimate.Business.Logic.SlicesArchive;

public class DeleteSliceDraftCommandHandler(
    ISlicesArchiveRepository slicesArchiveRepository)
    : IRequestHandler<DeleteSliceDraftCommand, bool>
{
    private readonly ISlicesArchiveRepository _slicesArchiveRepository = slicesArchiveRepository;

    public async Task<bool> Handle(DeleteSliceDraftCommand request, CancellationToken cancellationToken)
    {
        return await _slicesArchiveRepository.DeleteSliceDraft(request.Id, cancellationToken);
    }
}
