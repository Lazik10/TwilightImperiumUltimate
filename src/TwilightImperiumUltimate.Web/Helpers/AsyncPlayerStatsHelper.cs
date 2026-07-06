using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;

namespace TwilightImperiumUltimate.Web.Helpers;

public static class AsyncPlayerStatsHelper
{
    public static float GetFloatValue(this AsyncPlayerFactionStatsByGameVp instance, Func<AsyncPlayerFactionStatsByGameVp, float> propertySelector)
    {
        ArgumentNullException.ThrowIfNull(propertySelector);

        return propertySelector(instance);
    }

    public static int GetIntValue(this AsyncPlayerFactionStatsByGameVp instance, Func<AsyncPlayerFactionStatsByGameVp, int> propertySelector)
    {
        ArgumentNullException.ThrowIfNull(propertySelector);

        return propertySelector(instance);
    }

    public static float GetFloatValue(this AsyncFactionStatsByGameVpDto instance, Func<AsyncFactionStatsByGameVpDto, float> propertySelector)
    {
        ArgumentNullException.ThrowIfNull(propertySelector);

        return propertySelector(instance);
    }

    public static int GetIntValue(this AsyncFactionStatsByGameVpDto instance, Func<AsyncFactionStatsByGameVpDto, int> propertySelector)
    {
        ArgumentNullException.ThrowIfNull(propertySelector);

        return propertySelector(instance);
    }
}
