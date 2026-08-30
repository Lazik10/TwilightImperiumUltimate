namespace TwilightImperiumUltimate.Web.Services.Cache;

/// <summary>
/// In-memory client-side cache keyed by <typeparamref name="TKey"/> with a fixed expiry per entry,
/// for reference data that is fetched per some selector (e.g. cards fetched per card type) and can
/// change server-side occasionally, so entries shouldn't be kept forever like <see cref="SingleLoadCache{T}"/>.
/// Keep one <c>static readonly</c> instance per resource on the consuming component.
/// </summary>
public sealed class TimedKeyedCache<TKey, TValue>(TimeSpan duration)
    where TKey : notnull
{
    private readonly Dictionary<TKey, (TValue Value, DateTimeOffset ExpiresAt)> _entries = [];

    private readonly SemaphoreSlim _lock = new(1, 1);

    private readonly TimeSpan _duration = duration;

    public async Task<TValue> GetOrLoadAsync(TKey key, Func<Task<TValue>> loader)
    {
        await _lock.WaitAsync();
        try
        {
            if (_entries.TryGetValue(key, out var entry) && entry.ExpiresAt > DateTimeOffset.UtcNow)
                return entry.Value;

            var value = await loader();
            _entries[key] = (value, DateTimeOffset.UtcNow + _duration);
            return value;
        }
        finally
        {
            _lock.Release();
        }
    }
}
