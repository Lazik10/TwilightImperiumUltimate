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
                TechnologyName = TechnologyName.NeuralMotivator,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level0,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Psychoarchaeology,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level0,
                IsFactionTechnology = false,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DacxiveAnimators,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.BioStims,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = false,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.HyperMetabolism,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.X89BacterialWeaponOmega,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AntimassDeflectors,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level0,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DarkEnergyTap,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level0,
                IsFactionTechnology = false,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.GravityDrive,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SlingRelay,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = false,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.FleetLogistics,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.LightWaveDeflector,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SarweenTools,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level0,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.ScanlinkDroneNetwork,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level0,
                IsFactionTechnology = false,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.GravitonLaserSystem,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.PredictiveIntelligence,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = false,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.TransitDiodes,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.IntegratedEconomy,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.PlasmaScoring,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level0,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AIDevelopmentAlgorithm,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level0,
                IsFactionTechnology = false,
                GameVersion = GameVersion.ProphecyOfKing,
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
                IsFactionTechnology = false,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DuraniumArmor,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AssaultCannon,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = false,
                GameVersion = GameVersion.BaseGame,
            },

            // Unit Upgrade technologies
            new Technology()
            {
                TechnologyName = TechnologyName.InfantryTwo,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = false,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SpecOps,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.LetaniWarrior,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.CrimsonLegionnaire,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.FighterTwo,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = false,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.HybridCrystalFighter,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DestroyerTwo,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = false,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.StrikeWingAlpha,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.CarrierTwo,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = false,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AdvancedCarrier,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.CruiserTwo,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = false,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SaturnEngine,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DreadnoughtTwo,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = false,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SuperDreadnought,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Exotrireme,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Memoria,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.WarSun,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level4,
                IsFactionTechnology = false,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.PrototypeWarSun,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level4,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SpaceDockTwo,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = false,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.FloatingFactory,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DimensionalTear,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.PdsTwo,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = false,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.HelTitan,
                Type = TechnologyType.UnitUpgrade,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },

            // Faction technologies
            new Technology()
            {
                TechnologyName = TechnologyName.Voidwatch,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.InstrinctTraining,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.TransparasteelPlating,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.GeneticRecombination,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Bioplasmosis,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.ProductionBiomes,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.YinSpinnerOmega,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Neuroglaive,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.PreFabArcologies,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.MageonImplants,
                Type = TechnologyType.Biotic,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.ChaosMapping,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SpacialConduitCylinder,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Aetherstream,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.WormholeGeneratorOmega,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.LazaxGateFolding,
                Type = TechnologyType.Propulsion,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AerieHololattice,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.L4Disruptors,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.TemporalCommandSuite,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.IihqModernization,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.SalvageOperations,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.InheritanceSystems,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.EResSiphons,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.HegemonicTradePolicy,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.NullificationField,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.ImpulseCore,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.AgencySupplyNetwork,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.QuantumDatahubNode,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.MirrorComputing,
                Type = TechnologyType.Cybernetic,
                Level = TechnologyLevel.Level3,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.DimensionalSplicer,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Supercharge,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.Vortex,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level1,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.NonEuclidianShielding,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.MagmusReactorOmega,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
            },
            new Technology()
            {
                TechnologyName = TechnologyName.WalkyrieParticleWeave,
                Type = TechnologyType.Warfare,
                Level = TechnologyLevel.Level2,
                IsFactionTechnology = true,
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
