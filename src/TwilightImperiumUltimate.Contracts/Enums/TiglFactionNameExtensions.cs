namespace TwilightImperiumUltimate.Contracts.Enums;

public static class TiglFactionNameExtensions
{
    /// <summary>
    /// Returns the identity used to count a faction prestige rank toward Thunder's Edge progression.
    /// </summary>
    public static TiglFactionName GetPrestigeCountIdentity(this TiglFactionName faction) => faction switch
    {
        TiglFactionName.TheCouncilKeleresArgent
            or TiglFactionName.TheCouncilKeleresMentak
            or TiglFactionName.TheCouncilKeleresXxcha => TiglFactionName.TheCouncilKeleresArgent,
        _ => faction,
    };
}
