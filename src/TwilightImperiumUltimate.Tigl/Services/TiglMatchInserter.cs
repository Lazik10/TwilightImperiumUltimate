using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Helpers;
using PlayerTiglResult = TwilightImperiumUltimate.Core.Entities.Tigl.PlayerResult;

namespace TwilightImperiumUltimate.Tigl.Services;

public class TiglMatchInserter(
    ITiglRepository tiglRepository,
    ISeasonRepository seasonRepository,
    ITiglUserRepository tiglUserRepository)
    : ITiglMatchInserter
{
    public async Task<IResult<MatchReport>> InsertGameReport(GameReport gameReport, CancellationToken cancellationToken)
    {
        var result = new Result<MatchReport>();

        var currentSeason = await seasonRepository.GetCurrentSeason(cancellationToken);
        var eventsFlag = TiglGalacticEventConverter.ConvertToFlags(gameReport.Events);

        if (gameReport.League == TiglLeague.ThundersEdge && eventsFlag != 0)
            return Result.Fail<MatchReport>($"Game {gameReport.GameId}: Thunders Edge league should not have any galactic events enabled, if this was game should have been marked as Fractured instead please report it manually.");

        var matchReport = new MatchReport
        {
            GameId = gameReport.GameId,
            Source = gameReport.Source,
            StartTimestamp = gameReport.StartTimestamp > 0 ? gameReport.StartTimestamp : new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
            EndTimestamp = gameReport.EndTimestamp > 0 ? gameReport.EndTimestamp : new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
            Score = gameReport.Score,
            Round = gameReport.Round,
            PlayerCount = gameReport.PlayerCount,
            Season = currentSeason,
            League = gameReport.League,
            Events = eventsFlag,
            State = MatchState.New,
        };

        var playerResults = new List<PlayerTiglResult>();

        foreach (var playerResult in gameReport.PlayerResults)
        {
            var tiglUser = await tiglUserRepository.FindTiglUserFromGameReportPlayerResult(playerResult, cancellationToken) ?? throw new InvalidOperationException($"TIGL user not found for Discord ID: {playerResult.DiscordId}");
            var parsedFaction = Result.Try(() => TiglFactionParser.ParseFaction(playerResult.Faction));

            if (tiglUser is not null && parsedFaction.IsSuccess)
            {
                playerResults.Add(new PlayerTiglResult
                {
                    TiglUserId = tiglUser.Id,
                    Faction = parsedFaction.Value,
                    Score = Math.Min(playerResult.Score, gameReport.Score),
                    IsWinner = playerResult.IsWinner,
                });

                result.WithSuccess($"Successfully created result for player: {playerResult.DiscordId} and faction {playerResult.Faction}");
            }
            else
            {
                result.WithError($"Couldn't create result for player: {playerResult.DiscordId} and faction {playerResult.Faction}");
            }
        }

        if (result.IsFailed)
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
