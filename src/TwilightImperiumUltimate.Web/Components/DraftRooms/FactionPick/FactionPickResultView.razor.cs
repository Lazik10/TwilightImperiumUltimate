using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms.FactionPick;

public partial class FactionPickResultView
{
    private FactionPickResultDto _result = new();

    [Parameter]
    [EditorRequired]
    public JsonElement Payload { get; set; }

    protected override void OnParametersSet() =>
        _result = Payload.Deserialize<FactionPickResultDto>() ?? throw new JsonException("A public Faction Pick result is required.");

    private static string Text(string key) => DraftRoomText.Get(key);

    private static string SeatLabel(int index) => string.Format(CultureInfo.CurrentCulture, Text("SeatNumber"), index);

    private static string SeatName(FactionPickResultSeatDto seat) =>
        string.Format(CultureInfo.CurrentCulture, Text("ResultSeat"), seat.SeatIndex, seat.DisplayName);

    private static string Decider(string name) => string.Format(CultureInfo.CurrentCulture, Text("ResultDecider"), name);
}
