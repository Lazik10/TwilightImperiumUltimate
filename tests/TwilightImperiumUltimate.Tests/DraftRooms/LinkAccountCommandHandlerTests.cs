using FluentAssertions;
using FluentResults;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class LinkAccountCommandHandlerTests
{
    [Theory]
    [InlineData(null, "account-user", "account-name", "guest-secret")]
    [InlineData("guest-id", null, null, "guest-secret")]
    [InlineData("guest-id", "account-user", "account-name", null)]
    public async Task Handle_WhenCallerDoesNotProveBothIdentities_ShouldRejectBeforeRoomAccess(
        string? guestParticipantId,
        string? userId,
        string? userName,
        string? guestSecret)
    {
        var unitOfWork = new Mock<IDraftRoomUnitOfWork>(MockBehavior.Strict);
        var handler = new LinkAccountCommandHandler(
            unitOfWork.Object,
            Mock.Of<IGuestSecretService>(),
            Mock.Of<IDraftRoomDisplayNameValidator>(),
            Mock.Of<IDraftRoomLog>());
        var participantId = guestParticipantId is null ? (Guid?)null : Guid.NewGuid();
        var caller = new DraftRoomCaller(userId, userName, participantId, guestSecret);

        var result = await handler.Handle(
            new LinkAccountCommand("ABC234", caller),
            CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<DraftRoomError>().Should().ContainSingle()
            .Which.Code.Should().Be(DraftRoomErrorCode.InvalidCaller);
        unitOfWork.Verify(
            repository => repository.ExecuteAsync(
                It.IsAny<string>(),
                It.IsAny<Func<DraftRoomScope, Task<Result>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>()),
            Times.Never);
    }
}
