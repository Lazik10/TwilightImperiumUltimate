namespace TwilightImperiumUltimate.Web.Services.Rules;

public sealed class RulesCatalogCache<T> : IRulesCatalogCache<T>
{
    private readonly object _sync = new();
    private readonly Dictionary<string, CacheEntry> _entries = [];

    public T GetOrCreate(string culture, long dependencyVersion, Func<T> factory) =>
        GetOrCreateAsync(culture, dependencyVersion, () => Task.FromResult(factory()))
            .GetAwaiter()
            .GetResult();

    public async Task<T> GetOrCreateAsync(
        string culture,
        long dependencyVersion,
        Func<Task<T>> factory)
    {
        CacheEntry entry;

        lock (_sync)
        {
            if (_entries.TryGetValue(culture, out var existing)
                && existing.DependencyVersion == dependencyVersion)
            {
                entry = existing;
            }
            else
            {
                entry = new CacheEntry(
                    dependencyVersion,
                    new Lazy<Task<T>>(factory, LazyThreadSafetyMode.ExecutionAndPublication));
                _entries[culture] = entry;
            }
        }

        try
        {
            return await entry.Value.Value;
        }
        catch
        {
            RemoveIfCurrent(culture, entry);
            throw;
        }
    }

    public void Invalidate()
    {
        lock (_sync)
            _entries.Clear();
    }

    private void RemoveIfCurrent(string culture, CacheEntry entry)
    {
        lock (_sync)
        {
            if (_entries.TryGetValue(culture, out var current) && ReferenceEquals(current, entry))
                _entries.Remove(culture);
        }
    }

    private sealed record CacheEntry(long DependencyVersion, Lazy<Task<T>> Value);
}
