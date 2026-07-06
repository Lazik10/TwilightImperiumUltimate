using System.Globalization;
using System.Text;
using Bogus;
using FluentAssertions;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;
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
        var league = TiglLeague.Test;

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
            Score = 10,
            Source = ResultSource.Async,
            PlayerResults = players.Select((p, i) => new Contracts.ApiContracts.Tigl.Report.PlayerResult
            {
                DiscordId = p.DiscordId,
                Score = scores[i],
                Faction = "The Arborec",
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

        players[0].GlickoStats!.First(x => x.League == league).Rating!.Rating.Should().Be(1860.6472750664675);
        players[0].GlickoStats!.First(x => x.League == league).Rating!.Rd.Should().Be(193.534277548303);
        players[0].GlickoStats!.First(x => x.League == league).Rating!.Volatility.Should().Be(0.0600012172435905);
    }

    [Fact]
    public async Task CalculateRatingCorrectlyForAdvancedPlayers()
    {
        // Arrange
        var glickoPlayerMatchStatsService = new GlickoPlayerMatchStatsService();
        var glickoRatingCalculatorService = new GlickoRatingCalculatorService();
        var faker = new Faker();
        double[] ratings = [1566.5733236524, 1578.7017568149, 1350.0544643849, 1454.0996842929, 1269.2556133809, 1443.2382439912];
        double[] rds = [41.2217409940, 62.0482490192, 102.8972232820, 47.0583781913, 45.5159316340, 41.6659251473];
        double[] volatility = [0.0618362785, 0.0602514116, 0.0600075489, 0.0602109603, 0.0601568714, 0.0605718185];
        var expectedFinalRatings = new[] { 1588.4810826472665, 1581.9039179129904, 1441.348416829956, 1418.951620452423, 1404.7479391667339, 1272.6976776541217, };
        var league = TiglLeague.Test;

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
                        Rd = rds[i],
                        Volatility = volatility[i],
                    },
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

        var matchStats = await glickoPlayerMatchStatsService.InitializePlayerMatchStats(report, 1, players, league);

        // Act
        await glickoRatingCalculatorService.UpdatePlayerMatchStats(matchStats, 1);
        await glickoRatingCalculatorService.UpdatePlayerRatings(players, matchStats, league);

        // Assert
        players.Should().NotBeNullOrEmpty();
        players.Should().HaveCount(6);

        var finalRatings = players
            .Select(x => x.GlickoStats!.First(y => y.League == league).Rating!.Rating)
            .OrderByDescending(x => x)
            .ToArray();

        for (int i = 0; i < finalRatings.Length; i++)
        {
            finalRatings[i].Should().BeApproximately(expectedFinalRatings[i], 0.01);
        }
    }

    [Fact(Skip = "This test is for simulating a season and generating a report. It takes a long time to run.")]
    public async Task SimulateSeasonResults()
    {
        // Arrange
        var glickoPlayerMatchStatsService = new GlickoPlayerMatchStatsService();
        var glickoRatingCalculatorService = new GlickoRatingCalculatorService();
        var faker = new Faker();
        var random = new Random();
        var league = TiglLeague.Test;

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

        for (int i = 0; i < 100000; i++)
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
