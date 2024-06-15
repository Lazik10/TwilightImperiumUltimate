using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Enums.Units;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class FactionUnitsData
{
    internal static List<FactionUnit> FactionUnits =>
    [
        new() { FactionName = FactionName.TheArborec, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheArborec, UnitName = UnitName.Pds, Count = 1 },
        new() { FactionName = FactionName.TheArborec, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheArborec, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheArborec, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheArborec, UnitName = UnitName.Cruiser, Count = 1 },

        new() { FactionName = FactionName.TheBaronyOfLetnev, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheBaronyOfLetnev, UnitName = UnitName.Infantry, Count = 3 },
        new() { FactionName = FactionName.TheBaronyOfLetnev, UnitName = UnitName.Fighter, Count = 1 },
        new() { FactionName = FactionName.TheBaronyOfLetnev, UnitName = UnitName.Destroyer, Count = 1 },
        new() { FactionName = FactionName.TheBaronyOfLetnev, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheBaronyOfLetnev, UnitName = UnitName.Dreadnought, Count = 1 },

        new() { FactionName = FactionName.TheClanOfSaar, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheClanOfSaar, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheClanOfSaar, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheClanOfSaar, UnitName = UnitName.Carrier, Count = 2 },
        new() { FactionName = FactionName.TheClanOfSaar, UnitName = UnitName.Cruiser, Count = 1 },

        new() { FactionName = FactionName.TheEmbersOfMuaat, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheEmbersOfMuaat, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheEmbersOfMuaat, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheEmbersOfMuaat, UnitName = UnitName.WarSun, Count = 1 },

        new() { FactionName = FactionName.TheEmiratesOfHacan, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheEmiratesOfHacan, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheEmiratesOfHacan, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheEmiratesOfHacan, UnitName = UnitName.Cruiser, Count = 1 },
        new() { FactionName = FactionName.TheEmiratesOfHacan, UnitName = UnitName.Carrier, Count = 2 },

        new() { FactionName = FactionName.TheFederationOfSol, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheFederationOfSol, UnitName = UnitName.Infantry, Count = 5 },
        new() { FactionName = FactionName.TheFederationOfSol, UnitName = UnitName.Fighter, Count = 3 },
        new() { FactionName = FactionName.TheFederationOfSol, UnitName = UnitName.Destroyer, Count = 1 },
        new() { FactionName = FactionName.TheFederationOfSol, UnitName = UnitName.Carrier, Count = 2 },

        new() { FactionName = FactionName.TheGhostsOfCreuss, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheGhostsOfCreuss, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheGhostsOfCreuss, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheGhostsOfCreuss, UnitName = UnitName.Destroyer, Count = 2 },
        new() { FactionName = FactionName.TheGhostsOfCreuss, UnitName = UnitName.Carrier, Count = 1 },

        new() { FactionName = FactionName.TheL1z1xMindnet, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheL1z1xMindnet, UnitName = UnitName.Pds, Count = 1 },
        new() { FactionName = FactionName.TheL1z1xMindnet, UnitName = UnitName.Infantry, Count = 5 },
        new() { FactionName = FactionName.TheL1z1xMindnet, UnitName = UnitName.Fighter, Count = 3 },
        new() { FactionName = FactionName.TheL1z1xMindnet, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheL1z1xMindnet, UnitName = UnitName.Dreadnought, Count = 1 },

        new() { FactionName = FactionName.TheMentakCoalition, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheMentakCoalition, UnitName = UnitName.Pds, Count = 1 },
        new() { FactionName = FactionName.TheMentakCoalition, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheMentakCoalition, UnitName = UnitName.Fighter, Count = 3 },
        new() { FactionName = FactionName.TheMentakCoalition, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheMentakCoalition, UnitName = UnitName.Cruiser, Count = 2 },

        new() { FactionName = FactionName.TheNaaluCollective, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheNaaluCollective, UnitName = UnitName.Pds, Count = 1 },
        new() { FactionName = FactionName.TheNaaluCollective, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheNaaluCollective, UnitName = UnitName.Fighter, Count = 3 },
        new() { FactionName = FactionName.TheNaaluCollective, UnitName = UnitName.Destroyer, Count = 1 },
        new() { FactionName = FactionName.TheNaaluCollective, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheNaaluCollective, UnitName = UnitName.Cruiser, Count = 1 },

        new() { FactionName = FactionName.TheNekroVirus, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheNekroVirus, UnitName = UnitName.Infantry, Count = 2 },
        new() { FactionName = FactionName.TheNekroVirus, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheNekroVirus, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheNekroVirus, UnitName = UnitName.Cruiser, Count = 1 },
        new() { FactionName = FactionName.TheNekroVirus, UnitName = UnitName.Dreadnought, Count = 1 },

        new() { FactionName = FactionName.SardakkNorr, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.SardakkNorr, UnitName = UnitName.Pds, Count = 1 },
        new() { FactionName = FactionName.SardakkNorr, UnitName = UnitName.Infantry, Count = 5 },
        new() { FactionName = FactionName.SardakkNorr, UnitName = UnitName.Carrier, Count = 2 },
        new() { FactionName = FactionName.SardakkNorr, UnitName = UnitName.Cruiser, Count = 1 },

        new() { FactionName = FactionName.TheUniversitiesOfJolNar, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheUniversitiesOfJolNar, UnitName = UnitName.Pds, Count = 2 },
        new() { FactionName = FactionName.TheUniversitiesOfJolNar, UnitName = UnitName.Infantry, Count = 2 },
        new() { FactionName = FactionName.TheUniversitiesOfJolNar, UnitName = UnitName.Fighter, Count = 1 },
        new() { FactionName = FactionName.TheUniversitiesOfJolNar, UnitName = UnitName.Carrier, Count = 2 },
        new() { FactionName = FactionName.TheUniversitiesOfJolNar, UnitName = UnitName.Dreadnought, Count = 1 },

        new() { FactionName = FactionName.TheWinnu, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheWinnu, UnitName = UnitName.Pds, Count = 1 },
        new() { FactionName = FactionName.TheWinnu, UnitName = UnitName.Infantry, Count = 2 },
        new() { FactionName = FactionName.TheWinnu, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheWinnu, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheWinnu, UnitName = UnitName.Cruiser, Count = 1 },

        new() { FactionName = FactionName.TheXxchaKingdom, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheXxchaKingdom, UnitName = UnitName.Pds, Count = 1 },
        new() { FactionName = FactionName.TheXxchaKingdom, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheXxchaKingdom, UnitName = UnitName.Fighter, Count = 3 },
        new() { FactionName = FactionName.TheXxchaKingdom, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheXxchaKingdom, UnitName = UnitName.Cruiser, Count = 2 },

        new() { FactionName = FactionName.TheYinBrotherhood, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheYinBrotherhood, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheYinBrotherhood, UnitName = UnitName.Fighter, Count = 4 },
        new() { FactionName = FactionName.TheYinBrotherhood, UnitName = UnitName.Destroyer, Count = 1 },
        new() { FactionName = FactionName.TheYinBrotherhood, UnitName = UnitName.Carrier, Count = 2 },

        new() { FactionName = FactionName.TheYssarilTribes, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheYssarilTribes, UnitName = UnitName.Pds, Count = 1 },
        new() { FactionName = FactionName.TheYssarilTribes, UnitName = UnitName.Infantry, Count = 5 },
        new() { FactionName = FactionName.TheYssarilTribes, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheYssarilTribes, UnitName = UnitName.Carrier, Count = 2 },
        new() { FactionName = FactionName.TheYssarilTribes, UnitName = UnitName.Cruiser, Count = 1 },

        new() { FactionName = FactionName.TheArgentFlight, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheArgentFlight, UnitName = UnitName.Pds, Count = 1 },
        new() { FactionName = FactionName.TheArgentFlight, UnitName = UnitName.Infantry, Count = 5 },
        new() { FactionName = FactionName.TheArgentFlight, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheArgentFlight, UnitName = UnitName.Destroyer, Count = 2 },
        new() { FactionName = FactionName.TheArgentFlight, UnitName = UnitName.Cruiser, Count = 1 },

        new() { FactionName = FactionName.TheEmpyrean, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheEmpyrean, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheEmpyrean, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheEmpyrean, UnitName = UnitName.Destroyer, Count = 1 },
        new() { FactionName = FactionName.TheEmpyrean, UnitName = UnitName.Carrier, Count = 2 },

        new() { FactionName = FactionName.TheMahactGeneSorcerers, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheMahactGeneSorcerers, UnitName = UnitName.Infantry, Count = 3 },
        new() { FactionName = FactionName.TheMahactGeneSorcerers, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheMahactGeneSorcerers, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheMahactGeneSorcerers, UnitName = UnitName.Cruiser, Count = 1 },
        new() { FactionName = FactionName.TheMahactGeneSorcerers, UnitName = UnitName.Dreadnought, Count = 1 },

        new() { FactionName = FactionName.TheNaazRokhaAlliance, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheNaazRokhaAlliance, UnitName = UnitName.Pds, Count = 1 },
        new() { FactionName = FactionName.TheNaazRokhaAlliance, UnitName = UnitName.Infantry, Count = 3 },
        new() { FactionName = FactionName.TheNaazRokhaAlliance, UnitName = UnitName.Mech, Count = 1 },
        new() { FactionName = FactionName.TheNaazRokhaAlliance, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheNaazRokhaAlliance, UnitName = UnitName.Destroyer, Count = 1 },
        new() { FactionName = FactionName.TheNaazRokhaAlliance, UnitName = UnitName.Carrier, Count = 2 },

        new() { FactionName = FactionName.TheNomad, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheNomad, UnitName = UnitName.Infantry, Count = 4 },
        new() { FactionName = FactionName.TheNomad, UnitName = UnitName.Fighter, Count = 3 },
        new() { FactionName = FactionName.TheNomad, UnitName = UnitName.Destroyer, Count = 1 },
        new() { FactionName = FactionName.TheNomad, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheNomad, UnitName = UnitName.Flagship, Count = 1 },

        new() { FactionName = FactionName.TheTitansOfUl, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheTitansOfUl, UnitName = UnitName.Infantry, Count = 3 },
        new() { FactionName = FactionName.TheTitansOfUl, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheTitansOfUl, UnitName = UnitName.Cruiser, Count = 2 },
        new() { FactionName = FactionName.TheTitansOfUl, UnitName = UnitName.Dreadnought, Count = 1 },

        new() { FactionName = FactionName.TheVuilRaithCabal, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheVuilRaithCabal, UnitName = UnitName.Infantry, Count = 3 },
        new() { FactionName = FactionName.TheVuilRaithCabal, UnitName = UnitName.Fighter, Count = 3 },
        new() { FactionName = FactionName.TheVuilRaithCabal, UnitName = UnitName.Carrier, Count = 1 },
        new() { FactionName = FactionName.TheVuilRaithCabal, UnitName = UnitName.Cruiser, Count = 1 },
        new() { FactionName = FactionName.TheVuilRaithCabal, UnitName = UnitName.Dreadnought, Count = 1 },

        new() { FactionName = FactionName.TheCouncilKeleres, UnitName = UnitName.SpaceDock, Count = 1 },
        new() { FactionName = FactionName.TheCouncilKeleres, UnitName = UnitName.Infantry, Count = 2 },
        new() { FactionName = FactionName.TheCouncilKeleres, UnitName = UnitName.Fighter, Count = 2 },
        new() { FactionName = FactionName.TheCouncilKeleres, UnitName = UnitName.Carrier, Count = 2 },
        new() { FactionName = FactionName.TheCouncilKeleres, UnitName = UnitName.Cruiser, Count = 1 },
    ];
}
