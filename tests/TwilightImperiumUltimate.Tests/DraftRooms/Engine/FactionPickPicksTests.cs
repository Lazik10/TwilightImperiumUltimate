using FluentAssertions;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;
using TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms.Engine;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class FactionPickPicksTests
{
    private static readonly Guid AliceSeat = Guid.NewGuid();
    private static readonly Guid BobSeat = Guid.NewGuid();
    private static readonly FactionName[] Pool = Enum.GetValues<FactionName>().Where(faction => faction != FactionName.None).ToArray();
    private readonly FactionPickDraftType engine = new();

    public static TheoryData<int, int, bool> CompleteDraftCases
    {
        get
        {
            var cases = new TheoryData<int, int, bool>();
            for (var seats = 2; seats <= 8; seats++)
            {
                for (var picks = 1; picks <= 5; picks++)
                {
                    cases.Add(seats, picks, false);
                    cases.Add(seats, picks, true);
                }
            }

            return cases;
        }
    }

    public static TheoryData<int, int> ProjectionCases
    {
        get
        {
            var cases = new TheoryData<int, int>();
            for (var viewer = 0; viewer < 6; viewer++)
            {
                for (var stage = 0; stage < 3; stage++)
                {
                    cases.Add(viewer, stage);
                }
            }

            return cases;
        }
    }

    [Theory]
    [MemberData(nameof(CompleteDraftCases))]
    public void Apply_WhenDraftIsPlayedToEnd_ShouldGiveEverySeatExactlyItsPicksAndExpectedDirection(int seats, int picks, bool snake)
    {
        var rules = new FactionPickSettingsDto { PicksPerPlayer = picks, Snake = snake };
        var order = Enumerable.Range(0, seats).Select(_ => Guid.NewGuid()).ToArray();
        var state = State(order);
        for (var turn = 0; turn < seats * picks; turn++)
        {
            var position = turn % seats;
            var expected = snake && (turn / seats) % 2 == 1 ? order[(seats - 1) - position] : order[position];
            var view = View(this.engine, rules, state, expected);
            view.ActiveSeatId.Should().Be(expected);
            view.MyActionRequired.Should().BeTrue();
            state = Next(this.engine.Apply(rules, state, Actor(expected), new SelectPickAction { Faction = Pool[turn] }));
            var locked = this.engine.Apply(rules, state, Actor(expected), new LockInPickAction { TurnIndex = turn });
            locked.Validation.IsValid.Should().BeTrue();
            locked.IsFinished.Should().Be(turn == (seats * picks) - 1);
            state = Next(locked);
        }

        state.Phase.Should().Be(FactionPickPhase.Finished);
        state.Choices.GroupBy(choice => choice.SeatId).Should().HaveCount(seats)
            .And.OnlyContain(group => group.Count() == picks);
        var finished = View(this.engine, rules, state, order[0]);
        finished.ActiveSeatId.Should().BeNull();
        finished.MyActionRequired.Should().BeFalse();
        finished.LockedPicks.Select(choice => choice.Faction).Should().Equal(Pool.Take(seats * picks));
        this.engine.Undo(rules, state, Manager()).Validation.ErrorCodes.Should().Contain("WrongPhase");
    }

    [Theory]
    [MemberData(nameof(ProjectionCases))]
    public void Project_WhenPendingOrLockedPicksExist_ShouldRespectSixViewerPrivacyMatrix(int viewerKind, int stage)
    {
        var rules = new FactionPickSettingsDto();
        var state = Next(this.engine.Apply(rules, State([AliceSeat, BobSeat]), Actor(AliceSeat), new SelectPickAction { Faction = Pool[0] }));
        if (stage > 0)
        {
            state = Next(this.engine.Apply(rules, state, Actor(AliceSeat), new LockInPickAction()));
            state = Next(this.engine.Apply(rules, state, Actor(BobSeat), new SelectPickAction { Faction = Pool[1] }));
        }

        if (stage > 1)
        {
            state = Next(this.engine.Apply(rules, state, Actor(BobSeat), new LockInPickAction { TurnIndex = 1 }));
        }

        var viewer = viewerKind switch
        {
            0 => new DraftViewer(Guid.NewGuid(), AliceSeat, DraftRoomRole.Member),
            1 => new DraftViewer(Guid.NewGuid(), BobSeat, DraftRoomRole.Member),
            2 => new DraftViewer(Guid.NewGuid(), null, DraftRoomRole.Host),
            3 => new DraftViewer(Guid.NewGuid(), null, DraftRoomRole.CoHost),
            4 => new DraftViewer(Guid.NewGuid(), null, DraftRoomRole.Member),
            _ => new DraftViewer(null, null, DraftRoomRole.Member),
        };
        var projected = (FactionPickViewDto)this.engine.Project(rules, state, viewer);

        FactionName? expected = null;
        if (stage == 0 && viewerKind == 0)
        {
            expected = Pool[0];
        }
        else if (stage == 1 && viewerKind == 1)
        {
            expected = Pool[1];
        }

        projected.MyPendingPick.Should().Be(expected);
        projected.LockedPicks.Select(pick => pick.Faction).Should().Equal(Pool.Take(stage));
        projected.MyActionRequired.Should().Be((stage == 0 && viewerKind == 0) || (stage == 1 && viewerKind == 1));
    }

    [Fact]
    public void Apply_WhenChoiceIsBannedPickedOutOfTurnOrStale_ShouldRejectWithoutEffects()
    {
        var rules = new FactionPickSettingsDto();
        var original = State([AliceSeat, BobSeat]) with
        {
            Choices = [new FactionPickChoice(null, FactionPickSelectionKind.Ban, Pool[0], FactionPickSelectionState.Locked)],
        };
        this.engine.Apply(rules, original, Actor(AliceSeat), new SelectPickAction { Faction = Pool[0] })
            .Validation.ErrorCodes.Should().Contain("FactionUnavailable");
        this.engine.Apply(rules, original, Actor(BobSeat), new SelectPickAction { Faction = Pool[1] })
            .Validation.ErrorCodes.Should().Contain("NotYourTurn");
        this.engine.Apply(rules, original, Actor(AliceSeat), new LockInPickAction())
            .Validation.ErrorCodes.Should().Contain("MissingSelection");
        var selected = Next(this.engine.Apply(rules, original, Actor(AliceSeat), new SelectPickAction { Faction = Pool[1] }));
        selected = Next(this.engine.Apply(rules, selected, Actor(AliceSeat), new SelectPickAction { Faction = Pool[2] }));
        selected.Choices.Count(choice => choice.Kind == FactionPickSelectionKind.Pick).Should().Be(1);
        var locked = Next(this.engine.Apply(rules, selected, Actor(AliceSeat), new LockInPickAction()));

        this.engine.Apply(rules, locked, Actor(AliceSeat), new LockInPickAction()).Validation.ErrorCodes.Should().Contain("StaleTurn");
        this.engine.Apply(rules, locked, Actor(BobSeat), new SelectPickAction { Faction = Pool[2] }).Validation.ErrorCodes.Should().Contain("FactionUnavailable");
        original.Choices.Should().HaveCount(1);
    }

    [Fact]
    public void DecideForAndUndo_WhenManagerActs_ShouldDiscloseReplaceAttributeAndRestoreTurnWithoutPrivateData()
    {
        var rules = new FactionPickSettingsDto();
        var state = Next(this.engine.Apply(rules, State([AliceSeat, BobSeat]), Actor(AliceSeat), new SelectPickAction { Faction = Pool[0] }));
        var manager = Manager();
        var decided = this.engine.DecideFor(rules, state, manager, AliceSeat, new DecidePickAction { Faction = Pool[1] });

        decided.LogEvents.Select(entry => entry.Code).Should().Equal("HostDecidedForSeat", "FactionPicked");
        decided.Disclosures.Single().Data.GetProperty("Factions")[0].GetInt32().Should().Be((int)Pool[0]);
        Next(decided).Choices.Single().DecidedByParticipantId.Should().Be(manager.ParticipantId);
        Next(decided).Choices.Single().Faction.Should().Be(Pool[1]);
        var withPending = Next(this.engine.Apply(rules, Next(decided), Actor(BobSeat), new SelectPickAction { Faction = Pool[2] }));
        var undone = this.engine.Undo(rules, withPending, manager);

        Next(undone).TurnIndex.Should().Be(0);
        Next(undone).Choices.Should().BeEmpty();
        undone.LogEvents.Single().Code.Should().Be("PickUndone");
        this.engine.Undo(rules, Next(undone), manager).Validation.ErrorCodes.Should().Contain("NothingToUndo");
        this.engine.Undo(rules, withPending, Actor(BobSeat)).Validation.ErrorCodes.Should().Contain("Forbidden");
        this.engine.DecideFor(rules, state, Actor(BobSeat), AliceSeat, new DecidePickAction { Faction = Pool[1] })
            .Validation.ErrorCodes.Should().Contain("Forbidden");
        this.engine.DecideFor(rules, state, manager, BobSeat, new DecidePickAction { Faction = Pool[1] })
            .Validation.ErrorCodes.Should().Contain("NotYourTurn");
    }

    private static FactionPickDraftState State(IReadOnlyList<Guid> order) => new()
    {
        IsStarted = true,
        Phase = FactionPickPhase.Picks,
        BansRevealed = true,
        SeatIds = order,
        TurnOrder = order,
        Pool = Pool,
    };

    private static DraftActor Actor(Guid seat) => new(Guid.NewGuid(), seat, DraftRoomRole.Member);

    private static DraftActor Manager() => new(Guid.NewGuid(), null, DraftRoomRole.Host);

    private static FactionPickDraftState Next(DraftTransition transition)
    {
        transition.Validation.IsValid.Should().BeTrue();
        return (FactionPickDraftState)transition.State;
    }

    private static FactionPickViewDto View(FactionPickDraftType engine, FactionPickSettingsDto settings, FactionPickDraftState state, Guid seat) =>
        (FactionPickViewDto)engine.Project(settings, state, new DraftViewer(Guid.NewGuid(), seat, DraftRoomRole.Member));
}
