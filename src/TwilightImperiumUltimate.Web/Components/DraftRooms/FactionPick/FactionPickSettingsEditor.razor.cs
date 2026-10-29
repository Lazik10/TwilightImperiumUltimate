using System.Globalization;
using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms.FactionPick;

public partial class FactionPickSettingsEditor
{
    private static readonly GameVersion[] SupportedVersions = [GameVersion.BaseGame, GameVersion.ProphecyOfKings, GameVersion.ThundersEdge];
    private readonly Dictionary<Guid, ElementReference> _seatRows = [];
    private FactionPickSettingsDto _settings = new();
    private DraftSettingsDto? _previousSettings;
    private Guid? _focusSeat;

    [Parameter]
    [EditorRequired]
    public DraftSettingsDto Settings { get; set; } = default!;

    [Parameter]
    public IReadOnlyList<DraftRoomViewSeatDto> Seats { get; set; } = [];

    [Parameter]
    public bool CanEdit { get; set; }

    [Parameter]
    public bool IsProcessing { get; set; }

    [Parameter]
    public EventCallback<DraftSettingsDto> OnSave { get; set; }

    private bool IsDisabled => !CanEdit || IsProcessing;

    protected override void OnParametersSet()
    {
        if (!ReferenceEquals(_previousSettings, Settings))
        {
            if (Settings is not FactionPickSettingsDto settings)
            {
                throw new InvalidOperationException("The Faction Pick editor requires Faction Pick settings.");
            }

            _settings = new FactionPickSettingsDto
            {
                GameVersions = settings.GameVersions.ToArray(),
                BansMode = settings.BansMode,
                BansPerPlayer = settings.BansPerPlayer,
                HostBanCount = settings.HostBanCount,
                PickOrderMode = settings.PickOrderMode,
                SeatOrder = settings.SeatOrder.ToArray(),
                PicksPerPlayer = settings.PicksPerPlayer,
                Snake = settings.Snake,
            };
            _previousSettings = Settings;
        }

        _settings.SeatOrder = _settings.SeatOrder.Where(id => Seats.Any(seat => seat.Id == id))
            .Concat(Seats.Select(seat => seat.Id).Except(_settings.SeatOrder)).ToArray();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_focusSeat is Guid seatId && _seatRows.TryGetValue(seatId, out var row))
        {
            _focusSeat = null;
            await row.FocusAsync();
        }
    }

    private static string Text(string key) => DraftRoomText.Get(key);

    private string SeatLabel(Guid seatId) =>
        string.Format(CultureInfo.CurrentCulture, Text("SeatNumber"), Seats.Single(seat => seat.Id == seatId).Index);

    private void MoveSeat(Guid seatId, int direction)
    {
        var order = _settings.SeatOrder.ToList();
        var index = order.IndexOf(seatId);
        var destination = index + direction;
        if (!IsDisabled && index >= 0 && destination >= 0 && destination < order.Count)
        {
            (order[index], order[destination]) = (order[destination], order[index]);
            _settings.SeatOrder = order;
            if (destination == 0 || destination == order.Count - 1)
            {
                _focusSeat = seatId;
            }
        }
    }

    private void ToggleVersion(GameVersion version)
    {
        if (!IsDisabled)
        {
            _settings.GameVersions = _settings.GameVersions.Contains(version)
                ? _settings.GameVersions.Where(value => value != version).ToArray()
                : _settings.GameVersions.Append(version).ToArray();
        }
    }

    private Task SaveAsync() => IsDisabled ? Task.CompletedTask : OnSave.InvokeAsync(_settings);
}
