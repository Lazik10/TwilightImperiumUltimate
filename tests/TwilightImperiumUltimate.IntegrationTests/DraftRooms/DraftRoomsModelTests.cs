using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;
using TwilightImperiumUltimate.IntegrationTests.Infrastructure;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomsModelTests : IClassFixture<DraftRoomsModelTestsFixture>
{
    private readonly DraftRoomsModelTestsFixture fixture;

    public DraftRoomsModelTests(DraftRoomsModelTestsFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task TestDatabase_WhenCreated_ShouldDisableAutoCloseBetweenFixtureConnections()
    {
        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();

        var autoClose = await context.Database
            .SqlQuery<bool>($"SELECT CONVERT(bit, DATABASEPROPERTYEX(DB_NAME(), 'IsAutoClose')) AS [Value]")
            .SingleAsync();

        autoClose.Should().BeFalse();
    }

    [Fact]
    public async Task SaveChanges_WhenRoomCodeIsNotUnique_ShouldRejectDuplicate()
    {
        var room = CreateRoom();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.AddRange(room, CreateRoom(room.Code));

        var act = () => context.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task SaveChanges_WhenSeatIndexIsRepeatedWithinRoom_ShouldRejectDuplicate()
    {
        var room = CreateRoom();
        var firstSeat = CreateSeat(room, 1);
        var duplicateSeat = CreateSeat(room, 1);

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        context.DraftRoomSeats.AddRange(firstSeat, duplicateSeat);

        var act = () => context.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task SaveChanges_WhenParticipantIsAssignedToTwoSeatsInRoom_ShouldRejectDuplicate()
    {
        var room = CreateRoom();
        var participant = CreateParticipant(room, "Player", "player");
        var firstSeat = CreateSeat(room, 1, participant);
        var secondSeat = CreateSeat(room, 2, participant);

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        context.DraftRoomParticipants.Add(participant);
        context.DraftRoomSeats.AddRange(firstSeat, secondSeat);

        var act = () => context.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task SaveChanges_WhenMultipleSeatsAreUnclaimed_ShouldAllowThemInOneRoom()
    {
        var room = CreateRoom();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        context.DraftRoomSeats.AddRange(CreateSeat(room, 1), CreateSeat(room, 2));

        var affectedRows = await context.SaveChangesAsync();

        affectedRows.Should().Be(3);
    }

    [Fact]
    public async Task SaveChanges_WhenParticipantIsDeleted_ShouldUnclaimItsSeat()
    {
        var room = CreateRoom();
        var participant = CreateParticipant(room, "Player", "player");
        var seat = CreateSeat(room, 1, participant);

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            context.DraftRoomSeats.Add(seat);
            await context.SaveChangesAsync();

            context.DraftRoomParticipants.Remove(participant);
            await context.SaveChangesAsync();
        }

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedSeat = await verificationContext.DraftRoomSeats.SingleAsync(item => item.Id == seat.Id);

        persistedSeat.ParticipantId.Should().BeNull();
    }

    [Fact]
    public async Task SaveChanges_WhenRoomIsDeleted_ShouldCascadeDeleteItsSeats()
    {
        var room = CreateRoom();
        var participant = CreateParticipant(room, "Player", "player");

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            context.DraftRoomSeats.Add(CreateSeat(room, 1, participant));
            await context.SaveChangesAsync();

            context.DraftRooms.Remove(room);
            await context.SaveChangesAsync();
        }

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await verificationContext.DraftRoomSeats.AnyAsync(item => item.RoomId == room.Id)).Should().BeFalse();
    }

    [Fact]
    public async Task SaveChanges_WhenChatAndLogRowsAreAdded_ShouldGenerateOrderedIdentityIds()
    {
        var room = CreateRoom();
        var now = DateTime.UtcNow;
        var firstMessage = new DraftRoomChatMessage
        {
            RoomId = room.Id,
            Room = room,
            Kind = ChatMessageKind.User,
            Text = "First message",
            CreatedAtUtc = now.AddMinutes(1),
        };
        var secondMessage = new DraftRoomChatMessage
        {
            RoomId = room.Id,
            Room = room,
            Kind = ChatMessageKind.User,
            Text = "Second message",
            CreatedAtUtc = now,
        };
        var firstEntry = new DraftRoomLogEntry
        {
            RoomId = room.Id,
            Room = room,
            Type = DraftLogEntryType.RoomCreated,
            DataJson = string.Concat('"', new string('x', 5000), '"'),
            CreatedAtUtc = now.AddMinutes(1),
        };
        var secondEntry = new DraftRoomLogEntry
        {
            RoomId = room.Id,
            Room = room,
            Type = DraftLogEntryType.SeatAdded,
            DataJson = "{}",
            CreatedAtUtc = now,
        };

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomChatMessages.AddRange(firstMessage, secondMessage);
            context.DraftRoomLogEntries.AddRange(firstEntry, secondEntry);
            await context.SaveChangesAsync();
        }

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var messageIds = await verificationContext.DraftRoomChatMessages
            .Where(message => message.RoomId == room.Id)
            .OrderBy(message => message.Id)
            .Select(message => message.Id)
            .ToArrayAsync();
        var entryIds = await verificationContext.DraftRoomLogEntries
            .Where(entry => entry.RoomId == room.Id)
            .OrderBy(entry => entry.Id)
            .Select(entry => entry.Id)
            .ToArrayAsync();

        messageIds.Should().Equal(firstMessage.Id, secondMessage.Id);
        entryIds.Should().Equal(firstEntry.Id, secondEntry.Id);
    }

    [Fact]
    public async Task SaveChanges_WhenRoomIsDeleted_ShouldCascadeDeleteChatAndLogRows()
    {
        var room = CreateRoom();

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomChatMessages.Add(new DraftRoomChatMessage
            {
                RoomId = room.Id,
                Room = room,
                Kind = ChatMessageKind.User,
                Text = "Message",
                CreatedAtUtc = DateTime.UtcNow,
            });
            context.DraftRoomLogEntries.Add(new DraftRoomLogEntry
            {
                RoomId = room.Id,
                Room = room,
                Type = DraftLogEntryType.RoomCreated,
                DataJson = "{}",
                CreatedAtUtc = DateTime.UtcNow,
            });
            await context.SaveChangesAsync();

            context.DraftRooms.Remove(room);
            await context.SaveChangesAsync();
        }

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        (await verificationContext.DraftRoomChatMessages.AnyAsync(message => message.RoomId == room.Id))
            .Should().BeFalse();
        (await verificationContext.DraftRoomLogEntries.AnyAsync(entry => entry.RoomId == room.Id))
            .Should().BeFalse();
    }

    [Fact]
    public async Task SaveChanges_WhenChatTextExceedsMaximumLength_ShouldRejectMessage()
    {
        var room = CreateRoom();

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        context.DraftRoomChatMessages.Add(new DraftRoomChatMessage
        {
            RoomId = room.Id,
            Room = room,
            Kind = ChatMessageKind.User,
            Text = new string('x', 301),
            CreatedAtUtc = DateTime.UtcNow,
        });

        var act = () => context.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task SaveChanges_WhenActiveDisplayNameKeyIsNotUnique_ShouldRejectDuplicate()
    {
        var room = CreateRoom();
        var firstParticipant = CreateParticipant(room, "Player", "player");
        var secondParticipant = CreateParticipant(room, "Player Two", "player");

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        context.DraftRoomParticipants.AddRange(firstParticipant, secondParticipant);

        var act = () => context.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task SaveChanges_WhenMultipleGuestsHaveNoUserId_ShouldAllowBothParticipants()
    {
        var room = CreateRoom();
        var firstParticipant = CreateParticipant(room, "Guest One", "guest one");
        var secondParticipant = CreateParticipant(room, "Guest Two", "guest two");

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.DraftRooms.Add(room);
        context.DraftRoomParticipants.AddRange(firstParticipant, secondParticipant);

        var affectedRows = await context.SaveChangesAsync();

        affectedRows.Should().Be(3);
    }

    [Fact]
    public async Task SaveChanges_WhenAccountUsernameIsLongerThanGuestNameLimit_ShouldPersistFullName()
    {
        const int accountUserNameLength = 256;
        var room = CreateRoom();
        var accountUserName = new string('A', accountUserNameLength);
        var participant = CreateParticipant(room, accountUserName, accountUserName.ToUpperInvariant());

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            await context.SaveChangesAsync();
        }

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedParticipant = await verificationContext.DraftRoomParticipants
            .SingleAsync(item => item.Id == participant.Id);

        persistedParticipant.DisplayName.Should().Be(accountUserName);
    }

    [Fact]
    public async Task SaveChanges_WhenAccountUsernameNeedsCollisionSuffix_ShouldPersistFullSuffixedName()
    {
        var account = CreateUser();
        var accountUserName = new string('A', 256);
        account.UserName = accountUserName;
        account.NormalizedUserName = accountUserName.ToUpperInvariant();
        var displayName = string.Concat(accountUserName, "32");
        var room = CreateRoom();
        var participant = CreateParticipant(room, displayName, displayName.ToUpperInvariant(), account.Id);

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.Users.Add(account);
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            await context.SaveChangesAsync();
        }

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedParticipant = await verificationContext.DraftRoomParticipants
            .SingleAsync(item => item.Id == participant.Id);

        persistedParticipant.DisplayName.Should().Be(displayName);
        persistedParticipant.DisplayName.Should().HaveLength(258);
    }

    [Fact]
    public async Task SaveChanges_WhenUserIdIsRepeatedInRoom_ShouldRejectDuplicate()
    {
        var user = CreateUser();
        var room = CreateRoom();
        var firstParticipant = CreateParticipant(room, "Player One", "player one", user.Id);
        var secondParticipant = CreateParticipant(room, "Player Two", "player two", user.Id);

        await using var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        context.Users.Add(user);
        context.DraftRooms.Add(room);
        context.DraftRoomParticipants.AddRange(firstParticipant, secondParticipant);

        var act = () => context.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task DeleteRoom_WhenParticipantsExist_ShouldCascadeDeleteParticipants()
    {
        var room = CreateRoom();
        var participant = CreateParticipant(room, "Guest", "guest");

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            await context.SaveChangesAsync();
        }

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            var persistedRoom = await context.DraftRooms.SingleAsync(item => item.Id == room.Id);
            context.DraftRooms.Remove(persistedRoom);
            await context.SaveChangesAsync();
        }

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var participantExists = await verificationContext.DraftRoomParticipants
            .AnyAsync(item => item.Id == participant.Id);

        participantExists.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteUser_WhenParticipantUsesAccount_ShouldClearParticipantUserId()
    {
        var user = CreateUser();
        var room = CreateRoom();
        var participant = CreateParticipant(room, "Account Player", "account player", user.Id);

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.Users.Add(user);
            context.DraftRooms.Add(room);
            context.DraftRoomParticipants.Add(participant);
            await context.SaveChangesAsync();
        }

        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            var persistedUser = await context.Users.SingleAsync(item => item.Id == user.Id);
            context.Users.Remove(persistedUser);
            await context.SaveChangesAsync();
        }

        await using var verificationContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var persistedParticipant = await verificationContext.DraftRoomParticipants
            .SingleAsync(item => item.Id == participant.Id);

        persistedParticipant.UserId.Should().BeNull();
    }

    [Fact]
    public async Task SaveChanges_WhenRoomVersionWasUpdatedByAnotherContext_ShouldThrowConcurrencyException()
    {
        var room = CreateRoom();
        await using (var context = await this.fixture.Database.ContextFactory.CreateDbContextAsync())
        {
            context.DraftRooms.Add(room);
            await context.SaveChangesAsync();
        }

        await using var firstContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        await using var secondContext = await this.fixture.Database.ContextFactory.CreateDbContextAsync();
        var firstRoom = await firstContext.DraftRooms.SingleAsync(item => item.Id == room.Id);
        var secondRoom = await secondContext.DraftRooms.SingleAsync(item => item.Id == room.Id);

        firstRoom.Version++;
        secondRoom.Version++;

        await firstContext.SaveChangesAsync();
        var act = () => secondContext.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }

    private static DraftRoom CreateRoom(string? code = null)
    {
        var now = DateTime.UtcNow;

        return new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = code ?? Guid.NewGuid().ToString("N")[..6],
            Name = "Model test room",
            PasswordHash = "test-hash",
            Status = DraftRoomStatus.Lobby,
            DraftType = "faction-pick",
            CreatedAtUtc = now,
            LastActivityUtc = now,
        };
    }

    private static DraftRoomParticipant CreateParticipant(
        DraftRoom room,
        string displayName,
        string displayNameKey,
        string? userId = null)
    {
        return new DraftRoomParticipant
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Room = room,
            UserId = userId,
            SecretHash = userId is null ? new byte[32] : null,
            DisplayName = displayName,
            DisplayNameKey = displayNameKey,
            Role = DraftRoomRole.Member,
            Status = DraftParticipantStatus.Active,
            JoinedAtUtc = DateTime.UtcNow,
            LastSeenUtc = DateTime.UtcNow,
        };
    }

    private static DraftRoomSeat CreateSeat(DraftRoom room, int index, DraftRoomParticipant? participant = null)
    {
        return new DraftRoomSeat
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Index = index,
            ParticipantId = participant?.Id,
            ClaimedAtUtc = participant is null ? null : DateTime.UtcNow,
            Room = room,
            Participant = participant,
        };
    }

    private static TwilightImperiumUser CreateUser()
    {
        var userId = Guid.NewGuid().ToString();

        return new TwilightImperiumUser
        {
            Id = userId,
            UserName = userId,
            NormalizedUserName = userId.ToUpperInvariant(),
        };
    }
}
