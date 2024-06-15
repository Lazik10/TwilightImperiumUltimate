using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class TechnologiesData
{
    internal static List<Technology> Technologies => GetTechnologies();

    private static List<Technology> GetTechnologies()
    {
        CreateCardImagePathService createCardImagePathService = new();

        var technologies = new List<Technology>()
        {
            // Basic technologies
            new() { TechnologyName = TechnologyName.NeuralMotivator, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level0, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.Psychoarchaeology, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level0, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.ProphecyOfKing },
            new() { TechnologyName = TechnologyName.DacxiveAnimators, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level1, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.BioStims, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level1, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.ProphecyOfKing },
            new() { TechnologyName = TechnologyName.HyperMetabolism, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level2, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.X89BacterialWeaponOmega, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level3, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.AntimassDeflectors, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level0, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.DarkEnergyTap, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level0, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.ProphecyOfKing },
            new() { TechnologyName = TechnologyName.GravityDrive, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level1, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.SlingRelay, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level1, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.ProphecyOfKing },
            new() { TechnologyName = TechnologyName.FleetLogistics, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level2, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.LightWaveDeflector, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level3, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.SarweenTools, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level0, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.ScanlinkDroneNetwork, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level0, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.ProphecyOfKing },
            new() { TechnologyName = TechnologyName.GravitonLaserSystem, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level1, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.PredictiveIntelligence, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level1, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.ProphecyOfKing },
            new() { TechnologyName = TechnologyName.TransitDiodes, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level2, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.IntegratedEconomy, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level3, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.PlasmaScoring, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level0, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.AIDevelopmentAlgorithm, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level0, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.ProphecyOfKing },
            new() { TechnologyName = TechnologyName.MagenDefenseGridOmega, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level1, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.SelfAssemblyRoutines, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level1, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.ProphecyOfKing },
            new() { TechnologyName = TechnologyName.DuraniumArmor, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level2, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.AssaultCannon, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level3, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },

            // Unit Upgrade technologies
            new() { TechnologyName = TechnologyName.InfantryTwo, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.SpecOps, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheFederationOfSol, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.LetaniWarrior, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheArborec, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.CrimsonLegionnaire, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheMahactGeneSorcerers, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.FighterTwo, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.HybridCrystalFighter, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheNaaluCollective, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.DestroyerTwo, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.StrikeWingAlpha, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheArgentFlight, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.CarrierTwo, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.AdvancedCarrier, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheFederationOfSol, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.CruiserTwo, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level3, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.SaturnEngine, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level3, IsFactionTechnology = true, FactionName = FactionName.TheTitansOfUl, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.DreadnoughtTwo, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level3, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.SuperDreadnought, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level3, IsFactionTechnology = true, FactionName = FactionName.TheL1z1xMindnet, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.Exotrireme, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.SardakkNorr, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.Memoria, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level3, IsFactionTechnology = true, FactionName = FactionName.TheNomad, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.WarSun, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level4, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.PrototypeWarSun, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level4, IsFactionTechnology = true, FactionName = FactionName.TheEmbersOfMuaat, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.SpaceDockTwo, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.FloatingFactory, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheClanOfSaar, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.DimensionalTear, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheVuilRaithCabal, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.PdsTwo, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = false, FactionName = FactionName.None, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.HelTitan, Type = TechnologyType.UnitUpgrade, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheTitansOfUl, GameVersion = GameVersion.BaseGame },

            // Faction technologies
            new() { TechnologyName = TechnologyName.Voidwatch, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level1, IsFactionTechnology = true, FactionName = FactionName.TheEmpyrean, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.InstrinctTraining, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level1, IsFactionTechnology = true, FactionName = FactionName.TheXxchaKingdom, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.TransparasteelPlating, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level1, IsFactionTechnology = true, FactionName = FactionName.TheYssarilTribes, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.GeneticRecombination, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level1, IsFactionTechnology = true, FactionName = FactionName.TheMahactGeneSorcerers, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.Bioplasmosis, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheArborec, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.ProductionBiomes, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheEmiratesOfHacan, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.YinSpinnerOmega, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheYinBrotherhood, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.Neuroglaive, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level3, IsFactionTechnology = true, FactionName = FactionName.TheNaaluCollective, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.PreFabArcologies, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level3, IsFactionTechnology = true, FactionName = FactionName.TheNaazRokhaAlliance, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.MageonImplants, Type = TechnologyType.Biotic, Level = TechnologyLevel.Level3, IsFactionTechnology = true, FactionName = FactionName.TheYssarilTribes, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.ChaosMapping, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level1, IsFactionTechnology = true, FactionName = FactionName.TheClanOfSaar, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.SpacialConduitCylinder, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheUniversitiesOfJolNar, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.Aetherstream, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheEmpyrean, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.WormholeGeneratorOmega, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level2, IsFactionTechnology = true, FactionName = FactionName.TheGhostsOfCreuss, GameVersion = GameVersion.BaseGame },
            new() { TechnologyName = TechnologyName.LazaxGateFolding, Type = TechnologyType.Propulsion, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.AerieHololattice, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level1, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.L4Disruptors, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level1, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.TemporalCommandSuite, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level1, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.IihqModernization, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level1, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.SalvageOperations, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.InheritanceSystems, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.EResSiphons, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.HegemonicTradePolicy, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.NullificationField, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.ImpulseCore, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.AgencySupplyNetwork, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.QuantumDatahubNode, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level3, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.MirrorComputing, Type = TechnologyType.Cybernetic, Level = TechnologyLevel.Level3, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.DimensionalSplicer, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level1, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.Supercharge, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level1, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.Vortex, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level1, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.NonEuclidianShielding, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.MagmusReactorOmega, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
            new() { TechnologyName = TechnologyName.WalkyrieParticleWeave, Type = TechnologyType.Warfare, Level = TechnologyLevel.Level2, IsFactionTechnology = true },
        };

        var updatedTechnologies = technologies.Select(technology =>
        {
            technology.Name = $"{technology.TechnologyName}_{nameof(Technology.TechnologyName)}";
            technology.Text = $"{technology.Text}_{nameof(ObjectiveCard.Text)}";
            technology.ImagePath = createCardImagePathService.GetCardImagePath(technology.TechnologyName, technology.Type);
            return technology;
        }).ToList();

        return updatedTechnologies;
    }
}
