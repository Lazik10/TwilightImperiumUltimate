using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Pages.Tools;

public partial class DraftRoomResult : IDisposable
{
    private readonly CancellationTokenSource _lifetime = new();
    private DraftResultDto? _result;
    private int _loadVersion;
    private bool _loading = true;
    private bool _notFound;
    private bool _error;
    private bool _disposed;

    [Parameter]
    public string Token { get; set; } = string.Empty;

    [Inject]
    private IDraftRoomApiService Api { get; set; } = default!;

    [Inject]
    private ILogger<DraftRoomResult> Logger { get; set; } = default!;

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

    protected override Task OnParametersSetAsync() => this.LoadAsync();

    private static string Text(string key) => DraftRoomText.Get(key);

    private async Task LoadAsync()
    {
        var version = ++_loadVersion;
        _loading = true;
        _notFound = false;
        _error = false;
        _result = null;
        try
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                _notFound = true;
                return;
            }

            var response = await Api.GetResultAsync(Token, _lifetime.Token);
            if (_disposed || version != _loadVersion)
            {
                return;
            }

            _notFound = response.Kind == DraftRoomApiResultKind.NotFound;
            _error = !_notFound && (!response.IsSuccess || response.Data is null);
            _result = response.Data;
        }
        catch (OperationCanceledException exception) when (_disposed)
        {
            Logger.LogDebug(exception, "Public draft result load was cancelled during disposal");
        }
        catch (JSException exception)
        {
            Logger.LogWarning(exception, "Public draft result browser access failed");
            if (!_disposed && version == _loadVersion)
            {
                _error = true;
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
}
