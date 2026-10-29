using System.Net;
using Moq;
using TwilightImperiumUltimate.Contracts.ApiContracts;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.ComponentTests.Services.DraftRooms;

public sealed class DraftRoomApiServiceManagementTests
{
    private const string ParticipantIdHeader = "X-Draft-Participant-Id";
    private const string ParticipantSecretHeader = "X-Draft-Participant-Secret";

    [Fact]
    public async Task GetResult_WhenPublic_ShouldPreserveCaseAndNeverReadGuestCredentials()
    {
        var client = new Mock<ITwilightImperiumApiHttpClient>(MockBehavior.Strict);
        var identities = new Mock<IGuestIdentityStore>(MockBehavior.Strict);
        client.Setup(api => api.GetAsync<DraftResultDto>(
                "api/draft-rooms/results/aB_-012345678901234567",
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                null))
            .ReturnsAsync(((DraftResultDto?)null, HttpStatusCode.NotFound));

        var response = await new DraftRoomApiService(client.Object, identities.Object).GetResultAsync("aB_-012345678901234567");

        response.Kind.Should().Be(DraftRoomApiResultKind.NotFound);
        identities.VerifyNoOtherCalls();
        client.VerifyAll();
    }

    [Fact]
    public async Task ResultCommands_WhenGuestManager_ShouldUseRoomCredentialHeadersAndNeverPutSecretsInUrls()
    {
        var (service, client, identity) = CreateService();
        var resultId = Guid.NewGuid();
        client.Setup(api => api.GetAsync<DraftResultDto>(
                "api/draft-rooms/ABC234/result",
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => headers != null && headers[ParticipantSecretHeader] == identity.Secret)))
            .ReturnsAsync(((DraftResultDto?)null, HttpStatusCode.NotFound));
        client.Setup(api => api.SendAsync(
                HttpMethod.Delete,
                $"api/draft-rooms/results/{resultId:D}",
                It.Is<IReadOnlyDictionary<string, string>?>(headers => headers != null && headers[ParticipantSecretHeader] == identity.Secret),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.NoContent);

        (await service.GetRoomResultAsync(" abc234 ")).Kind.Should().Be(DraftRoomApiResultKind.NotFound);
        (await service.DeleteResultAsync(resultId, " abc234 ")).IsSuccess.Should().BeTrue();
        client.VerifyAll();
    }

    [Fact]
    public async Task DraftActions_WhenInvoked_ShouldSendTypedPayloadsWithGuestCredentials()
    {
        var (service, client, identity) = CreateService();
        var action = new LockInBansAction();
        var decision = new DecideForRequest(Guid.NewGuid(), new DecideBansAction());
        client.Setup(api => api.PostApiAsync<DraftActionDto, DraftRoomViewDto>(
                "api/draft-rooms/ABC234/draft/actions",
                action,
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => headers != null && headers[ParticipantSecretHeader] == identity.Secret)))
            .ReturnsAsync((new ApiResponse<DraftRoomViewDto> { Success = true }, HttpStatusCode.NoContent));
        client.Setup(api => api.PostApiAsync<DecideForRequest, DraftRoomViewDto>(
                "api/draft-rooms/ABC234/draft/decide-for",
                decision,
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => headers != null && headers[ParticipantSecretHeader] == identity.Secret)))
            .ReturnsAsync((new ApiResponse<DraftRoomViewDto> { Success = true }, HttpStatusCode.NoContent));

        (await service.ApplyDraftActionAsync(identity.Code, action)).IsSuccess.Should().BeTrue();
        (await service.DecideForAsync(identity.Code, decision)).IsSuccess.Should().BeTrue();
        client.VerifyAll();
    }

    [Fact]
    public async Task UpdateDraftSettingsAsync_WhenSaved_ShouldSendBaseTypedPolymorphicRulesWithGuestHeaders()
    {
        var (service, client, identity) = CreateService();
        var settings = new FactionPickSettingsDto();
        client.Setup(api => api.PutAsync<DraftSettingsDto, DraftRoomViewDto>(
                "api/draft-rooms/ABC234/draft/settings",
                settings,
                It.Is<IReadOnlyDictionary<string, string>?>(headers => headers != null
                    && headers[ParticipantSecretHeader] == identity.Secret),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((new ApiResponse<DraftRoomViewDto> { Success = true }, HttpStatusCode.NoContent));

        var result = await service.UpdateDraftSettingsAsync(" abc234 ", settings);

        result.IsSuccess.Should().BeTrue();
        client.VerifyAll();
    }

    [Fact]
    public async Task AssignSeatAsync_WhenAssigned_ShouldSendParticipantToSeatRoute()
    {
        var (service, client, identity) = CreateService();
        var seatId = Guid.NewGuid();
        var participantId = Guid.NewGuid();
        client.Setup(api => api.PostApiAsync<AssignSeatRequest, DraftRoomViewDto>(
                $"api/draft-rooms/ABC234/seats/{seatId:D}/assign",
                It.Is<AssignSeatRequest>(request => request.ParticipantId == participantId),
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => headers != null
                    && headers[ParticipantSecretHeader] == identity.Secret)))
            .ReturnsAsync((new ApiResponse<DraftRoomViewDto> { Success = true }, HttpStatusCode.NoContent));

        var result = await service.AssignSeatAsync(identity.Code, seatId, participantId);

        result.IsSuccess.Should().BeTrue();
        client.VerifyAll();
    }

    [Fact]
    public async Task GetViewAsync_WhenGuestIdentityIsStored_ShouldSendIdentityHeaders()
    {
        var (service, apiHttpClient, identity) = CreateService();
        var view = new DraftRoomViewDto { Code = identity.Code, Name = "Room One" };
        apiHttpClient
            .Setup(client => client.GetAsync<DraftRoomViewDto>(
                "api/draft-rooms/ABC234",
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.Is<IReadOnlyDictionary<string, string>?>(headers =>
                    !ReferenceEquals(headers, null)
                    && headers[ParticipantIdHeader] == identity.ParticipantId.ToString()
                    && headers[ParticipantSecretHeader] == identity.Secret)))
            .ReturnsAsync((view, HttpStatusCode.OK));

        var result = await service.GetViewAsync("abc234");

        result.Kind.Should().Be(DraftRoomApiResultKind.Success);
        result.Data.Should().BeSameAs(view);
    }

    [Theory]
    [InlineData(HttpStatusCode.Forbidden, DraftRoomApiResultKind.Forbidden)]
    [InlineData(HttpStatusCode.NotFound, DraftRoomApiResultKind.NotFound)]
    public async Task GetViewAsync_WhenApiDeniesRoomAccess_ShouldPreserveErrorKind(
        HttpStatusCode statusCode,
        DraftRoomApiResultKind expectedKind)
    {
        var (service, apiHttpClient, identity) = CreateService();
        apiHttpClient
            .Setup(client => client.GetAsync<DraftRoomViewDto>(
                "api/draft-rooms/ABC234",
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IReadOnlyDictionary<string, string>?>()))
            .ReturnsAsync(((DraftRoomViewDto?)null, statusCode));

        var result = await service.GetViewAsync(identity.Code);

        result.Kind.Should().Be(expectedKind);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task RoomCommands_WhenInvoked_ShouldUseExpectedMethodsPathsAndGuestHeaders()
    {
        var (service, apiHttpClient, identity) = CreateService();
        var seatId = Guid.NewGuid();
        var participantId = Guid.NewGuid();
        var calls = new List<(HttpMethod Method, string Path, IReadOnlyDictionary<string, string>? Headers)>();
        apiHttpClient
            .Setup(client => client.SendAsync(
                It.IsAny<HttpMethod>(),
                It.IsAny<string>(),
                It.IsAny<IReadOnlyDictionary<string, string>?>(),
                It.IsAny<CancellationToken>()))
            .Callback<HttpMethod, string, IReadOnlyDictionary<string, string>?, CancellationToken>(
                (method, path, headers, _) => calls.Add((method, path, headers)))
            .ReturnsAsync(HttpStatusCode.NoContent);

        var results = new[]
        {
            await service.AddSeatAsync(identity.Code),
            await service.RemoveSeatAsync(identity.Code, seatId),
            await service.ClaimSeatAsync(identity.Code, seatId),
            await service.UnclaimSeatAsync(identity.Code, seatId),
            await service.LeaveAsync(identity.Code),
            await service.KickAsync(identity.Code, participantId),
            await service.CloseAsync(identity.Code),
            await service.StartDraftAsync(identity.Code),
            await service.ResetDraftAsync(identity.Code),
        };

        results.Should().OnlyContain(result => result.IsSuccess);
        calls.Select(call => (call.Method, call.Path)).Should().Equal(
            (HttpMethod.Post, "api/draft-rooms/ABC234/seats"),
            (HttpMethod.Delete, $"api/draft-rooms/ABC234/seats/{seatId:D}"),
            (HttpMethod.Post, $"api/draft-rooms/ABC234/seats/{seatId:D}/claim"),
            (HttpMethod.Post, $"api/draft-rooms/ABC234/seats/{seatId:D}/unclaim"),
            (HttpMethod.Post, "api/draft-rooms/ABC234/leave"),
            (HttpMethod.Post, $"api/draft-rooms/ABC234/participants/{participantId:D}/kick"),
            (HttpMethod.Delete, "api/draft-rooms/ABC234"),
            (HttpMethod.Post, "api/draft-rooms/ABC234/draft/start"),
            (HttpMethod.Post, "api/draft-rooms/ABC234/draft/reset"));
        calls.Should().OnlyContain(call =>
            !ReferenceEquals(call.Headers, null)
            && call.Headers[ParticipantIdHeader] == identity.ParticipantId.ToString()
            && call.Headers[ParticipantSecretHeader] == identity.Secret);
    }

    [Fact]
    public async Task LinkAccountAsync_WhenApiSucceeds_ShouldSendGuestProofAndRemoveStoredIdentity()
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
        var apiRequestCompleted = false;
        guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(identity);
        apiHttpClient
            .Setup(client => client.SendAsync(
                HttpMethod.Post,
                "api/draft-rooms/ABC234/link-account",
                It.Is<IReadOnlyDictionary<string, string>?>(headers => HasGuestHeaders(headers, identity)),
                It.IsAny<CancellationToken>()))
            .Callback(() => apiRequestCompleted = true)
            .ReturnsAsync(HttpStatusCode.NoContent);
        guestIdentityStore
            .Setup(store => store.RemoveAsync("ABC234", It.IsAny<CancellationToken>()))
            .Callback(() => apiRequestCompleted.Should().BeTrue())
            .Returns(Task.CompletedTask);
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.LinkAccountAsync("abc234");

        result.IsSuccess.Should().BeTrue();
        guestIdentityStore.Verify(
            store => store.RemoveAsync("ABC234", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task LinkAccountAsync_WhenApiFails_ShouldKeepStoredGuestIdentity()
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
            .Setup(client => client.SendAsync(
                HttpMethod.Post,
                "api/draft-rooms/ABC234/link-account",
                It.IsAny<IReadOnlyDictionary<string, string>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.Conflict);
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.LinkAccountAsync(identity.Code);

        result.Kind.Should().Be(DraftRoomApiResultKind.Conflict);
        guestIdentityStore.Verify(
            store => store.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task LinkAccountAsync_WhenNoGuestIdentityIsStored_ShouldNotCallApi()
    {
        var apiHttpClient = new Mock<ITwilightImperiumApiHttpClient>(MockBehavior.Strict);
        var guestIdentityStore = new Mock<IGuestIdentityStore>();
        guestIdentityStore
            .Setup(store => store.GetAsync("ABC234", It.IsAny<CancellationToken>()))
            .ReturnsAsync((GuestIdentity?)null);
        var service = new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object);

        var result = await service.LinkAccountAsync("abc234");

        result.Kind.Should().Be(DraftRoomApiResultKind.Failed);
        guestIdentityStore.Verify(
            store => store.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task SetRoleAsync_WhenInvoked_ShouldSendRoleRequestWithGuestHeaders()
    {
        var (service, apiHttpClient, identity) = CreateService();
        var participantId = Guid.NewGuid();
        apiHttpClient
            .Setup(client => client.PutAsync<SetParticipantRoleRequest, DraftRoomViewDto>(
                $"api/draft-rooms/ABC234/participants/{participantId:D}/role",
                It.Is<SetParticipantRoleRequest>(request => request.Role == DraftRoomRole.CoHost),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => HasGuestHeaders(headers, identity)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((new ApiResponse<DraftRoomViewDto> { Success = true }, HttpStatusCode.NoContent));

        var result = await service.SetRoleAsync(identity.Code, participantId, DraftRoomRole.CoHost);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateSettingsAsync_WhenInvoked_ShouldSendSettingsRequestWithGuestHeaders()
    {
        var (service, apiHttpClient, identity) = CreateService();
        apiHttpClient
            .Setup(client => client.PutAsync<UpdateRoomSettingsRequest, DraftRoomViewDto>(
                "api/draft-rooms/ABC234/settings",
                It.Is<UpdateRoomSettingsRequest>(request =>
                    request.Name == "New Name"
                    && request.IsLocked),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => HasGuestHeaders(headers, identity)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((new ApiResponse<DraftRoomViewDto> { Success = true }, HttpStatusCode.NoContent));

        var result = await service.UpdateSettingsAsync(identity.Code, "New Name", true);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ChangePasswordAsync_WhenInvoked_ShouldSendPasswordRequestWithGuestHeaders()
    {
        var (service, apiHttpClient, identity) = CreateService();
        apiHttpClient
            .Setup(client => client.PutAsync<ChangeRoomPasswordRequest, DraftRoomViewDto>(
                "api/draft-rooms/ABC234/password",
                It.Is<ChangeRoomPasswordRequest>(request => request.Password == "secret2"),
                It.Is<IReadOnlyDictionary<string, string>?>(headers => HasGuestHeaders(headers, identity)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((new ApiResponse<DraftRoomViewDto> { Success = true }, HttpStatusCode.NoContent));

        var result = await service.ChangePasswordAsync(identity.Code, "secret2");

        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(HttpStatusCode.Conflict, DraftRoomApiResultKind.Conflict)]
    [InlineData(HttpStatusCode.Forbidden, DraftRoomApiResultKind.Forbidden)]
    [InlineData(HttpStatusCode.NotFound, DraftRoomApiResultKind.NotFound)]
    [InlineData(HttpStatusCode.BadRequest, DraftRoomApiResultKind.ValidationFailed)]
    public async Task AddSeatAsync_WhenApiReturnsErrorStatus_ShouldMapTheResult(
        HttpStatusCode statusCode,
        DraftRoomApiResultKind expectedKind)
    {
        var (service, apiHttpClient, identity) = CreateService();
        apiHttpClient
            .Setup(client => client.SendAsync(
                HttpMethod.Post,
                "api/draft-rooms/ABC234/seats",
                It.IsAny<IReadOnlyDictionary<string, string>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(statusCode);

        var result = await service.AddSeatAsync(identity.Code);

        result.Kind.Should().Be(expectedKind);
    }

    private static (DraftRoomApiService Service, Mock<ITwilightImperiumApiHttpClient> ApiHttpClient, GuestIdentity Identity)
        CreateService()
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
            .Setup(store => store.GetAsync(identity.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(identity);

        return (new DraftRoomApiService(apiHttpClient.Object, guestIdentityStore.Object), apiHttpClient, identity);
    }

    private static bool HasGuestHeaders(
        IReadOnlyDictionary<string, string>? headers,
        GuestIdentity identity)
    {
        return headers is not null
            && headers[ParticipantIdHeader] == identity.ParticipantId.ToString()
            && headers[ParticipantSecretHeader] == identity.Secret;
    }
}
