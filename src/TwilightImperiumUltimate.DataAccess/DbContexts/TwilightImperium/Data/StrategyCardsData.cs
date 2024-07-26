namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class StrategyCardsData
{
    internal static List<StrategyCard> StrategyCards => new List<StrategyCard>
    {
        new() { EnumName = StrategyCardName.Leadership, InitiativeOrder = InitiativeOrder.One, GameVersion = GameVersion.BaseGame },
        new() { EnumName = StrategyCardName.Diplomacy, InitiativeOrder = InitiativeOrder.Two, GameVersion = GameVersion.Deprecated },
        new() { EnumName = StrategyCardName.Politics, InitiativeOrder = InitiativeOrder.Three, GameVersion = GameVersion.BaseGame },
        new() { EnumName = StrategyCardName.Construction, InitiativeOrder = InitiativeOrder.Four, GameVersion = GameVersion.Deprecated },
        new() { EnumName = StrategyCardName.Trade, InitiativeOrder = InitiativeOrder.Five, GameVersion = GameVersion.BaseGame },
        new() { EnumName = StrategyCardName.Warfare, InitiativeOrder = InitiativeOrder.Six, GameVersion = GameVersion.BaseGame },
        new() { EnumName = StrategyCardName.Technology, InitiativeOrder = InitiativeOrder.Seven, GameVersion = GameVersion.BaseGame },
        new() { EnumName = StrategyCardName.Imperial, InitiativeOrder = InitiativeOrder.Eight, GameVersion = GameVersion.BaseGame },

        // Updated in Prophecy of Kings
        new() { EnumName = StrategyCardName.DiplomacyCodexOrdinian, InitiativeOrder = InitiativeOrder.Two, GameVersion = GameVersion.CodexOrdinian },
        new() { EnumName = StrategyCardName.ConstructionProphecyOfKings, InitiativeOrder = InitiativeOrder.Four, GameVersion = GameVersion.ProphecyOfKings },
    };
}
