using System.Globalization;
using System.Text;
using System.Text.Json;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

public sealed partial class FactionPickDraftType
{
    private static JsonElement BuildFactionResult(FactionPickSettingsDto settings, FactionPickDraftState state)
    {
        if (!state.IsStarted || state.Phase != FactionPickPhase.Finished || !state.BansRevealed
            || state.Choices.Any(choice => choice.State != FactionPickSelectionState.Locked))
        {
            throw new InvalidOperationException("Only a completely revealed, finished draft can become a result.");
        }

        var seats = state.ResultSeats.ToDictionary(seat => seat.SeatId);
        if (seats.Count != state.SeatIds.Count || !seats.Keys.ToHashSet().SetEquals(state.SeatIds))
        {
            throw new InvalidOperationException("A result requires an immutable label for every draft seat.");
        }

        var picks = OrderedPicks(settings, state).ToArray();
        if (picks.Length != state.SeatIds.Count * settings.PicksPerPlayer
            || state.SeatIds.Any(seatId => picks.Count(pick => pick.SeatId == seatId) != settings.PicksPerPlayer))
        {
            throw new InvalidOperationException("A finished result must contain every required locked pick.");
        }

        return JsonSerializer.SerializeToElement(new FactionPickResultDto
        {
            GameVersions = settings.GameVersions.ToArray(),
            BansMode = settings.BansMode,
            BansPerPlayer = settings.BansPerPlayer,
            HostBanCount = settings.HostBanCount,
            PickOrderMode = settings.PickOrderMode,
            PicksPerPlayer = settings.PicksPerPlayer,
            Snake = settings.Snake,
            SeatOrder = state.TurnOrder.Select(seatId => seats[seatId].Index).ToArray(),
            Bans = state.Choices.Where(choice => choice.Kind == FactionPickSelectionKind.Ban)
                .OrderBy(choice => choice.SeatId is Guid seatId ? seats[seatId].Index : 0).ThenBy(choice => choice.Faction)
                .Select(choice => new FactionPickResultBanDto(
                    choice.SeatId is Guid seatId ? seats[seatId].Index : null,
                    choice.Faction,
                    DeciderName(state, choice)))
                .ToArray(),
            Seats = state.ResultSeats.OrderBy(seat => seat.Index)
                .Select(seat => new FactionPickResultSeatDto(
                    seat.Index,
                    seat.DisplayName,
                    picks.Where(pick => pick.SeatId == seat.SeatId)
                        .Select(pick => new FactionPickResultPickDto(pick.Faction, pick.Round, DeciderName(state, pick))).ToArray()))
                .ToArray(),
        });
    }

    private static string? DeciderName(FactionPickDraftState state, FactionPickChoice choice) =>
        choice.DecidedByParticipantId is Guid id ? state.ParticipantNames[id] : null;

    private static string FormatFactionResult(JsonElement result)
    {
        var payload = result.Deserialize<FactionPickResultDto>()
            ?? throw new JsonException("A Faction Pick result payload is required.");
        var text = new StringBuilder();
        foreach (var seat in payload.Seats.OrderBy(seat => seat.SeatIndex))
        {
            var picks = seat.Picks.Select(pick => pick.DecidedByDisplayName is string manager
                ? $"{pick.Faction} (decided by {manager})"
                : pick.Faction.ToString());
            text.Append(CultureInfo.InvariantCulture, $"Seat {seat.SeatIndex} - {seat.DisplayName}: ").AppendJoin(", ", picks).AppendLine();
        }

        text.Append("Bans: ").AppendJoin(", ", payload.Bans.Select(ban => ban.Faction).Distinct()).AppendLine();
        text.Append("Order: ").AppendJoin(", ", payload.SeatOrder.Select(index => string.Create(CultureInfo.InvariantCulture, $"Seat {index}")));
        return text.ToString();
    }
}
