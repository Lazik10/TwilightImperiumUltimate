namespace TwilightImperiumUltimate.Web.Helpers.Factions;

public static class FactionSourceAliasResolver
{
    private static readonly Dictionary<FactionSource, string[]> Aliases = new()
    {
        [FactionSource.Official] = ["official", "basegame", "base", "core"],
        [FactionSource.TwilightsFall] = ["twilightsfall", "tf"],
        [FactionSource.DiscordantStars] = ["discordantstars", "ds"],
        [FactionSource.BlueRiverie] = ["blueriverie", "br"],
        [FactionSource.WhispersFromTheVoid] = ["whispersfromthevoid", "wftv"],
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
