namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class FrontierCardsData
{
    internal static List<FrontierCard> FrontierCards => new List<FrontierCard>
    {
        new() { EnumName = FrontierCardName.DeadWorld, GameVersion = GameVersion.CodexVigil },
        new() { EnumName = FrontierCardName.DerelictVessel, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = FrontierCardName.EnigmaticDevice, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = FrontierCardName.EntropicField, GameVersion = GameVersion.CodexVigil },
        new() { EnumName = FrontierCardName.GammaRelay, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = FrontierCardName.IonStorm, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = FrontierCardName.KeleresShip, GameVersion = GameVersion.CodexVigil },
        new() { EnumName = FrontierCardName.LostCrew, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = FrontierCardName.MajorEntropicField, GameVersion = GameVersion.CodexVigil },
        new() { EnumName = FrontierCardName.MerchantStation, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = FrontierCardName.MinorEntropicField, GameVersion = GameVersion.CodexVigil },
        new() { EnumName = FrontierCardName.Mirage, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = FrontierCardName.UnknownRelicFragment, GameVersion = GameVersion.ProphecyOfKings },

        new() { EnumName = FrontierCardName.DarkVisions, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = FrontierCardName.FoldedSpace, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = FrontierCardName.StarChart, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = FrontierCardName.SuspiciousWreckage, GameVersion = GameVersion.UnchartedSpace },
    };
}
