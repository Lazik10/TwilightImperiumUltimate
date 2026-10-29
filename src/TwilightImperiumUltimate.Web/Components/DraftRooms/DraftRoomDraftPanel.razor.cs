using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms.FactionPick;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftRoomDraftPanel : IDisposable
{
    private bool _processing;
    private bool _error;
    private bool _confirmReset;
    private bool _disposed;

    [CascadingParameter]
    public IDraftRoomSession Session { get; set; } = default!;

    [Inject]
    private IDraftRoomApiService Api { get; set; } = default!;

    [Inject]
    private IDraftTypeUiRegistry UiRegistry { get; set; } = default!;

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            Session.Changed -= this.OnSessionChanged;
        }

        _disposed = true;
    }

    protected override void OnInitialized() => Session.Changed += this.OnSessionChanged;

    private static string Text(string key) => DraftRoomText.Get(key);

    private static string BlockerText(string code) => code switch
    {
        "SeatsNotFull" => Text("StartSeatsNotFull"),
        "NotEnoughSeats" => Text("StartNotEnoughSeats"),
        "SettingsInvalid" => Text("StartSettingsInvalid"),
        "RoomChanged" => Text("DraftSettingsLocked"),
        _ => throw new InvalidOperationException($"Unsupported Start blocker '{code}'."),
    };

    private static bool IsManager(DraftRoomViewDto room) => room.Me.Role is DraftRoomRole.Host or DraftRoomRole.CoHost;

    private Dictionary<string, object> EditorParameters(DraftRoomViewDto room) => new()
    {
        [nameof(FactionPickSettingsEditor.Settings)] = room.Settings,
        [nameof(FactionPickSettingsEditor.Seats)] = room.Seats,
        [nameof(FactionPickSettingsEditor.CanEdit)] = room.Me.CanChangeSettings,
        [nameof(FactionPickSettingsEditor.IsProcessing)] = _processing,
        [nameof(FactionPickSettingsEditor.OnSave)] = EventCallback.Factory.Create<DraftSettingsDto>(this, this.SaveAsync),
    };

    private Dictionary<string, object> BoardParameters(DraftRoomViewDto room) => new()
    {
        [nameof(FactionPickBoard.Room)] = room,
        [nameof(FactionPickBoard.IsProcessing)] = _processing,
        [nameof(FactionPickBoard.OnAction)] = EventCallback.Factory.Create<DraftActionDto>(this, this.ApplyActionAsync),
        [nameof(FactionPickBoard.OnDecideFor)] = EventCallback.Factory.Create<DecideForRequest>(this, this.DecideForAsync),
        [nameof(FactionPickBoard.OnUndo)] = EventCallback.Factory.Create(this, this.UndoAsync),
    };

    private Task ApplyActionAsync(DraftActionDto action) => this.ExecuteAsync(code => Api.ApplyDraftActionAsync(code, action));

    private Task DecideForAsync(DecideForRequest request) => this.ExecuteAsync(code => Api.DecideForAsync(code, request));

    private Task UndoAsync() => this.ExecuteAsync(code => Api.UndoLastPickAsync(code));

    private void OnSessionChanged() => _ = this.InvokeAsync(this.StateHasChanged);

    private void ShowReset() => _confirmReset = true;

    private void CancelReset() => _confirmReset = false;

    private Task SaveAsync(DraftSettingsDto settings) =>
        this.ExecuteAsync(code => Api.UpdateDraftSettingsAsync(code, settings));

    private Task StartAsync() => this.ExecuteAsync(code => Api.StartDraftAsync(code));

    private Task ResetAsync()
    {
        _confirmReset = false;
        return this.ExecuteAsync(code => Api.ResetDraftAsync(code));
    }

    private async Task ExecuteAsync(Func<string, Task<DraftRoomApiCommandResult>> command)
    {
        if (_processing || Session.View is not { } room)
        {
            return;
        }

        _processing = true;
        _error = false;
        try
        {
            var result = await Session.ExecuteAsync(() => command(room.Code));
            _error = !result.IsSuccess;
        }
        catch (JSException)
        {
            _error = true;
        }
        finally
        {
            _processing = false;
        }
    }
}
