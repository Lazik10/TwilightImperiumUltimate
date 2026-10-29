using System.Globalization;
using Microsoft.AspNetCore.Components;
using Moq;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class DraftRoomLogPanelTests : BunitContext
{
    private readonly Mock<IDraftRoomSession> _session = new();

    public DraftRoomLogPanelTests()
    {
        _session
            .Setup(session => session.GetLogHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>(
                DraftRoomApiResultKind.Success,
                Array.Empty<DraftLogEntryDto>()));
    }

    [Fact]
    public void DraftRoomLogPanel_WhenDecisionIsPublic_ShouldDescribeActorSeatAndDisclosedChoices()
    {
        var entry = new DraftLogEntryDto
        {
            Id = 1,
            Type = DraftLogEntryType.HostDecidedForSeat,
            DataJson = """{"ActorDisplayName":"Manager","SeatIndex":2,"Factions":[]}""",
            CreatedAtUtc = DateTime.UtcNow,
        };
        _session.Setup(session => session.GetLogHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>(DraftRoomApiResultKind.Success, [entry]));

        var panel = RenderPanel();

        panel.Find("[role=log]").TextContent.Should().Contain("Manager decided for Seat 2. Disclosed unlocked selection: None.");
    }

    [Fact]
    public void DraftRoomLogPanel_WhenDraftRandomized_ShouldRenderActualStoredSeatOrder()
    {
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        _session.SetupGet(session => session.View).Returns(new DraftRoomViewDto
        {
            Seats =
            [
                new DraftRoomViewSeatDto { Id = first, Index = 1 },
                new DraftRoomViewSeatDto { Id = second, Index = 2 },
            ],
        });
        DraftLogEntryDto[] entries =
        [
            new()
            {
                Id = 1,
                Type = DraftLogEntryType.DraftRandomized,
                DataJson = System.Text.Json.JsonSerializer.Serialize(new { Code = "PickOrderRandomized", Data = new { SeatIds = new[] { second, first } } }),
                CreatedAtUtc = DateTime.UtcNow,
            },
        ];
        _session.Setup(session => session.GetLogHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>(
                DraftRoomApiResultKind.Success,
                entries));

        var panel = RenderPanel();

        panel.Find("[role=log]").TextContent.Should().Contain("The pick order was randomized: Seat 2, Seat 1.");
    }

    [Fact]
    public void DraftRoomLogPanel_WhenHistoryLoads_ShouldRenderReadableActivityAndLocalTimestamp()
    {
        var timestamp = DateTime.UtcNow;
        var entry = new DraftLogEntryDto
        {
            Id = 1,
            ActorParticipantId = Guid.NewGuid(),
            Type = DraftLogEntryType.ParticipantJoined,
            DataJson = """{"DisplayName":"Guest One","Role":"Member","Reentered":false}""",
            CreatedAtUtc = timestamp,
        };
        _session
            .Setup(session => session.GetLogHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>(
                DraftRoomApiResultKind.Success,
                [entry]));

        var panel = RenderPanel();

        var log = panel.Find("[role='log']");
        log.GetAttribute("aria-live").Should().Be("polite");
        log.GetAttribute("aria-relevant").Should().Be("additions");
        log.GetAttribute("tabindex").Should().Be("0");
        log.TextContent.Should().Contain("Guest One joined the room.");
        panel.Find("time").GetAttribute("datetime").Should().Be(timestamp.ToString("O", CultureInfo.InvariantCulture));
        panel.Find("time").TextContent.Should().Be(timestamp.ToLocalTime().ToString("t", CultureInfo.CurrentCulture));
    }

    [Fact]
    public void DraftRoomLogPanel_WhenParticipantLinksAccount_ShouldDescribeTheDisplayNameChange()
    {
        var entry = new DraftLogEntryDto
        {
            Id = 1,
            ActorParticipantId = Guid.NewGuid(),
            Type = DraftLogEntryType.ParticipantLinked,
            DataJson = """{"PreviousDisplayName":"Guest","NewDisplayName":"AccountUser"}""",
            CreatedAtUtc = DateTime.UtcNow,
        };
        _session
            .Setup(session => session.GetLogHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>(
                DraftRoomApiResultKind.Success,
                [entry]));

        var panel = RenderPanel();

        panel.Find("[role='log'] li").TextContent.Should()
            .Contain("Guest linked this participant to an account and is now shown as AccountUser.");
    }

    [Fact]
    public void DraftRoomLogPanel_WhenHistoryIsPending_ShouldKeepLiveAnnouncementsOffUntilHistoryRenders()
    {
        var history = new TaskCompletionSource<DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        _session
            .Setup(session => session.GetLogHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .Returns(history.Task);

        var panel = RenderPanel();

        panel.Find("[role='log']").GetAttribute("aria-live").Should().Be("off");
        DraftLogEntryDto[] entries =
        [
            new()
            {
                Id = 1,
                Type = DraftLogEntryType.ParticipantJoined,
                DataJson = """{"DisplayName":"Guest One"}""",
                CreatedAtUtc = DateTime.UtcNow,
            },
        ];
        history.SetResult(new DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>(
            DraftRoomApiResultKind.Success,
            entries));

        panel.WaitForAssertion(() =>
        {
            panel.Find("[role='log']").GetAttribute("aria-live").Should().Be("polite");
            panel.FindAll("[role='log'] li").Should().ContainSingle()
                .Which.TextContent.Should().Contain("Guest One joined the room.");
        });
    }

    [Fact]
    public void DraftRoomLogPanel_WhenLiveEntryArrives_ShouldAppendOneReadableLine()
    {
        var panel = RenderPanel();
        var entry = new DraftLogEntryDto
        {
            Id = 2,
            Type = DraftLogEntryType.SeatAdded,
            DataJson = """{"Id":"seat-id","Index":4}""",
            CreatedAtUtc = DateTime.UtcNow,
        };

        _session.Raise(session => session.LogEntryAdded += null!, entry);

        panel.WaitForAssertion(() =>
        {
            panel.FindAll("[role='log'] li").Should().ContainSingle()
                .Which.TextContent.Should().Contain("A participant added seat 4.");
        });
    }

    [Fact]
    public void DraftRoomLogPanel_WhenHistoryFails_ShouldShowUserFacingError()
    {
        _session
            .Setup(session => session.GetLogHistoryAsync(
                null,
                DraftRoomLimits.DefaultHistoryPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>(
                DraftRoomApiResultKind.Failed,
                null));

        var panel = RenderPanel();

        panel.Find("[role='alert']").TextContent.Should().Contain("Room activity could not be loaded");
    }

    private IRenderedComponent<DraftRoomLogPanel> RenderPanel()
    {
        var host = Render<CascadingValue<IDraftRoomSession>>(parameters => parameters
            .Add(parameter => parameter.Value, this._session.Object)
            .AddChildContent<DraftRoomLogPanel>());

        return host.FindComponent<DraftRoomLogPanel>();
    }
}
