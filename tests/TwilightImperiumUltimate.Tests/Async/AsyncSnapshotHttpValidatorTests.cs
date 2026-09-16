using FluentAssertions;
using TwilightImperiumUltimate.Core.Helpers;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Async;

public sealed class AsyncSnapshotHttpValidatorTests
{
    [Fact]
    public void IsNotModified_WhenEtagMatches_ShouldReturnTrue()
    {
        var result = AsyncSnapshotHttpValidator.IsNotModified("\"42\"", null, "\"42\"", DateTimeOffset.UtcNow);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsNotModified_WhenEtagDoesNotMatch_ShouldIgnoreMatchingModifiedSince()
    {
        var lastModified = new DateTimeOffset(2026, 9, 16, 12, 0, 0, TimeSpan.Zero);

        var result = AsyncSnapshotHttpValidator.IsNotModified("\"41\"", lastModified.AddMinutes(1), "\"42\"", lastModified);

        result.Should().BeFalse();
    }

    [Fact]
    public void IsNotModified_WhenNoEtagAndModifiedSinceIsCurrent_ShouldReturnTrue()
    {
        var lastModified = new DateTimeOffset(2026, 9, 16, 12, 0, 0, 123, TimeSpan.Zero);

        var result = AsyncSnapshotHttpValidator.IsNotModified(null, lastModified, "\"42\"", lastModified);

        result.Should().BeTrue();
    }
}
