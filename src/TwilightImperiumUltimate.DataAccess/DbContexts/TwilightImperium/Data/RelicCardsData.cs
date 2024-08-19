namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class RelicCardsData
{
    internal static List<RelicCard> RelicCards => new List<RelicCard>
    {
        new() { EnumName = RelicCardName.DominusOrb, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = RelicCardName.MawOfWorlds, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = RelicCardName.ScepterOfEmelpar, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = RelicCardName.ShardOfTheThrone, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = RelicCardName.StellarConverter, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = RelicCardName.TheCodex, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = RelicCardName.TheCrownOfEmphidia, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = RelicCardName.TheCrownOfThalnos, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = RelicCardName.TheObsidian, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = RelicCardName.TheProphetsTears, GameVersion = GameVersion.ProphecyOfKings },

        new() { EnumName = RelicCardName.DynamisCore, GameVersion = GameVersion.CodexAffinity },
        new() { EnumName = RelicCardName.JRXS455OLostTitanPrototypeAgent, GameVersion = GameVersion.CodexAffinity },
        new() { EnumName = RelicCardName.NanoForge, GameVersion = GameVersion.CodexAffinity },

        new() { EnumName = RelicCardName.StarfallArray, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = RelicCardName.AccretionEngine, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = RelicCardName.EyeOfVogul, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = RelicCardName.AzdelsKey, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = RelicCardName.TwilightMirror, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = RelicCardName.ESixGoNetwork, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = RelicCardName.FotgottenThrone, GameVersion = GameVersion.UnchartedSpace },
    };
}
