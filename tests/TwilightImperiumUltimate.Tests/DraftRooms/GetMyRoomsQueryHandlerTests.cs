using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class GetMyRoomsQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenUserIdIsValid_ShouldMapRoomSummariesAndLimitResults()
    {
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = "ABC234",
            Name = "Friday TI",
            Status = DraftRoomStatus.InProgress,
            LastActivityUtc = DateTime.UtcNow,
        };
        var participantId = Guid.NewGuid();
        room.Seats.Add(new DraftRoomSeat
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Room = room,
            Index = 4,
            ParticipantId = participantId,
        });
        var participant = new DraftRoomParticipant
        {
            Id = participantId,
            RoomId = room.Id,
            Room = room,
            Role = DraftRoomRole.CoHost,
        };
        var repository = new Mock<IDraftRoomRepository>(MockBehavior.Strict);
        repository
            .Setup(item => item.GetActiveParticipantsForUserAsync(
                "account-id",
                50,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([participant]);
        var handler = new GetMyRoomsQueryHandler(repository.Object);

        var result = await handler.Handle(new GetMyRoomsQuery("account-id"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle().Which.Should().BeEquivalentTo(new MyDraftRoomDto
        {
            Code = "ABC234",
            Name = "Friday TI",
            Status = DraftRoomStatus.InProgress,
            Role = DraftRoomRole.CoHost,
            SeatIndex = 4,
            LastActivityUtc = room.LastActivityUtc,
        });
        repository.Verify(
            item => item.GetActiveParticipantsForUserAsync(
                "account-id",
                50,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Handle_WhenUserIdIsMissing_ShouldRejectWithoutQueryingRepository(string? userId)
    {
        var repository = new Mock<IDraftRoomRepository>(MockBehavior.Strict);
        var handler = new GetMyRoomsQueryHandler(repository.Object);

        var result = await handler.Handle(new GetMyRoomsQuery(userId!), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<DraftRoomError>()
            .Which.Code.Should().Be(DraftRoomErrorCode.InvalidCaller);
        repository.Verify(
            item => item.GetActiveParticipantsForUserAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
