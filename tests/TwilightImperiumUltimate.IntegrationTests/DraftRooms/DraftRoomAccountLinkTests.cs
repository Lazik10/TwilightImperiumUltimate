using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.IntegrationTests.Infrastructure;
using Xunit;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomAccountLinkTests : IClassFixture<DraftRoomsModelTestsFixture>
{
    private readonly DraftRoomsModelTestsFixture _fixture;

    public DraftRoomAccountLinkTests(DraftRoomsModelTestsFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        this._fixture = fixture;
    }

    [Fact]
    public async Task Handle_WhenGuestAndAccountAreVerified_ShouldLinkWithoutLosingSeatRoleOrHistory()
    {
        var account = CreateUser(Guid.NewGuid().ToString(), $"Alice{Guid.NewGuid():N}");
        var otherAccount = CreateUser(Guid.NewGuid().ToString(), $"Other{Guid.NewGuid():N}");
        var room = CreateRoom(DraftRoomStatus.Finished);
        var guest = CreateParticipant(room, "Guest", "GUEST", status: DraftParticipantStatus.Active);
        var existingAccountParticipant = CreateParticipant(
            room,
            account.UserName!,
            account.UserName!.ToUpperInvariant(),
            otherAccount.Id,
            DraftParticipantStatus.Active);
        var originalJoinedAt = guest.JoinedAtUtc;
        var seat = CreateSeat(room, guest, 1);
        room.Participants.Add(guest);
        room.Participants.Add(existingAccountParticipant);
        room.Seats.Add(seat);

        await SaveRoomAsync(room, account, otherAccount);
        var handler = this.CreateHandler();
        var result = await handler.Handle(
            CreateCommand(room, guest, account, "guest-secret"),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var context = await this._fixture.Database.ContextFactory.CreateDbContextAsync();
        var linkedRoom = await context.DraftRooms
            .Include(item => item.Participants)
            .Include(item => item.Seats)
            .Include(item => item.LogEntries)
            .SingleAsync(item => item.Id == room.Id);
        var linkedParticipant = linkedRoom.Participants.Single(item => item.Id == guest.Id);

        linkedParticipant.UserId.Should().Be(account.Id);
        linkedParticipant.SecretHash.Should().BeNull();
        linkedParticipant.DisplayName.Should().Be($"{account.UserName}2");
        linkedParticipant.DisplayNameKey.Should().Be($"{account.UserName!.ToUpperInvariant()}2");
        linkedParticipant.Role.Should().Be(DraftRoomRole.Member);
        linkedParticipant.Status.Should().Be(DraftParticipantStatus.Active);
        linkedParticipant.JoinedAtUtc.Should().Be(originalJoinedAt);
        linkedRoom.Seats.Single(item => item.Id == seat.Id).ParticipantId.Should().Be(guest.Id);
        linkedRoom.Version.Should().Be(room.Version + 1);
        linkedRoom.LogEntries.Should().ContainSingle()
            .Which.Type.Should().Be(DraftLogEntryType.ParticipantLinked);
        var logData = JsonDocument.Parse(linkedRoom.LogEntries.Single().DataJson);
        logData.RootElement.GetProperty("PreviousDisplayName").GetString().Should().Be("Guest");
        logData.RootElement.GetProperty("NewDisplayName").GetString().Should().Be($"{account.UserName}2");
        linkedRoom.LogEntries.Single().DataJson.Should().NotContain("guest-secret");
    }

    [Fact]
    public async Task Handle_WhenAccountAlreadyHasParticipantRecord_ShouldReturnConflictWithoutChangingGuest()
    {
        var account = CreateUser(Guid.NewGuid().ToString(), $"Alice{Guid.NewGuid():N}");
        var room = CreateRoom();
        var guest = CreateParticipant(room, "Guest", "GUEST");
        var existingAccountParticipant = CreateParticipant(
            room,
            account.UserName!,
            account.UserName!.ToUpperInvariant(),
            account.Id,
            DraftParticipantStatus.Left);
        room.Participants.Add(guest);
        room.Participants.Add(existingAccountParticipant);
        await SaveRoomAsync(room, account);

        var result = await this.CreateHandler().Handle(
            CreateCommand(room, guest, account, "guest-secret"),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.AccountAlreadyInRoom);
        await this.AssertGuestWasNotChangedAsync(room, guest);
    }

    [Fact]
    public async Task Handle_WhenGuestSecretIsWrong_ShouldRejectWithoutChangingParticipant()
    {
        var account = CreateUser(Guid.NewGuid().ToString(), $"Alice{Guid.NewGuid():N}");
        var room = CreateRoom();
        var guest = CreateParticipant(room, "Guest", "GUEST");
        room.Participants.Add(guest);
        await SaveRoomAsync(room, account);

        var result = await this.CreateHandler().Handle(
            CreateCommand(room, guest, account, "wrong-secret"),
            CancellationToken.None);

        GetErrorCode(result).Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword);
        await this.AssertGuestWasNotChangedAsync(room, guest);
    }

    [Fact]
    public async Task SaveChanges_WhenTwoParticipantsUseSameAccountInOneRoom_ShouldEnforceUniqueIndex()
    {
        var account = CreateUser(Guid.NewGuid().ToString(), $"Alice{Guid.NewGuid():N}");
        var room = CreateRoom();
        room.Participants.Add(CreateParticipant(room, "First", "FIRST", account.Id));
        await SaveRoomAsync(room, account);

        await using var context = await this._fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedRoom = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
        context.DraftRoomParticipants.Add(
            CreateParticipant(persistedRoom, "Second", "SECOND", account.Id));

        var act = async () => await context.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateException>();
    }

    private static LinkAccountCommand CreateCommand(
        DraftRoom room,
        DraftRoomParticipant guest,
        TwilightImperiumUser account,
        string secret)
    {
        return new LinkAccountCommand(
            room.Code,
            new DraftRoomCaller(account.Id, account.UserName, guest.Id, secret));
    }

    private static DraftRoom CreateRoom(DraftRoomStatus status = DraftRoomStatus.Lobby)
    {
        var now = DateTime.UtcNow;

        return new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant(),
            Name = "Account link test room",
            PasswordHash = "not-used",
            Status = status,
            DraftType = "faction-pick",
            Version = 1,
            CreatedAtUtc = now,
            LastActivityUtc = now,
        };
    }

    private static DraftRoomParticipant CreateParticipant(
        DraftRoom room,
        string displayName,
        string displayNameKey,
        string? userId = null,
        DraftParticipantStatus status = DraftParticipantStatus.Active)
    {
        var now = DateTime.UtcNow;

        return new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Room = room,
            UserId = userId,
            SecretHash = userId is null ? new GuestSecretService().Hash("guest-secret") : null,
            DisplayName = displayName,
            DisplayNameKey = displayNameKey,
            Role = DraftRoomRole.Member,
            Status = status,
            JoinedAtUtc = now,
            LastSeenUtc = now,
        };
    }

    private static DraftRoomSeat CreateSeat(
        DraftRoom room,
        DraftRoomParticipant participant,
        int index)
    {
        return new DraftRoomSeat
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Room = room,
            Index = index,
            ParticipantId = participant.Id,
            Participant = participant,
            ClaimedAtUtc = DateTime.UtcNow,
        };
    }

    private static TwilightImperiumUser CreateUser(string id, string userName)
    {
        return new TwilightImperiumUser
        {
            Id = id,
            UserName = userName,
            NormalizedUserName = userName.ToUpperInvariant(),
        };
    }

    private static DraftRoomErrorCode GetErrorCode(FluentResults.Result result)
    {
        return result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<DraftRoomError>()
            .Which.Code;
    }

    private async Task SaveRoomAsync(DraftRoom room, params TwilightImperiumUser[] users)
    {
        await using var context = await this._fixture.Database.ContextFactory.CreateDbContextAsync();
        context.Users.AddRange(users);
        context.DraftRooms.Add(room);
        await context.SaveChangesAsync();
    }

    private async Task AssertGuestWasNotChangedAsync(DraftRoom room, DraftRoomParticipant guest)
    {
        await using var context = await this._fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedRoom = await context.DraftRooms
            .Include(item => item.Participants)
            .Include(item => item.LogEntries)
            .SingleAsync(item => item.Id == room.Id);
        var persistedGuest = persistedRoom.Participants.Single(item => item.Id == guest.Id);

        persistedGuest.UserId.Should().BeNull();
        persistedGuest.SecretHash.Should().NotBeNull();
        persistedGuest.DisplayName.Should().Be("Guest");
        persistedRoom.Version.Should().Be(room.Version);
        persistedRoom.LogEntries.Should().BeEmpty();
    }

    private LinkAccountCommandHandler CreateHandler()
    {
        return new LinkAccountCommandHandler(
            new DraftRoomUnitOfWork(
                this._fixture.Database.ContextFactory,
                new NullRoomChangeNotifier()),
            new GuestSecretService(),
            new DraftRoomDisplayNameValidator(Mock.Of<IUserRepository>()),
            new DraftRoomLog());
    }
}
