using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomCleanupTests(DraftRoomsModelTestsFixture fixture) : IClassFixture<DraftRoomsModelTestsFixture>
{
    private static readonly DateTimeOffset Now = new(2026, 10, 17, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Cleanup_WhenRoomsReachRetentionBoundary_ShouldDeleteOnlyOlderRoomsAndPreserveResultsAndPlayers()
    {
        var old = CreateRoom(Now.AddDays(-31).UtcDateTime);
        var boundary = CreateRoom(Now.AddDays(-30).UtcDateTime);
        var recent = CreateRoom(Now.AddDays(-1).UtcDateTime);
        var result = new DraftResult
        {
            Id = Guid.NewGuid(),
            ShareToken = Guid.NewGuid().ToString("N")[..22],
            SourceRoomId = old.Id,
            RoomName = old.Name,
            PayloadJson = "{}",
            FinishedAtUtc = old.LastActivityUtc,
            CreatedAtUtc = old.CreatedAtUtc,
        };
        result.Players.Add(new DraftResultPlayer { ResultId = result.Id, SeatIndex = 1, DisplayName = "Alice" });
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.AddRange(old, boundary, recent);
        context.DraftResults.Add(result);
        await context.SaveChangesAsync();
        var notifier = new Mock<IRoomChangeNotifier>();
        var cleanup = this.CreateService(new DraftRoomRepository(fixture.Database.ContextFactory), notifier.Object);

        var count = await cleanup.CleanupAsync(CancellationToken.None);

        count.Should().Be(1);
        (await context.DraftRooms.AnyAsync(room => room.Id == old.Id)).Should().BeFalse();
        (await context.DraftRooms.AnyAsync(room => room.Id == boundary.Id)).Should().BeTrue();
        (await context.DraftRooms.AnyAsync(room => room.Id == recent.Id)).Should().BeTrue();
        (await context.DraftResults.AnyAsync(item => item.Id == result.Id)).Should().BeTrue();
        (await context.DraftResultPlayers.AnyAsync(item => item.ResultId == result.Id)).Should().BeTrue();
        notifier.Verify(service => service.NotifyRoomClosedAsync(old.Id), Times.Once);
        notifier.Verify(service => service.NotifyRoomClosedAsync(boundary.Id), Times.Never);
    }

    [Fact]
    public async Task Cleanup_WhenCandidateBecameActive_ShouldRecheckActivityInsideRoomTransaction()
    {
        var recent = CreateRoom(Now.UtcDateTime);
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(recent);
        await context.SaveChangesAsync();
        var repository = new Mock<IDraftRoomRepository>(MockBehavior.Strict);
        repository.SetupSequence(service => service.GetInactiveRoomCodesAsync(Now.AddDays(-30).UtcDateTime, 100, It.IsAny<CancellationToken>()))
            .ReturnsAsync([recent.Code])
            .ReturnsAsync([]);
        var notifier = new Mock<IRoomChangeNotifier>(MockBehavior.Strict);

        var count = await this.CreateService(repository.Object, notifier.Object).CleanupAsync(CancellationToken.None);

        count.Should().Be(0);
        (await context.DraftRooms.SingleAsync(room => room.Id == recent.Id)).LastActivityUtc.Should().Be(Now.UtcDateTime);
        notifier.VerifyNoOtherCalls();
        repository.VerifyAll();
    }

    [Fact]
    public async Task Cleanup_WhenCandidateWasAlreadyDeleted_ShouldContinueWithoutClosingAnotherRoom()
    {
        var repository = new Mock<IDraftRoomRepository>(MockBehavior.Strict);
        repository.SetupSequence(service => service.GetInactiveRoomCodesAsync(Now.AddDays(-30).UtcDateTime, 100, It.IsAny<CancellationToken>()))
            .ReturnsAsync(["MISSING1"])
            .ReturnsAsync([]);
        var notifier = new Mock<IRoomChangeNotifier>(MockBehavior.Strict);

        var count = await this.CreateService(repository.Object, notifier.Object).CleanupAsync(CancellationToken.None);

        count.Should().Be(0);
        notifier.VerifyNoOtherCalls();
    }

    private static DraftRoom CreateRoom(DateTime activityUtc) => new()
    {
        Id = Guid.NewGuid(),
        Code = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant(),
        Name = "Retention test",
        PasswordHash = "test-hash",
        Status = DraftRoomStatus.Lobby,
        CreatedAtUtc = activityUtc,
        LastActivityUtc = activityUtc,
    };

    private DraftRoomCleanupService CreateService(IDraftRoomRepository repository, IRoomChangeNotifier notifier) => new(
        repository,
        new DraftRoomUnitOfWork(fixture.Database.ContextFactory, notifier),
        Options.Create(new DraftRoomRetentionOptions()),
        new FixedClock(),
        NullLogger<DraftRoomCleanupService>.Instance);

    private sealed class FixedClock : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => Now;
    }
}
