namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class DeleteMapCommandHandler(
    IMapArchiveRepository mapArchiveRepository)
    : IRequestHandler<DeleteMapCommand, bool>
{
    private readonly IMapArchiveRepository _mapArchiveRepository = mapArchiveRepository;

    public async Task<bool> Handle(DeleteMapCommand request, CancellationToken cancellationToken)
    {
        return await _mapArchiveRepository.DeleteMap(request.Id, cancellationToken);
    }
}
