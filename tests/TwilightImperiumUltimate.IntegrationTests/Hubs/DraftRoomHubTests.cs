using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.IntegrationTests.Hosting;
using TwilightImperiumUltimate.IntegrationTests.Infrastructure;

namespace TwilightImperiumUltimate.IntegrationTests.Hubs;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1001:Types that own disposable fields should be disposable",
    Justification = "xUnit disposes the test factory through IAsyncLifetime.DisposeAsync.")]
public sealed class DraftRoomHubTests : IAsyncLifetime
{
    private const string TestUserId = "draft-room-hub-test-user";
    private const string RoomPassword = "test-password";

    private static readonly JsonSerializerOptions JsonOptions = CreateJsonOptions();

    private TestDatabase _database = default!;
    private TestApiFactory _factory = default!;
    private HttpClient _client = default!;

    public async Task InitializeAsync()
    {
        this._database = await TestDatabase.CreateAsync();
        this._factory = new TestApiFactory(this._database, new TestPresenceTimeProvider());
        var userManager = this._factory.Services.GetRequiredService<UserManager<TwilightImperiumUser>>();
        var userCreation = await userManager.CreateAsync(
            new TwilightImperiumUser
            {
                Id = TestUserId,
                UserName = "DraftRoomTestHost",
                Email = "draft-room-test-host@example.test",
            },
            "Hub-Test-Password1!");
        if (!userCreation.Succeeded)
        {
            throw new InvalidOperationException(
                $"The test identity could not be created: {string.Join(", ", userCreation.Errors.Select(error => error.Code))}");
        }

        this._client = this._factory.CreateClient();
        this._client.DefaultRequestHeaders.Add(TestAuthenticationHandler.UserIdHeader, TestUserId);
    }

    public async Task DisposeAsync()
    {
        this._client.Dispose();
        await this._factory.DisposeAsync();
        await this._database.DisposeAsync();
    }

    [Fact]
    public async Task FactionPickFlowRedaction_WhenSixViewersObserveSecretChoices_ShouldPreservePrivacyThroughUndoAndFinish()
    {
        var entry = await this.CreateAccountRoomAsync("Redaction matrix");
        var initial = await this.GetRoomViewAsync(entry.Code);
        foreach (var seat in initial.Seats.Where(seat => seat.ParticipantId is not null))
        {
            using var unclaimed = await this._client.PostAsync(new Uri($"/api/draft-rooms/{entry.Code}/seats/{seat.Id}/unclaim", UriKind.Relative), null);
            unclaimed.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        foreach (var seat in initial.Seats.Skip(3))
        {
            using var removed = await this._client.DeleteAsync(new Uri($"/api/draft-rooms/{entry.Code}/seats/{seat.Id}", UriKind.Relative));
            removed.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        var guests = new List<DraftRoomEntryDto>();
        foreach (var name in new[] { "Alice", "Bob", "Carol", "CoHost", "Spectator" })
        {
            guests.Add(await this.JoinAsGuestAsync(entry.Code, name));
        }

        using var role = await this._client.PutAsJsonAsync(
            $"/api/draft-rooms/{entry.Code}/participants/{guests[3].ParticipantId}/role",
            new SetParticipantRoleRequest(DraftRoomRole.CoHost),
            JsonOptions);
        role.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var guestClients = guests.Select(this.CreateGuestClient).ToArray();
        var connections = new[] { this.CreateHubConnection() }.Concat(guests.Select(_ => this.CreateHubConnection(userId: null))).ToArray();
        try
        {
            var room = await this.GetRoomViewAsync(entry.Code);
            for (var index = 0; index < 3; index++)
            {
                using var claimed = await guestClients[index].PostAsync(new Uri($"/api/draft-rooms/{entry.Code}/seats/{room.Seats[index].Id}/claim", UriKind.Relative), null);
                claimed.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }

            using var settingsContent = JsonContent.Create<DraftSettingsDto>(
                new FactionPickSettingsDto { BansMode = FactionPickBansMode.SimultaneousPlayerBans, PicksPerPlayer = 2 }, options: JsonOptions);
            using var settings = await this._client.PutAsync(
                new Uri($"/api/draft-rooms/{entry.Code}/draft/settings", UriKind.Relative),
                settingsContent);
            settings.StatusCode.Should().Be(HttpStatusCode.NoContent);
            using var started = await this._client.PostAsync(new Uri($"/api/draft-rooms/{entry.Code}/draft/start", UriKind.Relative), null);
            started.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var payloads = connections.Select(_ => new ConcurrentQueue<string>()).ToArray();
            var versions = connections.Select(_ => Channel.CreateUnbounded<int>()).ToArray();
            var reveals = connections.Select(_ => new TaskCompletionSource<DraftLogEntryDto>(TaskCreationOptions.RunContinuationsAsynchronously)).ToArray();
            var disclosures = connections.Select(_ => new TaskCompletionSource<DraftLogEntryDto>(TaskCreationOptions.RunContinuationsAsynchronously)).ToArray();
            for (var index = 0; index < connections.Length; index++)
            {
                var viewer = index;
                connections[index].On<int>("RoomChanged", version =>
                {
                    payloads[viewer].Enqueue(JsonSerializer.Serialize(version));
                    versions[viewer].Writer.TryWrite(version);
                });
                connections[index].On<ChatMessageDto>("ChatMessageReceived", message => payloads[viewer].Enqueue(JsonSerializer.Serialize(message)));
                connections[index].On<DraftLogEntryDto>("LogEntryAdded", log =>
                {
                    payloads[viewer].Enqueue(JsonSerializer.Serialize(log));
                    if (log.Type == DraftLogEntryType.BansRevealed)
                    {
                        reveals[viewer].TrySetResult(log);
                    }

                    if (log.Type == DraftLogEntryType.HostDecidedForSeat)
                    {
                        disclosures[viewer].TrySetResult(log);
                    }
                });
                await connections[index].StartAsync();
                var credentials = index == 0 ? new DraftRoomCredentials() : new DraftRoomCredentials
                {
                    GuestParticipantId = guests[index - 1].ParticipantId,
                    GuestSecret = guests[index - 1].GuestSecret,
                };
                await connections[index].InvokeAsync<int>("Subscribe", entry.Code, credentials);
            }

            var allClients = new[] { this._client }.Concat(guestClients).ToArray();
            var ownView = await guestClients[0].GetFromJsonAsync<DraftRoomViewDto>($"/api/draft-rooms/{entry.Code}", JsonOptions);
            var faction = ((FactionPickViewDto)ownView!.Draft!).Pool[0];
            await SendDraftActionAsync(guestClients[0], entry.Code, new SelectBanAction { Faction = faction });
            var selectedVersion = (await this.GetRoomViewAsync(entry.Code)).Version;
            await WaitForVersionsAsync(versions, selectedVersion);

            for (var index = 0; index < allClients.Length; index++)
            {
                var personalized = await allClients[index].GetFromJsonAsync<DraftRoomViewDto>($"/api/draft-rooms/{entry.Code}", JsonOptions);
                var projected = (FactionPickViewDto)personalized!.Draft!;
                projected.PublicBans.Should().BeEmpty();
                projected.MyBans.Should().Equal(index == 1 ? new[] { faction } : []);
                payloads[index].Should().OnlyContain(payload => !payload.Contains("Factions", StringComparison.Ordinal));
            }

            await SendDraftActionAsync(guestClients[0], entry.Code, new LockInBansAction());
            using var released = await this._client.PostAsync(new Uri($"/api/draft-rooms/{entry.Code}/seats/{room.Seats[0].Id}/unclaim", UriKind.Relative), null);
            released.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var cleared = await guestClients[0].GetFromJsonAsync<DraftRoomViewDto>($"/api/draft-rooms/{entry.Code}", JsonOptions);
            ((FactionPickViewDto)cleared!.Draft!).MyBans.Should().BeEmpty();
            ((FactionPickViewDto)cleared.Draft!).LockedBanSeatIds.Should().NotContain(room.Seats[0].Id);
            await using (var context = await this._database.ContextFactory.CreateDbContextAsync())
            {
                var discarded = await context.DraftRoomLogEntries.SingleAsync(log => log.RoomId == initial.RoomId && log.Type == DraftLogEntryType.SeatChoicesDiscarded);
                discarded.DataJson.Should().NotContain("Factions");
            }

            using var reclaimed = await guestClients[0].PostAsync(new Uri($"/api/draft-rooms/{entry.Code}/seats/{room.Seats[0].Id}/claim", UriKind.Relative), null);
            reclaimed.StatusCode.Should().Be(HttpStatusCode.NoContent);
            await SendDraftActionAsync(guestClients[0], entry.Code, new SelectBanAction { Faction = faction });
            await SendDraftActionAsync(guestClients[0], entry.Code, new LockInBansAction());
            var bobFaction = ((FactionPickViewDto)ownView.Draft!).Pool[1];
            await SendDraftActionAsync(guestClients[1], entry.Code, new SelectBanAction { Faction = bobFaction });
            var decision = new DecideForRequest(room.Seats[1].Id, new DecideBansAction());
            using var forbidden = await guestClients[1].PostAsJsonAsync($"/api/draft-rooms/{entry.Code}/draft/decide-for", decision, JsonOptions);
            forbidden.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            using var decided = await guestClients[3].PostAsJsonAsync($"/api/draft-rooms/{entry.Code}/draft/decide-for", decision, JsonOptions);
            decided.StatusCode.Should().Be(HttpStatusCode.NoContent);
            foreach (var disclosure in disclosures)
            {
                var log = await disclosure.Task.WaitAsync(TimeSpan.FromSeconds(10));
                using var document = JsonDocument.Parse(log.DataJson);
                document.RootElement.GetProperty("Factions").EnumerateArray().Select(value => value.GetInt32()).Should().Equal((int)bobFaction);
            }

            await SendDraftActionAsync(guestClients[2], entry.Code, new LockInBansAction());
            foreach (var reveal in reveals)
            {
                var log = await reveal.Task.WaitAsync(TimeSpan.FromSeconds(10));
                using var document = JsonDocument.Parse(log.DataJson);
                document.RootElement.GetProperty("Factions")[0].GetInt32().Should().Be((int)faction);
            }

            foreach (var client in allClients)
            {
                var personalized = await client.GetFromJsonAsync<DraftRoomViewDto>($"/api/draft-rooms/{entry.Code}", JsonOptions);
                ((FactionPickViewDto)personalized!.Draft!).PublicBans.Should().Equal(faction);
            }

            await this.PlayPicksWithUndoAndManagerDecisionAsync(entry.Code, allClients, guestClients);
        }
        finally
        {
            foreach (var connection in connections)
            {
                await connection.DisposeAsync();
            }

            foreach (var client in guestClients)
            {
                client.Dispose();
            }
        }
    }

    [Fact]
    public async Task ClaimSeat_WhenTwoHubClientsAreSubscribed_ShouldNotifyBothClients()
    {
        var entry = await this.CreateAccountRoomAsync("Hub integration");
        var room = await this.GetRoomViewAsync(entry.Code);
        var openSeat = room.Seats.First(seat => seat.ParticipantId is null);

        var firstNotification = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        var secondNotification = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        await using var firstConnection = this.CreateHubConnection();
        await using var secondConnection = this.CreateHubConnection();
        firstConnection.On<int>("RoomChanged", version => firstNotification.TrySetResult(version));
        secondConnection.On<int>("RoomChanged", version => secondNotification.TrySetResult(version));

        await firstConnection.StartAsync();
        var firstSubscribedVersion = await firstConnection.InvokeAsync<int>(
            "Subscribe",
            entry.Code,
            new DraftRoomCredentials());
        await secondConnection.StartAsync();
        var secondSubscribedVersion = await secondConnection.InvokeAsync<int>(
            "Subscribe",
            entry.Code,
            new DraftRoomCredentials());

        firstSubscribedVersion.Should().Be(secondSubscribedVersion);

        using var claimResponse = await this._client.PostAsync(
            new Uri($"/api/draft-rooms/{entry.Code}/seats/{openSeat.Id}/claim", UriKind.Relative),
            content: null);
        claimResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var firstVersion = await firstNotification.Task.WaitAsync(TimeSpan.FromSeconds(10));
        var secondVersion = await secondNotification.Task.WaitAsync(TimeSpan.FromSeconds(10));
        firstVersion.Should().BeGreaterThan(firstSubscribedVersion);
        secondVersion.Should().Be(firstVersion);
    }

    [Fact]
    public async Task Presence_WhenGuestReconnectsWithinGracePeriod_ShouldPublishOneJoinAndOneLeave()
    {
        var room = await this.CreateAccountRoomAsync("Presence integration");
        var guest = await this.JoinAsGuestAsync(room.Code, "Guest participant");
        var initialRoomVersion = (await this.GetRoomViewAsync(room.Code)).Version;
        var guestJoinedNotification = new TaskCompletionSource<ChatMessageDto>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var guestLeftNotification = new TaskCompletionSource<ChatMessageDto>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        await using var hostConnection = this.CreateHubConnection();
        hostConnection.On<ChatMessageDto>("ChatMessageReceived", message =>
        {
            if (message.ParticipantId != guest.ParticipantId)
            {
                return;
            }

            if (message.Kind == ChatMessageKind.Join)
            {
                guestJoinedNotification.TrySetResult(message);
            }
            else if (message.Kind == ChatMessageKind.Leave)
            {
                guestLeftNotification.TrySetResult(message);
            }
        });
        await hostConnection.StartAsync();
        await hostConnection.InvokeAsync<int>("Subscribe", room.Code, new DraftRoomCredentials());

        await using var guestConnection = this.CreateHubConnection(userId: null);
        await guestConnection.StartAsync();
        await guestConnection.InvokeAsync<int>(
            "Subscribe",
            room.Code,
            new DraftRoomCredentials
            {
                GuestParticipantId = guest.ParticipantId,
                GuestSecret = guest.GuestSecret,
            });

        (await this.GetRoomViewAsync(room.Code))
            .Participants.Single(participant => participant.Id == guest.ParticipantId)
            .IsOnline.Should().BeTrue();
        (await guestJoinedNotification.Task.WaitAsync(TimeSpan.FromSeconds(10)))
            .Text.Should().Be("Guest participant joined");
        var messages = await this.WaitForPresenceMessagesAsync(room.RoomId, guest.ParticipantId, 1);
        messages.Should().ContainSingle().Which.Kind.Should().Be(ChatMessageKind.Join);

        await guestConnection.StopAsync();
        await Task.Delay(TimeSpan.FromMilliseconds(100));

        await using var reconnectedGuestConnection = this.CreateHubConnection(userId: null);
        await reconnectedGuestConnection.StartAsync();
        await reconnectedGuestConnection.InvokeAsync<int>(
            "Subscribe",
            room.Code,
            new DraftRoomCredentials
            {
                GuestParticipantId = guest.ParticipantId,
                GuestSecret = guest.GuestSecret,
            });
        await Task.Delay(TimeSpan.FromMilliseconds(1200));

        guestLeftNotification.Task.IsCompleted.Should().BeFalse();
        messages = await this.GetPresenceMessagesAsync(room.RoomId, guest.ParticipantId);
        messages.Should().ContainSingle().Which.Kind.Should().Be(ChatMessageKind.Join);
        (await this.GetRoomViewAsync(room.Code))
            .Participants.Single(participant => participant.Id == guest.ParticipantId)
            .IsOnline.Should().BeTrue();

        await reconnectedGuestConnection.StopAsync();
        (await guestLeftNotification.Task.WaitAsync(TimeSpan.FromSeconds(10)))
            .Text.Should().Be("Guest participant left");
        messages = await this.WaitForPresenceMessagesAsync(room.RoomId, guest.ParticipantId, 2);
        messages.Select(message => message.Kind).Should().ContainInOrder(
            ChatMessageKind.Join,
            ChatMessageKind.Leave);
        messages.Select(message => message.Text).Should().ContainInOrder(
            "Guest participant joined",
            "Guest participant left");

        var roomView = await this.GetRoomViewAsync(room.Code);
        roomView.Participants.Single(participant => participant.Id == guest.ParticipantId).IsOnline.Should().BeFalse();
        roomView.Version.Should().Be(initialRoomVersion);
    }

    [Fact]
    public async Task Subscribe_WhenGuestSecretIsWrong_ShouldRejectConnection()
    {
        var guestRoom = await this.CreateGuestRoomAsync("Guest hub", "Guest host");
        await using var connection = this.CreateHubConnection(userId: null);
        await connection.StartAsync();

        var subscribe = connection.InvokeAsync<int>(
            "Subscribe",
            guestRoom.Code,
            new DraftRoomCredentials
            {
                GuestParticipantId = guestRoom.ParticipantId,
                GuestSecret = "incorrect-secret",
            });

        await FluentActions.Awaiting(() => subscribe).Should().ThrowAsync<HubException>();
    }

    [Fact]
    public async Task Subscribe_WhenAuthenticatedAccountHasGuestCredentials_ShouldUseGuestParticipant()
    {
        var guestRoom = await this.CreateGuestRoomAsync("Authenticated guest hub", "Guest host");
        await using var connection = this.CreateHubConnection();
        await connection.StartAsync();

        var version = await connection.InvokeAsync<int>(
            "Subscribe",
            guestRoom.Code,
            new DraftRoomCredentials
            {
                GuestParticipantId = guestRoom.ParticipantId,
                GuestSecret = guestRoom.GuestSecret,
            });

        version.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ClaimSeat_WhenAnotherRoomIsSubscribed_ShouldNotNotifyThatRoom()
    {
        var roomA = await this.CreateAccountRoomAsync("Room A");
        var roomB = await this.CreateAccountRoomAsync("Room B");
        var roomAView = await this.GetRoomViewAsync(roomA.Code);
        var seatToClaim = roomAView.Seats.First(seat => seat.ParticipantId is null);
        var roomAChanged = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        var roomBChanged = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);

        await using var roomAConnection = this.CreateHubConnection();
        await using var roomBConnection = this.CreateHubConnection();
        roomAConnection.On<int>("RoomChanged", version => roomAChanged.TrySetResult(version));
        roomBConnection.On<int>("RoomChanged", version => roomBChanged.TrySetResult(version));
        await roomAConnection.StartAsync();
        await roomAConnection.InvokeAsync<int>("Subscribe", roomA.Code, new DraftRoomCredentials());
        await roomBConnection.StartAsync();
        await roomBConnection.InvokeAsync<int>("Subscribe", roomB.Code, new DraftRoomCredentials());

        using var claimResponse = await this._client.PostAsync(
            new Uri($"/api/draft-rooms/{roomA.Code}/seats/{seatToClaim.Id}/claim", UriKind.Relative),
            content: null);
        claimResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await roomAChanged.Task.WaitAsync(TimeSpan.FromSeconds(10));

        await Assert.ThrowsAsync<TimeoutException>(
            () => roomBChanged.Task.WaitAsync(TimeSpan.FromSeconds(2)));
    }

    [Fact]
    public async Task ChatAndLogEvents_WhenGeneratedInAnotherRoom_ShouldNotBeDelivered()
    {
        var roomA = await this.CreateAccountRoomAsync("Chat and log room A");
        var roomB = await this.CreateAccountRoomAsync("Chat and log room B");
        var roomAView = await this.GetRoomViewAsync(roomA.Code);
        var seatToClaim = roomAView.Seats.First(seat => seat.ParticipantId is null);
        var roomAChat = new TaskCompletionSource<ChatMessageDto>(TaskCreationOptions.RunContinuationsAsynchronously);
        var roomBChat = new TaskCompletionSource<ChatMessageDto>(TaskCreationOptions.RunContinuationsAsynchronously);
        var roomALog = new TaskCompletionSource<DraftLogEntryDto>(TaskCreationOptions.RunContinuationsAsynchronously);
        var roomBLog = new TaskCompletionSource<DraftLogEntryDto>(TaskCreationOptions.RunContinuationsAsynchronously);

        await using var roomAConnection = this.CreateHubConnection();
        await using var roomBConnection = this.CreateHubConnection();
        roomAConnection.On<ChatMessageDto>("ChatMessageReceived", message =>
        {
            if (message.Kind == ChatMessageKind.User && message.Text == "Room A only")
            {
                roomAChat.TrySetResult(message);
            }
        });
        roomBConnection.On<ChatMessageDto>("ChatMessageReceived", message =>
        {
            if (message.Kind == ChatMessageKind.User && message.Text == "Room A only")
            {
                roomBChat.TrySetResult(message);
            }
        });
        roomAConnection.On<DraftLogEntryDto>("LogEntryAdded", entry =>
        {
            if (entry.Type == DraftLogEntryType.SeatClaimed)
            {
                roomALog.TrySetResult(entry);
            }
        });
        roomBConnection.On<DraftLogEntryDto>("LogEntryAdded", entry => roomBLog.TrySetResult(entry));
        await roomAConnection.StartAsync();
        await roomAConnection.InvokeAsync<int>("Subscribe", roomA.Code, new DraftRoomCredentials());
        await roomBConnection.StartAsync();
        await roomBConnection.InvokeAsync<int>("Subscribe", roomB.Code, new DraftRoomCredentials());

        using var chatResponse = await this._client.PostAsJsonAsync(
            $"/api/draft-rooms/{roomA.Code}/chat",
            new SendDraftRoomChatMessageRequest { Text = "Room A only" });
        chatResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        (await roomAChat.Task.WaitAsync(TimeSpan.FromSeconds(10))).Text.Should().Be("Room A only");

        using var claimResponse = await this._client.PostAsync(
            new Uri($"/api/draft-rooms/{roomA.Code}/seats/{seatToClaim.Id}/claim", UriKind.Relative),
            content: null);
        claimResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await roomALog.Task.WaitAsync(TimeSpan.FromSeconds(10))).Type.Should().Be(DraftLogEntryType.SeatClaimed);

        await Assert.ThrowsAsync<TimeoutException>(
            () => roomBChat.Task.WaitAsync(TimeSpan.FromSeconds(2)));
        await Assert.ThrowsAsync<TimeoutException>(
            () => roomBLog.Task.WaitAsync(TimeSpan.FromSeconds(2)));
    }

    [Fact]
    public async Task Join_WhenGuestJoins_ShouldBroadcastPersistedLogEntry()
    {
        var room = await this.CreateAccountRoomAsync("Join log integration");
        var logNotification = new TaskCompletionSource<(DraftLogEntryDto Entry, bool IsPersisted)>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        await using var hostConnection = this.CreateHubConnection();
        hostConnection.On<DraftLogEntryDto>("LogEntryAdded", async entry =>
        {
            if (entry.Type != DraftLogEntryType.ParticipantJoined)
            {
                return;
            }

            await using var context = await this._database.ContextFactory.CreateDbContextAsync();
            var isPersisted = await context.DraftRoomLogEntries
                .AnyAsync(item => item.Id == entry.Id && item.RoomId == room.RoomId);
            logNotification.TrySetResult((entry, isPersisted));
        });
        await hostConnection.StartAsync();
        await hostConnection.InvokeAsync<int>("Subscribe", room.Code, new DraftRoomCredentials());

        var guest = await this.JoinAsGuestAsync(room.Code, "Joining guest");
        var notification = await logNotification.Task.WaitAsync(TimeSpan.FromSeconds(10));

        notification.Entry.ActorParticipantId.Should().Be(guest.ParticipantId);
        notification.IsPersisted.Should().BeTrue();
    }

    [Fact]
    public async Task ChatAndLogHistory_WhenActiveMemberRequestsPages_ShouldReturnAuthorizedBoundedHistory()
    {
        var room = await this.CreateAccountRoomAsync("History integration");

        using var firstSendResponse = await this._client.PostAsJsonAsync(
            $"/api/draft-rooms/{room.Code}/chat",
            new SendDraftRoomChatMessageRequest { Text = "First history message" });
        firstSendResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var firstMessage = await firstSendResponse.Content.ReadFromJsonAsync<ChatMessageDto>(JsonOptions);
        firstMessage.Should().NotBeNull();

        using var secondSendResponse = await this._client.PostAsJsonAsync(
            $"/api/draft-rooms/{room.Code}/chat",
            new SendDraftRoomChatMessageRequest { Text = "Second history message" });
        secondSendResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        using var chatResponse = await this._client.GetAsync(
            new Uri($"/api/draft-rooms/{room.Code}/chat?afterId={firstMessage!.Id}&limit=1", UriKind.Relative));
        chatResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var chatMessages = await chatResponse.Content.ReadFromJsonAsync<List<ChatMessageDto>>(JsonOptions);
        chatMessages.Should().ContainSingle()
            .Which.Text.Should().Be("Second history message");

        using var logResponse = await this._client.GetAsync(
            new Uri($"/api/draft-rooms/{room.Code}/log?limit=1", UriKind.Relative));
        logResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var logEntries = await logResponse.Content.ReadFromJsonAsync<List<DraftLogEntryDto>>(JsonOptions);
        var roomCreatedEntry = logEntries.Should().ContainSingle()
            .Which;
        roomCreatedEntry.Type.Should().Be(DraftLogEntryType.RoomCreated);

        var roomView = await this.GetRoomViewAsync(room.Code);
        var seatToClaim = roomView.Seats.First(seat => seat.ParticipantId is null);
        using var claimResponse = await this._client.PostAsync(
            new Uri($"/api/draft-rooms/{room.Code}/seats/{seatToClaim.Id}/claim", UriKind.Relative),
            content: null);
        claimResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var nextLogResponse = await this._client.GetAsync(
            new Uri($"/api/draft-rooms/{room.Code}/log?afterId={roomCreatedEntry.Id}&limit=2", UriKind.Relative));
        nextLogResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var nextLogEntries = await nextLogResponse.Content.ReadFromJsonAsync<List<DraftLogEntryDto>>(JsonOptions);
        nextLogEntries.Should().NotBeNull();
        nextLogEntries!.Select(entry => entry.Type).Should().Equal(
            DraftLogEntryType.SeatUnclaimed,
            DraftLogEntryType.SeatClaimed);

        using var invalidLimitResponse = await this._client.GetAsync(
            new Uri(
                $"/api/draft-rooms/{room.Code}/chat?limit={DraftRoomLimits.MaxHistoryPageSize + 1}",
                UriKind.Relative));
        invalidLimitResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        using var invalidCursorResponse = await this._client.GetAsync(
            new Uri($"/api/draft-rooms/{room.Code}/log?afterId=-1", UriKind.Relative));
        invalidCursorResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ChatHistory_WhenCallerIsNotRoomMember_ShouldReturnNotFound()
    {
        var guestRoom = await this.CreateGuestRoomAsync("Private history", "Guest host");

        using var response = await this._client.GetAsync(
            new Uri($"/api/draft-rooms/{guestRoom.Code}/chat", UriKind.Relative));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task KickParticipant_WhenGuestIsSubscribed_ShouldNotifyThatParticipant()
    {
        var room = await this.CreateAccountRoomAsync("Kick integration");
        var guest = await this.JoinAsGuestAsync(room.Code, "Guest participant");
        var removedFromRoom = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        await using var guestConnection = this.CreateHubConnection(userId: null);
        guestConnection.On("RemovedFromRoom", () => removedFromRoom.TrySetResult());
        await guestConnection.StartAsync();
        await guestConnection.InvokeAsync<int>(
            "Subscribe",
            room.Code,
            new DraftRoomCredentials
            {
                GuestParticipantId = guest.ParticipantId,
                GuestSecret = guest.GuestSecret,
            });

        using var kickResponse = await this._client.PostAsync(
            new Uri($"/api/draft-rooms/{room.Code}/participants/{guest.ParticipantId}/kick", UriKind.Relative),
            content: null);
        kickResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        await removedFromRoom.Task.WaitAsync(TimeSpan.FromSeconds(10));
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }

    private static async Task SendDraftActionAsync(HttpClient client, string code, DraftActionDto action)
    {
        using var content = JsonContent.Create(action, options: JsonOptions);
        using var response = await client.PostAsync(
            new Uri($"/api/draft-rooms/{code}/draft/actions", UriKind.Relative),
            content);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private static async Task WaitForVersionsAsync(IReadOnlyList<Channel<int>> versions, int target)
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        foreach (var channel in versions)
        {
            var observed = await channel.Reader.ReadAsync(timeout.Token);
            while (observed < target)
            {
                observed = await channel.Reader.ReadAsync(timeout.Token);
            }
        }
    }

    private HttpClient CreateGuestClient(DraftRoomEntryDto guest)
    {
        var client = this._factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Draft-Participant-Id", guest.ParticipantId.ToString());
        client.DefaultRequestHeaders.Add("X-Draft-Participant-Secret", guest.GuestSecret);
        return client;
    }

    private async Task PlayPicksWithUndoAndManagerDecisionAsync(string code, IReadOnlyList<HttpClient> allClients, HttpClient[] guestClients)
    {
        var view = await this.GetRoomViewAsync(code);
        var draft = (FactionPickViewDto)view.Draft!;
        var originalOrder = draft.TurnOrder.ToArray();
        var actedSeats = new List<Guid>();
        while (view.Status == DraftRoomStatus.InProgress)
        {
            draft = (FactionPickViewDto)view.Draft!;
            var seat = view.Seats.Single(item => item.Id == draft.ActiveSeatId);
            var actor = guestClients[seat.Index - 1];
            var faction = draft.Pool.First(candidate => !draft.PublicBans.Contains(candidate) && !draft.LockedPicks.Any(pick => pick.Faction == candidate));
            await SendDraftActionAsync(actor, code, new SelectPickAction { Faction = faction });
            foreach (var client in allClients)
            {
                var personalized = await client.GetFromJsonAsync<DraftRoomViewDto>($"/api/draft-rooms/{code}", JsonOptions);
                ((FactionPickViewDto)personalized!.Draft!).MyPendingPick.Should().Be(ReferenceEquals(actor, client) ? faction : null);
            }

            if (draft.TurnIndex == 0 && actedSeats.Count == 0)
            {
                var path = new Uri($"/api/draft-rooms/{code}/draft/actions", UriKind.Relative);
                using var contentOne = JsonContent.Create<DraftActionDto>(new LockInPickAction(), options: JsonOptions);
                using var contentTwo = JsonContent.Create<DraftActionDto>(new LockInPickAction(), options: JsonOptions);
                var responses = await Task.WhenAll(actor.PostAsync(path, contentOne), actor.PostAsync(path, contentTwo));
                using var firstResponse = responses[0];
                using var secondResponse = responses[1];

                responses.Count(response => response.StatusCode == HttpStatusCode.NoContent).Should().Be(1);
                responses.Count(response => response.StatusCode == HttpStatusCode.Conflict).Should().Be(1);
                using var undo = await this._client.PostAsync(new Uri($"/api/draft-rooms/{code}/draft/undo", UriKind.Relative), null);
                undo.StatusCode.Should().Be(HttpStatusCode.NoContent);
                view = await this.GetRoomViewAsync(code);
                ((FactionPickViewDto)view.Draft!).TurnIndex.Should().Be(0);
                ((FactionPickViewDto)view.Draft!).LockedPicks.Should().BeEmpty();
                await SendDraftActionAsync(actor, code, new SelectPickAction { Faction = faction });
                using var decision = await this._client.PostAsJsonAsync(
                    $"/api/draft-rooms/{code}/draft/decide-for",
                    new DecideForRequest(seat.Id, new DecidePickAction { Faction = faction, TurnIndex = 0 }),
                    JsonOptions);
                decision.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
            else
            {
                await SendDraftActionAsync(actor, code, new LockInPickAction { TurnIndex = draft.TurnIndex });
            }

            actedSeats.Add(seat.Id);
            view = await this.GetRoomViewAsync(code);
        }

        actedSeats.Should().Equal(originalOrder.Concat(originalOrder.Reverse()));
        view.Status.Should().Be(DraftRoomStatus.Finished);
        var finished = (FactionPickViewDto)view.Draft!;
        finished.LockedPicks.Should().HaveCount(6);
        finished.LockedPicks[0].DecidedByHost.Should().BeTrue();
        finished.MyPendingPick.Should().BeNull();
        using var refusedUndo = await this._client.PostAsync(new Uri($"/api/draft-rooms/{code}/draft/undo", UriKind.Relative), null);
        refusedUndo.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    private async Task<DraftRoomEntryDto> CreateAccountRoomAsync(string name)
    {
        using var response = await this._client.PostAsJsonAsync(
            "/api/draft-rooms",
            new CreateDraftRoomRequest(name, RoomPassword, null));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var entry = await response.Content.ReadFromJsonAsync<DraftRoomEntryDto>(JsonOptions);
        entry.Should().NotBeNull();
        return entry!;
    }

    private async Task<DraftRoomEntryDto> CreateGuestRoomAsync(string name, string displayName)
    {
        using var guestClient = this._factory.CreateClient();
        using var response = await guestClient.PostAsJsonAsync(
            "/api/draft-rooms",
            new CreateDraftRoomRequest(name, RoomPassword, displayName));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var entry = await response.Content.ReadFromJsonAsync<DraftRoomEntryDto>(JsonOptions);
        entry.Should().NotBeNull();
        return entry!;
    }

    private async Task<DraftRoomEntryDto> JoinAsGuestAsync(string code, string displayName)
    {
        using var guestClient = this._factory.CreateClient();
        using var response = await guestClient.PostAsJsonAsync(
            "/api/draft-rooms/join",
            new JoinDraftRoomRequest(code, RoomPassword, displayName));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var entry = await response.Content.ReadFromJsonAsync<DraftRoomEntryDto>(JsonOptions);
        entry.Should().NotBeNull();
        return entry!;
    }

    private async Task<DraftRoomViewDto> GetRoomViewAsync(string code)
    {
        using var response = await this._client.GetAsync(new Uri($"/api/draft-rooms/{code}", UriKind.Relative));
        response.EnsureSuccessStatusCode();

        var view = await response.Content.ReadFromJsonAsync<DraftRoomViewDto>(JsonOptions);
        view.Should().NotBeNull();
        return view!;
    }

    private async Task<List<DraftRoomChatMessage>> WaitForPresenceMessagesAsync(
        Guid roomId,
        Guid participantId,
        int expectedCount)
    {
        for (var attempt = 0; attempt < 100; attempt++)
        {
            var messages = await this.GetPresenceMessagesAsync(roomId, participantId);
            if (messages.Count >= expectedCount)
            {
                return messages;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(50));
        }

        throw new TimeoutException("Expected draft-room presence chat messages were not persisted.");
    }

    private async Task<List<DraftRoomChatMessage>> GetPresenceMessagesAsync(Guid roomId, Guid participantId)
    {
        await using var context = await this._database.ContextFactory.CreateDbContextAsync();

        return await context.DraftRoomChatMessages
            .AsNoTracking()
            .Where(message => message.RoomId == roomId && message.ParticipantId == participantId)
            .OrderBy(message => message.Id)
            .ToListAsync();
    }

    private HubConnection CreateHubConnection(string? userId = TestUserId)
    {
        return new HubConnectionBuilder()
            .WithUrl(
                new Uri(this._factory.Server.BaseAddress, "/hubs/draft-rooms"),
                options =>
                {
                    options.Transports = HttpTransportType.LongPolling;
                    if (userId is not null)
                    {
                        options.Headers.Add(TestAuthenticationHandler.UserIdHeader, userId);
                    }

                    options.HttpMessageHandlerFactory = _ => this._factory.Server.CreateHandler();
                })
                .Build();
    }
}
