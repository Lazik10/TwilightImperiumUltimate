namespace TwilightImperiumUltimate.Web.Enums;

/// <summary>
/// Groups <see cref="FactionName"/> values by the expansion/fan-content pack they were published
/// in. This is independent of <see cref="GameVersion"/>, which only covers officially released
/// game data and has no members for fan expansions such as Blue Riverie or Twilight's Fall.
/// </summary>
public enum FactionSource
{
    Official,
    TwilightsFall,
    DiscordantStars,
    BlueRiverie,
    WhispersFromTheVoid,
}
