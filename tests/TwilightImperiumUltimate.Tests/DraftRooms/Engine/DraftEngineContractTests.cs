using System.Text.Json;
using FluentAssertions;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms.Engine;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftEngineContractTests
{
    [Theory]
    [InlineData(DraftRoomRole.Host, false, false)]
    [InlineData(DraftRoomRole.CoHost, false, false)]
    [InlineData(DraftRoomRole.Member, false, false)]
    [InlineData(DraftRoomRole.Member, false, true)]
    [InlineData(DraftRoomRole.Host, true, false)]
    [InlineData(DraftRoomRole.CoHost, true, false)]
    [InlineData(DraftRoomRole.Member, true, false)]
    public void Project_WhenViewerDoesNotOwnSeat_ShouldOmitPrivateFieldFromSerializedView(
        DraftRoomRole role,
        bool unseated,
        bool anonymous)
    {
        var state = new FakeDraftState(Guid.NewGuid(), "private-choice");
        var module = new DraftTypeModule<FactionPickSettingsDto, FakeDraftState>(new FakeDraftType());
        var viewer = new DraftViewer(
            anonymous ? null : Guid.NewGuid(),
            unseated ? null : Guid.NewGuid(),
            role);

        var projected = module.Project(module.CreateDefaultSettings(), state, viewer);
        var view = projected.Should().BeOfType<FakeDraftView>().Which;
        var json = JsonSerializer.Serialize(view);

        view.PublicValue.Should().Be("Visible to all");
        view.PrivateValue.Should().BeNull();
        json.Should().NotContain("PrivateValue").And.NotContain("private-choice");
    }

    [Fact]
    public void Project_WhenViewerOwnsSeat_ShouldIncludeOnlyTheirPrivateChoice()
    {
        var state = new FakeDraftState(Guid.NewGuid(), "private-choice");
        var module = new DraftTypeModule<FactionPickSettingsDto, FakeDraftState>(new FakeDraftType());
        var viewer = new DraftViewer(Guid.NewGuid(), state.OwnerSeatId, DraftRoomRole.Member);

        var projected = module.Project(module.CreateDefaultSettings(), state, viewer);

        projected.Should().BeOfType<FakeDraftView>().Which.PrivateValue.Should().Be("private-choice");
        viewer.CanSee(DraftVisibility.ServerOnly, state.OwnerSeatId).Should().BeFalse();
    }

    [Fact]
    public void Apply_WhenDispatchedThroughAdapter_ShouldReturnNewStateAndPreserveInput()
    {
        var state = new FakeDraftState(Guid.NewGuid(), "private-choice");
        var module = new DraftTypeModule<FactionPickSettingsDto, FakeDraftState>(new FakeDraftType());
        var actor = new DraftActor(Guid.NewGuid(), state.OwnerSeatId, DraftRoomRole.Member);

        var result = module.Apply(module.CreateDefaultSettings(), state, actor, new LockInBansAction());

        result.Validation.IsValid.Should().BeTrue();
        result.State.Should().BeOfType<FakeDraftState>().Which.ActionCount.Should().Be(1);
        state.ActionCount.Should().Be(0);
        result.Disclosures.Should().BeEmpty();
    }

    [Fact]
    public void Project_WhenStateDoesNotMatchEngine_ShouldFailClearly()
    {
        var module = new DraftTypeModule<FactionPickSettingsDto, FakeDraftState>(new FakeDraftType());
        var viewer = new DraftViewer(Guid.NewGuid(), null, DraftRoomRole.Host);

        var project = () => module.Project(module.CreateDefaultSettings(), new OtherState(), viewer);

        project.Should().Throw<ArgumentException>().WithParameterName("state");
    }

    [Fact]
    public void ValidateSettings_WhenSettingsDoNotMatchEngine_ShouldFailClearly()
    {
        var module = new DraftTypeModule<FactionPickSettingsDto, FakeDraftState>(new FakeDraftType());

        var validate = () => module.ValidateSettings(new OtherSettings(), 2, 20);

        validate.Should().Throw<ArgumentException>().WithParameterName("settings");
    }

    [Fact]
    public void DecideFor_WhenManagerActs_ShouldReturnExplicitDisclosureWithoutChangingOriginalState()
    {
        var state = new FakeDraftState(Guid.NewGuid(), "private-choice");
        var module = new DraftTypeModule<FactionPickSettingsDto, FakeDraftState>(new FakeDraftType());
        var actor = new DraftActor(Guid.NewGuid(), null, DraftRoomRole.Host);

        var transition = module.DecideFor(
            module.CreateDefaultSettings(),
            state,
            actor,
            state.OwnerSeatId,
            new LockInBansAction());

        transition.Disclosures.Should().ContainSingle()
            .Which.Data.GetString().Should().Be("private-choice");
        transition.LogEvents.Should().ContainSingle().Which.Code.Should().Be("HostDecidedForSeat");
        state.PrivateValue.Should().Be("private-choice");
    }

    private sealed record OtherState : IDraftState
    {
        public DraftPhaseKind PhaseKind => DraftPhaseKind.Automatic;
    }

    private sealed class OtherSettings : DraftSettingsDto;
}
