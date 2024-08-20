namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class StrategyCardsData
{
    internal static List<StrategyCard> StrategyCards => new List<StrategyCard>
    {
        new() { Id = 1, EnumName = StrategyCardName.Leadership, InitiativeOrder = InitiativeOrder.One, GameVersion = GameVersion.BaseGame },
        new() { Id = 2, EnumName = StrategyCardName.Diplomacy, InitiativeOrder = InitiativeOrder.Two, GameVersion = GameVersion.Deprecated },
        new() { Id = 3, EnumName = StrategyCardName.Politics, InitiativeOrder = InitiativeOrder.Three, GameVersion = GameVersion.BaseGame },
        new() { Id = 4, EnumName = StrategyCardName.Construction, InitiativeOrder = InitiativeOrder.Four, GameVersion = GameVersion.Deprecated },
        new() { Id = 5, EnumName = StrategyCardName.Trade, InitiativeOrder = InitiativeOrder.Five, GameVersion = GameVersion.BaseGame },
        new() { Id = 6, EnumName = StrategyCardName.Warfare, InitiativeOrder = InitiativeOrder.Six, GameVersion = GameVersion.BaseGame },
        new() { Id = 7, EnumName = StrategyCardName.Technology, InitiativeOrder = InitiativeOrder.Seven, GameVersion = GameVersion.BaseGame },
        new() { Id = 8, EnumName = StrategyCardName.Imperial, InitiativeOrder = InitiativeOrder.Eight, GameVersion = GameVersion.BaseGame },

        // Updated in Prophecy of Kings
        new() { Id = 9, EnumName = StrategyCardName.DiplomacyCodexOrdinian, InitiativeOrder = InitiativeOrder.Two, GameVersion = GameVersion.CodexOrdinian },
        new() { Id = 10, EnumName = StrategyCardName.ConstructionProphecyOfKings, InitiativeOrder = InitiativeOrder.Four, GameVersion = GameVersion.ProphecyOfKings },
    };
}
