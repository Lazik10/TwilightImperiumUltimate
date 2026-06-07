namespace TwilightImperiumUltimate.Web.Services.Rules;

public interface IRulesCatalogCache<T>
{
    T GetOrCreate(string culture, long dependencyVersion, Func<T> factory);

    Task<T> GetOrCreateAsync(string culture, long dependencyVersion, Func<Task<T>> factory);

    void Invalidate();
}
