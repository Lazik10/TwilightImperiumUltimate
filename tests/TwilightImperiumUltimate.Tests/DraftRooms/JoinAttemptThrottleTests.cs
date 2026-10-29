using FluentAssertions;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class JoinAttemptThrottleTests
{
    private static readonly DateTimeOffset InitialTime = new(2026, 10, 16, 0, 0, 0, TimeSpan.Zero);

    private readonly TestTimeProvider timeProvider = new(InitialTime);
    private readonly JoinAttemptThrottle throttle;

    public JoinAttemptThrottleTests()
    {
        this.throttle = new JoinAttemptThrottle(this.timeProvider);
    }

    [Fact]
    public void IsBlocked_WhenFailureLimitIsReached_ShouldReturnTrue()
    {
        RegisterFailures(this.throttle, DraftRoomLimits.MaxJoinFailuresPerIpAndCode);

        this.throttle.IsBlocked("192.0.2.1", "ABC234").Should().BeTrue();
    }

    [Fact]
    public void IsBlocked_WhenBelowFailureLimit_ShouldReturnFalse()
    {
        RegisterFailures(this.throttle, DraftRoomLimits.MaxJoinFailuresPerIpAndCode - 1);

        this.throttle.IsBlocked("192.0.2.1", "ABC234").Should().BeFalse();
    }

    [Fact]
    public void IsBlocked_WhenFailureWindowHasElapsed_ShouldReturnFalse()
    {
        RegisterFailures(this.throttle, DraftRoomLimits.MaxJoinFailuresPerIpAndCode);
        this.timeProvider.Advance(DraftRoomLimits.JoinFailureWindow);

        this.throttle.IsBlocked("192.0.2.1", "ABC234").Should().BeFalse();
    }

    [Fact]
    public void RegisterSuccess_WhenFailuresExist_ShouldClearOnlyThatClientAndRoomCode()
    {
        RegisterFailures(this.throttle, DraftRoomLimits.MaxJoinFailuresPerIpAndCode);
        for (var index = 0; index < DraftRoomLimits.MaxJoinFailuresPerIpAndCode; index++)
        {
            this.throttle.RegisterFailure("192.0.2.1", "DEF567");
        }

        this.throttle.RegisterSuccess("192.0.2.1", "ABC234");

        this.throttle.IsBlocked("192.0.2.1", "ABC234").Should().BeFalse();
        this.throttle.IsBlocked("192.0.2.1", "DEF567").Should().BeTrue();
    }

    [Fact]
    public void IsBlocked_WhenOtherRoomCodeHasFailures_ShouldRemainUnblocked()
    {
        RegisterFailures(this.throttle, DraftRoomLimits.MaxJoinFailuresPerIpAndCode);

        this.throttle.IsBlocked("192.0.2.1", "DEF567").Should().BeFalse();
    }

    [Fact]
    public void IsBlocked_WhenOtherIpAddressHasFailures_ShouldRemainUnblocked()
    {
        RegisterFailures(this.throttle, DraftRoomLimits.MaxJoinFailuresPerIpAndCode);

        this.throttle.IsBlocked("192.0.2.2", "ABC234").Should().BeFalse();
    }

    [Fact]
    public void RegisterFailure_WhenOldFailuresExpire_ShouldOnlyCountFailuresInActiveWindow()
    {
        RegisterFailures(this.throttle, DraftRoomLimits.MaxJoinFailuresPerIpAndCode - 1);
        this.timeProvider.Advance(DraftRoomLimits.JoinFailureWindow);
        this.throttle.RegisterFailure("192.0.2.1", "ABC234");

        this.throttle.IsBlocked("192.0.2.1", "ABC234").Should().BeFalse();
    }

    [Fact]
    public void RegisterFailure_WhenCalledConcurrently_ShouldRecordEveryFailure()
    {
        Parallel.For(
            0,
            DraftRoomLimits.MaxJoinFailuresPerIpAndCode,
            _ => this.throttle.RegisterFailure("192.0.2.1", "ABC234"));

        this.throttle.IsBlocked("192.0.2.1", "ABC234").Should().BeTrue();
    }

    private static void RegisterFailures(JoinAttemptThrottle throttle, int count)
    {
        for (var index = 0; index < count; index++)
        {
            throttle.RegisterFailure("192.0.2.1", "ABC234");
        }
    }

    private sealed class TestTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        private DateTimeOffset _utcNow = utcNow;

        public override DateTimeOffset GetUtcNow()
        {
            return this._utcNow;
        }

        public void Advance(TimeSpan duration)
        {
            this._utcNow += duration;
        }
    }
}
