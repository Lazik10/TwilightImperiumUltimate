using TwilightImperiumUltimate.Contracts.DraftRooms;

namespace TwilightImperiumUltimate.IntegrationTests.Hosting;

internal sealed class TestPresenceTimeProvider : TimeProvider
{
    private readonly TimeProvider _systemTimeProvider = TimeProvider.System;

    public override ITimer CreateTimer(
        TimerCallback callback,
        object? state,
        TimeSpan dueTime,
        TimeSpan period)
    {
        var testDueTime = dueTime == DraftRoomLimits.PresenceGracePeriod
            ? TimeSpan.FromSeconds(1)
            : dueTime;
        return this._systemTimeProvider.CreateTimer(callback, state, testDueTime, period);
    }
}
