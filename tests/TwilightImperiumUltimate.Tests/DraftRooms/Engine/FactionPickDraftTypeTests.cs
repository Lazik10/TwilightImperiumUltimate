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
public sealed class FactionPickDraftTypeTests
{
    private readonly FactionPickDraftType engine = new();

    [Fact]
    public void CreateDefaultSettings_WhenCalledTwice_ShouldReturnIndependentDefaults()
    {
        var first = this.engine.CreateDefaultSettings();
        first.PicksPerPlayer = 5;

        this.engine.CreateDefaultSettings().PicksPerPlayer.Should().Be(1);
        this.engine.MinSeats.Should().Be(2);
        this.engine.MaxSeats.Should().Be(8);
    }

    [Theory]
    [InlineData(FactionPickBansMode.None, 0, 11, false)]
    [InlineData(FactionPickBansMode.None, 0, 12, true)]
    [InlineData(FactionPickBansMode.HostBans, 3, 14, false)]
    [InlineData(FactionPickBansMode.HostBans, 3, 15, true)]
    [InlineData(FactionPickBansMode.SimultaneousPlayerBans, 0, 23, false)]
    [InlineData(FactionPickBansMode.SimultaneousPlayerBans, 0, 24, true)]
    public void ValidateSettings_WhenPoolIsAtBoundary_ShouldAccountForMaximumBans(
        FactionPickBansMode mode,
        int hostBanCount,
        int poolSize,
        bool valid)
    {
        var settings = new FactionPickSettingsDto
        {
            PicksPerPlayer = 2,
            BansPerPlayer = 2,
            BansMode = mode,
            HostBanCount = hostBanCount,
        };

        var result = this.engine.ValidateSettings(settings, 6, poolSize);

        result.IsValid.Should().Be(valid);
        if (!valid)
        {
            result.ErrorCodes.Should().Contain("InsufficientFactionPool");
        }
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    public void ValidateSettings_WhenBansPerPlayerIsAtBoundary_ShouldValidateRange(int count, bool valid)
    {
        var result = this.engine.ValidateSettings(new FactionPickSettingsDto { BansPerPlayer = count }, 2, 100);

        result.IsValid.Should().Be(valid);
        result.ErrorCodes.Contains("InvalidBansPerPlayer").Should().Be(!valid);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(5, true)]
    [InlineData(6, false)]
    public void ValidateSettings_WhenPicksPerPlayerIsAtBoundary_ShouldValidateRange(int count, bool valid)
    {
        var result = this.engine.ValidateSettings(new FactionPickSettingsDto { PicksPerPlayer = count }, 2, 100);

        result.IsValid.Should().Be(valid);
        result.ErrorCodes.Contains("InvalidPicksPerPlayer").Should().Be(!valid);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(8, true)]
    [InlineData(9, false)]
    public void ValidateSettings_WhenSeatCountIsAtBoundary_ShouldValidateRange(int count, bool valid)
    {
        var result = this.engine.ValidateSettings(new FactionPickSettingsDto(), count, 100);

        result.IsValid.Should().Be(valid);
        result.ErrorCodes.Contains("InvalidSeatCount").Should().Be(!valid);
    }

    [Fact]
    public void ValidateSettings_WhenVersionsAreEmpty_ShouldReturnSpecificCode()
    {
        var result = this.engine.ValidateSettings(new FactionPickSettingsDto { GameVersions = [] }, 2, 100);

        result.ErrorCodes.Should().ContainSingle().Which.Should().Be("GameVersionsRequired");
    }

    [Fact]
    public void ValidateSettings_WhenMultipleFieldsAreInvalid_ShouldReturnEveryRelevantCodeWithoutOverflow()
    {
        var settings = new FactionPickSettingsDto
        {
            GameVersions = [(GameVersion)999],
            BansMode = (FactionPickBansMode)999,
            BansPerPlayer = int.MaxValue,
            HostBanCount = -1,
            PicksPerPlayer = int.MaxValue,
            PickOrderMode = (FactionPickOrderMode)999,
        };

        var result = this.engine.ValidateSettings(settings, int.MaxValue, 0);

        result.ErrorCodes.Should().BeEquivalentTo(
            "UnsupportedGameVersion",
            "InvalidBansMode",
            "InvalidBansPerPlayer",
            "InvalidHostBanCount",
            "InvalidPicksPerPlayer",
            "InvalidPickOrderMode",
            "InvalidSeatCount",
            "InsufficientFactionPool");
    }

    [Fact]
    public void ValidateSettings_WhenHostOrderIsMissing_ShouldRefuseExplicitly()
    {
        var result = this.engine.ValidateSettings(
            new FactionPickSettingsDto { PickOrderMode = FactionPickOrderMode.HostDefined }, 2, 100);

        result.ErrorCodes.Should().ContainSingle().Which.Should().Be("InvalidSeatOrder");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Start_WhenConfiguredOrderIsInvalid_ShouldRejectMissingDuplicateEmptyOrForeignSeats(int kind)
    {
        var state = CreateState();
        var order = kind switch
        {
            0 => state.SeatIds.Take(1).ToArray(),
            1 => new[] { state.SeatIds[0], state.SeatIds[0] },
            2 => new[] { state.SeatIds[0], Guid.Empty },
            _ => new[] { state.SeatIds[0], Guid.NewGuid() },
        };
        var settings = new FactionPickSettingsDto { PickOrderMode = FactionPickOrderMode.HostDefined, SeatOrder = order };

        var result = this.engine.Start(settings, state, new DeterministicRandomnessSource());

        result.Validation.ErrorCodes.Should().Contain("InvalidSeatOrder");
        result.State.Should().BeSameAs(state);
        result.LogEvents.Should().BeEmpty();
    }

    [Fact]
    public void Start_WhenHostOrderIsValid_ShouldCopyItWithoutShufflingAndLogConfiguredOrder()
    {
        var state = CreateState();
        var order = state.SeatIds.ToArray();
        var settings = new FactionPickSettingsDto { PickOrderMode = FactionPickOrderMode.HostDefined, SeatOrder = order };

        var result = this.engine.Start(settings, state, new DeterministicRandomnessSource(0));

        result.Validation.IsValid.Should().BeTrue();
        var started = result.State.Should().BeOfType<FactionPickDraftState>().Which;
        started.TurnOrder.Should().Equal(order);
        result.LogEvents.Should().ContainSingle().Which.Code.Should().Be("PickOrderConfigured");
        order[0] = Guid.NewGuid();
        started.TurnOrder.Should().Equal(state.SeatIds);
    }

    [Theory]
    [InlineData(FactionPickBansMode.None, FactionPickPhase.Picks, DraftPhaseKind.Turn)]
    [InlineData(FactionPickBansMode.HostBans, FactionPickPhase.Bans, DraftPhaseKind.HostOnly)]
    [InlineData(FactionPickBansMode.SimultaneousPlayerBans, FactionPickPhase.Bans, DraftPhaseKind.Simultaneous)]
    public void Start_WhenSettingsAreValid_ShouldSelectPhaseAndLogActualRandomOrder(
        FactionPickBansMode mode,
        FactionPickPhase phase,
        DraftPhaseKind kind)
    {
        var state = CreateState();
        var settings = new FactionPickSettingsDto { BansMode = mode };

        var result = this.engine.Start(settings, state, new DeterministicRandomnessSource(0));

        result.Validation.IsValid.Should().BeTrue();
        var started = result.State.Should().BeOfType<FactionPickDraftState>().Which;
        started.Phase.Should().Be(phase);
        started.PhaseKind.Should().Be(kind);
        started.TurnOrder.Should().Equal(state.SeatIds.Reverse());
        state.IsStarted.Should().BeFalse();
        state.TurnOrder.Should().BeEmpty();
        result.LogEvents.Should().ContainSingle().Which.Code.Should().Be("PickOrderRandomized");
        var logged = JsonSerializer.Deserialize<Guid[]>(result.LogEvents[0].Data.GetProperty("SeatIds"));
        logged.Should().Equal(started.TurnOrder);
        result.Disclosures.Should().BeEmpty();
    }

    [Fact]
    public void Start_WhenAlreadyStarted_ShouldNotRandomizeOrMutate()
    {
        var state = CreateState() with { IsStarted = true };

        var result = this.engine.Start(new FactionPickSettingsDto(), state, new DeterministicRandomnessSource());

        result.State.Should().BeSameAs(state);
        result.Validation.ErrorCodes.Should().Contain("DraftAlreadyStarted");
        result.LogEvents.Should().BeEmpty();
    }

    [Fact]
    public void Start_WhenPoolContainsDuplicates_ShouldNotCountDuplicatesAsAvailableFactions()
    {
        var state = CreateState() with { Pool = [FactionName.TheArborec, FactionName.TheArborec] };

        var result = this.engine.Start(new FactionPickSettingsDto(), state, new DeterministicRandomnessSource());

        result.Validation.ErrorCodes.Should().Contain("InsufficientFactionPool");
        result.State.Should().BeSameAs(state);
    }

    [Fact]
    public void Start_WhenSeatsAreRepeated_ShouldRejectInvalidInput()
    {
        var seatId = Guid.NewGuid();
        var state = CreateState() with { SeatIds = [seatId, seatId] };

        var result = this.engine.Start(new FactionPickSettingsDto(), state, new DeterministicRandomnessSource());

        result.Validation.ErrorCodes.Should().Contain("InvalidSeats");
        result.LogEvents.Should().BeEmpty();
    }

    private static FactionPickDraftState CreateState() => new()
    {
        SeatIds = [Guid.NewGuid(), Guid.NewGuid()],
        Pool = Enum.GetValues<FactionName>().Where(faction => faction != FactionName.None).Take(10).ToArray(),
    };
}
