using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.Business.Helpers;

public static class TiglGalacticEventConverter
{
    private static readonly Dictionary<string, GalacticEvent> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Age of Commerce", GalacticEvent.AgeOfCommerce },
        { "Call of the Void", GalacticEvent.CallOfTheVoid },
        { "Dangerous Wilds", GalacticEvent.DangerousWilds },
        { "Hidden Agenda", GalacticEvent.HiddenAgenda },
        { "Age of Exploration", GalacticEvent.AgeOfExploration },
        { "Civilized Society", GalacticEvent.CivilizedSociety },
        { "Minor Factions", GalacticEvent.MinorFactions },
        { "Stellar Atomics", GalacticEvent.StellarAtomics },
        { "Age of Fighters", GalacticEvent.AgeOfFighters },
        { "Total War", GalacticEvent.TotalWar},
        { "Wild Wild Galaxy", GalacticEvent.WildWildGalaxy },
        { "Weird Wormholes", GalacticEvent.WeirdWormholes },
        { "Monuments to the Ages", GalacticEvent.MonumentsToTheAges },
        { "Cosmic Phenomenae", GalacticEvent.CosmicPhenomenae },
        { "Cultural Exchange Program", GalacticEvent.CulturalExchangeProgram },
        { "Mercenaries for Hire", GalacticEvent.MercenariesForHire },
        { "Rapid Mobilization", GalacticEvent.RapidMobilization },
        { "Zealous Orthodoxy", GalacticEvent.ZealousOrthodoxy },
        { "Advent of the War Sun", GalacticEvent.AdventOfTheWarSun },
        { "Conventions of War Abandoned", GalacticEvent.ConventionsOfWarAbandoned },
    };

    public static long ConvertToFlags(IEnumerable<string> eventNames)
    {
        GalacticEvent result = GalacticEvent.None;

        foreach (var name in eventNames)
        {
            if (_map.TryGetValue(name, out var flag))
            {
                result |= flag;
            }
        }

        return (long)result;
    }

    public static IReadOnlyCollection<string> ConvertFromFlag(long eventValue)
    {
        var flags = (GalacticEvent)eventValue;
        return _map
            .Where(kvp => flags.HasFlag(kvp.Value))
            .Select(kvp => kvp.Key)
            .ToList();
    }
}
