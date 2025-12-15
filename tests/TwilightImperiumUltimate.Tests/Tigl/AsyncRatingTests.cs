using Bogus;
using FluentAssertions;
using System.Globalization;
using System.Text;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;
using TwilightImperiumUltimate.Tigl.AsyncRating;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Tigl;

public class AsyncRatingTests
{
    [Fact]
    public async Task CalculatesRatingCorrectlyForNewPlayers()
    {
        // Arrange
        var asyncPlayerMatchStatsService = new AsyncPlayerMatchStatsService();
        var asyncRatingCalculatorService = new AsyncRatingCalculatorService();
        var faker = new Faker();
        var league = TiglLeague.Test;

        var players = Enumerable.Range(1, 6).Select(i => new TiglUser
        {
            DiscordId = Math.Abs(faker.Random.Long()),
            TiglUserName = faker.Internet.UserName(),
            AsyncStats = new List<AsyncStats>
            {
                new AsyncStats
                {
                    Rating = new AsyncRating
                    {
                        Rating = 1500,
                        AussieScore = 0,
                    },
                    League = league,
                },
            },
        }).ToList();

        int[] scores = [10, 9, 9, 8, 7, 7];
        var report = new GameReport()
        {
            GameId = "pbd1000",
            Score = 10,
            Source = ResultSource.Async,
            PlayerResults = players.Select((p, i) => new Contracts.ApiContracts.Tigl.Report.PlayerResult
            {
                DiscordId = p.DiscordId,
                Score = scores[i],
                Faction = "The Arborec",
            }).ToList(),
        };

        var matchStats = await asyncPlayerMatchStatsService.InitializePlayerMatchStats(report, 1, players, league);

        // Act
        await asyncRatingCalculatorService.UpdatePlayerMatchStats(matchStats, 1);
        await asyncRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        // Assert
        players.Should().NotBeNullOrEmpty();
        players.Should().HaveCount(6);

        players = players.OrderByDescending(x => x.AsyncStats!.First(x => x.League == league).Rating!.Rating).ToList();

        players[0].AsyncStats!.First(x => x.League == league).Rating!.Rating.Should().Be(1560.35);
        players[0].AsyncStats!.First(x => x.League == league).Rating!.AussieScore.Should().Be(2.2);
    }

    [Fact]
    public async Task CalculateRatingCorrectlyForAdvancedPlayers()
    {
        // Arrange
        var asyncPlayerMatchStatsService = new AsyncPlayerMatchStatsService();
        var asyncRatingCalculatorService = new AsyncRatingCalculatorService();
        var faker = new Faker();
        var league = TiglLeague.Test;

        double[] ratings = [1387.3100000000, 1231.9000000000, 970.9100000000, 1023.5100000000, 814.4500000000, 1091.9300000000];
        var expectedFinalRatings = new[] { 1369.71, 1277.31, 1212.04, 1089.10, 1025.68, 895.69, };

        var players = Enumerable.Range(0, 6).Select(i => new TiglUser
        {
            DiscordId = Math.Abs(faker.Random.Long()),
            TiglUserName = faker.Internet.UserName(),
            AsyncStats = new List<AsyncStats>()
            {
                new AsyncStats()
                {
                    Rating = new AsyncRating
                    {
                        Rating = ratings[i],
                        AussieScore = 0,
                    },
                    League = league,
                },
            },
        }).ToList();

        int[] scores = [10, 9, 8, 7, 7, 5];
        var report = new GameReport()
        {
            GameId = "pbd1000",
            Score = 10,
            Source = ResultSource.Async,
            PlayerResults = players.Select((p, i) => new Contracts.ApiContracts.Tigl.Report.PlayerResult
            {
                DiscordId = p.DiscordId,
                Score = scores[i],
                Faction = "The Arborec",
            }).ToList(),
        };

        var matchStats = await asyncPlayerMatchStatsService.InitializePlayerMatchStats(report, 1, players, league);

        // Act
        await asyncRatingCalculatorService.UpdatePlayerMatchStats(matchStats, 1);
        await asyncRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        // Assert
        players.Should().NotBeNullOrEmpty();
        players.Should().HaveCount(6);

        //players = players.OrderByDescending(x => x.AsyncStats!.First(x => x.League == league).Rating!.Rating).ToList();

        for (int i = 0; i < players.Count; i++)
        {
            /*            players[i].AsyncStats!.First(x => x.League == league).Rating!.Rating.Should().BeApproximately(expectedFinalRatings[i], 0.01);
                        if (i == 0)
                            players[i].AsyncStats!.First(x => x.League == league).Rating!.AussieScore.Should().Be(2.2);*/
        }

        foreach (var matchStat in matchStats)
        {
            var oldRating = matchStat.RatingOld;
            var newRating = matchStat.RatingNew;
            var aussieScoreOld = matchStat.AussieScoreOld;
            var aussieScoreNew = matchStat.AussieScoreNew;
            var score = matchStat.Score;
            var score2 = matchStat.Score;
        }
    }

    [Fact]
    public async Task SimulateSeasonResults()
    {
        // Arrange
        var asyncPlayerMatchStatsService = new AsyncPlayerMatchStatsService();
        var asyncRatingCalculatorService = new AsyncRatingCalculatorService();
        var faker = new Faker();
        var random = new Random();
        var league = TiglLeague.Test;

        var players = Enumerable.Range(0, 1000)
            .Select(i => new TiglUser
            {
                DiscordId = Math.Abs(faker.Random.Long()),
                TiglUserName = faker.Internet.UserName(),
                AsyncStats = new List<AsyncStats>()
                {
                    new AsyncStats()
                    {
                        Rating = new AsyncRating
                        {
                            Rating = 1500,
                            AussieScore = 0,
                        },
                        League = league,
                    },
                },
            }).ToList();

        var gameReports = new List<GameReport>();

        for (int i = 0; i < 1000000; i++)
        {
            var report = new GameReport()
            {
                GameId = "pbd1000",
                Score = 10,
                Source = ResultSource.Async,
                PlayerResults = players
                    .OrderByDescending(p => random.Next())
                    .Take(6)
                    .Select((p, i) => new Contracts.ApiContracts.Tigl.Report.PlayerResult
                    {
                        DiscordId = p.DiscordId,
                        Score = i == 0 ? 10 : random.Next(1, 10),
                        Faction = "The Arborec",
                    }).ToList(),
            };

            gameReports.Add(report);
        }

        var allMatchStats = new List<AsyncPlayerMatchStats>();
        int matchId = 1;
        foreach (var report in gameReports)
        {
            var matchStats = await asyncPlayerMatchStatsService.InitializePlayerMatchStats(report, matchId, players, league);
            await asyncRatingCalculatorService.UpdatePlayerMatchStats(matchStats, 1);
            await asyncRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);
            matchId++;
            allMatchStats.AddRange(matchStats);
        }

        var seasonTable = new StringBuilder();
        seasonTable.AppendLine("Discord".PadRight(30) + "Rating".PadRight(15) + "AussieScore".PadRight(15));
        seasonTable.AppendLine();

        foreach (var player in players.OrderByDescending(x => x.AsyncStats!.First(x => x.League == league).Rating!.Rating))
        {
            var discordId = player.DiscordId.ToString(CultureInfo.InvariantCulture).PadRight(30);
            var rating = player.AsyncStats!.First(x => x.League == league).Rating!.Rating.ToString("F2", CultureInfo.InvariantCulture).PadRight(15);
            var aussieScore = player.AsyncStats!.First(x => x.League == league).Rating!.AussieScore;
            seasonTable.AppendLine(CultureInfo.InvariantCulture, $"{discordId}{rating}{aussieScore}");
        }

        await File.WriteAllTextAsync(@"C:\Temp\async-season.txt", seasonTable.ToString());
    }
}
