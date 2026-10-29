using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms.FactionPick;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Helpers.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class FactionPickBoardTests : BunitContext
{
    private static readonly Guid OwnSeat = Guid.NewGuid();
    private static readonly Guid OtherSeat = Guid.NewGuid();
    private static readonly FactionName[] Factions = Enum.GetValues<FactionName>().Where(value => value != FactionName.None).Take(3).ToArray();

    public FactionPickBoardTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
    }

    [Fact]
    public void FactionBoard_WhenChoicesRendered_ShouldNameEachFactionUsingItsLocalizedTitle()
    {
        var board = Render<FactionPickBoard>(parameters => parameters.Add(component => component.Room, CreateRoom()));

        var names = board.FindAll("button[aria-pressed]").Select(button => button.GetAttribute("aria-label")).ToArray();
        names.Should().Equal(Factions.Select(faction => faction.GetFactionUIText(FactionResourceType.Title)));
        names.Should().OnlyHaveUniqueItems().And.NotContain(nameof(FactionName));
    }

    [Fact]
    public void Settings_WhenRendered_ShouldAssociateLabelsWithActualInputsAndSaveSelectedBansMode()
    {
        DraftSettingsDto? saved = null;
        var editor = Render<FactionPickSettingsEditor>(parameters => parameters
            .Add(component => component.Settings, new FactionPickSettingsDto())
            .Add(component => component.CanEdit, true)
            .Add(component => component.OnSave, settings => saved = settings));

        foreach (var id in new[] { "draft-bans-per-player", "draft-host-ban-count", "draft-picks-per-player" })
        {
            editor.Find($"#{id}").TagName.Should().Be("INPUT");
            var labelId = editor.Find($"#{id}").GetAttribute("aria-labelledby");
            editor.Find($"#{labelId}").TextContent.Should().Be(editor.Find($"label[for='{id}']").TextContent);
            editor.FindAll($"#{id}").Should().ContainSingle();
        }

        editor.Find("label[for='draft-bans-mode']").TextContent.Should().Be("Bans mode");
        editor.Find("#draft-bans-mode").Change(nameof(FactionPickBansMode.SimultaneousPlayerBans));
        editor.FindAll("button").Single(button => button.TextContent.Trim() == "Save draft settings").Click();

        saved.Should().BeOfType<FactionPickSettingsDto>().Which.BansMode.Should().Be(FactionPickBansMode.SimultaneousPlayerBans);
    }

    [Fact]
    public void FactionBoard_WhenActiveSeatSelectsAndLocksPick_ShouldSendObservedCursor()
    {
        var room = CreateRoom();
        var draft = (FactionPickViewDto)room.Draft!;
        draft.Phase = FactionPickPhase.Picks;
        draft.ActiveSeatId = OwnSeat;
        draft.TurnIndex = 2;
        var sent = new List<DraftActionDto>();
        var board = Render<FactionPickBoard>(parameters => parameters
            .Add(component => component.Room, room)
            .Add(component => component.OnAction, action => sent.Add(action)));

        board.FindAll("button[aria-pressed]")[1].Click();
        sent.Should().ContainSingle().Which.Should().BeOfType<SelectPickAction>().Which.Faction.Should().Be(Factions[1]);
        draft.MyPendingPick = Factions[1];
        board.Render();
        board.FindAll("button").Single(button => button.TextContent.Trim() == "Lock in pick").Click();

        sent[^1].Should().BeOfType<LockInPickAction>().Which.TurnIndex.Should().Be(2);
        board.Find("li[aria-current=step]").TextContent.Should().Contain("Seat 1");
    }

    [Theory]
    [InlineData(DraftRoomRole.Host)]
    [InlineData(DraftRoomRole.CoHost)]
    [InlineData(DraftRoomRole.Member)]
    public void FactionBoard_WhenAnotherSeatHasTurn_ShouldNotAllowNormalPicks(DraftRoomRole role)
    {
        var room = CreateRoom();
        room.Me.Role = role;
        var draft = (FactionPickViewDto)room.Draft!;
        draft.Phase = FactionPickPhase.Picks;
        draft.ActiveSeatId = OtherSeat;

        var board = Render<FactionPickBoard>(parameters => parameters.Add(component => component.Room, room));

        board.FindAll("button[aria-pressed]").Should().OnlyContain(button => button.HasAttribute("disabled"));
        board.FindAll("button").Should().NotContain(button => button.TextContent.Trim() == "Lock in pick");
    }

    [Fact]
    public void FactionBoard_WhenManagerReplacesPick_ShouldRequireIndependentChoiceAndConfirmation()
    {
        var room = CreateRoom();
        room.Me.Role = DraftRoomRole.CoHost;
        var draft = (FactionPickViewDto)room.Draft!;
        draft.Phase = FactionPickPhase.Picks;
        draft.ActiveSeatId = OwnSeat;
        draft.MyPendingPick = Factions[0];
        draft.TurnIndex = 1;
        DecideForRequest? sent = null;
        var normalActions = 0;
        var board = Render<FactionPickBoard>(parameters => parameters
            .Add(component => component.Room, room)
            .Add(component => component.OnAction, _ => normalActions++)
            .Add(component => component.OnDecideFor, request => sent = request));

        board.Find("#faction-pick-decide-seat").Change(OwnSeat.ToString());
        board.FindAll("button[aria-pressed]").Should().OnlyContain(button => button.GetAttribute("aria-pressed") == "false");
        board.FindAll("button[aria-pressed]")[1].Click();
        board.FindAll("button").Single(button => button.TextContent.Trim() == "Confirm decision for seat").Click();
        sent.Should().BeNull();
        board.FindAll("dialog button").Single(button => button.TextContent.Trim() == "Confirm decision for seat").Click();

        sent!.Action.Should().BeOfType<DecidePickAction>().Which.Should().BeEquivalentTo(new DecidePickAction { Faction = Factions[1], TurnIndex = 1 });
        normalActions.Should().Be(0);
    }

    [Fact]
    public void FactionBoard_WhenTurnChangesDuringConfirmation_ShouldDiscardStaleManagerDecision()
    {
        var room = CreateRoom();
        room.Me.Role = DraftRoomRole.Host;
        var draft = (FactionPickViewDto)room.Draft!;
        draft.Phase = FactionPickPhase.Picks;
        draft.ActiveSeatId = OtherSeat;
        draft.TurnIndex = 1;
        var board = Render<FactionPickBoard>(parameters => parameters.Add(component => component.Room, room));
        board.Find("#faction-pick-decide-seat").Change(OtherSeat.ToString());
        board.FindAll("button[aria-pressed]")[1].Click();
        board.FindAll("button").Single(button => button.TextContent.Trim() == "Confirm decision for seat").Click();

        draft.TurnIndex = 2;
        board.Render();

        board.FindAll("dialog[open]").Should().BeEmpty();
        board.Find("#faction-pick-decide-seat").GetAttribute("value").Should().BeNullOrEmpty();
        board.FindAll("button[aria-pressed]").Should().OnlyContain(button => button.GetAttribute("aria-pressed") == "false");
    }

    [Fact]
    public void FactionBoard_WhenUndoRequested_ShouldRequireConfirmationAndRemoveActionsAfterFinish()
    {
        var room = CreateRoom();
        room.Me.Role = DraftRoomRole.Host;
        var draft = (FactionPickViewDto)room.Draft!;
        draft.Phase = FactionPickPhase.Picks;
        draft.ActiveSeatId = OwnSeat;
        draft.CanUndo = true;
        draft.LockedPicks = [new FactionPickLockedPickDto(OtherSeat, Factions[0], 0, true)];
        var undone = 0;
        var board = Render<FactionPickBoard>(parameters => parameters
            .Add(component => component.Room, room)
            .Add(component => component.OnUndo, () => undone++));

        board.FindAll("button").Single(button => button.TextContent.Trim() == "Undo last pick").Click();
        undone.Should().Be(0);
        board.FindAll("dialog button").Single(button => button.TextContent.Trim() == "Undo last pick").Click();
        undone.Should().Be(1);
        draft.Phase = FactionPickPhase.Finished;
        draft.ActiveSeatId = null;
        draft.CanUndo = false;
        board.Render();

        board.Find("ul[aria-label='Locked picks by seat']").TextContent.Should().Contain("Chosen by host or co-host");
        board.FindAll("button[aria-pressed]").Should().OnlyContain(button => button.HasAttribute("disabled"));
        board.FindAll("button").Should().NotContain(button => button.TextContent.Trim() == "Undo last pick");
    }

    [Fact]
    public void FactionBoard_WhenSeatSelects_ShouldSendOnlyOwnActionAndShowTextualSelection()
    {
        DraftActionDto? sent = null;
        var room = CreateRoom();
        var board = Render<FactionPickBoard>(parameters => parameters
            .Add(component => component.Room, room)
            .Add(component => component.OnAction, action => sent = action));

        board.FindAll("button[aria-pressed]")[0].Click();
        var accessibleName = board.FindAll("button[aria-pressed]")[0].GetAttribute("aria-label");

        sent.Should().BeOfType<SelectBanAction>().Which.Faction.Should().Be(Factions[0]);
        ((FactionPickViewDto)room.Draft!).MyBans = [Factions[0]];
        board.Render();
        board.FindAll("button[aria-pressed]")[0].GetAttribute("aria-pressed").Should().Be("true");
        board.FindAll("button[aria-pressed]")[0].TextContent.Should().Contain("Selected");
        board.FindAll("button[aria-pressed]")[0].GetAttribute("aria-label").Should().Be(accessibleName);
        var statusId = board.FindAll("button[aria-pressed]")[0].GetAttribute("aria-describedby");
        board.Find($"#{statusId}").TextContent.Should().Be("Selected");
    }

    [Fact]
    public void FactionBoard_WhenSeatLocked_ShouldDisableChoicesAndRemoveLockAction()
    {
        var room = CreateRoom();
        ((FactionPickViewDto)room.Draft!).LockedBanSeatIds = [OwnSeat];
        var board = Render<FactionPickBoard>(parameters => parameters.Add(component => component.Room, room));

        board.FindAll("button[aria-pressed]").Should().OnlyContain(button => button.HasAttribute("disabled"));
        board.FindAll("button").Should().NotContain(button => button.TextContent.Trim() == "Lock in bans");
        board.Markup.Should().Contain("Locked in");
    }

    [Theory]
    [InlineData(DraftRoomRole.Host)]
    [InlineData(DraftRoomRole.CoHost)]
    [InlineData(DraftRoomRole.Member)]
    public void FactionBoard_WhenUnseatedInSimultaneousBans_ShouldNotReadOrSelectSeatPrivateChoices(DraftRoomRole role)
    {
        var room = CreateRoom();
        room.Me.Role = role;
        room.Me.SeatId = null;
        var board = Render<FactionPickBoard>(parameters => parameters.Add(component => component.Room, room));

        board.FindAll("button[aria-pressed]").Should().OnlyContain(button =>
            button.HasAttribute("disabled") && button.GetAttribute("aria-pressed") == "false");
        board.FindAll("#faction-pick-decide-seat").Count.Should().Be(role == DraftRoomRole.Member ? 0 : 1);
    }

    [Fact]
    public void FactionBoard_WhenManagerDecidesForSeat_ShouldUseIndependentChoicesAndRequirePublicDisclosureConfirmation()
    {
        DecideForRequest? sent = null;
        var actions = 0;
        var room = CreateRoom();
        room.Me.Role = DraftRoomRole.Host;
        ((FactionPickViewDto)room.Draft!).MyBans = [Factions[0]];
        var board = Render<FactionPickBoard>(parameters => parameters
            .Add(component => component.Room, room)
            .Add(component => component.OnAction, _ => actions++)
            .Add(component => component.OnDecideFor, request => sent = request));

        board.Find("#faction-pick-decide-seat").Change(OtherSeat.ToString());
        board.FindAll("button[aria-pressed]").Should().OnlyContain(button => button.GetAttribute("aria-pressed") == "false");
        board.FindAll("button[aria-pressed]")[1].Click();
        actions.Should().Be(0);
        board.FindAll("button").Single(button => button.TextContent.Trim() == "Confirm decision for seat").Click();
        sent.Should().BeNull();
        board.Find("dialog").TextContent.Should().Contain("reveals Seat 2's unlocked selection to everyone");
        board.FindAll("dialog button").Single(button => button.TextContent.Trim() == "Confirm decision for seat").Click();

        sent.Should().NotBeNull();
        sent!.SeatId.Should().Be(OtherSeat);
        sent.Action.Should().BeOfType<DecideBansAction>().Which.Factions.Should().Equal(Factions[1]);
    }

    [Fact]
    public void FactionBoard_WhenBansRevealed_ShouldShowBannedTextWithoutInteractiveBans()
    {
        var room = CreateRoom();
        var draft = (FactionPickViewDto)room.Draft!;
        draft.Phase = FactionPickPhase.Picks;
        draft.BansRevealed = true;
        draft.PublicBans = [Factions[0]];
        var board = Render<FactionPickBoard>(parameters => parameters.Add(component => component.Room, room));

        board.FindAll("button[aria-pressed]")[0].TextContent.Should().Contain("Banned");
        board.FindAll("button[aria-pressed]").Should().OnlyContain(button => button.HasAttribute("disabled"));
    }

    [Fact]
    public void FactionBoard_WhenHostBans_ShouldAllowOnlyManagersToConfirmPublicBans()
    {
        var room = CreateRoom();
        room.Me.Role = DraftRoomRole.CoHost;
        ((FactionPickSettingsDto)room.Settings).BansMode = FactionPickBansMode.HostBans;
        DraftActionDto? sent = null;
        var board = Render<FactionPickBoard>(parameters => parameters
            .Add(component => component.Room, room)
            .Add(component => component.OnAction, action => sent = action));

        board.FindAll("button").Single(button => button.TextContent.Trim() == "Confirm bans").Click();

        sent.Should().BeOfType<ConfirmHostBansAction>();
        board.FindAll("#faction-pick-decide-seat").Should().BeEmpty();
    }

    private static DraftRoomViewDto CreateRoom() => new()
    {
        Status = DraftRoomStatus.InProgress,
        Settings = new FactionPickSettingsDto { BansMode = FactionPickBansMode.SimultaneousPlayerBans },
        Me = new DraftRoomViewMeDto { SeatId = OwnSeat, Role = DraftRoomRole.Member },
        Seats =
        [
            new DraftRoomViewSeatDto { Id = OwnSeat, Index = 1 },
            new DraftRoomViewSeatDto { Id = OtherSeat, Index = 2 },
        ],
        Draft = new FactionPickViewDto { Phase = FactionPickPhase.Bans, Pool = Factions, TurnOrder = [OtherSeat, OwnSeat] },
    };
}
