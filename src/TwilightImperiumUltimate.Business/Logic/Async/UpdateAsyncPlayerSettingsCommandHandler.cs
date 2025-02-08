namespace TwilightImperiumUltimate.Business.Logic.Async;

public class UpdateAsyncPlayerSettingsCommandHandler(
    IAsyncStatsRepository asyncStatsRepository)
    : IRequestHandler<UpdateAsyncPlayerSettingsCommand, bool>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<bool> Handle(UpdateAsyncPlayerSettingsCommand request, CancellationToken cancellationToken)
    {
        return await _asyncStatsRepository.UpdateAsyncPlayerProfileSettings(request.PlayerDiscordId, request.PlayerProfileSettings, cancellationToken);
    }
}
