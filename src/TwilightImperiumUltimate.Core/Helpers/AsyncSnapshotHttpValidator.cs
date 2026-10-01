namespace TwilightImperiumUltimate.Core.Helpers;

public static class AsyncSnapshotHttpValidator
{
    public static bool IsNotModified(
        string? ifNoneMatch,
        DateTimeOffset? ifModifiedSince,
        string etag,
        DateTimeOffset lastModified)
    {
        var hasIfNoneMatch = !string.IsNullOrWhiteSpace(ifNoneMatch);
        var hasMatchingEtag = hasIfNoneMatch && ifNoneMatch!
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Any(candidate => candidate.Trim() is "*" || string.Equals(candidate.Trim(), etag, StringComparison.Ordinal));
        var normalizedLastModified = lastModified.AddTicks(-(lastModified.Ticks % TimeSpan.TicksPerSecond));

        return hasMatchingEtag
            || (!hasIfNoneMatch && ifModifiedSince is not null && ifModifiedSince >= normalizedLastModified);
    }
}
