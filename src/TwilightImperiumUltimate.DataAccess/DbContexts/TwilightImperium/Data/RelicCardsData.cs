namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class RelicCardsData
{
    internal static List<RelicCard> RelicCards => new List<RelicCard>
    {
        new() { Id = 1, EnumName = RelicCardName.DominusOrb, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 2, EnumName = RelicCardName.MawOfWorlds, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 3, EnumName = RelicCardName.ScepterOfEmelpar, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 4, EnumName = RelicCardName.ShardOfTheThrone, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 5, EnumName = RelicCardName.StellarConverter, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 6, EnumName = RelicCardName.TheCodex, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 7, EnumName = RelicCardName.TheCrownOfEmphidia, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 8, EnumName = RelicCardName.TheCrownOfThalnos, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 9, EnumName = RelicCardName.TheObsidian, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 10, EnumName = RelicCardName.TheProphetsTears, GameVersion = GameVersion.ProphecyOfKings },

        new() { Id = 11, EnumName = RelicCardName.DynamisCore, GameVersion = GameVersion.CodexAffinity },
        new() { Id = 12, EnumName = RelicCardName.JRXS455OLostTitanPrototypeAgent, GameVersion = GameVersion.CodexAffinity },
        new() { Id = 13, EnumName = RelicCardName.NanoForge, GameVersion = GameVersion.CodexAffinity },

        new() { Id = 14, EnumName = RelicCardName.StarfallArray, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 15, EnumName = RelicCardName.AccretionEngine, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 16, EnumName = RelicCardName.EyeOfVogul, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 17, EnumName = RelicCardName.AzdelsKey, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 18, EnumName = RelicCardName.TwilightMirror, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 19, EnumName = RelicCardName.ESixGoNetwork, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 20, EnumName = RelicCardName.FotgottenThrone, GameVersion = GameVersion.UnchartedSpace },
    };
}
