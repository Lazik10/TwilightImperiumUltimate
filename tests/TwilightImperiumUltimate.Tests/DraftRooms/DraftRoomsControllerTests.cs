using System.Reflection;
using System.Security.Claims;
using FluentAssertions;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using TwilightImperiumUltimate.API.Controllers;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomsControllerTests
{
    [Theory]
    [InlineData(DraftRoomErrorCode.InvalidRoomName, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.InvalidPassword, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.InvalidGuestDisplayName, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.InvalidCaller, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.InvalidRole, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.InvalidChatMessage, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.InvalidHistoryCursor, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.InvalidHistoryLimit, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.NotFoundOrWrongPassword, StatusCodes.Status404NotFound)]
    [InlineData(DraftRoomErrorCode.ParticipantNotFound, StatusCodes.Status404NotFound)]
    [InlineData(DraftRoomErrorCode.JoinAttemptsThrottled, StatusCodes.Status429TooManyRequests)]
    [InlineData(DraftRoomErrorCode.ChatRateLimited, StatusCodes.Status429TooManyRequests)]
    [InlineData(DraftRoomErrorCode.ParticipantKicked, StatusCodes.Status403Forbidden)]
    [InlineData(DraftRoomErrorCode.Forbidden, StatusCodes.Status403Forbidden)]
    [InlineData(DraftRoomErrorCode.OpenRoomLimitReached, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.RoomFull, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.RoomLocked, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.DuplicateDisplayName, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.RoomChanged, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.AccountAlreadyInRoom, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.SeatLimitReached, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.MinimumSeatCountReached, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.SeatNotFound, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.SeatOccupied, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.SeatNotOccupied, StatusCodes.Status409Conflict)]
    [InlineData(DraftRoomErrorCode.RoomCodeGenerationFailed, StatusCodes.Status503ServiceUnavailable)]
    public async Task Create_WhenHandlerFails_ShouldReturnMappedProblemDetails(
        DraftRoomErrorCode errorCode,
        int expectedStatusCode)
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(service => service.Send(It.IsAny<CreateDraftRoomCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<DraftRoomEntryDto>(new DraftRoomError(errorCode)));
        var controller = CreateController(mediator.Object);

        var response = await controller.Create(
            new CreateDraftRoomRequest("Friday TI", "friday1", "Alice"),
            CancellationToken.None);

        var problem = response.Result.Should().BeOfType<ObjectResult>().Which;
        problem.StatusCode.Should().Be(expectedStatusCode);
        problem.Value.Should().BeOfType<ProblemDetails>()
            .Which.Extensions["code"].Should().Be(errorCode.ToString());
    }

    [Fact]
    public async Task Create_WhenHandlerSucceeds_ShouldReturnRoomEntry()
    {
        var entry = new DraftRoomEntryDto { Code = "ABC234", RoomName = "Friday TI" };
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(service => service.Send(It.IsAny<CreateDraftRoomCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(entry));
        var controller = CreateController(mediator.Object);

        var response = await controller.Create(
            new CreateDraftRoomRequest("Friday TI", "friday1", "Alice"),
            CancellationToken.None);

        response.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(entry);
    }

    [Fact]
    public async Task Join_WhenHandlerSucceeds_ShouldSendCallerAndClientIp()
    {
        var participantId = Guid.NewGuid();
        var entry = new DraftRoomEntryDto { Code = "ABC234" };
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(service => service.Send(
                It.Is<JoinDraftRoomCommand>(command =>
                    command.ClientIp == "192.0.2.1"
                    && command.Caller.GuestParticipantId == participantId
                    && command.Caller.GuestSecret == "guest-secret"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(entry));
        var controller = CreateController(mediator.Object);
        controller.HttpContext.Request.Headers["X-Draft-Participant-Id"] = participantId.ToString();
        controller.HttpContext.Request.Headers["X-Draft-Participant-Secret"] = "guest-secret";
        controller.HttpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.0.2.1");

        var response = await controller.Join(
            new JoinDraftRoomRequest("ABC234", "friday1", null),
            CancellationToken.None);

        response.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(entry);
    }

    [Fact]
    public async Task LinkAccount_WhenBothIdentitiesArePresent_ShouldSendCombinedCaller()
    {
        var participantId = Guid.NewGuid();
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(service => service.Send(
                It.Is<LinkAccountCommand>(command =>
                    command.Code == "ABC234"
                    && command.Caller.UserId == "account-id"
                    && command.Caller.UserName == "AccountName"
                    && command.Caller.GuestParticipantId == participantId
                    && command.Caller.GuestSecret == "guest-secret"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        var controller = CreateController(mediator.Object);
        controller.HttpContext.User = CreateAuthenticatedPrincipal("account-id", "AccountName");
        controller.HttpContext.Request.Headers["X-Draft-Participant-Id"] = participantId.ToString();
        controller.HttpContext.Request.Headers["X-Draft-Participant-Secret"] = "guest-secret";

        var response = await controller.LinkAccount("ABC234", CancellationToken.None);

        response.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task LinkAccount_WhenAccountIsNotAuthenticated_ShouldReturnUnauthorized()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        var controller = CreateController(mediator.Object);
        _ = AddGuestCallerHeaders(controller);

        var response = await controller.LinkAccount("ABC234", CancellationToken.None);

        response.Should().BeOfType<UnauthorizedResult>();
        mediator.Verify(
            service => service.Send(It.IsAny<LinkAccountCommand>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task LinkAccount_WhenGuestHeadersAreMissing_ShouldRejectBeforeMediator()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        var controller = CreateController(mediator.Object);
        controller.HttpContext.User = CreateAuthenticatedPrincipal("account-id", "AccountName");

        var response = await controller.LinkAccount("ABC234", CancellationToken.None);

        response.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        mediator.Verify(
            service => service.Send(It.IsAny<LinkAccountCommand>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Get_WhenHandlerSucceeds_ShouldSendCallerAndReturnPersonalizedView()
    {
        var participantId = Guid.NewGuid();
        var view = new DraftRoomViewDto { Code = "ABC234", Name = "Friday TI" };
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(service => service.Send(
                It.Is<GetDraftRoomViewQuery>(query =>
                    query.Code == "ABC234"
                    && query.Caller.GuestParticipantId == participantId
                    && query.Caller.GuestSecret == "guest-secret"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(view));
        var controller = CreateController(mediator.Object);
        controller.HttpContext.Request.Headers["X-Draft-Participant-Id"] = participantId.ToString();
        controller.HttpContext.Request.Headers["X-Draft-Participant-Secret"] = "guest-secret";

        var response = await controller.Get("ABC234", CancellationToken.None);

        response.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(view);
    }

    [Fact]
    public async Task GetMine_WhenAuthenticated_ShouldQueryOnlyTheAccountEvenWithGuestHeaders()
    {
        var rooms = new MyDraftRoomDto[]
        {
            new() { Code = "ABC234", Name = "Friday TI" },
        };
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(service => service.Send(
                It.Is<GetMyRoomsQuery>(query => query.UserId == "account-id"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok<IReadOnlyList<MyDraftRoomDto>>(rooms));
        var controller = CreateController(mediator.Object);
        controller.HttpContext.User = CreateAuthenticatedPrincipal("account-id", "AccountName");
        _ = AddGuestCallerHeaders(controller);

        var response = await controller.GetMine(CancellationToken.None);

        response.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(rooms);
    }

    [Fact]
    public async Task GetMine_WhenAccountIsNotAuthenticated_ShouldReturnUnauthorized()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        var controller = CreateController(mediator.Object);

        var response = await controller.GetMine(CancellationToken.None);

        response.Result.Should().BeOfType<UnauthorizedResult>();
        mediator.Verify(
            service => service.Send(It.IsAny<GetMyRoomsQuery>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetMine_WhenAuthenticatedClaimsHaveNoAccountId_ShouldRejectRequest()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        var controller = CreateController(mediator.Object);
        controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Name, "AccountName")],
            "Test"));

        var response = await controller.GetMine(CancellationToken.None);

        response.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        mediator.Verify(
            service => service.Send(It.IsAny<GetMyRoomsQuery>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Get_WhenHandlerRejectsStranger_ShouldReturnNotFoundProblemDetails()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(service => service.Send(
                It.IsAny<GetDraftRoomViewQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<DraftRoomViewDto>(
                new DraftRoomError(DraftRoomErrorCode.NotFoundOrWrongPassword)));
        var controller = CreateController(mediator.Object);

        var response = await controller.Get("ABC234", CancellationToken.None);

        var problem = response.Result.Should().BeOfType<ObjectResult>().Which;
        problem.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        problem.Value.Should().BeOfType<ProblemDetails>()
            .Which.Extensions["code"].Should().Be(DraftRoomErrorCode.NotFoundOrWrongPassword.ToString());
    }

    [Fact]
    public async Task Get_WhenCallerHeadersAreInvalid_ShouldReturnBadRequestWithoutSendingQuery()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        var controller = CreateController(mediator.Object);
        controller.HttpContext.Request.Headers["X-Draft-Participant-Id"] = Guid.NewGuid().ToString();

        var response = await controller.Get("ABC234", CancellationToken.None);

        response.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        mediator.Verify(
            service => service.Send(It.IsAny<GetDraftRoomViewQuery>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task SeatAndParticipantCommands_WhenCalled_ShouldSendCommandsAndReturnNoContent()
    {
        var participantId = Guid.NewGuid();
        var seatId = Guid.NewGuid();
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(service => service.Send(It.IsAny<AddSeatCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        mediator
            .Setup(service => service.Send(It.IsAny<RemoveSeatCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        mediator
            .Setup(service => service.Send(It.IsAny<ClaimSeatCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        mediator
            .Setup(service => service.Send(It.IsAny<UnclaimSeatCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        mediator
            .Setup(service => service.Send(It.IsAny<LeaveRoomCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        mediator
            .Setup(service => service.Send(It.IsAny<KickParticipantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        mediator
            .Setup(service => service.Send(It.IsAny<CloseRoomCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        var controller = CreateController(mediator.Object);
        var guestParticipantId = AddGuestCallerHeaders(controller);

        (await controller.AddSeat("ABC234", CancellationToken.None))
            .Should().BeOfType<NoContentResult>();
        (await controller.RemoveSeat("ABC234", seatId, CancellationToken.None))
            .Should().BeOfType<NoContentResult>();
        (await controller.ClaimSeat("ABC234", seatId, CancellationToken.None))
            .Should().BeOfType<NoContentResult>();
        (await controller.UnclaimSeat("ABC234", seatId, CancellationToken.None))
            .Should().BeOfType<NoContentResult>();
        (await controller.Leave("ABC234", CancellationToken.None))
            .Should().BeOfType<NoContentResult>();
        (await controller.KickParticipant("ABC234", participantId, CancellationToken.None))
            .Should().BeOfType<NoContentResult>();
        (await controller.Close("ABC234", CancellationToken.None))
            .Should().BeOfType<NoContentResult>();

        mediator.Verify(
            service => service.Send(
                It.Is<AddSeatCommand>(command =>
                    command.Code == "ABC234"
                    && command.Caller.GuestParticipantId == guestParticipantId
                    && command.Caller.GuestSecret == "guest-secret"),
                It.IsAny<CancellationToken>()),
            Times.Once);
        mediator.Verify(
            service => service.Send(
                It.Is<RemoveSeatCommand>(command => command.Code == "ABC234" && command.SeatId == seatId),
                It.IsAny<CancellationToken>()),
            Times.Once);
        mediator.Verify(
            service => service.Send(
                It.Is<ClaimSeatCommand>(command => command.Code == "ABC234" && command.SeatId == seatId),
                It.IsAny<CancellationToken>()),
            Times.Once);
        mediator.Verify(
            service => service.Send(
                It.Is<UnclaimSeatCommand>(command => command.Code == "ABC234" && command.SeatId == seatId),
                It.IsAny<CancellationToken>()),
            Times.Once);
        mediator.Verify(
            service => service.Send(
                It.Is<LeaveRoomCommand>(command => command.Code == "ABC234"),
                It.IsAny<CancellationToken>()),
            Times.Once);
        mediator.Verify(
            service => service.Send(
                It.Is<KickParticipantCommand>(
                    command => command.Code == "ABC234" && command.ParticipantId == participantId),
                It.IsAny<CancellationToken>()),
            Times.Once);
        mediator.Verify(
            service => service.Send(
                It.Is<CloseRoomCommand>(command => command.Code == "ABC234"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task SettingsCommands_WhenCalled_ShouldSendRequestValuesAndReturnNoContent()
    {
        var participantId = Guid.NewGuid();
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(service => service.Send(
                It.IsAny<SetParticipantRoleCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        mediator
            .Setup(service => service.Send(
                It.IsAny<UpdateRoomSettingsCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        mediator
            .Setup(service => service.Send(
                It.IsAny<ChangeRoomPasswordCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        var controller = CreateController(mediator.Object);
        var guestParticipantId = AddGuestCallerHeaders(controller);

        (await controller.SetParticipantRole(
            "ABC234",
            participantId,
            new SetParticipantRoleRequest(DraftRoomRole.CoHost),
            CancellationToken.None))
            .Should().BeOfType<NoContentResult>();
        (await controller.UpdateSettings(
            "ABC234",
            new UpdateRoomSettingsRequest("New Room", true),
            CancellationToken.None))
            .Should().BeOfType<NoContentResult>();
        (await controller.ChangePassword(
            "ABC234",
            new ChangeRoomPasswordRequest("newpass"),
            CancellationToken.None))
            .Should().BeOfType<NoContentResult>();

        mediator.Verify(
            service => service.Send(
                It.Is<SetParticipantRoleCommand>(
                    command => command.Code == "ABC234"
                        && command.ParticipantId == participantId
                        && command.Role == DraftRoomRole.CoHost
                        && command.Caller.GuestParticipantId == guestParticipantId),
                It.IsAny<CancellationToken>()),
            Times.Once);
        mediator.Verify(
            service => service.Send(
                It.Is<UpdateRoomSettingsCommand>(
                    command => command.Code == "ABC234"
                        && command.Name == "New Room"
                        && command.IsLocked),
                It.IsAny<CancellationToken>()),
            Times.Once);
        mediator.Verify(
            service => service.Send(
                It.Is<ChangeRoomPasswordCommand>(
                    command => command.Code == "ABC234" && command.Password == "newpass"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(DraftRoomErrorCode.InvalidRoomName, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.NotFoundOrWrongPassword, StatusCodes.Status404NotFound)]
    [InlineData(DraftRoomErrorCode.Forbidden, StatusCodes.Status403Forbidden)]
    [InlineData(DraftRoomErrorCode.RoomChanged, StatusCodes.Status409Conflict)]
    public async Task UpdateSettings_WhenHandlerFails_ShouldReturnMappedProblemDetails(
        DraftRoomErrorCode errorCode,
        int expectedStatusCode)
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(service => service.Send(
                It.IsAny<UpdateRoomSettingsCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new DraftRoomError(errorCode)));
        var controller = CreateController(mediator.Object);

        var response = await controller.UpdateSettings(
            "ABC234",
            new UpdateRoomSettingsRequest("Friday TI", false),
            CancellationToken.None);

        var problem = response.Should().BeOfType<ObjectResult>().Which;
        problem.StatusCode.Should().Be(expectedStatusCode);
        problem.Value.Should().BeOfType<ProblemDetails>()
            .Which.Extensions["code"].Should().Be(errorCode.ToString());
    }

    [Fact]
    public void Controller_ShouldDisableResponseCaching()
    {
        var responseCache = typeof(DraftRoomsController)
            .GetCustomAttributes(typeof(ResponseCacheAttribute), inherit: true)
            .Cast<ResponseCacheAttribute>()
            .Should()
            .ContainSingle()
            .Which;

        responseCache.NoStore.Should().BeTrue();
        responseCache.Location.Should().Be(ResponseCacheLocation.None);
    }

    [Theory]
    [InlineData(DraftRoomErrorCode.InvalidDraftAction, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.Forbidden, StatusCodes.Status403Forbidden)]
    [InlineData(DraftRoomErrorCode.RoomChanged, StatusCodes.Status409Conflict)]
    public async Task DraftActions_WhenRefused_ShouldMapProblemsForBothCommandEndpoints(DraftRoomErrorCode code, int status)
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator.Setup(service => service.Send(It.IsAny<ApplyDraftActionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new DraftRoomError(code)));
        mediator.Setup(service => service.Send(It.IsAny<DecideForCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new DraftRoomError(code)));
        var controller = CreateController(mediator.Object);
        AddGuestCallerHeaders(controller);

        var own = await controller.ApplyDraftAction("ABC234", new LockInBansAction(), CancellationToken.None);
        var manager = await controller.DecideFor("ABC234", new DecideForRequest(Guid.NewGuid(), new DecideBansAction()), CancellationToken.None);

        own.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(status);
        manager.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(status);
        mediator.VerifyAll();
    }

    [Fact]
    public void Controller_ShouldExposeAllDraftRoomRoutes()
    {
        var routes = typeof(DraftRoomsController)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .SelectMany(method => method.GetCustomAttributes<HttpMethodAttribute>())
            .SelectMany(attribute => attribute.HttpMethods.Select(
                httpMethod => $"{httpMethod} {attribute.Template ?? "[root]"}"));

        routes.Should().BeEquivalentTo(
            "POST [root]",
            "POST join",
            "POST {code}/link-account",
            "GET mine",
            "GET {code}",
            "GET {code}/chat",
            "POST {code}/chat",
            "GET {code}/log",
            "POST {code}/seats",
            "DELETE {code}/seats/{seatId:guid}",
            "POST {code}/seats/{seatId:guid}/claim",
            "POST {code}/seats/{seatId:guid}/unclaim",
            "POST {code}/seats/{seatId:guid}/assign",
            "POST {code}/leave",
            "POST {code}/participants/{participantId:guid}/kick",
            "PUT {code}/participants/{participantId:guid}/role",
            "PUT {code}/settings",
            "PUT {code}/draft/settings",
            "POST {code}/draft/start",
            "POST {code}/draft/reset",
            "POST {code}/draft/actions",
            "POST {code}/draft/decide-for",
            "POST {code}/draft/undo",
            "GET results/{token}",
            "GET results/mine",
            "GET {code}/result",
            "DELETE results/{id:guid}",
            "PUT {code}/password",
            "DELETE {code}");
    }

    [Theory]
    [InlineData(DraftRoomErrorCode.SettingsInvalid, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.SeatsNotFull, StatusCodes.Status400BadRequest)]
    [InlineData(DraftRoomErrorCode.Forbidden, StatusCodes.Status403Forbidden)]
    [InlineData(DraftRoomErrorCode.RoomChanged, StatusCodes.Status409Conflict)]
    public async Task StartDraft_WhenRejected_ShouldReturnStatusAndAllValidationCodes(
        DraftRoomErrorCode code,
        int expectedStatus)
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator.Setup(service => service.Send(It.IsAny<StartDraftCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new DraftRoomError(code, ["InsufficientFactionPool"])));
        var controller = CreateController(mediator.Object);
        AddGuestCallerHeaders(controller);

        var result = await controller.StartDraft("ABC234", CancellationToken.None);

        var response = result.Should().BeOfType<ObjectResult>().Which;
        response.StatusCode.Should().Be(expectedStatus);
        var problem = response.Value.Should().BeOfType<ProblemDetails>().Which;
        problem.Extensions["codes"].Should().BeEquivalentTo(new[] { code.ToString(), "InsufficientFactionPool" });
    }

    [Fact]
    public async Task DraftCommands_WhenSuccessful_ShouldSendTypedPayloadAndReturnNoContent()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator.Setup(service => service.Send(It.IsAny<UpdateDraftSettingsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        mediator.Setup(service => service.Send(It.IsAny<StartDraftCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        mediator.Setup(service => service.Send(It.IsAny<ResetDraftCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        var controller = CreateController(mediator.Object);
        AddGuestCallerHeaders(controller);
        var settings = new FactionPickSettingsDto { PicksPerPlayer = 2 };

        (await controller.UpdateDraftSettings("ABC234", settings, CancellationToken.None)).Should().BeOfType<NoContentResult>();
        (await controller.StartDraft("ABC234", CancellationToken.None)).Should().BeOfType<NoContentResult>();
        (await controller.ResetDraft("ABC234", CancellationToken.None)).Should().BeOfType<NoContentResult>();
        mediator.Verify(
            service => service.Send(
                It.Is<UpdateDraftSettingsCommand>(command => command.Settings == settings && command.Code == "ABC234"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetMyResults_WhenAnonymous_ShouldRequireAuthenticationBeforeSendingQuery()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        var controller = CreateController(mediator.Object);

        var response = await controller.GetMyResults(cancellationToken: CancellationToken.None);

        response.Result.Should().BeOfType<UnauthorizedResult>();
        mediator.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetMyResults_WhenAuthenticated_ShouldUseClaimIdentityAndRequestedPage()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator.Setup(service => service.Send(
                It.Is<GetMyResultsQuery>(query => query.UserId == "account-id" && query.Page == 2),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new DraftResultHistoryDto([], 2, false)));
        var controller = CreateController(mediator.Object);
        controller.HttpContext.User = CreateAuthenticatedPrincipal("account-id", "Account");

        var response = await controller.GetMyResults(2, CancellationToken.None);

        response.Result.Should().BeOfType<OkObjectResult>();
        mediator.VerifyAll();
    }

    [Fact]
    public async Task GetResult_WhenUnknownOrDeleted_ShouldReturnIdenticalNotFoundProblems()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator.Setup(service => service.Send(It.IsAny<GetResultByTokenQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<DraftResultDto>(new DraftRoomError(DraftRoomErrorCode.ResultNotFound)));
        var controller = CreateController(mediator.Object);

        var unknown = await controller.GetResult("unknown-token", CancellationToken.None);
        var deleted = await controller.GetResult("deleted-token", CancellationToken.None);

        var unknownProblem = unknown.Result.Should().BeOfType<ObjectResult>().Which;
        var deletedProblem = deleted.Result.Should().BeOfType<ObjectResult>().Which;
        unknownProblem.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        deletedProblem.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        deletedProblem.Value.Should().BeEquivalentTo(unknownProblem.Value);
    }

    private static DraftRoomsController CreateController(IMediator mediator)
    {
        var controller = new DraftRoomsController(mediator)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext(),
            },
        };
        return controller;
    }

    private static ClaimsPrincipal CreateAuthenticatedPrincipal(string userId, string userName)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
            ],
            "Test"));
    }

    private static Guid AddGuestCallerHeaders(DraftRoomsController controller)
    {
        var participantId = Guid.NewGuid();
        controller.HttpContext.Request.Headers["X-Draft-Participant-Id"] = participantId.ToString();
        controller.HttpContext.Request.Headers["X-Draft-Participant-Secret"] = "guest-secret";
        return participantId;
    }
}
