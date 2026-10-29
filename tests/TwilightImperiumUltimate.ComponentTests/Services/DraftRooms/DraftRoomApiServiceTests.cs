using System.Net;
using Moq;
using TwilightImperiumUltimate.Contracts.ApiContracts;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.ComponentTests.Services.DraftRooms;

public sealed class DraftRoomApiServiceTests
{
    private const string ParticipantIdHeader = "X-Draft-Participant-Id";
    private const string ParticipantSecretHeader = "X-Draft-Participant-Secret";

    [Fact]
    public async Task CreateAsync_WhenGuestModelIsSubmitted_ShouldSendGuestDisplayName()
    {
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        var entry = CreateEntry();
        apiHttpClient
            .Setup(client => client.PostApiAsync<CreateDraftRoomRequest, DraftRoomEntryDto>(
                Paths.ApiPath_DraftRooms,
                It.Is<CreateDraftRoomRequest>(request =>
                    request.Name == "Room One"
                    && request.Password == "secret1"
                    && request.DisplayName == "Guest One"),
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => ReferenceEquals(headers, null))))
            .ReturnsAsync((new ApiResponse<DraftRoomEntryDto> { Success = true, Data = entry }, HttpStatusCode.OK));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);
        var model = new CreateDraftRoomFormModel
        {
            Name = "Room One",
            Password = "secret1",
            DisplayName = "Guest One",
            IsGuest = true,
        };

        var result = await service.CreateAsync(model);

        result.Kind.Should().Be(DraftRoomApiResultKind.Success);
        result.Data.Should().BeEquivalentTo(entry);
    }

    [Fact]
    public async Task CreateAsync_WhenAccountModelIsSubmitted_ShouldOmitGuestDisplayName()
    {
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        apiHttpClient
            .Setup(client => client.PostApiAsync<CreateDraftRoomRequest, DraftRoomEntryDto>(
                Paths.ApiPath_DraftRooms,
                It.Is<CreateDraftRoomRequest>(request => ReferenceEquals(request.DisplayName, null)),
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => ReferenceEquals(headers, null))))
            .ReturnsAsync((new ApiResponse<DraftRoomEntryDto> { Success = true, Data = CreateEntry() }, HttpStatusCode.OK));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.CreateAsync(new CreateDraftRoomFormModel
        {
            Name = "Room One",
            Password = "secret1",
            DisplayName = "Ignored",
        });

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task JoinAsync_WhenGuestIdentityIsStored_ShouldSendIdentityHeadersWithoutPassword()
    {
        var identity = new GuestIdentity(
            Guid.NewGuid(),
            "ABC234",
            Guid.NewGuid(),
            "guest-secret",
            "Guest One",
            DateTimeOffset.UtcNow);
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(identity);
        apiHttpClient
            .Setup(client => client.PostApiAsync<JoinDraftRoomRequest, DraftRoomEntryDto>(
                Paths.ApiPath_DraftRoomsJoin,
                It.Is<JoinDraftRoomRequest>(request =>
                    request.Code == "ABC234"
                    && ReferenceEquals(request.Password, null)
                    && ReferenceEquals(request.DisplayName, null)),
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers =>
                    !ReferenceEquals(headers, null)
                    && headers[ParticipantIdHeader] == identity.ParticipantId.ToString()
                    && headers[ParticipantSecretHeader] == identity.Secret)))
            .ReturnsAsync((new ApiResponse<DraftRoomEntryDto> { Success = true, Data = CreateEntry() }, HttpStatusCode.OK));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.JoinAsync("abc234", null, null);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task JoinAsync_WhenCredentialsAreProvided_ShouldPreferThemOverStoredGuestIdentity()
    {
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        apiHttpClient
            .Setup(client => client.PostApiAsync<JoinDraftRoomRequest, DraftRoomEntryDto>(
                Paths.ApiPath_DraftRoomsJoin,
                It.Is<JoinDraftRoomRequest>(request =>
                    request.Code == "ABC234"
                    && request.Password == "secret1"
                    && request.DisplayName == "Account User"),
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => ReferenceEquals(headers, null))))
            .ReturnsAsync((new ApiResponse<DraftRoomEntryDto> { Success = true, Data = CreateEntry() }, HttpStatusCode.OK));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.JoinAsync("ABC234", "secret1", "Account User");

        result.IsSuccess.Should().BeTrue();
        guestIdentityStore.Verify(
            store => store.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task JoinAsync_WhenNoGuestIdentityIsStored_ShouldSendEnteredCredentials()
    {
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync((GuestIdentity?)null);
        apiHttpClient
            .Setup(client => client.PostApiAsync<JoinDraftRoomRequest, DraftRoomEntryDto>(
                Paths.ApiPath_DraftRoomsJoin,
                It.Is<JoinDraftRoomRequest>(request =>
                    request.Code == "ABC234"
                    && request.Password == "secret1"
                    && request.DisplayName == "Guest One"),
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => ReferenceEquals(headers, null))))
            .ReturnsAsync((new ApiResponse<DraftRoomEntryDto> { Success = true, Data = CreateEntry() }, HttpStatusCode.OK));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.JoinAsync("abc234", "secret1", "Guest One");

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetMyRoomsAsync_WhenApiReturnsRooms_ShouldUseAccountOnlyEndpoint()
    {
        MyDraftRoomDto[] rooms =
        [
            new() { Code = "ABC234", Name = "Friday TI" },
        ];
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        apiHttpClient
            .Setup(client => client.GetAsync<IReadOnlyList<MyDraftRoomDto>>(
                Paths.ApiPath_DraftRoomsMine,
                string.Empty,
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => ReferenceEquals(headers, null))))
            .ReturnsAsync((rooms, HttpStatusCode.OK));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.GetMyRoomsAsync();

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeSameAs(rooms);
        guestIdentityStore.Verify(
            store => store.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound, DraftRoomApiResultKind.NotFoundOrWrongPassword)]
    [InlineData(HttpStatusCode.TooManyRequests, DraftRoomApiResultKind.Throttled)]
    [InlineData(HttpStatusCode.BadRequest, DraftRoomApiResultKind.ValidationFailed)]
    [InlineData(HttpStatusCode.InternalServerError, DraftRoomApiResultKind.Failed)]
    public async Task JoinAsync_WhenApiReturnsErrorStatus_ShouldMapTheOutcome(
        HttpStatusCode statusCode,
        DraftRoomApiResultKind expectedKind)
    {
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync((GuestIdentity?)null);
        apiHttpClient
            .Setup(client => client.PostApiAsync<JoinDraftRoomRequest, DraftRoomEntryDto>(
                Paths.ApiPath_DraftRoomsJoin,
                It.IsAny<JoinDraftRoomRequest>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IReadOnlyDictionary<string, string>?>()))
            .ReturnsAsync((new ApiResponse<DraftRoomEntryDto>(), statusCode));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.JoinAsync("ABC234", "secret1", "Guest One");

        result.Kind.Should().Be(expectedKind);
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetChatHistoryAsync_WhenGuestIdentityIsStored_ShouldSendCursorAndIdentityHeaders()
    {
        var identity = new GuestIdentity(
            Guid.NewGuid(),
            "ABC234",
            Guid.NewGuid(),
            "guest-secret",
            "Guest One",
            DateTimeOffset.UtcNow);
        var message = new ChatMessageDto
        {
            Id = 8,
            DisplayName = "Guest One",
            Kind = ChatMessageKind.User,
            Text = "Hello",
        };
        IReadOnlyList<ChatMessageDto> messages = [message];
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(identity);
        apiHttpClient
            .Setup(client => client.GetAsync<IReadOnlyList<ChatMessageDto>>(
                It.Is<string>(path => path.EndsWith("/ABC234/chat", StringComparison.Ordinal)),
                "?limit=25&afterId=7",
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers =>
                    headers != null
                    && headers[ParticipantIdHeader] == identity.ParticipantId.ToString()
                    && headers[ParticipantSecretHeader] == identity.Secret)))
            .ReturnsAsync((messages, HttpStatusCode.OK));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.GetChatHistoryAsync("abc234", afterId: 7, limit: 25);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().ContainSingle().Which.Should().BeSameAs(message);
    }

    [Fact]
    public async Task GetChatHistoryAsync_WhenLimitIsOutsideServerBounds_ShouldThrowBeforeReadingIdentity()
    {
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        var service = new DraftRoomApiService(
            Mock.Of<ITwilightImperiumApiHttpClient>(),
            guestIdentityStore.Object);

        var act = () => service.GetChatHistoryAsync("ABC234", afterId: null, DraftRoomLimits.MaxHistoryPageSize + 1);

        await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        guestIdentityStore.Verify(
            store => store.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetLogHistoryAsync_WhenApiReturnsEntries_ShouldUseRoomLogEndpointAndCursor()
    {
        var entry = new DraftLogEntryDto
        {
            Id = 5,
            Type = DraftLogEntryType.SeatAdded,
            DataJson = """{"Index":3}""",
        };
        IReadOnlyList<DraftLogEntryDto> entries = [entry];
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync((GuestIdentity?)null);
        apiHttpClient
            .Setup(client => client.GetAsync<IReadOnlyList<DraftLogEntryDto>>(
                It.Is<string>(path => path.EndsWith("/ABC234/log", StringComparison.Ordinal)),
                "?limit=10&afterId=4",
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => headers == null)))
            .ReturnsAsync((entries, HttpStatusCode.OK));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.GetLogHistoryAsync("abc234", afterId: 4, limit: 10);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().ContainSingle().Which.Should().BeSameAs(entry);
    }

    [Fact]
    public async Task SendChatAsync_WhenApiAcceptsMessage_ShouldPostItWithStoredGuestIdentity()
    {
        var identity = new GuestIdentity(
            Guid.NewGuid(),
            "ABC234",
            Guid.NewGuid(),
            "guest-secret",
            "Guest One",
            DateTimeOffset.UtcNow);
        var message = new ChatMessageDto
        {
            Id = 8,
            DisplayName = "Guest One",
            Kind = ChatMessageKind.User,
            Text = "Hello",
        };
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(identity);
        apiHttpClient
            .Setup(client => client.PostApiAsync<SendDraftRoomChatMessageRequest, ChatMessageDto>(
                It.Is<string>(path => path.EndsWith("/ABC234/chat", StringComparison.Ordinal)),
                It.Is<SendDraftRoomChatMessageRequest>(request => request.Text == "Hello"),
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers =>
                    headers != null
                    && headers[ParticipantIdHeader] == identity.ParticipantId.ToString()
                    && headers[ParticipantSecretHeader] == identity.Secret)))
            .ReturnsAsync((new ApiResponse<ChatMessageDto> { Success = true, Data = message }, HttpStatusCode.OK));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.SendChatAsync("abc234", "Hello");

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeSameAs(message);
    }

    [Fact]
    public async Task SendChatAsync_WhenApiRateLimitsTheCaller_ShouldReturnThrottled()
    {
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>();
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync((GuestIdentity?)null);
        apiHttpClient
            .Setup(client => client.PostApiAsync<SendDraftRoomChatMessageRequest, ChatMessageDto>(
                It.IsAny<string>(),
                It.IsAny<SendDraftRoomChatMessageRequest>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IReadOnlyDictionary<string, string>?>()))
            .ReturnsAsync((new ApiResponse<ChatMessageDto>(), HttpStatusCode.TooManyRequests));
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.SendChatAsync("ABC234", "Hello");

        result.Kind.Should().Be(DraftRoomApiResultKind.Throttled);
        result.Data.Should().BeNull();
    }

    private static DraftRoomEntryDto CreateEntry() =>
        new()
        {
            RoomId = Guid.NewGuid(),
            Code = "ABC234",
            RoomName = "Room One",
            ParticipantId = Guid.NewGuid(),
            DisplayName = "Guest One",
            IsGuest = true,
            GuestSecret = "guest-secret",
        };
}
