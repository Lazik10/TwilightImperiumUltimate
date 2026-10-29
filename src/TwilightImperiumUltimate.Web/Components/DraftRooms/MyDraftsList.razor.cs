using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.DraftRooms;
using PageRoutes = TwilightImperiumUltimate.Web.Pages.Pages;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class MyDraftsList : IDisposable
{
    private readonly CancellationTokenSource _lifetime = new();
    private IReadOnlyList<DraftResultHistoryItemDto> _items = [];
    private DraftResultHistoryItemDto? _deleteTarget;
    private int _page = 1;
    private int _loadVersion;
    private bool _hasMore;
    private bool _loading = true;
    private bool _loadError;
    private bool _deleteError;
    private bool _deleting;
    private bool _disposed;
    private bool _focusSection;
    private ElementReference _section;

    [Inject]
    private ICurrentUserState CurrentUserState { get; set; } = default!;

    [Inject]
    private IDraftRoomApiService Api { get; set; } = default!;

    [Inject]
    private ILogger<MyDraftsList> Logger { get; set; } = default!;

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
            CurrentUserState.UserChanged -= this.OnUserChanged;
            _lifetime.Cancel();
            _lifetime.Dispose();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await CurrentUserState.InitializeAsync();
        CurrentUserState.UserChanged += this.OnUserChanged;
        await this.LoadAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_focusSection && !_loading && CurrentUserState.User is not null)
        {
            _focusSection = false;
            await _section.FocusAsync();
        }
    }

    private static string Text(string key) => DraftRoomText.Get(key);

    private static string Format(string key, object value) => string.Format(CultureInfo.CurrentCulture, Text(key), value);

    private static string ResultUrl(DraftResultHistoryItemDto result) => $"{PageRoutes.DraftRoomResultPrefix}{Uri.EscapeDataString(result.ShareToken)}";

    private Task PreviousAsync() => this.ChangePageAsync(_page - 1);

    private Task NextAsync() => this.ChangePageAsync(_page + 1);

    private async Task ChangePageAsync(int page)
    {
        _page = page;
        _focusSection = true;
        await this.LoadAsync();
    }

    private void ShowDelete(DraftResultHistoryItemDto result) => _deleteTarget = result;

    private void CancelDelete() => _deleteTarget = null;

    private void OnUserChanged() => _ = this.InvokeAsync(async () =>
    {
        _page = 1;
        _deleteTarget = null;
        await this.LoadAsync();
        if (!_disposed)
        {
            this.StateHasChanged();
        }
    });

    private async Task LoadAsync()
    {
        var version = ++_loadVersion;
        _items = [];
        _loadError = false;
        _deleteError = false;
        _loading = true;
        try
        {
            if (CurrentUserState.User is null)
            {
                return;
            }

            var response = await Api.GetMyResultsAsync(_page, _lifetime.Token);
            if (_disposed || version != _loadVersion)
            {
                return;
            }

            _loadError = !response.IsSuccess || response.Data is null;
            if (response.Data is not null && response.IsSuccess)
            {
                _items = response.Data.Items;
                _hasMore = response.Data.HasMore;
            }
        }
        catch (OperationCanceledException exception) when (_disposed)
        {
            Logger.LogDebug(exception, "Draft result history load was cancelled during disposal");
        }
        catch (JSException exception)
        {
            Logger.LogWarning(exception, "Draft result history browser access failed");
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
        var target = _deleteTarget;
        _deleteTarget = null;
        if (_deleting || target is null || !target.CanDelete)
        {
            return;
        }

        var userId = CurrentUserState.User?.Id;
        _deleting = true;
        _deleteError = false;
        try
        {
            var response = await Api.DeleteResultAsync(target.Id, cancellationToken: _lifetime.Token);
            if (_disposed || CurrentUserState.User?.Id != userId)
            {
                return;
            }

            _deleteError = !response.IsSuccess;
            if (response.IsSuccess)
            {
                if (_items.Count == 1 && _page > 1)
                {
                    _page--;
                }

                _focusSection = true;
                await this.LoadAsync();
            }
        }
        catch (OperationCanceledException exception) when (_disposed)
        {
            Logger.LogDebug(exception, "Draft result history deletion was cancelled during disposal");
        }
        catch (JSException exception)
        {
            Logger.LogWarning(exception, "Draft result history deletion browser access failed");
            _deleteError = true;
        }
        finally
        {
            _deleting = false;
        }
    }
}
