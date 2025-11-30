namespace TwilightImperiumUltimate.Contracts.Enums;

[Flags]
public enum GalacticEvent : long
{
    None = 0,
    AgeOfCommerce = 1L << 1,
    CallOfTheVoid = 1L << 2,
    DangerousWilds = 1L << 3,
    HiddenAgenda = 1L << 4,
    AgeOfExploration = 1L << 5,
    CivilizedSociety = 1L << 6,
    MinorFactions = 1L << 7,
    StellarAtomics = 1L << 8,
    AgeOfFighters = 1L << 9,
    TotalWar = 1L << 10,
    WildWildGalaxy = 1L << 11,
    WeirdWormholes = 1L << 12,
    MonumentsToTheAges = 1L << 13,
    CosmicPhenomenae = 1L << 14,
    CulturalExchangeProgram = 1L << 15,
    MercenariesForHire = 1L << 16,
    RapidMobilization = 1L << 17,
    ZealousOrthodoxy = 1L << 18,
    AdventOfTheWarSun = 1L << 19,
    ConventionsOfWarAbandoned = 1L << 20,
}
