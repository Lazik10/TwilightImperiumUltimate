using Bogus;
using FluentAssertions;
using System.Globalization;
using System.Text;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;
using TwilightImperiumUltimate.Tigl.TrueSkillRating;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Tigl;

public class TrueSkillRatingTests
{
    [Fact]
    public async Task CalculatesRatingCorrectlyForNewPlayers()
    {
        // Arrange
        var trueSkillPlayerMatchStatsService = new TrueSkillPlayerMatchStatsService();
        var trueSkillRatingCalculatorService = new TrueSkillRatingCalculatorService();
        var faker = new Faker();
        var league = TiglLeague.Tigl;

        var players = Enumerable.Range(1, 6).Select(i => new TiglUser
        {
            DiscordId = Math.Abs(faker.Random.Long()),
            TiglUserName = faker.Internet.UserName(),
            TrueSkillStats = new List<TrueSkillStats>()
            {
                new TrueSkillStats
                {
                    League = league,
                    TrueSkillRating = new TrueSkillRating
                    {
                        Mu = 1500,
                        Sigma = 500,
                    },
                },
            },
        }).ToList();

        int[] scores = [10, 9, 9, 8, 7, 7];
        var report = new GameReport()
        {
            GameId = "pbd1000",
            Source = Contracts.Enums.ResultSource.Async,
            PlayerResults = players.Select((p, i) => new Contracts.ApiContracts.Tigl.Report.PlayerResult
            {
                DiscordId = p.DiscordId,
                Score = scores[i],
            }).ToList(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        };

        var matchStats = await trueSkillPlayerMatchStatsService.InitializePlayerMatchStats(report, 1, players, league);

        // Act
        await trueSkillRatingCalculatorService.UpdatePlayerMatchStats(matchStats, 1);
        await trueSkillRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        // Assert
        players.Should().NotBeNullOrEmpty();
        players.Should().HaveCount(6);

        players = players.OrderByDescending(x => x.TrueSkillStats!.First(x => x.League == league)!.TrueSkillRating).ToList();

        players[0].TrueSkillStats!.First(x => x.League == league)!.TrueSkillRating!.Rating.Should().Be(1508.575750346636);
        players[0].TrueSkillStats!.First(x => x.League == league)!.TrueSkillRating!.Mu.Should().Be(1508.575750346636);
        players[0].TrueSkillStats!.First(x => x.League == league)!.TrueSkillRating!.Sigma.Should().Be(500.02499771523156);
    }

    [Fact(Skip = "Cant' confirm if results are valid")]
    public async Task CalculateRatingCorrectlyForAdvancedPlayers()
    {
        // Arrange
        var trueSkillPlayerMatchStatsService = new TrueSkillPlayerMatchStatsService();
        var trueSkillRatingCalculatorService = new TrueSkillRatingCalculatorService();
        var faker = new Faker();
        double[] ratings = [1323.00, 1272.00, 1068.39, 1000.00, 1244.00, 906.00];
        var expectedFinalRatings = new[] { 1369.71, 1277.31, 1212.04, 1089.10, 1025.68, 895.69, };
        var league = TiglLeague.Tigl;

        var players = Enumerable.Range(0, 6).Select(i => new TiglUser
        {
            DiscordId = Math.Abs(faker.Random.Long()),
            TiglUserName = faker.Internet.UserName(),
            TrueSkillStats = new List<TrueSkillStats>()
            {
                new TrueSkillStats
                {
                    League = league,
                    TrueSkillRating = new TrueSkillRating
                    {
                        Mu = ratings[i],
                        Sigma = 2.2,
                    },
                },
            },
        }).ToList();

        int[] scores = [10, 9, 9, 9, 7, 6];
        var report = new GameReport()
        {
            GameId = "pbd1000",
            Source = Contracts.Enums.ResultSource.Async,
            PlayerResults = players.Select((p, i) => new Contracts.ApiContracts.Tigl.Report.PlayerResult
            {
                DiscordId = p.DiscordId,
                Score = scores[i],
            }).ToList(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        };

        var matchStats = await trueSkillPlayerMatchStatsService.InitializePlayerMatchStats(report, 1, players, league);

        // Act
        await trueSkillRatingCalculatorService.UpdatePlayerMatchStats(matchStats, 1);
        await trueSkillRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        // Assert
        players.Should().NotBeNullOrEmpty();
        players.Should().HaveCount(6);

        players = players.OrderByDescending(x => x.TrueSkillStats!.First(x => x.League == league).TrueSkillRating!.Rating).ToList();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].TrueSkillStats!.First(x => x.League == league).TrueSkillRating!.Rating.Should().BeApproximately(expectedFinalRatings[i], 0.01);
            if (i == 0)
                players[i].TrueSkillStats!.First(x => x.League == league).TrueSkillRating!.Sigma.Should().Be(2.2);
        }
    }

    [Fact]
    public async Task SimulateSeasonResults()
    {
        // Arrange
        var trueSkillPlayerMatchStatsService = new TrueSkillPlayerMatchStatsService();
        var trueSkillRatingCalculatorService = new TrueSkillRatingCalculatorService();
        var faker = new Faker();
        var random = new Random();
        var league = TiglLeague.Tigl;

        var players = Enumerable.Range(0, 1000).Select(i => new TiglUser
        {
            DiscordId = Math.Abs(faker.Random.Long()),
            TiglUserName = faker.Internet.UserName(),
            TrueSkillStats = new List<TrueSkillStats>()
            {
                new TrueSkillStats
                {
                    League = TiglLeague.Tigl,
                    TrueSkillRating = new TrueSkillRating
                    {
                        Mu = 25.0,
                        Sigma = 25.0 / 3,
                    },
                },
            },
        }).ToList();

        var gameReports = new List<GameReport>();

        for (int i = 0; i < 1000000; i++)
        {
            var report = new GameReport()
            {
                GameId = "pbd1000",
                Source = Contracts.Enums.ResultSource.Async,
                PlayerResults = players
                    .OrderByDescending(p => random.Next())
                    .Take(6)
                    .Select((p, i) => new Contracts.ApiContracts.Tigl.Report.PlayerResult
                    {
                        DiscordId = p.DiscordId,
                        Score = i == 0 ? 10 : random.Next(1, 10),
                    }).ToList(),
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };

            gameReports.Add(report);
        }

        int matchId = 1;
        foreach (var report in gameReports)
        {
            var matchStats = await trueSkillPlayerMatchStatsService.InitializePlayerMatchStats(report, matchId, players, league);
            await trueSkillRatingCalculatorService.UpdatePlayerMatchStats(matchStats, 1);
            await trueSkillRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);
            matchId++;
        }

        var seasonTable = new StringBuilder();
        seasonTable.AppendLine("Discord".PadRight(30) + "Rating".PadRight(15) + "Conservative Rating".PadRight(15) + "Sigma".PadRight(15));
        seasonTable.AppendLine();

        foreach (var player in players.OrderByDescending(x => x.TrueSkillStats!.First(x => x.League == league).TrueSkillRating!.Rating))
        {
            var discordId = player.DiscordId.ToString(CultureInfo.InvariantCulture).PadRight(30);
            var rating = player.TrueSkillStats!.First(x => x.League == league).TrueSkillRating!.Rating.ToString("F2", CultureInfo.InvariantCulture).PadRight(15);
            var sigma = player.TrueSkillStats!.First(x => x.League == league).TrueSkillRating!.Sigma.ToString("F8", CultureInfo.InvariantCulture).PadRight(15);
            var conservativeRating = player.TrueSkillStats!.First(x => x.League == league).TrueSkillRating!.ConservativeRating.ToString("F4", CultureInfo.InvariantCulture).PadRight(15);
            seasonTable.AppendLine(CultureInfo.InvariantCulture, $"{discordId}{rating}{conservativeRating}{sigma}");
        }

        await File.WriteAllTextAsync(@"C:\Temp\trueskill-season.txt", seasonTable.ToString());
    }
}
