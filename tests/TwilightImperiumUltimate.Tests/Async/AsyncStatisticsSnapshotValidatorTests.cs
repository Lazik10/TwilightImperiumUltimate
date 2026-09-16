using FluentAssertions;
using TwilightImperiumUltimate.Business.Services.Async.Implementations;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Async;

public sealed class AsyncStatisticsSnapshotValidatorTests
{
    [Fact]
    public void Validate_WhenCategoryFilterIsMissing_ShouldRejectSnapshot()
    {
        var snapshot = new AsyncStatisticsSnapshotDto
        {
            Games = new AsyncGamesSummaryStatsDto { Custom = null! },
        };

        var action = () => AsyncStatisticsSnapshotValidator.Validate(snapshot);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*Games*Custom*");
    }

    [Fact]
    public void Validate_WhenRankingExceedsCanonicalLimit_ShouldRejectSnapshot()
    {
        var oversizedPlayers = Enumerable.Range(1, 201)
            .Select(index => new AsyncGamesPlayerDto(index, $"Player {index}", index, 0))
            .ToList();
        var snapshot = new AsyncStatisticsSnapshotDto
        {
            Games = new AsyncGamesSummaryStatsDto
            {
                All = new AsyncGamesStatsDto(oversizedPlayers, []),
            },
        };

        var action = () => AsyncStatisticsSnapshotValidator.Validate(snapshot);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*Games.All*201*200*");
    }

    [Fact]
    public void Validate_WhenSnapshotHasAllCategoriesAndFilters_ShouldAcceptSnapshot()
    {
        var action = () => AsyncStatisticsSnapshotValidator.Validate(new AsyncStatisticsSnapshotDto());

        action.Should().NotThrow();
    }
}
