using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Helpers;
using PlayerTiglResult = TwilightImperiumUltimate.Core.Entities.Tigl.PlayerResult;

namespace TwilightImperiumUltimate.Tigl.Services;

public class TiglMatchInserter(
    ITiglRepository tiglRepository)
    : ITiglMatchInserter
{
    public async Task<IResult<MatchReport>> InsertGameReport(GameReport gameReport, CancellationToken cancellationToken)
    {
        var result = new Result<MatchReport>();

        var matchReport = new MatchReport
        {
            GameId = gameReport.GameId,
            Source = gameReport.Source,
            Timestamp = gameReport.Timestamp > 0 ? gameReport.Timestamp : new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
            State = Contracts.Enums.MatchState.New,
        };

        var playerResults = new List<PlayerTiglResult>();

        foreach (var playerResult in gameReport.PlayerResults)
        {
            var tiglUser = await tiglRepository.FindTiglUserFromGameReportPlayerResult(playerResult, cancellationToken) ?? throw new InvalidOperationException($"TIGL user not found for Discord ID: {playerResult.DiscordId}");
            var parsedFaction = Result.Try(() => TiglFactionParser.ParseFaction(playerResult.Faction));

            if (tiglUser is not null && parsedFaction.IsSuccess)
            {
                playerResults.Add(new PlayerTiglResult
                {
                    TiglUser = tiglUser.Id,
                    Faction = parsedFaction.Value,
                    Score = playerResult.Score,
                });

                result.WithSuccess($"Successfully created result for player: {playerResult.DiscordId} and faction {playerResult.Faction}");
            }
            else
            {
                result.WithError($"Couldn't create result for player: {playerResult.DiscordId} and faction {playerResult.Faction}");
            }
        }

        if (result.IsFailed || playerResults.Count != 6)
            return result;

        matchReport.PlayerResults = playerResults;

        var insertedMatchReport = await tiglRepository.InsertNewGameReport(matchReport, cancellationToken);
        if (insertedMatchReport is not null && insertedMatchReport.Id != 0)
        {
            return Result.Ok(insertedMatchReport);
        }

        return Result.Fail<MatchReport>($"An error occurred during inserting Game report: {matchReport.GameId} into the database");
    }
}
