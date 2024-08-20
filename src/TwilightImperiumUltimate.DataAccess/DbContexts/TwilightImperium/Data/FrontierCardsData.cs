namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class FrontierCardsData
{
    internal static List<FrontierCard> FrontierCards => new List<FrontierCard>
    {
        new() { Id = 1, EnumName = FrontierCardName.DeadWorld, GameVersion = GameVersion.CodexVigil },
        new() { Id = 2, EnumName = FrontierCardName.DerelictVessel, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 3, EnumName = FrontierCardName.EnigmaticDevice, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 4, EnumName = FrontierCardName.EntropicField, GameVersion = GameVersion.CodexVigil },
        new() { Id = 5, EnumName = FrontierCardName.GammaRelay, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 6, EnumName = FrontierCardName.IonStorm, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 7, EnumName = FrontierCardName.KeleresShip, GameVersion = GameVersion.CodexVigil },
        new() { Id = 8, EnumName = FrontierCardName.LostCrew, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 9, EnumName = FrontierCardName.MajorEntropicField, GameVersion = GameVersion.CodexVigil },
        new() { Id = 10, EnumName = FrontierCardName.MerchantStation, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 11, EnumName = FrontierCardName.MinorEntropicField, GameVersion = GameVersion.CodexVigil },
        new() { Id = 12, EnumName = FrontierCardName.Mirage, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 13, EnumName = FrontierCardName.UnknownRelicFragment, GameVersion = GameVersion.ProphecyOfKings },

        new() { Id = 14, EnumName = FrontierCardName.DarkVisions, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 15, EnumName = FrontierCardName.FoldedSpace, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 16, EnumName = FrontierCardName.StarChart, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 17, EnumName = FrontierCardName.SuspiciousWreckage, GameVersion = GameVersion.UnchartedSpace },
    };
}
