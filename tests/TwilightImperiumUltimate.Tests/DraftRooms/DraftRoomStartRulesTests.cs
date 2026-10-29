using FluentAssertions;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;
using TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomStartRulesTests
{
    [Fact]
    public void GetBlockers_WhenLobbyIsReady_ShouldReturnNoBlockers()
    {
        var room = CreateRoom();

        var result = DraftRoomStartRules.GetBlockers(room, CreateModule(), DraftValidationResult.Valid);

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetBlockers_WhenSeveralConditionsFail_ShouldReturnEveryApplicableCode()
    {
        var room = new DraftRoom { Status = DraftRoomStatus.InProgress };
        room.Seats.Add(new DraftRoomSeat());

        var result = DraftRoomStartRules.GetBlockers(room, CreateModule(), new DraftValidationResult(["InvalidPicksPerPlayer"]));

        result.Should().BeEquivalentTo(new[]
        {
            DraftRoomErrorCode.RoomChanged,
            DraftRoomErrorCode.NotEnoughSeats,
            DraftRoomErrorCode.SeatsNotFull,
            DraftRoomErrorCode.SettingsInvalid,
        });
    }

    [Theory]
    [InlineData(DraftParticipantStatus.Left)]
    [InlineData(DraftParticipantStatus.Kicked)]
    public void GetBlockers_WhenSeatOccupantIsNotActive_ShouldTreatSeatAsNotReady(DraftParticipantStatus status)
    {
        var room = CreateRoom();
        room.Participants.First().Status = status;

        var result = DraftRoomStartRules.GetBlockers(room, CreateModule(), DraftValidationResult.Valid);

        result.Should().ContainSingle().Which.Should().Be(DraftRoomErrorCode.SeatsNotFull);
    }

    [Fact]
    public void GetBlockers_WhenSeatReferencesParticipantOutsideRoom_ShouldTreatSeatAsNotReady()
    {
        var room = CreateRoom();
        room.Seats.First().ParticipantId = Guid.NewGuid();

        var result = DraftRoomStartRules.GetBlockers(room, CreateModule(), DraftValidationResult.Valid);

        result.Should().ContainSingle().Which.Should().Be(DraftRoomErrorCode.SeatsNotFull);
    }

    private static DraftTypeModule<FactionPickSettingsDto, FactionPickDraftState> CreateModule() => new(new FactionPickDraftType());

    private static DraftRoom CreateRoom()
    {
        var room = new DraftRoom();
        for (var index = 0; index < 2; index++)
        {
            var participant = new DraftRoomParticipant { Id = Guid.NewGuid(), Status = DraftParticipantStatus.Active };
            room.Participants.Add(participant);
            room.Seats.Add(new DraftRoomSeat { ParticipantId = participant.Id });
        }

        return room;
    }
}
