using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Tigl.Helpers;

public static class TiglFactionParser
{
    public static TiglFactionName ParseFaction(string faction)
    {
        return faction switch
        {
            "The Arborec" => TiglFactionName.TheArborec,
            "The Barony of Letnev" => TiglFactionName.TheBaronyOfLetnev,
            "The Clan of Saar" => TiglFactionName.TheClanOfSaar,
            "The Embers of Muaat" => TiglFactionName.TheEmbersOfMuaat,
            "The Emirates of Hacan" => TiglFactionName.TheEmiratesOfHacan,
            "The Federation of Sol" => TiglFactionName.TheFederationOfSol,
            "The Ghosts of Creuss" => TiglFactionName.TheGhostsOfCreuss,
            "The L1Z1X Mindnet" => TiglFactionName.TheL1z1xMindnet,
            "The Mentak Coalition" => TiglFactionName.TheMentakCoalition,
            "The Naalu Collective" => TiglFactionName.TheNaaluCollective,
            "The Nekro Virus" => TiglFactionName.TheNekroVirus,
            "Sardakk Norr" => TiglFactionName.SardakkNorr,
            "The Universities of Jol-Nar" => TiglFactionName.TheUniversitiesOfJolNar,
            "The Winnu" => TiglFactionName.TheWinnu,
            "The Xxcha Kingdom" => TiglFactionName.TheXxchaKingdom,
            "The Yin Brotherhood" => TiglFactionName.TheYinBrotherhood,
            "The Yssaril Tribes" => TiglFactionName.TheYssarilTribes,

            "The Argent Flight" => TiglFactionName.TheArgentFlight,
            "The Empyrean" => TiglFactionName.TheEmpyrean,
            "The Mahact Gene-Sorcerers" => TiglFactionName.TheMahactGeneSorcerers,
            "The Naaz-Rokha Alliance" => TiglFactionName.TheNaazRokhaAlliance,
            "The Nomad" => TiglFactionName.TheNomad,
            "The Titans of Ul" => TiglFactionName.TheTitansOfUl,
            "The Vuil'raith Cabal" => TiglFactionName.TheVuilRaithCabal,

            "Augers of Ilyxum" => TiglFactionName.TheAugursOfIlyxum,
            "Celdauri Trade Confederation" => TiglFactionName.TheCeldauriTradeConfederation,
            "Dih-Mohn Flotilla" => TiglFactionName.TheDihMohnFlotilla,
            "Florzen Profiteers" => TiglFactionName.TheFlorzenProfiteers,
            "Free Systems Compact" => TiglFactionName.TheFreeSystemsCompact,
            "Ghemina Raiders" => TiglFactionName.TheGheminaRaiders,
            "Glimmer of Mortheus" => TiglFactionName.TheGlimmerOfMortheus,
            "Kollecc Society" => TiglFactionName.TheKolleccSociety,
            "Kortali Tribunal" => TiglFactionName.TheKortaliTribunal,
            "Li-Zho Dynasty" => TiglFactionName.TheLiZhoDynasty,
            "L'Tokk Khrask" => TiglFactionName.TheLTokkKhrask,
            "Mirveda Protectorate" => TiglFactionName.TheMirvedaProtectorate,
            "Myko-Mentori" => TiglFactionName.TheMykoMentori,
            "Nivyn Star Kings" => TiglFactionName.TheNivynStarKings,
            "Olradin League" => TiglFactionName.TheOlradinLeague,
            "Roh'Dhna Mechatronics" => TiglFactionName.RohDhnaMechatronics,
            "Savages of Cymiae" => TiglFactionName.TheSavagesOfCymiae,
            "Shipwrights of Axis" => TiglFactionName.TheShipwrightsofAxis,
            "Tnelis Syndicate" => TiglFactionName.TheTnelisSyndicate,
            "Vaden Banking Clans" => TiglFactionName.TheVadenBankingClans,
            "Vaylerian Scourge" => TiglFactionName.TheVaylerianScourge,
            "Veldyr Sovereignty" => TiglFactionName.TheVeldyrSovereignty,
            "Zealots of Rhodun" => TiglFactionName.TheZealotsOfRhodun,
            "Zelian Purifier" => TiglFactionName.TheZelianPurifier,
            "Bentor Conglomerate" => TiglFactionName.TheBentorConglomerate,
            "Cheiran Hordes" => TiglFactionName.TheCheiranHordes,
            "Edyn Mandate" => TiglFactionName.TheEdynMandate,
            "Ghoti Wayfarers" => TiglFactionName.TheGhotiWayfarers,
            "Gledge Union" => TiglFactionName.TheGledgeUnion,
            "Berserkers of Kjalengard" => TiglFactionName.TheBerserkersOfKjalengard,
            "Monks of Kolume" => TiglFactionName.TheMonksOfKolume,
            "Kyro Sodality" => TiglFactionName.TheKyroSodality,
            "Lanefir Remnants" => TiglFactionName.TheLanefirRemnants,
            "Nokar Sellships" => TiglFactionName.TheNokarSellships,

            "The Council Keleres - Mentak" => TiglFactionName.TheCouncilKeleresMentak,
            "The Council Keleres - Xxcha" => TiglFactionName.TheCouncilKeleresXxcha,
            "The Council Keleres - Argent" => TiglFactionName.TheCouncilKeleresArgent,

            _ => throw new ArgumentOutOfRangeException(nameof(faction), $"Unknown faction: {faction}"),
        };
    }
}
