using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class PresenceTrackerTests : IDisposable
{
    private static readonly DateTimeOffset InitialTime = new(2026, 10, 16, 0, 0, 0, TimeSpan.Zero);

    private readonly FakeTimeProvider _timeProvider = new(InitialTime);
    private readonly PresenceTracker _presenceTracker;

    public PresenceTrackerTests()
    {
        this._presenceTracker = new PresenceTracker(this._timeProvider);
    }

    public void Dispose()
    {
        this._presenceTracker.Dispose();
    }

    [Fact]
    public void AddConnection_WhenParticipantGetsMultipleConnections_ShouldRaiseOnlineOnlyOnce()
    {
        var roomId = Guid.NewGuid();
        var participantId = Guid.NewGuid();
        var onlineChanges = new List<DraftRoomPresenceChange>();
        this._presenceTracker.ParticipantOnline += onlineChanges.Add;

        this._presenceTracker.AddConnection(roomId, participantId, "connection-1");
        this._presenceTracker.AddConnection(roomId, participantId, "connection-2");

        onlineChanges.Should().ContainSingle().Which.Should().Be(new DraftRoomPresenceChange(roomId, participantId));
        this._presenceTracker.IsOnline(roomId, participantId).Should().BeTrue();
    }

    [Fact]
    public void RemoveConnection_WhenOneOfMultipleConnectionsLeaves_ShouldRemainOnline()
    {
        var roomId = Guid.NewGuid();
        var participantId = Guid.NewGuid();
        var offlineChanges = new List<DraftRoomPresenceChange>();
        this._presenceTracker.ParticipantOffline += offlineChanges.Add;
        this._presenceTracker.AddConnection(roomId, participantId, "connection-1");
        this._presenceTracker.AddConnection(roomId, participantId, "connection-2");

        this._presenceTracker.RemoveConnection("connection-1");
        this._timeProvider.Advance(DraftRoomLimits.PresenceGracePeriod);

        this._presenceTracker.IsOnline(roomId, participantId).Should().BeTrue();
        offlineChanges.Should().BeEmpty();
    }

    [Fact]
    public void RemoveConnection_WhenLastConnectionLeaves_ShouldRaiseOfflineAfterGracePeriod()
    {
        var roomId = Guid.NewGuid();
        var participantId = Guid.NewGuid();
        var offlineChanges = new List<DraftRoomPresenceChange>();
        this._presenceTracker.ParticipantOffline += offlineChanges.Add;
        this._presenceTracker.AddConnection(roomId, participantId, "connection-1");

        this._presenceTracker.RemoveConnection("connection-1");
        this._timeProvider.Advance(DraftRoomLimits.PresenceGracePeriod - TimeSpan.FromMilliseconds(1));

        this._presenceTracker.IsOnline(roomId, participantId).Should().BeTrue();
        offlineChanges.Should().BeEmpty();

        this._timeProvider.Advance(TimeSpan.FromMilliseconds(1));

        this._presenceTracker.IsOnline(roomId, participantId).Should().BeFalse();
        offlineChanges.Should().ContainSingle().Which.Should().Be(new DraftRoomPresenceChange(roomId, participantId));
    }

    [Fact]
    public void AddConnection_WhenParticipantReconnectsDuringGracePeriod_ShouldCancelOfflineTransition()
    {
        var roomId = Guid.NewGuid();
        var participantId = Guid.NewGuid();
        var offlineChanges = new List<DraftRoomPresenceChange>();
        this._presenceTracker.ParticipantOffline += offlineChanges.Add;
        this._presenceTracker.AddConnection(roomId, participantId, "connection-1");
        this._presenceTracker.RemoveConnection("connection-1");
        this._timeProvider.Advance(TimeSpan.FromSeconds(20));

        this._presenceTracker.AddConnection(roomId, participantId, "connection-2");
        this._timeProvider.Advance(TimeSpan.FromSeconds(10));

        this._presenceTracker.IsOnline(roomId, participantId).Should().BeTrue();
        offlineChanges.Should().BeEmpty();

        this._presenceTracker.RemoveConnection("connection-2");
        this._timeProvider.Advance(DraftRoomLimits.PresenceGracePeriod);

        this._presenceTracker.IsOnline(roomId, participantId).Should().BeFalse();
        offlineChanges.Should().ContainSingle().Which.Should().Be(new DraftRoomPresenceChange(roomId, participantId));
    }

    [Fact]
    public void IsOnline_WhenSameParticipantIsTrackedInDifferentRooms_ShouldRemainRoomScoped()
    {
        var participantId = Guid.NewGuid();
        var firstRoomId = Guid.NewGuid();
        var secondRoomId = Guid.NewGuid();
        this._presenceTracker.AddConnection(firstRoomId, participantId, "connection-1");
        this._presenceTracker.AddConnection(secondRoomId, participantId, "connection-2");

        this._presenceTracker.RemoveConnection("connection-1");
        this._timeProvider.Advance(DraftRoomLimits.PresenceGracePeriod);

        this._presenceTracker.IsOnline(firstRoomId, participantId).Should().BeFalse();
        this._presenceTracker.IsOnline(secondRoomId, participantId).Should().BeTrue();
    }
}
