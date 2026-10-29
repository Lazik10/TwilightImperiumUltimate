using System.Text.Json;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Tests.DraftRooms.Engine;

internal sealed class FakeDraftType : IDraftType<FactionPickSettingsDto, FakeDraftState>
{
    public DraftTypeId Id => DraftTypeId.FactionPick;

    public int MinSeats => 2;

    public int MaxSeats => 8;

    public FactionPickSettingsDto CreateDefaultSettings() => new();

    public DraftValidationResult ValidateSettings(FactionPickSettingsDto settings, int seatCount, int poolSize)
    {
        return seatCount < MinSeats || seatCount > MaxSeats || poolSize < seatCount
            ? new DraftValidationResult(["InvalidSeatCountOrPool"])
            : DraftValidationResult.Valid;
    }

    public DraftTransition Start(
        FactionPickSettingsDto settings,
        FakeDraftState state,
        IRandomnessSource randomness) =>
        new(state with { Started = true }, DraftValidationResult.Valid, [], []);

    public DraftTransition Apply(
        FactionPickSettingsDto settings,
        FakeDraftState state,
        DraftActor actor,
        DraftActionDto action) =>
        new(state with { ActionCount = state.ActionCount + 1 }, DraftValidationResult.Valid, [], []);

    public DraftTransition DecideFor(
        FactionPickSettingsDto settings,
        FakeDraftState state,
        DraftActor actor,
        Guid seatId,
        DraftActionDto action)
    {
        if (!actor.IsManager)
        {
            return new DraftTransition(state, new DraftValidationResult(["Forbidden"]), [], []);
        }

        var data = JsonSerializer.SerializeToElement(state.PrivateValue);

        return new DraftTransition(
            state with { PrivateValue = string.Empty },
            DraftValidationResult.Valid,
            [new DraftLogEvent("HostDecidedForSeat", data)],
            [new DraftDisclosure(seatId, data)]);
    }

    public DraftTransition Undo(FactionPickSettingsDto settings, FakeDraftState state, DraftActor actor) =>
        new(state with { ActionCount = Math.Max(0, state.ActionCount - 1) }, DraftValidationResult.Valid, [], []);

    public DraftViewDto Project(FactionPickSettingsDto settings, FakeDraftState state, DraftViewer viewer)
    {
        return new FakeDraftView
        {
            PublicValue = "Visible to all",
            PrivateValue = viewer.CanSee(DraftVisibility.SeatPrivate, state.OwnerSeatId)
                ? state.PrivateValue
                : null,
        };
    }

    public JsonElement BuildResult(FactionPickSettingsDto settings, FakeDraftState state) =>
        JsonSerializer.SerializeToElement(new { state.ActionCount });

    public string FormatResultAsText(JsonElement result) => result.GetRawText();
}
