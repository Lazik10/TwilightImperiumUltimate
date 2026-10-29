using System.Text.Json;
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
public sealed class FactionPickBansTests
{
    private static readonly Guid AliceSeat = Guid.NewGuid();
    private static readonly Guid BobSeat = Guid.NewGuid();
    private static readonly FactionName[] Factions = Enum.GetValues<FactionName>().Take(4).ToArray();
    private readonly FactionPickDraftType engine = new();
    private readonly FactionPickSettingsDto settings = new() { BansMode = FactionPickBansMode.SimultaneousPlayerBans, BansPerPlayer = 2 };

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
    [MemberData(nameof(ProjectionCases))]
    public void Project_WhenViewerAndLockStageVary_ShouldExposeOnlyOwnOrPublicBans(int viewerKind, int stage)
    {
        var state = WithChoices();
        if (stage > 0)
        {
            state = Next(this.engine.Apply(this.settings, state, Actor(AliceSeat), new LockInBansAction()));
        }

        if (stage > 1)
        {
            state = Next(this.engine.Apply(this.settings, state, Actor(BobSeat), new LockInBansAction()));
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

        var view = (FactionPickViewDto)this.engine.Project(this.settings, state, viewer);

        view.PublicBans.Should().BeEquivalentTo(stage == 2 ? new[] { Factions[0], Factions[1] } : []);
        view.MyBans.Should().Equal(viewerKind switch { 0 => [Factions[0]], 1 => [Factions[1]], _ => [] });
        view.LockedBanSeatIds.Count.Should().Be(stage);
        view.BansRevealed.Should().Be(stage == 2);
    }

    [Fact]
    public void Apply_WhenLastSeatLocksOverlappingBans_ShouldRevealOnceAndDeduplicate()
    {
        var state = WithChoices() with
        {
            Choices = [Choice(AliceSeat, Factions[0]), Choice(BobSeat, Factions[0])],
        };
        var first = this.engine.Apply(this.settings, state, Actor(AliceSeat), new LockInBansAction());
        var last = this.engine.Apply(this.settings, Next(first), Actor(BobSeat), new LockInBansAction());

        first.LogEvents.Should().OnlyContain(entry => entry.Code == "SeatBansLocked");
        JsonSerializer.Serialize(first.LogEvents).Should().NotContain(nameof(FactionPickChoice.Faction));
        last.LogEvents.Select(entry => entry.Code).Should().Equal("SeatBansLocked", "BansRevealed");
        last.LogEvents[1].Data.GetProperty("Factions").GetArrayLength().Should().Be(1);
        Next(last).Phase.Should().Be(FactionPickPhase.Picks);
        this.engine.Apply(this.settings, Next(last), Actor(BobSeat), new LockInBansAction()).Validation.ErrorCodes.Should().Contain("WrongPhase");
        state.LockedBanSeatIds.Should().BeEmpty();
        state.Choices.Should().OnlyContain(choice => choice.State == FactionPickSelectionState.Pending);
    }

    [Fact]
    public void Apply_WhenSeatLocksNoBans_ShouldPersistIndependentLockFlagAndRefuseEditing()
    {
        var state = Next(this.engine.Apply(this.settings, CreateState(), Actor(AliceSeat), new LockInBansAction()));

        state.LockedBanSeatIds.Should().Equal(AliceSeat);
        state.Choices.Should().BeEmpty();
        this.engine.Apply(this.settings, state, Actor(AliceSeat), new SelectBanAction { Faction = Factions[0] })
            .Validation.ErrorCodes.Should().Contain("AlreadyLocked");
    }

    [Fact]
    public void Apply_WhenSeatSelectsAndDeselects_ShouldValidatePoolCountAndOwnershipWithoutPublicContents()
    {
        var state = CreateState();
        var first = this.engine.Apply(this.settings, state, Actor(AliceSeat), new SelectBanAction { Faction = Factions[0] });
        first.LogEvents.Should().BeEmpty();
        this.engine.Apply(this.settings, Next(first), Actor(AliceSeat), new SelectBanAction { Faction = Factions[0] })
            .Validation.ErrorCodes.Should().Contain("AlreadySelected");
        var second = Next(this.engine.Apply(this.settings, Next(first), Actor(AliceSeat), new SelectBanAction { Faction = Factions[1] }));
        this.engine.Apply(this.settings, second, Actor(AliceSeat), new SelectBanAction { Faction = Factions[2] })
            .Validation.ErrorCodes.Should().Contain("BanLimitReached");
        this.engine.Apply(this.settings, state, Actor(AliceSeat), new SelectBanAction { Faction = (FactionName)int.MaxValue })
            .Validation.ErrorCodes.Should().Contain("InvalidFaction");
        this.engine.Apply(this.settings, state, Actor(Guid.NewGuid()), new LockInBansAction())
            .Validation.ErrorCodes.Should().Contain("Forbidden");
        var removed = Next(this.engine.Apply(this.settings, second, Actor(AliceSeat), new DeselectBanAction { Faction = Factions[0] }));
        removed.Choices.Select(choice => choice.Faction).Should().Equal(Factions[1]);
        this.engine.Apply(this.settings, removed, Actor(BobSeat), new DeselectBanAction { Faction = Factions[1] })
            .Validation.ErrorCodes.Should().Contain("NotSelected");
    }

    [Theory]
    [InlineData(DraftRoomRole.Host)]
    [InlineData(DraftRoomRole.CoHost)]
    public void Apply_WhenHostBans_ShouldPublishTogglesAndRequireManagerConfirmation(DraftRoomRole role)
    {
        var rules = new FactionPickSettingsDto { BansMode = FactionPickBansMode.HostBans, HostBanCount = 1 };
        var state = CreateState() with { BansMode = rules.BansMode };
        var manager = new DraftActor(Guid.NewGuid(), null, role);
        var selected = this.engine.Apply(rules, state, manager, new SelectBanAction { Faction = Factions[0] });

        selected.LogEvents.Single().Code.Should().Be("HostBansChanged");
        var view = (FactionPickViewDto)this.engine.Project(rules, Next(selected), new DraftViewer(null, null, DraftRoomRole.Member));
        view.PublicBans.Should().Equal(Factions[0]);
        this.engine.Apply(rules, state, Actor(AliceSeat), new ConfirmHostBansAction()).Validation.ErrorCodes.Should().Contain("Forbidden");
        var confirmed = Next(this.engine.Apply(rules, Next(selected), manager, new ConfirmHostBansAction()));
        confirmed.BansRevealed.Should().BeTrue();
        confirmed.Phase.Should().Be(FactionPickPhase.Picks);
    }

    [Fact]
    public void DecideFor_WhenManagerReplacesBans_ShouldDiscloseBeforeLockAndPreserveOtherSeats()
    {
        var state = WithChoices();
        var manager = new DraftActor(Guid.NewGuid(), null, DraftRoomRole.CoHost);
        var result = this.engine.DecideFor(this.settings, state, manager, AliceSeat, new DecideBansAction { Factions = [Factions[2]] });

        result.Validation.IsValid.Should().BeTrue();
        result.LogEvents.Select(entry => entry.Code).Should().Equal("HostDecidedForSeat", "SeatBansLocked");
        result.Disclosures.Single().Data.GetProperty("Factions")[0].GetInt32().Should().Be((int)Factions[0]);
        Next(result).Choices.Should().Contain(choice => choice.SeatId == AliceSeat && choice.Faction == Factions[2]
            && choice.DecidedByParticipantId == manager.ParticipantId && choice.State == FactionPickSelectionState.Locked);
        Next(result).Choices.Should().NotContain(choice => choice.SeatId == AliceSeat && choice.Faction == Factions[0]);
        Next(result).Choices.Should().Contain(Choice(BobSeat, Factions[1]));
    }

    [Fact]
    public void DecideFor_WhenUnauthorizedLockedOrInvalid_ShouldHaveNoEffectsOrDisclosures()
    {
        var state = WithChoices();
        var manager = new DraftActor(Guid.NewGuid(), null, DraftRoomRole.Host);
        DraftTransition[] refusals =
        [
            this.engine.DecideFor(this.settings, state, Actor(BobSeat), AliceSeat, new DecideBansAction()),
            this.engine.DecideFor(this.settings, state with { LockedBanSeatIds = [AliceSeat] }, manager, AliceSeat, new DecideBansAction()),
            this.engine.DecideFor(this.settings, state, manager, Guid.NewGuid(), new DecideBansAction()),
            this.engine.DecideFor(this.settings, state, manager, AliceSeat, new DecideBansAction { Factions = [(FactionName)int.MaxValue] }),
            this.engine.DecideFor(this.settings, state, manager, AliceSeat, new DecideBansAction { Factions = Factions }),
        ];

        refusals.Should().OnlyContain(result => !result.Validation.IsValid && result.LogEvents.Count == 0 && result.Disclosures.Count == 0);
    }

    private static DraftActor Actor(Guid seat) => new(Guid.NewGuid(), seat, DraftRoomRole.Member);

    private static FactionPickChoice Choice(Guid seat, FactionName faction) =>
        new(seat, FactionPickSelectionKind.Ban, faction, FactionPickSelectionState.Pending);

    private static FactionPickDraftState CreateState() => new()
    {
        IsStarted = true,
        Phase = FactionPickPhase.Bans,
        BansMode = FactionPickBansMode.SimultaneousPlayerBans,
        SeatIds = [AliceSeat, BobSeat],
        TurnOrder = [AliceSeat, BobSeat],
        Pool = Factions,
    };

    private static FactionPickDraftState WithChoices() => CreateState() with { Choices = [Choice(AliceSeat, Factions[0]), Choice(BobSeat, Factions[1])] };

    private static FactionPickDraftState Next(DraftTransition transition) => (FactionPickDraftState)transition.State;
}
