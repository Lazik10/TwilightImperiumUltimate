using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms.FactionPick;

public partial class FactionPickBoard
{
    private readonly HashSet<FactionName> _replacementBans = [];
    private Guid? _decisionSeat;
    private FactionName? _replacementPick;
    private int _decisionTurn;
    private bool _confirmDecision;
    private bool _confirmUndo;
    private bool _focusBoard;
    private ElementReference _heading;

    [Parameter]
    [EditorRequired]
    public DraftRoomViewDto Room { get; set; } = default!;

    [Parameter]
    public bool IsProcessing { get; set; }

    [Parameter]
    public EventCallback<DraftActionDto> OnAction { get; set; }

    [Parameter]
    public EventCallback<DecideForRequest> OnDecideFor { get; set; }

    [Parameter]
    public EventCallback OnUndo { get; set; }

    private FactionPickViewDto Draft => Room.Draft as FactionPickViewDto
        ?? throw new InvalidOperationException("Faction Pick requires a personalized Faction Pick projection.");

    private FactionPickSettingsDto Settings => Room.Settings as FactionPickSettingsDto
        ?? throw new InvalidOperationException("Faction Pick requires its own settings.");

    private bool IsManager => Room.Me.Role is DraftRoomRole.Host or DraftRoomRole.CoHost;

    private bool IsHostBans => Settings.BansMode == FactionPickBansMode.HostBans;

    private bool IsBans => Draft.Phase == FactionPickPhase.Bans;

    private bool IsPicks => Draft.Phase == FactionPickPhase.Picks;

    private bool CanPickOwn => IsPicks && Room.Me.SeatId is Guid seatId && seatId == Draft.ActiveSeatId && _decisionSeat is null;

    private bool IsFinalPick => IsPicks && Draft.TurnIndex == (Draft.TurnOrder.Count * Settings.PicksPerPlayer) - 1;

    private IEnumerable<Guid> DecisionSeats => IsPicks
        ? Room.Seats.Where(seat => seat.Id == Draft.ActiveSeatId).Select(seat => seat.Id)
        : Room.Seats.Where(seat => !Draft.LockedBanSeatIds.Contains(seat.Id)).Select(seat => seat.Id);

    private IReadOnlyCollection<FactionName> CurrentSelection
    {
        get
        {
            if (_decisionSeat is not null)
            {
                return _replacementBans;
            }

            return IsHostBans ? Draft.PublicBans : Draft.MyBans;
        }
    }

    private bool CanLockOwnBans => IsBans && _decisionSeat is null && (IsHostBans
        ? IsManager
        : Room.Me.SeatId is Guid seatId && !Draft.LockedBanSeatIds.Contains(seatId));

    private string PhaseText => Text(Draft.Phase switch
    {
        FactionPickPhase.Bans => "BansPhase",
        FactionPickPhase.Picks => "PicksPhase",
        FactionPickPhase.Finished => "FinishedPhase",
        _ => throw new InvalidOperationException("Unsupported Faction Pick phase."),
    });

    private string DecisionWarning => string.Format(CultureInfo.CurrentCulture, Text("DecisionWarning"), _decisionSeat is Guid seat ? SeatLabel(seat) : string.Empty);

    protected override void OnParametersSet()
    {
        if (_decisionSeat is Guid seat && (!IsManager
            || (IsBans && Draft.LockedBanSeatIds.Contains(seat))
            || (!IsBans && (!IsPicks || seat != Draft.ActiveSeatId || _decisionTurn != Draft.TurnIndex))))
        {
            _decisionSeat = null;
            _replacementBans.Clear();
            _replacementPick = null;
            _confirmDecision = false;
        }

        if (!Draft.CanUndo)
        {
            _confirmUndo = false;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_focusBoard)
        {
            _focusBoard = false;
            await _heading.FocusAsync();
        }
    }

    private static string Text(string key) => DraftRoomText.Get(key);

    private static string FactionStatusId(FactionName faction) => $"draft-faction-status-{faction}";

    private string SeatLabel(Guid seatId)
    {
        var seat = Room.Seats.Single(item => item.Id == seatId);
        return string.Format(CultureInfo.CurrentCulture, Text("SeatNumber"), seat.Index);
    }

    private bool IsSelected(FactionName faction)
    {
        if (!IsPicks)
        {
            return IsBans && CurrentSelection.Contains(faction);
        }

        var selected = _decisionSeat is null ? Draft.MyPendingPick : _replacementPick;
        return selected == faction;
    }

    private string ChoiceStatus(FactionName faction)
    {
        if (Draft.BansRevealed && Draft.PublicBans.Contains(faction))
        {
            return Text("FactionBanned");
        }

        if (Draft.LockedPicks.Any(pick => pick.Faction == faction))
        {
            return Text("FactionPicked");
        }

        return Text(this.IsSelected(faction) ? "FactionSelected" : "FactionAvailable");
    }

    private bool IsDisabled(FactionName faction)
    {
        if (IsPicks)
        {
            return IsProcessing || (!CanPickOwn && _decisionSeat is null) || Draft.PublicBans.Contains(faction)
                || Draft.LockedPicks.Any(pick => pick.Faction == faction);
        }

        if (IsProcessing || !IsBans || (!CanLockOwnBans && _decisionSeat is null))
        {
            return true;
        }

        var limit = IsHostBans ? Settings.HostBanCount : Settings.BansPerPlayer;
        return !this.IsSelected(faction) && CurrentSelection.Count >= limit;
    }

    private Task ToggleAsync(FactionName faction)
    {
        if (this.IsDisabled(faction))
        {
            return Task.CompletedTask;
        }

        if (_decisionSeat is not null)
        {
            if (IsPicks)
            {
                _replacementPick = faction;
            }
            else if (!_replacementBans.Add(faction))
            {
                _replacementBans.Remove(faction);
            }

            return Task.CompletedTask;
        }

        if (IsPicks)
        {
            return OnAction.InvokeAsync(new SelectPickAction { Faction = faction });
        }

        DraftActionDto action = this.IsSelected(faction) ? new DeselectBanAction { Faction = faction } : new SelectBanAction { Faction = faction };
        return OnAction.InvokeAsync(action);
    }

    private void SelectDecisionSeat(ChangeEventArgs args)
    {
        _decisionSeat = Guid.TryParse(args.Value?.ToString(), out var seatId) ? seatId : null;
        _replacementBans.Clear();
        _replacementPick = null;
        _decisionTurn = Draft.TurnIndex;
        _confirmDecision = false;
    }

    private void ShowDecisionConfirmation() => _confirmDecision = true;

    private void CancelDecision() => _confirmDecision = false;

    private async Task ConfirmDecisionAsync()
    {
        _confirmDecision = false;
        if (_decisionSeat is Guid seatId)
        {
            DraftActionDto action;
            if (IsPicks)
            {
                if (_replacementPick is not FactionName faction)
                {
                    return;
                }

                action = new DecidePickAction { Faction = faction, TurnIndex = _decisionTurn };
            }
            else
            {
                action = new DecideBansAction { Factions = _replacementBans.Order().ToArray() };
            }

            await OnDecideFor.InvokeAsync(new DecideForRequest(seatId, action));
            _focusBoard = true;
        }
    }

    private async Task LockAsync()
    {
        DraftActionDto action;
        if (IsPicks)
        {
            action = new LockInPickAction { TurnIndex = Draft.TurnIndex };
        }
        else
        {
            action = IsHostBans ? new ConfirmHostBansAction() : new LockInBansAction();
        }

        await OnAction.InvokeAsync(action);
        _focusBoard = true;
    }

    private void ShowUndo() => _confirmUndo = true;

    private void CancelUndo() => _confirmUndo = false;

    private async Task ConfirmUndoAsync()
    {
        _confirmUndo = false;
        await OnUndo.InvokeAsync();
        _focusBoard = true;
    }
}
