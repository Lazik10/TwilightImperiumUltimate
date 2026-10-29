using System.Text.Json;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

public sealed partial class FactionPickDraftType
{
    private static Guid SeatAt(FactionPickSettingsDto settings, FactionPickDraftState state, int turnIndex)
    {
        var count = state.TurnOrder.Count;
        if (count == 0 || turnIndex < 0 || turnIndex >= (long)count * settings.PicksPerPlayer)
        {
            throw new InvalidOperationException("The running Faction Pick turn cursor is invalid.");
        }

        var position = turnIndex % count;
        if (settings.Snake && (turnIndex / count) % 2 == 1)
        {
            position = count - position - 1;
        }

        return state.TurnOrder[position];
    }

    private static IEnumerable<FactionPickChoice> OrderedPicks(FactionPickSettingsDto settings, FactionPickDraftState state)
    {
        var positions = state.TurnOrder.Select((id, position) => (id, position)).ToDictionary(item => item.id, item => item.position);
        return state.Choices.Where(choice => choice.Kind == FactionPickSelectionKind.Pick && choice.State == FactionPickSelectionState.Locked)
            .OrderBy(choice => choice.Round)
            .ThenBy(choice => settings.Snake && choice.Round % 2 == 1 ? -positions[choice.SeatId!.Value] : positions[choice.SeatId!.Value]);
    }

    private static bool RequiresAction(FactionPickSettingsDto settings, FactionPickDraftState state, DraftViewer viewer)
    {
        if (!state.IsStarted || state.Phase == FactionPickPhase.Finished)
        {
            return false;
        }

        if (state.Phase == FactionPickPhase.Picks)
        {
            return viewer.CanSee(DraftVisibility.SeatPrivate, SeatAt(settings, state, state.TurnIndex));
        }

        if (settings.BansMode == FactionPickBansMode.HostBans)
        {
            return viewer.ParticipantId is Guid participantId && participantId != Guid.Empty
                && viewer.Role is DraftRoomRole.Host or DraftRoomRole.CoHost;
        }

        return viewer.CanSee(DraftVisibility.SeatPrivate, viewer.SeatId) && state.SeatIds.Contains(viewer.SeatId!.Value)
            && !state.LockedBanSeatIds.Contains(viewer.SeatId.Value);
    }

    private static bool Available(FactionPickDraftState state, FactionName faction) =>
        state.Pool.Contains(faction) && !state.Choices.Any(choice => choice.Faction == faction
            && (choice.Kind == FactionPickSelectionKind.Ban || choice.State == FactionPickSelectionState.Locked));

    private static DraftTransition ApplyPicks(
        FactionPickSettingsDto settings,
        FactionPickDraftState state,
        DraftActor actor,
        DraftActionDto action)
    {
        if (!state.IsStarted)
        {
            return Refuse(state, "WrongPhase");
        }

        if (action is SelectBanAction or DeselectBanAction or LockInBansAction or ConfirmHostBansAction)
        {
            return Refuse(state, "WrongPhase");
        }

        if (actor.ParticipantId == Guid.Empty || actor.SeatId is null)
        {
            return Refuse(state, "Forbidden");
        }

        if (action is LockInPickAction locked && locked.TurnIndex != state.TurnIndex)
        {
            return Refuse(state, "StaleTurn");
        }

        var activeSeat = SeatAt(settings, state, state.TurnIndex);
        if (actor.SeatId != activeSeat)
        {
            return Refuse(state, "NotYourTurn");
        }

        if (action is SelectPickAction select)
        {
            if (!Available(state, select.Faction))
            {
                return Refuse(state, "FactionUnavailable");
            }

            var next = state with
            {
                Choices = state.Choices.Where(choice => choice.Kind != FactionPickSelectionKind.Pick
                        || choice.State != FactionPickSelectionState.Pending)
                    .Append(new FactionPickChoice(activeSeat, FactionPickSelectionKind.Pick, select.Faction, FactionPickSelectionState.Pending, state.TurnIndex / state.SeatIds.Count))
                    .ToArray(),
            };
            return new DraftTransition(next, DraftValidationResult.Valid, [], []);
        }

        if (action is not LockInPickAction)
        {
            return Refuse(state, "InvalidAction");
        }

        var pending = state.Choices.SingleOrDefault(choice => choice.Kind == FactionPickSelectionKind.Pick
            && choice.State == FactionPickSelectionState.Pending && choice.SeatId == activeSeat);
        return pending is null ? Refuse(state, "MissingSelection") : LockPick(settings, state, pending.Faction, null);
    }

    private static DraftTransition LockPick(
        FactionPickSettingsDto settings,
        FactionPickDraftState state,
        FactionName faction,
        Guid? decidedBy)
    {
        if (!Available(state, faction))
        {
            return Refuse(state, "FactionUnavailable");
        }

        var seatId = SeatAt(settings, state, state.TurnIndex);
        var turn = state.TurnIndex + 1;
        var finished = turn == state.SeatIds.Count * settings.PicksPerPlayer;
        var choice = new FactionPickChoice(seatId, FactionPickSelectionKind.Pick, faction, FactionPickSelectionState.Locked, state.TurnIndex / state.SeatIds.Count, decidedBy);
        var next = state with
        {
            Choices = state.Choices.Where(item => item.Kind != FactionPickSelectionKind.Pick || item.State != FactionPickSelectionState.Pending)
                .Append(choice).ToArray(),
            TurnIndex = turn,
            Phase = finished ? FactionPickPhase.Finished : FactionPickPhase.Picks,
        };
        var events = new List<DraftLogEvent> { Event("FactionPicked", new { SeatId = seatId, Faction = faction, choice.Round, DecidedByHost = decidedBy is not null }) };
        if (finished)
        {
            events.Add(Event("DraftFinished", new { }));
        }

        return new DraftTransition(next, DraftValidationResult.Valid, events, []) { IsFinished = finished };
    }

    private static DraftTransition DecidePick(
        FactionPickSettingsDto settings,
        FactionPickDraftState state,
        DraftActor actor,
        Guid seatId,
        DraftActionDto action)
    {
        if (!actor.IsManager || actor.ParticipantId == Guid.Empty)
        {
            return Refuse(state, "Forbidden");
        }

        if (!state.IsStarted || state.Phase != FactionPickPhase.Picks)
        {
            return Refuse(state, "WrongPhase");
        }

        if (action is not DecidePickAction pick)
        {
            return Refuse(state, "InvalidAction");
        }

        if (pick.TurnIndex != state.TurnIndex)
        {
            return Refuse(state, "StaleTurn");
        }

        if (seatId != SeatAt(settings, state, state.TurnIndex))
        {
            return Refuse(state, "NotYourTurn");
        }

        var transition = LockPick(settings, state, pick.Faction, actor.ParticipantId);
        if (!transition.Validation.IsValid)
        {
            return transition;
        }

        var pending = state.Choices.Where(choice => choice.Kind == FactionPickSelectionKind.Pick
            && choice.State == FactionPickSelectionState.Pending && choice.SeatId == seatId).Select(choice => choice.Faction).ToArray();
        return transition with
        {
            LogEvents = new[] { Event("HostDecidedForSeat", new { actor.ParticipantId, SeatId = seatId, Factions = pending }) }
                .Concat(transition.LogEvents).ToArray(),
            Disclosures = [new DraftDisclosure(seatId, JsonSerializer.SerializeToElement(new { Factions = pending }))],
        };
    }

    private static DraftTransition UndoPick(FactionPickSettingsDto settings, FactionPickDraftState state, DraftActor actor)
    {
        if (!actor.IsManager || actor.ParticipantId == Guid.Empty)
        {
            return Refuse(state, "Forbidden");
        }

        if (!state.IsStarted || state.Phase != FactionPickPhase.Picks)
        {
            return Refuse(state, "WrongPhase");
        }

        if (state.TurnIndex == 0)
        {
            return Refuse(state, "NothingToUndo");
        }

        var previousTurn = state.TurnIndex - 1;
        var seatId = SeatAt(settings, state, previousTurn);
        var round = previousTurn / state.SeatIds.Count;
        var removed = state.Choices.Single(choice => choice.Kind == FactionPickSelectionKind.Pick && choice.State == FactionPickSelectionState.Locked
            && choice.SeatId == seatId && choice.Round == round);
        var next = state with
        {
            TurnIndex = previousTurn,
            Choices = state.Choices.Where(choice => choice != removed
                && (choice.Kind != FactionPickSelectionKind.Pick || choice.State != FactionPickSelectionState.Pending)).ToArray(),
        };
        return new DraftTransition(next, DraftValidationResult.Valid, [Event("PickUndone", new { SeatId = seatId, removed.Faction })], []);
    }
}
