using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;
using TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftResultAccessTests(DraftRoomsModelTestsFixture fixture) : IClassFixture<DraftRoomsModelTestsFixture>
{
    [Fact]
    public async Task Read_WhenTokenIsPublic_ShouldReturnOnlyImmutablePublicDataWithoutMembership()
    {
        var (room, result, _) = await this.CreateAsync();
        var query = this.CreateQueries();

        var response = await query.Handle(new GetResultByTokenQuery(result.ShareToken), CancellationToken.None);

        response.IsSuccess.Should().BeTrue();
        response.Value.Text.Should().Contain("Seat 1 - Original guest: TheArborec");
        var json = JsonSerializer.Serialize(response.Value);
        json.Should().NotContain(room.Id.ToString()).And.NotContain(room.Participants.Single().Id.ToString())
            .And.NotContain("UserId").And.NotContain("ParticipantId").And.NotContain("SourceRoomId").And.NotContain("Secret");
        response.Value.Payload.Deserialize<FactionPickResultDto>()!.Seats.Single().DisplayName.Should().Be("Original guest");
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaa")]
    [InlineData("!!!!!!!!!!!!!!!!!!!!!!")]
    public async Task Read_WhenTokenIsUnknownOrMalformed_ShouldReturnSameNotFoundCode(string token)
    {
        var response = await this.CreateQueries().Handle(new GetResultByTokenQuery(token), CancellationToken.None);

        response.IsFailed.Should().BeTrue();
        response.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.ResultNotFound);
    }

    [Fact]
    public async Task ReadRoom_WhenCallerIsUnverified_ShouldNotDiscloseTheShareToken()
    {
        var (room, result, caller) = await this.CreateAsync();
        var query = this.CreateQueries();
        var invalid = caller with { GuestSecret = "wrong-secret" };

        var refused = await query.Handle(new GetRoomResultQuery(room.Code, invalid), CancellationToken.None);
        var allowed = await query.Handle(new GetRoomResultQuery(room.Code, caller), CancellationToken.None);

        refused.IsFailed.Should().BeTrue();
        refused.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
        allowed.Value.ShareToken.Should().Be(result.ShareToken);
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, DraftParticipantStatus.Active, false, true)]
    [InlineData(DraftRoomRole.CoHost, DraftParticipantStatus.Active, false, true)]
    [InlineData(DraftRoomRole.Member, DraftParticipantStatus.Active, false, false)]
    [InlineData(DraftRoomRole.Host, DraftParticipantStatus.Left, false, false)]
    [InlineData(DraftRoomRole.CoHost, DraftParticipantStatus.Kicked, false, false)]
    [InlineData(DraftRoomRole.Host, DraftParticipantStatus.Active, true, false)]
    public async Task Delete_WhenRoomExists_ShouldRequireVerifiedActiveManager(
        DraftRoomRole role,
        DraftParticipantStatus status,
        bool wrongSecret,
        bool permitted)
    {
        var (room, result, caller) = await this.CreateAsync(role, status);
        if (wrongSecret)
        {
            caller = caller with { GuestSecret = "wrong-secret" };
        }

        var response = await this.CreateDelete().Handle(new DeleteResultCommand(result.Id, caller), CancellationToken.None);

        response.IsSuccess.Should().Be(permitted);
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        (await context.DraftResults.AnyAsync(item => item.Id == result.Id)).Should().Be(!permitted);
        var stored = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
        stored.ResultId.Should().Be(permitted ? null : result.Id);
        stored.Version.Should().Be(permitted ? 1 : 0);
        (await context.DraftRoomLogEntries.CountAsync(item => item.RoomId == room.Id && item.Type == DraftLogEntryType.DraftResultDeleted))
            .Should().Be(permitted ? 1 : 0);
        if (permitted)
        {
            (await context.DraftResultPlayers.AnyAsync(item => item.ResultId == result.Id)).Should().BeFalse();
            var missing = await this.CreateQueries().Handle(new GetResultByTokenQuery(result.ShareToken), CancellationToken.None);
            missing.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.ResultNotFound);
        }
    }

    [Fact]
    public async Task Delete_WhenRoomWasRemoved_ShouldRequireOwningAccountAndRemoveHistory()
    {
        var (room, result, owner) = await this.CreateAsync(account: true);
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        await context.DraftRooms.Where(item => item.Id == room.Id).ExecuteDeleteAsync();
        var handler = this.CreateDelete();

        var refused = await handler.Handle(new DeleteResultCommand(result.Id, new DraftRoomCaller("another-account", "Other", null, null)), CancellationToken.None);
        var allowed = await handler.Handle(new DeleteResultCommand(result.Id, owner), CancellationToken.None);

        refused.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.Forbidden);
        allowed.IsSuccess.Should().BeTrue();
        (await new DraftResultRepository(fixture.Database.ContextFactory).GetHistoryAsync(owner.UserId!, 0, 21, CancellationToken.None))
            .Should().BeEmpty();
    }

    [Fact]
    public async Task Delete_WhenGuestRoomWasRemoved_ShouldNotRetainGuestManagerAuthority()
    {
        var (room, result, guest) = await this.CreateAsync();
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        await context.DraftRooms.Where(item => item.Id == room.Id).ExecuteDeleteAsync();

        var refused = await this.CreateDelete().Handle(new DeleteResultCommand(result.Id, guest), CancellationToken.None);

        refused.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.Forbidden);
        (await context.DraftResults.AnyAsync(item => item.Id == result.Id)).Should().BeTrue();
    }

    [Theory]
    [InlineData(DraftRoomRole.Host, DraftParticipantStatus.Active, true)]
    [InlineData(DraftRoomRole.CoHost, DraftParticipantStatus.Active, true)]
    [InlineData(DraftRoomRole.Member, DraftParticipantStatus.Active, false)]
    [InlineData(DraftRoomRole.Host, DraftParticipantStatus.Left, false)]
    [InlineData(DraftRoomRole.Host, DraftParticipantStatus.Kicked, false)]
    public async Task History_WhenRoomExists_ShouldProjectCurrentManagerPermissionInsteadOfOwnership(
        DraftRoomRole role,
        DraftParticipantStatus status,
        bool canDelete)
    {
        var (room, result, caller) = await this.CreateAsync(role, status, account: true);
        var repository = new DraftResultRepository(fixture.Database.ContextFactory);

        var history = await repository.GetHistoryAsync(caller.UserId!, 0, 21, CancellationToken.None);
        history.Should().ContainSingle().Which.CanDelete.Should().Be(canDelete);
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        await context.DraftRooms.Where(item => item.Id == room.Id).ExecuteDeleteAsync();
        var orphaned = await repository.GetHistoryAsync(caller.UserId!, 0, 21, CancellationToken.None);
        orphaned.Should().ContainSingle().Which.Id.Should().Be(result.Id);
        orphaned.Single().CanDelete.Should().BeTrue();
    }

    [Fact]
    public async Task LinkAccount_WhenGuestFinishedDraft_ShouldAssociateHistoryWithoutChangingSnapshotNamesOrPayload()
    {
        var (room, result, guest) = await this.CreateAsync();
        var user = new TwilightImperiumUser { Id = Guid.NewGuid().ToString("N"), UserName = "Account name" };
        await using (var context = await fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        var handler = new LinkAccountCommandHandler(
            new DraftRoomUnitOfWork(fixture.Database.ContextFactory, new NullRoomChangeNotifier()),
            new GuestSecretService(),
            new DraftRoomDisplayNameValidator(Mock.Of<IUserRepository>()),
            new DraftRoomLog());
        var linked = await handler.Handle(
            new LinkAccountCommand(room.Code, new DraftRoomCaller(user.Id, user.UserName, guest.GuestParticipantId, guest.GuestSecret)),
            CancellationToken.None);

        linked.IsSuccess.Should().BeTrue();
        var repository = new DraftResultRepository(fixture.Database.ContextFactory);
        var stored = await repository.GetByIdAsync(result.Id, CancellationToken.None);
        stored!.OwnerUserId.Should().Be(user.Id);
        stored.Players.Single().UserId.Should().Be(user.Id);
        stored.Players.Single().DisplayName.Should().Be("Original guest");
        stored.PayloadJson.Should().Be(result.PayloadJson);
        (await repository.GetHistoryAsync(user.Id, 0, 21, CancellationToken.None)).Should().ContainSingle().Which.Id.Should().Be(result.Id);
    }

    private DraftResultQueryHandler CreateQueries() => new(
        new DraftResultRepository(fixture.Database.ContextFactory),
        new DraftRoomRepository(fixture.Database.ContextFactory),
        new DraftRoomAuthorizer(new GuestSecretService()),
        new DraftTypeRegistry([new DraftTypeModule<FactionPickSettingsDto, FactionPickDraftState>(new FactionPickDraftType())]));

    private DeleteResultCommandHandler CreateDelete() => new(
        new DraftResultRepository(fixture.Database.ContextFactory),
        new DraftRoomUnitOfWork(fixture.Database.ContextFactory, new NullRoomChangeNotifier()),
        new DraftRoomAuthorizer(new GuestSecretService()),
        new DraftRoomLog());

    private async Task<(DraftRoom Room, DraftResult Result, DraftRoomCaller Caller)> CreateAsync(
        DraftRoomRole role = DraftRoomRole.Host,
        DraftParticipantStatus status = DraftParticipantStatus.Active,
        bool account = false)
    {
        var user = new TwilightImperiumUser { Id = Guid.NewGuid().ToString("N"), UserName = $"Result {Guid.NewGuid().ToString("N")[..8]}" };
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant(),
            Name = "Finished draft",
            PasswordHash = "test-hash",
            Status = DraftRoomStatus.Finished,
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        var participant = new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            UserId = account ? user.Id : null,
            SecretHash = account ? null : new GuestSecretService().Hash("test-secret"),
            DisplayName = "Original guest",
            DisplayNameKey = "ORIGINAL GUEST",
            Role = role,
            Status = status,
            JoinedAtUtc = room.CreatedAtUtc,
            LastSeenUtc = room.CreatedAtUtc,
        };
        room.Participants.Add(participant);
        room.Seats.Add(new DraftRoomSeat { Id = Guid.NewGuid(), RoomId = room.Id, Index = 1, ParticipantId = participant.Id });
        var payload = new FactionPickResultDto
        {
            PicksPerPlayer = 1,
            SeatOrder = [1],
            Seats = [new FactionPickResultSeatDto(1, participant.DisplayName, [new FactionPickResultPickDto(FactionName.TheArborec, 0, null)])],
        };
        var result = new DraftResult
        {
            Id = Guid.NewGuid(),
            ShareToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16)).TrimEnd('=').Replace('+', '-').Replace('/', '_'),
            SourceRoomId = room.Id,
            OwnerUserId = account ? user.Id : null,
            RoomName = room.Name,
            PayloadJson = JsonSerializer.Serialize(payload),
            CreatedAtUtc = room.CreatedAtUtc,
            FinishedAtUtc = room.CreatedAtUtc,
        };
        result.Players.Add(new DraftResultPlayer { ResultId = result.Id, SeatIndex = 1, DisplayName = participant.DisplayName, UserId = participant.UserId });
        room.ResultId = result.Id;
        await using var context = await fixture.Database.ContextFactory.CreateDbContextAsync();
        if (account)
        {
            context.Users.Add(user);
        }

        context.DraftRooms.Add(room);
        context.DraftResults.Add(result);
        await context.SaveChangesAsync();
        var caller = account
            ? new DraftRoomCaller(user.Id, user.UserName, null, null)
            : new DraftRoomCaller(null, null, participant.Id, "test-secret");
        return (room, result, caller);
    }
}
