namespace TwilightImperiumUltimate.Web.Helpers;

/// <summary>
/// Validates local URLs used as post-authentication destinations.
/// </summary>
public static class LocalUrlHelper
{
    private const int MaximumDecodePasses = 8;

    /// <summary>
    /// Determines whether a path is rooted on the current site and cannot be interpreted as an external URL.
    /// </summary>
    /// <param name="path">The candidate local path.</param>
    /// <returns><see langword="true"/> when the path is safe to use as a local navigation destination.</returns>
    public static bool IsLocalPath(string? path)
    {
        if (path is null || !IsValidLocalPath(path))
        {
            return false;
        }

        var decodedPath = path;
        for (var pass = 0; pass < MaximumDecodePasses; pass++)
        {
            var nextDecodedPath = Uri.UnescapeDataString(decodedPath);
            if (nextDecodedPath == decodedPath)
            {
                return true;
            }

            if (!IsValidLocalPath(nextDecodedPath))
            {
                return false;
            }

            decodedPath = nextDecodedPath;
        }

        return Uri.UnescapeDataString(decodedPath) == decodedPath;
    }

    private static bool IsValidLocalPath(string? path)
    {
        return !string.IsNullOrWhiteSpace(path)
            && path.StartsWith('/')
            && !path.StartsWith("//", StringComparison.Ordinal)
            && !path.StartsWith("/\\", StringComparison.Ordinal)
            && !path.Contains('\\', StringComparison.Ordinal)
            && !path.Contains(':', StringComparison.Ordinal)
            && !path.Any(char.IsControl);
    }
}
