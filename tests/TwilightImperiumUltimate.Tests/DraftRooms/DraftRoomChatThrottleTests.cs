using FluentAssertions;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomChatThrottleTests
{
    private static readonly DateTimeOffset InitialTime = new(2026, 10, 16, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public void TryAcquire_WhenParticipantReachesLimit_ShouldRejectAdditionalMessages()
    {
        var throttle = new DraftRoomChatThrottle(new TestTimeProvider(InitialTime));
        var participantId = Guid.NewGuid();

        var allowedMessages = Enumerable.Range(0, DraftRoomLimits.MaxChatMessagesPerWindow)
            .Count(_ => throttle.TryAcquire(participantId));

        allowedMessages.Should().Be(DraftRoomLimits.MaxChatMessagesPerWindow);
        throttle.TryAcquire(participantId).Should().BeFalse();
    }

    [Fact]
    public void TryAcquire_WhenWindowExpires_ShouldAllowMessagesAgain()
    {
        var timeProvider = new TestTimeProvider(InitialTime);
        var throttle = new DraftRoomChatThrottle(timeProvider);
        var participantId = Guid.NewGuid();
        for (var index = 0; index < DraftRoomLimits.MaxChatMessagesPerWindow; index++)
        {
            throttle.TryAcquire(participantId).Should().BeTrue();
        }

        throttle.TryAcquire(participantId).Should().BeFalse();
        timeProvider.Advance(DraftRoomLimits.ChatMessageWindow);

        throttle.TryAcquire(participantId).Should().BeTrue();
    }

    [Fact]
    public void TryAcquire_WhenParticipantsDiffer_ShouldTrackLimitsIndependently()
    {
        var throttle = new DraftRoomChatThrottle(new TestTimeProvider(InitialTime));
        var firstParticipantId = Guid.NewGuid();
        var secondParticipantId = Guid.NewGuid();
        for (var index = 0; index < DraftRoomLimits.MaxChatMessagesPerWindow; index++)
        {
            throttle.TryAcquire(firstParticipantId).Should().BeTrue();
        }

        throttle.TryAcquire(firstParticipantId).Should().BeFalse();
        throttle.TryAcquire(secondParticipantId).Should().BeTrue();
    }

    [Fact]
    public void TryAcquire_WhenCalledConcurrently_ShouldEnforceLimitAtomically()
    {
        var throttle = new DraftRoomChatThrottle(new TestTimeProvider(InitialTime));
        var participantId = Guid.NewGuid();
        var allowedMessages = 0;

        Parallel.For(
            0,
            DraftRoomLimits.MaxChatMessagesPerWindow * 4,
            _ =>
            {
                if (throttle.TryAcquire(participantId))
                {
                    Interlocked.Increment(ref allowedMessages);
                }
            });

        allowedMessages.Should().Be(DraftRoomLimits.MaxChatMessagesPerWindow);
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
