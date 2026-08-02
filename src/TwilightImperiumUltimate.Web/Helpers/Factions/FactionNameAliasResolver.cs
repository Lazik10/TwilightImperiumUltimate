namespace TwilightImperiumUltimate.Web.Helpers.Factions;

/// <summary>
/// Resolves a free-form value (e.g. from the "?faction=" query string or the "/game/factions/{FactionOrSource}"
/// route segment, such as "arbo", "arborec", "TheArborec" or "thearborec") to the matching
/// <see cref="FactionName"/>. The exact enum name always matches, case-insensitively, regardless of
/// the alias lists below. Add short-hand aliases to the arrays as needed -- no other code needs to
/// change to support a new alias.
/// </summary>
public static class FactionNameAliasResolver
{
    private static readonly Dictionary<FactionName, string[]> Aliases = new()
    {
        // Base game - 17 factions
        [FactionName.TheArborec] = Array.Empty<string>(),
        [FactionName.TheBaronyOfLetnev] = Array.Empty<string>(),
        [FactionName.TheClanOfSaar] = Array.Empty<string>(),
        [FactionName.TheEmbersOfMuaat] = Array.Empty<string>(),
        [FactionName.TheEmiratesOfHacan] = Array.Empty<string>(),
        [FactionName.TheFederationOfSol] = Array.Empty<string>(),
        [FactionName.TheGhostsOfCreuss] = Array.Empty<string>(),
        [FactionName.TheL1z1xMindnet] = Array.Empty<string>(),
        [FactionName.TheMentakCoalition] = Array.Empty<string>(),
        [FactionName.TheNaaluCollective] = Array.Empty<string>(),
        [FactionName.TheNekroVirus] = Array.Empty<string>(),
        [FactionName.SardakkNorr] = Array.Empty<string>(),
        [FactionName.TheUniversitiesOfJolNar] = Array.Empty<string>(),
        [FactionName.TheWinnu] = Array.Empty<string>(),
        [FactionName.TheXxchaKingdom] = Array.Empty<string>(),
        [FactionName.TheYinBrotherhood] = Array.Empty<string>(),
        [FactionName.TheYssarilTribes] = Array.Empty<string>(),

        // Prophecy of Kings - 7 factions
        [FactionName.TheArgentFlight] = Array.Empty<string>(),
        [FactionName.TheEmpyrean] = Array.Empty<string>(),
        [FactionName.TheMahactGeneSorcerers] = Array.Empty<string>(),
        [FactionName.TheNaazRokhaAlliance] = Array.Empty<string>(),
        [FactionName.TheNomad] = Array.Empty<string>(),
        [FactionName.TheTitansOfUl] = Array.Empty<string>(),
        [FactionName.TheVuilRaithCabal] = Array.Empty<string>(),

        // Codex Vigil - 1 faction
        [FactionName.TheCouncilKeleres] = Array.Empty<string>(),

        // Thunder's Edge - 5 factions
        [FactionName.TheCrimsonRebellion] = Array.Empty<string>(),
        [FactionName.TheDeepwroughtScholarate] = Array.Empty<string>(),
        [FactionName.TheFirmamentTheObsidian] = Array.Empty<string>(),
        [FactionName.LastBastion] = Array.Empty<string>(),
        [FactionName.TheRalNelConsortium] = Array.Empty<string>(),

        // Discordant Stars - 34 factions
        [FactionName.TheAugursOfIlyxum] = Array.Empty<string>(),
        [FactionName.TheCeldauriTradeConfederation] = Array.Empty<string>(),
        [FactionName.TheDihMohnFlotilla] = Array.Empty<string>(),
        [FactionName.TheFlorzenProfiteers] = Array.Empty<string>(),
        [FactionName.TheFreeSystemsCompact] = Array.Empty<string>(),
        [FactionName.TheGheminaRaiders] = Array.Empty<string>(),
        [FactionName.TheGlimmerOfMortheus] = Array.Empty<string>(),
        [FactionName.TheKolleccSociety] = Array.Empty<string>(),
        [FactionName.TheKortaliTribunal] = Array.Empty<string>(),
        [FactionName.TheLiZhoDynasty] = Array.Empty<string>(),
        [FactionName.TheLTokkKhrask] = Array.Empty<string>(),
        [FactionName.TheMirvedaProtectorate] = Array.Empty<string>(),
        [FactionName.TheMykoMentori] = Array.Empty<string>(),
        [FactionName.TheNivynStarKings] = Array.Empty<string>(),
        [FactionName.TheOlradinLeague] = Array.Empty<string>(),
        [FactionName.RohDhnaMechatronics] = Array.Empty<string>(),
        [FactionName.TheSavagesOfCymiae] = Array.Empty<string>(),
        [FactionName.TheShipwrightsofAxis] = Array.Empty<string>(),
        [FactionName.TheTnelisSyndicate] = Array.Empty<string>(),
        [FactionName.TheVadenBankingClans] = Array.Empty<string>(),
        [FactionName.TheVaylerianScourge] = Array.Empty<string>(),
        [FactionName.TheVeldyrSovereignty] = Array.Empty<string>(),
        [FactionName.TheZealotsOfRhodun] = Array.Empty<string>(),
        [FactionName.TheZelianPurifier] = Array.Empty<string>(),
        [FactionName.TheBentorConglomerate] = Array.Empty<string>(),
        [FactionName.TheCheiranHordes] = Array.Empty<string>(),
        [FactionName.TheEdynMandate] = Array.Empty<string>(),
        [FactionName.TheGhotiWayfarers] = Array.Empty<string>(),
        [FactionName.TheGledgeUnion] = Array.Empty<string>(),
        [FactionName.TheBerserkersOfKjalengard] = Array.Empty<string>(),
        [FactionName.TheMonksOfKolume] = Array.Empty<string>(),
        [FactionName.TheKyroSodality] = Array.Empty<string>(),
        [FactionName.TheLanefirRemnants] = Array.Empty<string>(),
        [FactionName.TheNokarSellships] = Array.Empty<string>(),

        // Blue Riverie - 9 factions
        [FactionName.AtokeraLegacy] = Array.Empty<string>(),
        [FactionName.BelkoseaAlliedStates] = Array.Empty<string>(),
        [FactionName.PrincipalityOfKaltrim] = Array.Empty<string>(),
        [FactionName.PharadnOrder] = Array.Empty<string>(),
        [FactionName.QhetRepublic] = Array.Empty<string>(),
        [FactionName.SarcosaDrift] = Array.Empty<string>(),
        [FactionName.ToldarConcordat] = Array.Empty<string>(),
        [FactionName.UydaiConclave] = Array.Empty<string>(),
        [FactionName.XinCourt] = Array.Empty<string>(),

        // Twilight's Fall - 8 factions
        [FactionName.TheRubyMonarch] = Array.Empty<string>(),
        [FactionName.RadiantAur] = Array.Empty<string>(),
        [FactionName.AvariceRex] = Array.Empty<string>(),
        [FactionName.IlSaiLakoeHeraldOfThorns] = Array.Empty<string>(),
        [FactionName.TheSaintOfSwords] = Array.Empty<string>(),
        [FactionName.IlNaViroset] = Array.Empty<string>(),
        [FactionName.ElNenJanovet] = Array.Empty<string>(),
        [FactionName.ASickeningLurch] = Array.Empty<string>(),
    };

    public static bool TryResolve(string? value, out FactionName factionName)
    {
        factionName = FactionName.None;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (Enum.TryParse(value, ignoreCase: true, out factionName) && factionName != FactionName.None)
        {
            return true;
        }

        foreach (var (candidate, aliases) in Aliases)
        {
            if (Array.Exists(aliases, alias => string.Equals(alias, value, StringComparison.OrdinalIgnoreCase)))
            {
                factionName = candidate;
                return true;
            }
        }

        factionName = FactionName.None;
        return false;
    }
}
