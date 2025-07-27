using Bogus;
using FluentAssertions;
using System.Globalization;
using System.Text;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;
using TwilightImperiumUltimate.Tigl.AsyncRating;
using TwilightImperiumUltimate.Tigl.Glicko2Rating;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Tigl;

public class GlickoRatingTests
{
    [Fact]
    public async Task CalculatesRatingCorrectlyForNewPlayers()
    {
        // Arrange
        var glickoPlayerMatchStatsService = new GlickoPlayerMatchStatsService();
        var glickoRatingCalculatorService = new GlickoRatingCalculatorService();
        var faker = new Faker();
        var league = TiglLeague.Tigl;

        var players = Enumerable.Range(1, 6).Select(i => new TiglUser
        {
            DiscordId = Math.Abs(faker.Random.Long()),
            TiglUserName = faker.Internet.UserName(),
            GlickoStats = new List<GlickoStats>()
            {
                new GlickoStats
                {
                    League = league,
                    Rating = new GlickoRating
                    {
                        Rating = 1500,
                        Rd = 350,
                        Volatility = 0.06,
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
        };

        var matchStats = await glickoPlayerMatchStatsService.InitializePlayerMatchStats(report, 1, players, league);

        // Act
        await glickoRatingCalculatorService.UpdatePlayerMatchStats(matchStats, 1);
        await glickoRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        // Assert
        players.Should().NotBeNullOrEmpty();
        players.Should().HaveCount(6);

        players = players.OrderByDescending(x => x.GlickoStats!.First(x => x.League == league).Rating!.Rating).ToList();

        players[0].GlickoStats!.First(x => x.League == league).Rating!.Rating.Should().Be(1578.24174083883);
        players[0].GlickoStats!.First(x => x.League == league).Rating!.Rd.Should().Be(193.5342746156295);
        players[0].GlickoStats!.First(x => x.League == league).Rating!.Volatility.Should().Be(0.05999785835473327);
    }

    [Fact(Skip = "Unable to confirm the results yet")]
    public async Task CalculateRatingCorrectlyForAdvancedPlayers()
    {
        // Arrange
        var glickoPlayerMatchStatsService = new GlickoPlayerMatchStatsService();
        var glickoRatingCalculatorService = new GlickoRatingCalculatorService();
        var faker = new Faker();
        double[] ratings = [1323.00, 1272.00, 1068.39, 1000.00, 1244.00, 906.00];
        var expectedFinalRatings = new[] { 1369.71, 1277.31, 1212.04, 1089.10, 1025.68, 895.69, };
        var league = TiglLeague.Tigl;

        var players = Enumerable.Range(0, 6).Select(i => new TiglUser
        {
            DiscordId = Math.Abs(faker.Random.Long()),
            TiglUserName = faker.Internet.UserName(),
            GlickoStats = new List<GlickoStats>
            {
                new GlickoStats
                {
                    League = league,
                    Rating = new GlickoRating
                    {
                        Rating = ratings[i],
                        Rd = 350,
                        Volatility = 0.06,
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
        };

        var matchStats = await glickoPlayerMatchStatsService.InitializePlayerMatchStats(report, 1, players, league);

        // Act
        await glickoRatingCalculatorService.UpdatePlayerMatchStats(matchStats, 1);
        await glickoRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        // Assert
        players.Should().NotBeNullOrEmpty();
        players.Should().HaveCount(6);

        players = players.OrderByDescending(x => x.GlickoStats!.First(x => x.League == league).Rating!.Rating).ToList();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].GlickoStats!.First(x => x.League == league).Rating!.Rating.Should().BeApproximately(expectedFinalRatings[i], 0.01);
            if (i == 0)
                players[i].GlickoStats!.First(x => x.League == league).Rating!.Rd.Should().Be(2.2);
        }
    }

    [Fact]
    public async Task SimulateSeasonResults()
    {
        // Arrange
        var glickoPlayerMatchStatsService = new GlickoPlayerMatchStatsService();
        var glickoRatingCalculatorService = new GlickoRatingCalculatorService();
        var faker = new Faker();
        var random = new Random();
        var league = TiglLeague.Tigl;

        var players = Enumerable.Range(0, 1000).Select(i => new TiglUser
        {
            DiscordId = Math.Abs(faker.Random.Long()),
            TiglUserName = faker.Internet.UserName(),
            GlickoStats = new List<GlickoStats>
            {
                new GlickoStats
                {
                    League = league,
                    Rating = new GlickoRating
                    {
                        Rating = 1500,
                        Rd = 350,
                        Volatility = 0.06,
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
            };

            gameReports.Add(report);
        }

        int matchId = 1;
        foreach (var report in gameReports)
        {
            var matchStats = await glickoPlayerMatchStatsService.InitializePlayerMatchStats(report, matchId, players, league);
            await glickoRatingCalculatorService.UpdatePlayerMatchStats(matchStats, 1);
            await glickoRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);
            matchId++;
        }

        var seasonTable = new StringBuilder();
        seasonTable.AppendLine("Discord".PadRight(30) + "Rating".PadRight(15) + "RD".PadRight(15) + "Volatility".PadRight(15));
        seasonTable.AppendLine();

        foreach (var player in players.OrderByDescending(x => x.GlickoStats!.First(x => x.League == league).Rating!.Rating))
        {
            var discordId = player.DiscordId.ToString(CultureInfo.InvariantCulture).PadRight(30);
            var rating = player.GlickoStats!.First(x => x.League == league).Rating!.Rating.ToString("F2", CultureInfo.InvariantCulture).PadRight(15);
            var rd = player.GlickoStats!.First(x => x.League == league).Rating!.Rd.ToString("F6", CultureInfo.InvariantCulture).PadRight(15);
            var volatility = player.GlickoStats!.First(x => x.League == league).Rating!.Volatility.ToString("F6", CultureInfo.InvariantCulture).PadRight(15);
            seasonTable.AppendLine(CultureInfo.InvariantCulture, $"{discordId}{rating}{rd}{volatility}");
        }

        await File.WriteAllTextAsync(@"C:\Temp\glicko-season.txt", seasonTable.ToString());
    }
}
