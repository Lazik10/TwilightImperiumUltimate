using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class SendChatCommandHandlerTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_WhenTextIsEmptyOrWhitespace_ShouldRejectBeforeRoomAccess(string text)
    {
        var unitOfWork = new Mock<IDraftRoomUnitOfWork>(MockBehavior.Strict);
        var handler = CreateHandler(unitOfWork.Object);

        var result = await handler.Handle(
            new SendChatCommand(
                "ABC234",
                text,
                new DraftRoomCaller(null, null, null, null)),
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.InvalidChatMessage);
    }

    [Fact]
    public async Task Handle_WhenTextExceedsMaximumLength_ShouldRejectBeforeRoomAccess()
    {
        var unitOfWork = new Mock<IDraftRoomUnitOfWork>(MockBehavior.Strict);
        var handler = CreateHandler(unitOfWork.Object);

        var result = await handler.Handle(
            new SendChatCommand(
                "ABC234",
                new string('a', DraftRoomLimits.MaxChatMessageLength + 1),
                new DraftRoomCaller(null, null, null, null)),
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.InvalidChatMessage);
    }

    private static SendChatCommandHandler CreateHandler(IDraftRoomUnitOfWork unitOfWork)
    {
        return new SendChatCommandHandler(
            unitOfWork,
            Mock.Of<IDraftRoomAuthorizer>(),
            Mock.Of<IDraftRoomChatThrottle>(),
            Mock.Of<IRoomChangeNotifier>(),
            NullLogger<SendChatCommandHandler>.Instance);
    }
}
