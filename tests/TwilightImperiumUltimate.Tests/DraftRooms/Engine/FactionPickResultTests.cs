using System.Text.Json;
using FluentAssertions;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms.Engine;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class FactionPickResultTests
{
    private static readonly Guid AliceSeat = Guid.NewGuid();
    private static readonly Guid BobSeat = Guid.NewGuid();
    private static readonly Guid Manager = Guid.NewGuid();
    private readonly FactionPickDraftType engine = new();

    [Fact]
    public void BuildResult_WhenFinished_ShouldUsePublicSeatNumbersNamesAndAttributionWithoutRuntimeIdentifiers()
    {
        var result = this.engine.BuildResult(new FactionPickSettingsDto(), FinishedState());
        var payload = result.Deserialize<FactionPickResultDto>()!;

        payload.SeatOrder.Should().Equal(7, 2);
        payload.Seats.Select(seat => seat.DisplayName).Should().Equal("Alice", "Bob");
        payload.Seats[1].Picks.Single().DecidedByDisplayName.Should().Be("Host");
        payload.Bans.Single().SeatIndex.Should().Be(2);
        foreach (var id in new[] { AliceSeat, BobSeat, Manager })
        {
            result.GetRawText().Should().NotContain(id.ToString());
        }

        foreach (var type in new[] { typeof(FactionPickResultDto), typeof(FactionPickResultSeatDto), typeof(FactionPickResultBanDto), typeof(FactionPickResultPickDto) })
        {
            type.GetProperties().Should().NotContain(property =>
                property.Name.Contains("UserId", StringComparison.OrdinalIgnoreCase)
                || property.Name.Contains("ParticipantId", StringComparison.OrdinalIgnoreCase)
                || property.Name.Contains("SeatId", StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact]
    public void FormatResultAsText_WhenFinished_ShouldProduceStableSeatLinesBansOrderAndManagerMarker()
    {
        var payload = this.engine.BuildResult(new FactionPickSettingsDto(), FinishedState());
        var expected = string.Join(
            Environment.NewLine,
            "Seat 2 - Alice: TheArborec",
            "Seat 7 - Bob: TheBaronyOfLetnev (decided by Host)",
            "Bans: TheClanOfSaar",
            "Order: Seat 7, Seat 2");

        this.engine.FormatResultAsText(payload).Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void BuildResult_WhenStateIsIncomplete_ShouldRefuseInsteadOfLeakingOrInventingData(int defect)
    {
        var state = FinishedState();
        state = defect switch
        {
            0 => state with { Phase = FactionPickPhase.Picks },
            1 => state with { Choices = state.Choices.Append(new FactionPickChoice(AliceSeat, FactionPickSelectionKind.Pick, FactionName.TheWinnu, FactionPickSelectionState.Pending)).ToArray() },
            2 => state with { ResultSeats = [] },
            _ => state with { Choices = state.Choices.Where(choice => choice.SeatId != BobSeat).ToArray() },
        };

        var build = () => this.engine.BuildResult(new FactionPickSettingsDto(), state);

        build.Should().Throw<InvalidOperationException>();
    }

    private static FactionPickDraftState FinishedState() => new()
    {
        IsStarted = true,
        Phase = FactionPickPhase.Finished,
        BansRevealed = true,
        SeatIds = [AliceSeat, BobSeat],
        TurnOrder = [BobSeat, AliceSeat],
        TurnIndex = 2,
        ResultSeats = [new FactionPickResultSeat(AliceSeat, 2, "Alice"), new FactionPickResultSeat(BobSeat, 7, "Bob")],
        ParticipantNames = new Dictionary<Guid, string> { [Manager] = "Host" },
        Choices =
        [
            new FactionPickChoice(AliceSeat, FactionPickSelectionKind.Ban, FactionName.TheClanOfSaar, FactionPickSelectionState.Locked),
            new FactionPickChoice(AliceSeat, FactionPickSelectionKind.Pick, FactionName.TheArborec, FactionPickSelectionState.Locked),
            new FactionPickChoice(BobSeat, FactionPickSelectionKind.Pick, FactionName.TheBaronyOfLetnev, FactionPickSelectionState.Locked, 0, Manager),
        ],
    };
}
