using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftResultRepositoryTests(DraftRoomsModelTestsFixture fixture) : IClassFixture<DraftRoomsModelTestsFixture>
{
    [Fact]
    public async Task DeleteRoom_WhenResultExists_ShouldPreserveSnapshotAndCascadePlayersOnlyWhenResultDeleted()
    {
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..8],
            Name = "Result source",
            PasswordHash = "test-hash",
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        var result = CreateResult();
        result.SourceRoomId = room.Id;
        result.Players.Add(new DraftResultPlayer { ResultId = result.Id, SeatIndex = 1, DisplayName = "Alice" });
        context.DraftRooms.Add(room);
        context.DraftResults.Add(result);
        await context.SaveChangesAsync();

        await context.DraftRooms.Where(item => item.Id == room.Id).ExecuteDeleteAsync();

        var repository = new DraftResultRepository(fixture.Database.ContextFactory);
        var stored = await repository.GetByIdAsync(result.Id, CancellationToken.None);
        stored.Should().NotBeNull();
        stored!.Players.Should().ContainSingle().Which.DisplayName.Should().Be("Alice");
        await context.DraftResults.Where(item => item.Id == result.Id).ExecuteDeleteAsync();
        (await context.DraftResultPlayers.AnyAsync(item => item.ResultId == result.Id)).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAccount_WhenItOwnsAndOccupiesSnapshot_ShouldNullHistoryAssociationsWithoutChangingPublicPayload()
    {
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        var user = new TwilightImperiumUser { Id = Guid.NewGuid().ToString("N"), UserName = "Snapshot owner" };
        var result = CreateResult();
        result.OwnerUserId = user.Id;
        result.Players.Add(new DraftResultPlayer { ResultId = result.Id, SeatIndex = 1, DisplayName = "Original guest", UserId = user.Id });
        context.Users.Add(user);
        context.DraftResults.Add(result);
        await context.SaveChangesAsync();

        await context.Users.Where(item => item.Id == user.Id).ExecuteDeleteAsync();
        context.ChangeTracker.Clear();

        var stored = await context.DraftResults.Include(item => item.Players).SingleAsync(item => item.Id == result.Id);
        stored.OwnerUserId.Should().BeNull();
        stored.Players.Single().UserId.Should().BeNull();
        stored.Players.Single().DisplayName.Should().Be("Original guest");
        stored.PayloadJson.Should().Be(result.PayloadJson);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Save_WhenTokenOrSourceIsDuplicated_ShouldRejectSecondSnapshot(bool duplicateToken)
    {
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        var first = CreateResult();
        var second = CreateResult();
        if (duplicateToken)
        {
            second.ShareToken = first.ShareToken;
        }
        else
        {
            second.SourceRoomId = first.SourceRoomId;
        }

        context.DraftResults.AddRange(first, second);

        var save = async () => await context.SaveChangesAsync();
        await save.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task GetByToken_WhenCaseDiffers_ShouldRequireExactToken()
    {
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        var result = CreateResult();
        result.ShareToken = "aB" + result.ShareToken[2..];
        context.DraftResults.Add(result);
        await context.SaveChangesAsync();
        var repository = new DraftResultRepository(fixture.Database.ContextFactory);

        (await repository.GetByTokenAsync(result.ShareToken, CancellationToken.None)).Should().NotBeNull();
        (await repository.GetByTokenAsync("Ab" + result.ShareToken[2..], CancellationToken.None)).Should().BeNull();
    }

    [Fact]
    public async Task History_WhenAccountOwnsOrOccupiesResult_ShouldPageNewestFirstWithoutOtherAccounts()
    {
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        var user = new TwilightImperiumUser { Id = Guid.NewGuid().ToString("N"), UserName = "History owner" };
        var owned = CreateResult();
        owned.OwnerUserId = user.Id;
        var occupied = CreateResult();
        occupied.FinishedAtUtc = owned.FinishedAtUtc.AddMinutes(-1);
        occupied.Players.Add(new DraftResultPlayer { ResultId = occupied.Id, SeatIndex = 1, DisplayName = "Alice", UserId = user.Id });
        context.Users.Add(user);
        context.DraftResults.AddRange(owned, occupied, CreateResult());
        await context.SaveChangesAsync();
        var repository = new DraftResultRepository(fixture.Database.ContextFactory);

        (await repository.GetHistoryAsync(user.Id, 0, 1, CancellationToken.None)).Select(item => item.Id).Should().Equal(owned.Id);
        (await repository.GetHistoryAsync(user.Id, 1, 21, CancellationToken.None)).Select(item => item.Id).Should().Equal(occupied.Id);
    }

    private static DraftResult CreateResult() => new()
    {
        Id = Guid.NewGuid(),
        ShareToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16)).TrimEnd('=').Replace('+', '-').Replace('/', '_'),
        SourceRoomId = Guid.NewGuid(),
        RoomName = "Finished draft",
        PayloadJson = "{\"version\":1}",
        CreatedAtUtc = DateTime.UtcNow,
        FinishedAtUtc = DateTime.UtcNow,
    };
}
