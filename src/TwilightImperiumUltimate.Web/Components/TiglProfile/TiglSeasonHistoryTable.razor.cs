using Microsoft.AspNetCore.Components.Web;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.Format;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglSeasonHistoryTable
{
    [Parameter] public IReadOnlyList<TiglProfileSeasonSummary> Seasons { get; set; } = [];
    [Parameter] public TiglLeagueProfileDto? LeagueProfile { get; set; }
    [Parameter] public EventCallback<int> OnGameSelected { get; set; }
    private readonly HashSet<int> _expandedSeasons = [];

    private IReadOnlyList<SeasonRow> Rows => BuildRows();

    private bool IsExpanded(int season) => _expandedSeasons.Contains(season);

    private void Toggle(int season)
    {
        if (!_expandedSeasons.Add(season))
            _expandedSeasons.Remove(season);
    }

    private Task OnSeasonKeyDown(KeyboardEventArgs args, int season)
    {
        if (args.Key is "Enter" or " ")
            Toggle(season);

        return Task.CompletedTask;
    }

    private Task OnGameKeyDown(KeyboardEventArgs args, int matchReportId) => args.Key is "Enter" or " "
        ? OnGameSelected.InvokeAsync(matchReportId)
        : Task.CompletedTask;

    private IReadOnlyList<SeasonRow> BuildRows()
    {
        var asyncStats = LeagueProfile?.AsyncMatchHistory ?? [];
        var glickoStats = LeagueProfile?.GlickoMatchHistory ?? [];
        var trueSkillStats = LeagueProfile?.TrueSkillMatchHistory ?? [];

        return Seasons
            .Select(season => new SeasonRow(
                season.Season,
                season.GamesPlayed,
                season.WinRate,
                trueSkillStats.Where(stat => stat.Season == season.Season).Sum(stat => stat.ConservativeRatingChange),
                glickoStats.Where(stat => stat.Season == season.Season).Sum(stat => stat.RatingChange),
                asyncStats.Where(stat => stat.Season == season.Season).Sum(stat => stat.RatingChange),
                season.Games.Select(game => BuildGameRow(game, trueSkillStats, glickoStats, asyncStats)).ToList()))
            .ToList();
    }

    private static SeasonGameRow BuildGameRow(
        TiglProfileGameDto game,
        IReadOnlyList<TrueSkillPlayerMatchStatsDto> trueSkillStats,
        IReadOnlyList<GlickoPlayerMatchStatsDto> glickoStats,
        IReadOnlyList<AsyncPlayerMatchStatsDto> asyncStats)
    {
        var trueSkill = trueSkillStats.FirstOrDefault(stat => Matches(game, stat.Season, stat.StartTimestamp, stat.EndTimestamp, stat.Faction, stat.Score));
        var glicko = glickoStats.FirstOrDefault(stat => Matches(game, stat.Season, stat.StartTimestamp, stat.EndTimestamp, stat.Faction, stat.Score));
        var async = asyncStats.FirstOrDefault(stat => Matches(game, stat.Season, stat.StartTimestamp, stat.EndTimestamp, stat.Faction, stat.Score));

        return new SeasonGameRow(
            game.MatchReportId,
            game.GameId,
            trueSkill?.ConservativeRatingChange ?? 0,
            glicko?.RatingChange ?? 0,
            async?.RatingChange ?? 0,
            game.Score,
            game.MaxScore,
            game.Faction.GetDisplayName(),
            FormatDuration(game));
    }

    private static bool Matches(TiglProfileGameDto game, int season, long startTimestamp, long endTimestamp, TiglFactionName faction, int score) =>
        game.Season == season
        && game.StartTimestamp == startTimestamp
        && game.EndTimestamp == endTimestamp
        && game.Faction == faction
        && game.Score == score;

    private static string FormatDelta(double value) => value.ToSignedFormat(2);

    private static TextColor GetChangeColor(double value) => value switch
    {
        > 0 => TextColor.Green,
        < 0 => TextColor.Red,
        _ => TextColor.White,
    };

    private static string FormatDuration(TiglProfileGameDto game)
    {
        if (game.StartTimestamp <= 0 || game.EndTimestamp <= 0)
            return "N/A";

        var duration = DateTimeOffset.FromUnixTimeMilliseconds(game.EndTimestamp) - DateTimeOffset.FromUnixTimeMilliseconds(game.StartTimestamp);
        return $"{(int)duration.TotalDays:D2} D {duration.Hours:D2} HH";
    }

    private sealed record SeasonRow(int Season, int GamesPlayed, double WinRate, double TrueSkillDelta, double GlickoDelta, double AsyncDelta, IReadOnlyList<SeasonGameRow> Games);

    private sealed record SeasonGameRow(int MatchReportId, string GameId, double TrueSkillDelta, double GlickoDelta, double AsyncDelta, int Score, int MaxScore, string FactionName, string Duration);
}
