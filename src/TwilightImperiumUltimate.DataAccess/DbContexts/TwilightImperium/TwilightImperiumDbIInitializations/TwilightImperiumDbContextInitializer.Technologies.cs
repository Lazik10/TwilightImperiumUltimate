using TwilightImperiumUltimate.Core.Entities.Cards;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeTechnologies()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var technologies = new List<Technology>()
        {
            // Basic technologies
            new Technology()
            {
                TechnologyName = TechnologyName.Psychoarchaeology,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level0,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DacxiveAnimators,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.BioStims,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.HyperMetabolism,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level2,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.X89BacterialWeapon,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level3,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.X89BacterialWeaponOmega,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level3,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AntimassDeflectors,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level0,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DarkEnergyTap,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level0,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.GravityDrive,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level1,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SlingRelay,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level1,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.FleetLogistics,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.LightWaveDeflector,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level3,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SarweenTools,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level0,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.ScanlinkDroneNetwork,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level0,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.GravitonLaserSystem,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.PredictiveIntelligence,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.TransitDiodes,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.IntegratedEconomy,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level3,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.PlasmaScoring,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level0,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AIDevelopementAlgorithm,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level0,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.MagenDefenseGrid,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level1,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.MagenDefenseGridOmega,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level1,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SelfAssemblyRoutines,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level1,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DuraniumArmor,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level2,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AssaultCannon,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level3,
                GameVersion = GameVersion.BaseGame,
            },

            // Faction technologies
            new Technology()
            {
                TechnologyName = TechnologyName.Infantry,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SpecOps,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.LetaniWarrior,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.CrimsonLegionnaire,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Carrier,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AdvancedCarrier,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SpaceDock,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.FloatingFactory,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DimensionalTear,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Destroyer,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.StrikeWingAlpha,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Fighter,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.HybridCrystalFighter,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Pds,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.HelTitan,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Dreadonought,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level3,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SuperDreadnought,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level3,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Exotrireme,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Cruiser,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level3,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SaturnEngine,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level3,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Memoria,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level3,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.WarSun,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level4,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.PrototypeWarSun,
                Type = TechnologyType.None,
                Level = TechnologyLevel.Level3,
            },

            // Faction technologies
            new Technology()
            {
                TechnologyName = TechnologyName.Voidwatch,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.InstrinctTraining,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.TransparasteelPlating,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.GeneticRecombination,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Bioplasmosis,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.ProductionBiomes,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.YinSpinner,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.YinSpinnerOmega,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Neuroglaive,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level3,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.PreFabArcologies,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level3,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.MageonImplants,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level3,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.ChaosMapping,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SpatialConduitCylinder,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Aetherstream,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.WormholeGenerator,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.WormholeGeneratorOmega,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.LazaxGateFolding,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AerieHololattice,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.L4Disruptors,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.TemporalCommandSuite,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.IihqModernization,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SalvageOperations,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.InheritanceSystems,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.EResSiphons,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.HegemonicTradePolicy,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.NullificationField,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.CoreImpulse,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AgencySupplyNetwork,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.QuantumDatahubNode,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level3,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.MirrorComputing,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level3,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DimensionalSplicer,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Supercharge,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Vortex,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level1,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.NonEuclidianShielding,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.MagmusReactor,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.MagmusReactorOmega,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level2,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.WalkyrieParticleWeave,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level2,
            },
        };

        var updatedTechnologies = technologies.Select(technology =>
        {
            technology.Name = $"{technology.TechnologyName}_{nameof(Technology.TechnologyName)}";
            technology.Text = $"{technology.Text}_{nameof(ObjectiveCard.Text)}";
            technology.ImagePath = _cardImagePathService.GetCardImagePath(technology.TechnologyName, technology.Type);
            return technology;
        });

        dbContext.AddRange(updatedTechnologies);
        dbContext.SaveChanges();
    }
}
