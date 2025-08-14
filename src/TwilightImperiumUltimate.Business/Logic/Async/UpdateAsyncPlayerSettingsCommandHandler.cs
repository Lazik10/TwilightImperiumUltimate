using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class UpdateAsyncPlayerSettingsCommandHandler(
    IAsyncStatsRepository asyncStatsRepository)
    : IRequestHandler<UpdateAsyncPlayerSettingsCommand, Result>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<Result> Handle(UpdateAsyncPlayerSettingsCommand request, CancellationToken cancellationToken)
    {
        var result = ValidateRequest(request);

        if (result.IsFailed)
            return result;

        var playerProfile = await _asyncStatsRepository.GetAsyncPlayerProfileByDiscordId(request.PlayerDiscordId, cancellationToken);
        if (playerProfile is null)
            return Result.Fail($"Player profile with Discord ID {request.PlayerDiscordId} not found.");

        var dbUpdateResult = await _asyncStatsRepository.UpdateAsyncPlayerProfileSettings(request.PlayerDiscordId, request.PlayerProfileSettings, cancellationToken);
        if (!dbUpdateResult)
            return Result.Fail($"Failed to update player settings for Discord ID {request.PlayerDiscordId}.");

        return Result.Ok();
    }

    private static Result ValidateRequest(UpdateAsyncPlayerSettingsCommand request)
    {
        var result = Result.Ok();

        if (request is null)
            return Result.Fail("Request cannot be null.");

        if (request.PlayerProfileSettings is null)
            return Result.Fail("Player profile settings cannot be null.");

        if (request.PlayerDiscordId <= 0)
            result.WithError("PlayerDiscordId must be a positive number.");

        if (string.IsNullOrWhiteSpace(request.PlayerProfileSettings.ExcludeFromAsyncStats.ToString()))
            result.WithError("ExcludeFromAsyncStats cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(request.PlayerProfileSettings.ShowWinRate.ToString()))
            result.WithError("ShowWinRate cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(request.PlayerProfileSettings.ShowTurnStats.ToString()))
            result.WithError("ShowTurnStats cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(request.PlayerProfileSettings.ShowCombatStats.ToString()))
            result.WithError("ShowCombatStats cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(request.PlayerProfileSettings.ShowVpStats.ToString()))
            result.WithError("ShowVpStats cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(request.PlayerProfileSettings.ShowFactionStats.ToString()))
            result.WithError("ShowFactionStats cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(request.PlayerProfileSettings.ShowOpponents.ToString()))
            result.WithError("ShowOpponents cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(request.PlayerProfileSettings.ShowGames.ToString()))
            result.WithError("ShowGames cannot be null or empty.");

        return result;
    }
}
