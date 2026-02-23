namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class StrategyCardsData
{
    internal static List<StrategyCard> StrategyCards => new List<StrategyCard>
    {
        new() { Id = 1, EnumName = StrategyCardName.Leadership, InitiativeOrder = InitiativeOrder.First, GameVersion = GameVersion.BaseGame },
        new() { Id = 2, EnumName = StrategyCardName.Diplomacy, InitiativeOrder = InitiativeOrder.Second, GameVersion = GameVersion.Deprecated },
        new() { Id = 3, EnumName = StrategyCardName.Politics, InitiativeOrder = InitiativeOrder.Third, GameVersion = GameVersion.BaseGame },
        new() { Id = 4, EnumName = StrategyCardName.Construction, InitiativeOrder = InitiativeOrder.Fourth, GameVersion = GameVersion.Deprecated },
        new() { Id = 5, EnumName = StrategyCardName.Trade, InitiativeOrder = InitiativeOrder.Fifth, GameVersion = GameVersion.BaseGame },
        new() { Id = 6, EnumName = StrategyCardName.Warfare, InitiativeOrder = InitiativeOrder.Sixth, GameVersion = GameVersion.Deprecated },
        new() { Id = 7, EnumName = StrategyCardName.Technology, InitiativeOrder = InitiativeOrder.Seventh, GameVersion = GameVersion.BaseGame },
        new() { Id = 8, EnumName = StrategyCardName.Imperial, InitiativeOrder = InitiativeOrder.Eighth, GameVersion = GameVersion.BaseGame },

        // Updated in Prophecy of Kings
        new() { Id = 9, EnumName = StrategyCardName.DiplomacyCodexOrdinian, InitiativeOrder = InitiativeOrder.Second, GameVersion = GameVersion.CodexOrdinian },
        new() { Id = 10, EnumName = StrategyCardName.ConstructionProphecyOfKings, InitiativeOrder = InitiativeOrder.Fourth, GameVersion = GameVersion.Deprecated },

        // Updated in Thunders Edge
        new() { Id = 11, EnumName = StrategyCardName.ConstructionThundersEdge, InitiativeOrder = InitiativeOrder.Fourth, GameVersion = GameVersion.ThundersEdge },
        new() { Id = 12, EnumName = StrategyCardName.WarfareThundersEdge, InitiativeOrder = InitiativeOrder.Sixth, GameVersion = GameVersion.ThundersEdge },
    };
}
