using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftRoomResultPanel : IDisposable
{
    private readonly CancellationTokenSource _lifetime = new();
    private DraftResultDto? _result;
    private string? _code;
    private int _roomVersion = -1;
    private int _loadVersion;
    private bool _loading = true;
    private bool _missing;
    private bool _loadError;
    private bool _deleteError;
    private bool _confirmDelete;
    private bool _processing;
    private bool _disposed;
    private bool _focusStatus;
    private ElementReference _status;

    [Parameter]
    [EditorRequired]
    public DraftRoomViewDto Room { get; set; } = default!;

    [CascadingParameter]
    public IDraftRoomSession Session { get; set; } = default!;

    [Inject]
    private IDraftRoomApiService Api { get; set; } = default!;

    [Inject]
    private ILogger<DraftRoomResultPanel> Logger { get; set; } = default!;

    private bool CanDelete => Room.Me.Role is DraftRoomRole.Host or DraftRoomRole.CoHost;

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _disposed = true;
            _lifetime.Cancel();
            _lifetime.Dispose();
        }
    }

    protected override Task OnParametersSetAsync()
    {
        if (_code == Room.Code && _roomVersion == Room.Version)
        {
            return Task.CompletedTask;
        }

        _code = Room.Code;
        _roomVersion = Room.Version;
        _confirmDelete = false;
        return this.LoadAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_focusStatus && _missing)
        {
            _focusStatus = false;
            await _status.FocusAsync();
        }
    }

    private static string Text(string key) => DraftRoomText.Get(key);

    private void ShowDelete() => _confirmDelete = true;

    private void CancelDelete() => _confirmDelete = false;

    private async Task LoadAsync()
    {
        var version = ++_loadVersion;
        _loading = true;
        _loadError = false;
        _missing = false;
        _result = null;
        try
        {
            var response = await Api.GetRoomResultAsync(Room.Code, _lifetime.Token);
            if (_disposed || version != _loadVersion)
            {
                return;
            }

            _missing = response.Kind == DraftRoomApiResultKind.NotFound;
            _loadError = !_missing && (!response.IsSuccess || response.Data is null);
            _result = response.Data;
        }
        catch (OperationCanceledException exception) when (_disposed)
        {
            Logger.LogDebug(exception, "Draft room result load was cancelled during disposal");
        }
        catch (JSException exception)
        {
            Logger.LogWarning(exception, "Draft room result browser access failed");
            if (!_disposed && version == _loadVersion)
            {
                _loadError = true;
            }
        }
        finally
        {
            if (!_disposed && version == _loadVersion)
            {
                _loading = false;
            }
        }
    }

    private async Task DeleteAsync()
    {
        _confirmDelete = false;
        if (_processing || _result is null || !CanDelete)
        {
            return;
        }

        _processing = true;
        _deleteError = false;
        var resultId = _result.Id;
        var code = Room.Code;
        try
        {
            var response = await Session.ExecuteAsync(() => Api.DeleteResultAsync(resultId, code, _lifetime.Token), _lifetime.Token);
            if (_disposed || Room.Code != code)
            {
                return;
            }

            _deleteError = !response.IsSuccess;
            if (response.IsSuccess)
            {
                _loadVersion++;
                _result = null;
                _loading = false;
                _missing = true;
                _focusStatus = true;
            }
        }
        catch (OperationCanceledException exception) when (_disposed)
        {
            Logger.LogDebug(exception, "Draft room result deletion was cancelled during disposal");
        }
        catch (JSException exception)
        {
            Logger.LogWarning(exception, "Draft room result deletion browser access failed");
            _deleteError = true;
        }
        finally
        {
            _processing = false;
        }
    }
}
