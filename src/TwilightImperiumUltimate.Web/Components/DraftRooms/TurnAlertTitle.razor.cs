using System.Globalization;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class TurnAlertTitle : IAsyncDisposable
{
    private const string PreferenceKey = "draftRooms.turnAlert.v1";
    private readonly CancellationTokenSource _lifetime = new();
    private Task? _timerTask;
    private Task? _initialization;
    private bool _enabled = true;
    private bool _initialized;
    private bool _unavailable;
    private bool _showTurnTitle;
    private bool _disposed;

    [Parameter]
    [EditorRequired]
    public string NormalTitle { get; set; } = string.Empty;

    [Parameter]
    public bool MyActionRequired { get; set; }

    [Inject]
    private IPageVisibility Visibility { get; set; } = default!;

    [Inject]
    private ILocalStorageService Storage { get; set; } = default!;

    [Inject]
    private TimeProvider Clock { get; set; } = default!;

    [Inject]
    private ILogger<TurnAlertTitle> Logger { get; set; } = default!;

    private bool ShouldBlink => _initialized && !_unavailable && _enabled && MyActionRequired && Visibility.IsHidden;

    private string DocumentTitle => _showTurnTitle
        ? string.Format(CultureInfo.CurrentCulture, Text("YourTurnTitle"), NormalTitle)
        : NormalTitle;

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        Visibility.Changed -= this.OnVisibilityChanged;
        await _lifetime.CancelAsync();
        if (_initialization is not null)
        {
            await _initialization;
        }

        if (_timerTask is not null)
        {
            await _timerTask;
        }

        _lifetime.Dispose();
        await Visibility.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    protected override void OnParametersSet()
    {
        if (!ShouldBlink)
        {
            _showTurnTitle = false;
        }
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return Task.CompletedTask;
        }

        _initialization = this.InitializeAsync();
        return _initialization;
    }

    private static string Text(string key) => DraftRoomText.Get(key);

    private async Task InitializeAsync()
    {
        Visibility.Changed += this.OnVisibilityChanged;
        try
        {
            _enabled = await Storage.GetItemAsync<bool?>(PreferenceKey) ?? true;
            await Visibility.InitializeAsync();
            if (!_disposed)
            {
                _initialized = true;
                _timerTask = this.RunTimerAsync(_lifetime.Token);
            }
        }
        catch (JSException exception)
        {
            this.ReportUnavailable(exception);
        }
        catch (JsonException exception)
        {
            this.ReportUnavailable(exception);
        }

        if (!_disposed)
        {
            this.StateHasChanged();
        }
    }

    private void OnVisibilityChanged() => _ = this.InvokeAsync(() =>
    {
        if (!_disposed && !ShouldBlink)
        {
            _showTurnTitle = false;
        }

        if (!_disposed)
        {
            this.StateHasChanged();
        }
    });

    private async Task RunTimerAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1), Clock);
        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                await this.InvokeAsync(() =>
                {
                    if (_disposed)
                    {
                        return;
                    }

                    var next = ShouldBlink && !_showTurnTitle;
                    if (_showTurnTitle != next)
                    {
                        _showTurnTitle = next;
                        this.StateHasChanged();
                    }
                });
            }
        }
        catch (OperationCanceledException exception) when (cancellationToken.IsCancellationRequested)
        {
            Logger.LogDebug(exception, "Draft room turn alert timer stopped.");
        }
    }

    private async Task SetEnabledAsync(bool enabled)
    {
        try
        {
            await Storage.SetItemAsync(PreferenceKey, enabled);
            _enabled = enabled;
            if (!ShouldBlink)
            {
                _showTurnTitle = false;
            }
        }
        catch (JSException exception)
        {
            this.ReportUnavailable(exception);
        }
    }

    private void ReportUnavailable(Exception exception)
    {
        _unavailable = true;
        _showTurnTitle = false;
        Logger.LogWarning(exception, "Draft room turn alert could not initialize or save its preference.");
    }
}
