using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Tigl.Services;

internal class TiglMatchUserValidator(
    ITiglUserRepository tiglUserRepository)
    : ITiglMatchUsersValidator
{
    public async Task<bool> AllTiglUsersExists(IGameReport gameReport, CancellationToken cancellationToken)
    {
        var users = await tiglUserRepository.GetUsersFromGameReport(gameReport, cancellationToken);
        return users.Count == gameReport.PlayerResults.Count;
    }

    public async Task<Result<bool>> RegisterNewTiglUsers(IGameReport gameReport, CancellationToken cancellationToken)
    {
        var registeredUsers = await tiglUserRepository.GetUsersFromGameReport(gameReport, cancellationToken);
        Result<bool> result = new Result();

        foreach (var user in gameReport.PlayerResults)
        {
            // If the user exists, we don't need to register them again
            if (!registeredUsers.Any(x => x.DiscordId == user.DiscordId))
            {
                var registrationResult = await tiglUserRepository.RegisterNewTiglUser(user.DiscordId, user.DiscordTag, gameReport.StartTimestamp, cancellationToken: cancellationToken, user.DiscordTag);
                if (registrationResult.IsFailed)
                {
                    result.WithError(new Error($"Could not register new user for Game report: {gameReport.GameId} with DiscordId: {user.DiscordId} and DiscordTag: {user.DiscordTag}"));
                }
            }
        }

        return result;
    }
}
