namespace TwilightImperiumUltimate.Web.Helpers.Factions;

/// <summary>
/// Resolves a free-form value (e.g. from the "?source=" query string or the
/// "/game/factions/{FactionOrSource}" route segment) to the matching <see cref="FactionSource"/>.
/// The exact enum name always matches, case-insensitively, regardless of the alias lists below.
/// Add short-hand aliases to the arrays as needed -- no other code needs to change to support a
/// new alias.
/// </summary>
public static class FactionSourceAliasResolver
{
    private static readonly Dictionary<FactionSource, string[]> Aliases = new()
    {
        [FactionSource.Official] = Array.Empty<string>(),
        [FactionSource.TwilightsFall] = Array.Empty<string>(),
        [FactionSource.DiscordantStars] = Array.Empty<string>(),
        [FactionSource.BlueRiverie] = Array.Empty<string>(),
        [FactionSource.WhispersFromTheVoid] = Array.Empty<string>(),
    };

    public static bool TryResolve(string? value, out FactionSource source)
    {
        source = FactionSource.Official;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (Enum.TryParse(value, ignoreCase: true, out source))
        {
            return true;
        }

        foreach (var (candidate, aliases) in Aliases)
        {
            if (Array.Exists(aliases, alias => string.Equals(alias, value, StringComparison.OrdinalIgnoreCase)))
            {
                source = candidate;
                return true;
            }
        }

        source = FactionSource.Official;
        return false;
    }
}
