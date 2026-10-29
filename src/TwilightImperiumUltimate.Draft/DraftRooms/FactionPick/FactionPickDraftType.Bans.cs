using System.Text.Json;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

public sealed partial class FactionPickDraftType
{
    private static DraftTransition ApplyBans(
        FactionPickSettingsDto settings,
        FactionPickDraftState state,
        DraftActor actor,
        DraftActionDto action)
    {
        if (!state.IsStarted || state.Phase != FactionPickPhase.Bans)
        {
            return Refuse(state, "WrongPhase");
        }

        var hostMode = settings.BansMode == FactionPickBansMode.HostBans;
        Guid? seatId = hostMode ? null : actor.SeatId;
        if (actor.ParticipantId == Guid.Empty || (hostMode
            ? !actor.IsManager
            : seatId is not Guid id || !state.SeatIds.Contains(id)))
        {
            return Refuse(state, "Forbidden");
        }

        if (seatId is Guid lockedSeat && state.LockedBanSeatIds.Contains(lockedSeat))
        {
            return Refuse(state, "AlreadyLocked");
        }

        var choices = state.Choices.ToList();
        var own = choices.Where(choice => choice.Kind == FactionPickSelectionKind.Ban && choice.SeatId == seatId).ToArray();
        var limit = hostMode ? settings.HostBanCount : settings.BansPerPlayer;
        switch (action)
        {
            case SelectBanAction select:
                if (!state.Pool.Contains(select.Faction))
                {
                    return Refuse(state, "InvalidFaction");
                }

                if (own.Any(choice => choice.Faction == select.Faction))
                {
                    return Refuse(state, "AlreadySelected");
                }

                if (own.Length >= limit)
                {
                    return Refuse(state, "BanLimitReached");
                }

                choices.Add(new FactionPickChoice(seatId, FactionPickSelectionKind.Ban, select.Faction, FactionPickSelectionState.Pending));
                break;
            case DeselectBanAction deselect:
                if (choices.RemoveAll(choice => choice.Kind == FactionPickSelectionKind.Ban
                    && choice.SeatId == seatId && choice.Faction == deselect.Faction) == 0)
                {
                    return Refuse(state, "NotSelected");
                }

                break;
            case LockInBansAction when !hostMode:
                return LockBans(state, seatId!.Value, null);
            case ConfirmHostBansAction when hostMode:
                return RevealBans(state with
                {
                    Choices = choices.Select(choice => choice.Kind == FactionPickSelectionKind.Ban
                        ? choice with { State = FactionPickSelectionState.Locked, DecidedByParticipantId = actor.ParticipantId }
                        : choice).ToArray(),
                });
            default:
                return Refuse(state, "InvalidAction");
        }

        var next = state with { Choices = choices.ToArray() };
        return new DraftTransition(
            next,
            DraftValidationResult.Valid,
            hostMode ? [Event("HostBansChanged", new { Factions = choices.Select(choice => choice.Faction).ToArray() })] : [],
            []);
    }

    private static DraftTransition DecideBans(
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

        if (!state.IsStarted || state.Phase != FactionPickPhase.Bans
            || settings.BansMode != FactionPickBansMode.SimultaneousPlayerBans)
        {
            return Refuse(state, "WrongPhase");
        }

        if (!state.SeatIds.Contains(seatId))
        {
            return Refuse(state, "InvalidSeat");
        }

        if (state.LockedBanSeatIds.Contains(seatId))
        {
            return Refuse(state, "AlreadyLocked");
        }

        if (action is not DecideBansAction { Factions: { } factions })
        {
            return Refuse(state, "InvalidAction");
        }

        if (factions.Length > settings.BansPerPlayer || factions.Distinct().Count() != factions.Length)
        {
            return Refuse(state, "BanLimitReached");
        }

        if (factions.Any(faction => !state.Pool.Contains(faction)))
        {
            return Refuse(state, "InvalidFaction");
        }

        var disclosed = state.Choices.Where(choice => choice.Kind == FactionPickSelectionKind.Ban && choice.SeatId == seatId)
            .Select(choice => choice.Faction).ToArray();
        var next = state with
        {
            Choices = state.Choices.Where(choice => choice.Kind != FactionPickSelectionKind.Ban || choice.SeatId != seatId)
                .Concat(factions.Select(faction => new FactionPickChoice(seatId, FactionPickSelectionKind.Ban, faction, FactionPickSelectionState.Pending)))
                .ToArray(),
        };
        var transition = LockBans(next, seatId, actor.ParticipantId);
        var disclosure = JsonSerializer.SerializeToElement(new { Factions = disclosed });
        return transition with
        {
            LogEvents = new[] { Event("HostDecidedForSeat", new { actor.ParticipantId, SeatId = seatId, Factions = disclosed }) }
                .Concat(transition.LogEvents).ToArray(),
            Disclosures = [new DraftDisclosure(seatId, disclosure)],
        };
    }

    private static DraftTransition LockBans(FactionPickDraftState state, Guid seatId, Guid? participantId)
    {
        var next = state with
        {
            Choices = state.Choices.Select(choice => choice.Kind == FactionPickSelectionKind.Ban && choice.SeatId == seatId
                ? choice with { State = FactionPickSelectionState.Locked, DecidedByParticipantId = participantId }
                : choice).ToArray(),
            LockedBanSeatIds = state.LockedBanSeatIds.Append(seatId).ToArray(),
        };
        var lockedEvent = Event("SeatBansLocked", new { SeatId = seatId });
        if (!next.SeatIds.All(id => next.LockedBanSeatIds.Contains(id)))
        {
            return new DraftTransition(next, DraftValidationResult.Valid, [lockedEvent], []);
        }

        var reveal = RevealBans(next);
        return reveal with { LogEvents = new[] { lockedEvent }.Concat(reveal.LogEvents).ToArray() };
    }

    private static DraftTransition RevealBans(FactionPickDraftState state)
    {
        var factions = state.Choices.Where(choice => choice.Kind == FactionPickSelectionKind.Ban)
            .Select(choice => choice.Faction).Distinct().Order().ToArray();
        return new DraftTransition(
            state with { BansRevealed = true, Phase = FactionPickPhase.Picks, TurnIndex = 0 },
            DraftValidationResult.Valid,
            [Event("BansRevealed", new { Factions = factions })],
            []);
    }

    private static DraftLogEvent Event(string code, object data) => new(code, JsonSerializer.SerializeToElement(data));
}
