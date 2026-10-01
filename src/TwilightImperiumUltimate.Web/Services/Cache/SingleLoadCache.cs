namespace TwilightImperiumUltimate.Web.Services.Cache;

/// <summary>
/// In-memory, load-once client-side cache for reference data that never changes for the lifetime
/// of the app (technologies, system tiles, planets, ...). Keep one <c>static readonly</c> instance
/// per resource on the consuming component so the underlying API call only ever happens once per
/// browser session, no matter how many times the component is re-initialized (e.g. navigating away
/// and back re-runs <c>OnInitializedAsync</c> every time otherwise).
/// </summary>
public sealed class SingleLoadCache<T>
{
    private readonly SemaphoreSlim _lock = new(1, 1);

    private IReadOnlyList<T>? _items;

    public async Task<IReadOnlyList<T>> GetOrLoadAsync(Func<Task<IReadOnlyList<T>>> loader)
    {
        if (_items is not null)
            return _items;

        await _lock.WaitAsync();
        try
        {
            if (_items is not null)
                return _items;

            _items = await loader();
            return _items;
        }
        finally
        {
            _lock.Release();
        }
    }
}
