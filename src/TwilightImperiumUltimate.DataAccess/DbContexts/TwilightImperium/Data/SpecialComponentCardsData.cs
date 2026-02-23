namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class SpecialComponentCardsData
{
    internal static List<SpecialComponentCard> SpecialComponentCards => new List<SpecialComponentCard>
    {
        // The Arborec
        new SpecialComponentCard() { Id = 1, EnumName = SpecialComponentName.LetaniWarrior, FactionName = FactionName.TheArborec, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },

        // The Clan of Saar
        new SpecialComponentCard() { Id = 2, EnumName = SpecialComponentName.FloatingFactory, FactionName = FactionName.TheClanOfSaar, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },

        // The Embers of Muaat
        new SpecialComponentCard() { Id = 3, EnumName = SpecialComponentName.PrototypeWarSun, FactionName = FactionName.TheEmbersOfMuaat, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },
        new SpecialComponentCard() { Id = 4, EnumName = SpecialComponentName.AvernusCard, FactionName = FactionName.TheEmbersOfMuaat, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },
        new SpecialComponentCard() { Id = 5, EnumName = SpecialComponentName.AvernusCardAbility, FactionName = FactionName.TheEmbersOfMuaat, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },
        new SpecialComponentCard() { Id = 6, EnumName = SpecialComponentName.AvernusToken, FactionName = FactionName.TheEmbersOfMuaat, Count = 1, SpecialType = SpecialComponentType.PlanetToken, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },

        // The Federation of Sol
        new SpecialComponentCard() { Id = 7, EnumName = SpecialComponentName.AdvancedCarrier, FactionName = FactionName.TheFederationOfSol, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },
        new SpecialComponentCard() { Id = 8, EnumName = SpecialComponentName.SpecOps, FactionName = FactionName.TheFederationOfSol, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },

        // The Ghosts of Creuss
        new SpecialComponentCard() { Id = 9, EnumName = SpecialComponentName.AlphaCreussToken, FactionName = FactionName.TheGhostsOfCreuss, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },
        new SpecialComponentCard() { Id = 10, EnumName = SpecialComponentName.BetaCreussToken, FactionName = FactionName.TheGhostsOfCreuss, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },
        new SpecialComponentCard() { Id = 11, EnumName = SpecialComponentName.GammaCreussToken, FactionName = FactionName.TheGhostsOfCreuss, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },

        // The Mentak Coalition
        new SpecialComponentCard() { Id = 12, EnumName = SpecialComponentName.Corsair, FactionName = FactionName.TheMentakCoalition, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },

        // The Naalu Collective
        new SpecialComponentCard() { Id = 13, EnumName = SpecialComponentName.HybridCrystalFighter, FactionName = FactionName.TheNaaluCollective, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },
        new SpecialComponentCard() { Id = 14, EnumName = SpecialComponentName.NaaluZeroToken, FactionName = FactionName.TheNaaluCollective, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },

        // The L1Z!X Mindnet
        new SpecialComponentCard() { Id = 15, EnumName = SpecialComponentName.SuperDreadnought, FactionName = FactionName.TheL1z1xMindnet, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },

        // The Nekro Virus
        new SpecialComponentCard() { Id = 16, EnumName = SpecialComponentName.AssimilationTokenX, FactionName = FactionName.TheNekroVirus, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },
        new SpecialComponentCard() { Id = 17, EnumName = SpecialComponentName.AssimilationTokenY, FactionName = FactionName.TheNekroVirus, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },
        new SpecialComponentCard() { Id = 18, EnumName = SpecialComponentName.AssimilationTokenZ, FactionName = FactionName.TheNekroVirus, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },
        new SpecialComponentCard() { Id = 19, EnumName = SpecialComponentName.DimensionalTearTokenNekro, FactionName = FactionName.TheNekroVirus, Count = 3, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },
        new SpecialComponentCard() { Id = 20, EnumName = SpecialComponentName.HeliosTokenNekro, FactionName = FactionName.TheNekroVirus, Count = 3, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },

        // Sardakk N'orr
        new SpecialComponentCard() { Id = 21, EnumName = SpecialComponentName.Exotrireme, FactionName = FactionName.SardakkNorr, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.BaseGame },

        // The Argent Flight
        new SpecialComponentCard() { Id = 22, EnumName = SpecialComponentName.StrikeWingAlpha, FactionName = FactionName.TheArgentFlight, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },

        // The Empyrean
        new SpecialComponentCard() { Id = 23, EnumName = SpecialComponentName.VoidTetherToken, FactionName = FactionName.TheEmpyrean, Count = 2, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },

        // The Mahact Gene-Sorcerers
        new SpecialComponentCard() { Id = 24, EnumName = SpecialComponentName.CrimsonLegionnaire, FactionName = FactionName.TheMahactGeneSorcerers, Count = 2, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },

        // The Naaz-Rokha Alliance
        new SpecialComponentCard() { Id = 25, EnumName = SpecialComponentName.EidolonMaximum, FactionName = FactionName.TheNaazRokhaAlliance, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },

        // The Titans of Ul
        new SpecialComponentCard() { Id = 26, EnumName = SpecialComponentName.HellTitan, FactionName = FactionName.TheTitansOfUl, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },
        new SpecialComponentCard() { Id = 27, EnumName = SpecialComponentName.SaturnEngine, FactionName = FactionName.TheTitansOfUl, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },
        new SpecialComponentCard() { Id = 28, EnumName = SpecialComponentName.SleeperToken, FactionName = FactionName.TheTitansOfUl, Count = 5, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },
        new SpecialComponentCard() { Id = 29, EnumName = SpecialComponentName.TerraformToken, FactionName = FactionName.TheTitansOfUl, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },
        new SpecialComponentCard() { Id = 30, EnumName = SpecialComponentName.GeoformToken, FactionName = FactionName.TheTitansOfUl, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },

        // The Vuil'Raith Cabal
        new SpecialComponentCard() { Id = 31, EnumName = SpecialComponentName.DimensionalTear, FactionName = FactionName.TheVuilRaithCabal, Count = 3, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },
        new SpecialComponentCard() { Id = 32, EnumName = SpecialComponentName.DimensionalTearToken, FactionName = FactionName.TheVuilRaithCabal, Count = 3, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ProphecyOfKings },

        // The Council Keleres
        new SpecialComponentCard() { Id = 33, EnumName = SpecialComponentName.CustodiaVigiliaCard, FactionName = FactionName.TheCouncilKeleres, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.CodexVigil },
        new SpecialComponentCard() { Id = 34, EnumName = SpecialComponentName.CustodiaVigiliaCardAbility, FactionName = FactionName.TheCouncilKeleres, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.CodexVigil },
        new SpecialComponentCard() { Id = 35, EnumName = SpecialComponentName.CustodiaVigiliaToken, FactionName = FactionName.TheCouncilKeleres, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.CodexVigil },

        // The Crimson Rebellion
        new SpecialComponentCard() { Id = 36, EnumName = SpecialComponentName.Exile, FactionName = FactionName.TheCrimsonRebellion, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 37, EnumName = SpecialComponentName.BreachToken, FactionName = FactionName.TheCrimsonRebellion, Count = 7, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 38, EnumName = SpecialComponentName.SeverToken, FactionName = FactionName.TheCrimsonRebellion, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },

        // The Deepwrought Scholarate
        new SpecialComponentCard() { Id = 39, EnumName = SpecialComponentName.DeepAbyss, FactionName = FactionName.TheDeepwroughtScholarate, Count = 1, SpecialType = SpecialComponentType.Ocean, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 40, EnumName = SpecialComponentName.BrinePool, FactionName = FactionName.TheDeepwroughtScholarate, Count = 1, SpecialType = SpecialComponentType.Ocean, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 41, EnumName = SpecialComponentName.CoralReef, FactionName = FactionName.TheDeepwroughtScholarate, Count = 1, SpecialType = SpecialComponentType.Ocean, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 42, EnumName = SpecialComponentName.IceShelf, FactionName = FactionName.TheDeepwroughtScholarate, Count = 1, SpecialType = SpecialComponentType.Ocean, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 43, EnumName = SpecialComponentName.LostFleet, FactionName = FactionName.TheDeepwroughtScholarate, Count = 1, SpecialType = SpecialComponentType.Ocean, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },

        // The Firmament / The Obsidian
        new SpecialComponentCard() { Id = 44, EnumName = SpecialComponentName.EnervatePlot, FactionName = FactionName.TheFirmamentTheObsidian, Count = 1, SpecialType = SpecialComponentType.Plot, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 45, EnumName = SpecialComponentName.SyphonPlot, FactionName = FactionName.TheFirmamentTheObsidian, Count = 1, SpecialType = SpecialComponentType.Plot, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 46, EnumName = SpecialComponentName.SeethePlot, FactionName = FactionName.TheFirmamentTheObsidian, Count = 1, SpecialType = SpecialComponentType.Plot, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 47, EnumName = SpecialComponentName.AssailPlot, FactionName = FactionName.TheFirmamentTheObsidian, Count = 1, SpecialType = SpecialComponentType.Plot, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 48, EnumName = SpecialComponentName.ExtractPlot, FactionName = FactionName.TheFirmamentTheObsidian, Count = 1, SpecialType = SpecialComponentType.Plot, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },

        // Last Bastion
        new SpecialComponentCard() { Id = 49, EnumName = SpecialComponentName.FourXFourOneCHeliosSix, FactionName = FactionName.LastBastion, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 50, EnumName = SpecialComponentName.OrdinianCard, FactionName = FactionName.LastBastion, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 51, EnumName = SpecialComponentName.OrdinianCardAbility, FactionName = FactionName.LastBastion, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 52, EnumName = SpecialComponentName.HeliosOneCard, FactionName = FactionName.LastBastion, Count = 3, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 53, EnumName = SpecialComponentName.HeliosTwoCard, FactionName = FactionName.LastBastion, Count = 3, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 54, EnumName = SpecialComponentName.HeliosThreeCard, FactionName = FactionName.LastBastion, Count = 3, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 55, EnumName = SpecialComponentName.HeliosToken, FactionName = FactionName.LastBastion, Count = 3, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },
        new SpecialComponentCard() { Id = 56, EnumName = SpecialComponentName.GalvanizeToken, FactionName = FactionName.LastBastion, Count = 7, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },

        // The Ral-Nel Consortium
        new SpecialComponentCard() { Id = 57, EnumName = SpecialComponentName.Linkship, FactionName = FactionName.TheRalNelConsortium, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.ThundersEdge },

        // The Celdauri Trade Confederation
        new SpecialComponentCard() { Id = 58, EnumName = SpecialComponentName.TradePort, FactionName = FactionName.TheCeldauriTradeConfederation, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 59, EnumName = SpecialComponentName.CeldauriDockToken, FactionName = FactionName.TheCeldauriTradeConfederation, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 60, EnumName = SpecialComponentName.CelagromToken, FactionName = FactionName.TheCeldauriTradeConfederation, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Dih-Mohn Flotilla
        new SpecialComponentCard() { Id = 61, EnumName = SpecialComponentName.Aegis, FactionName = FactionName.TheDihMohnFlotilla, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Florzen Profiteers
        new SpecialComponentCard() { Id = 62, EnumName = SpecialComponentName.CorsairFlorzen, FactionName = FactionName.TheFlorzenProfiteers, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Free Systems Compact
        new SpecialComponentCard() { Id = 63, EnumName = SpecialComponentName.HeartOfRebellionToken, FactionName = FactionName.TheFreeSystemsCompact, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Ghemina Raiders
        new SpecialComponentCard() { Id = 64, EnumName = SpecialComponentName.CombatTransport, FactionName = FactionName.TheGheminaRaiders, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 65, EnumName = SpecialComponentName.GeminaRaidersAdditionalFlagshipToken, FactionName = FactionName.TheGheminaRaiders, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Kortali Tribunal
        new SpecialComponentCard() { Id = 66, EnumName = SpecialComponentName.Tribune, FactionName = FactionName.TheKortaliTribunal, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Li-Zho Dynasty
        new SpecialComponentCard() { Id = 67, EnumName = SpecialComponentName.HeavyBomber, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 68, EnumName = SpecialComponentName.TrapInterferenceGrid, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 69, EnumName = SpecialComponentName.TrapMinefields, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 70, EnumName = SpecialComponentName.TrapAccountSiphon, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 71, EnumName = SpecialComponentName.TrapGraviticInhibitor, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 72, EnumName = SpecialComponentName.TrapFeint, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 73, EnumName = SpecialComponentName.TrapSaboteurs, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 74, EnumName = SpecialComponentName.CardInterferenceGrid, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 75, EnumName = SpecialComponentName.CardMinefields, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 76, EnumName = SpecialComponentName.CardAccountSiphon, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 77, EnumName = SpecialComponentName.CardGraviticInhibitor, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 78, EnumName = SpecialComponentName.CardFeint, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 79, EnumName = SpecialComponentName.CardSaboteurs, FactionName = FactionName.TheLiZhoDynasty, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The L'tokk Khrask
        new SpecialComponentCard() { Id = 80, EnumName = SpecialComponentName.ShatteredSky, FactionName = FactionName.TheLTokkKhrask, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 81, EnumName = SpecialComponentName.Grove, FactionName = FactionName.TheLTokkKhrask, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Mirveda Protectorate
        new SpecialComponentCard() { Id = 82, EnumName = SpecialComponentName.GaussCannon, FactionName = FactionName.TheMirvedaProtectorate, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Myko-Mentori
        new SpecialComponentCard() { Id = 83, EnumName = SpecialComponentName.MyceliumRing, FactionName = FactionName.TheMykoMentori, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 84, EnumName = SpecialComponentName.MykoMentoriCommodityTwo, FactionName = FactionName.TheMykoMentori, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 85, EnumName = SpecialComponentName.MykoMentoriCommodityThree, FactionName = FactionName.TheMykoMentori, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 86, EnumName = SpecialComponentName.MykoMentoriCommodityFour, FactionName = FactionName.TheMykoMentori, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Nivyn Star Kings
        new SpecialComponentCard() { Id = 87, EnumName = SpecialComponentName.WoundToken, FactionName = FactionName.TheNivynStarKings, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Olradin League
        new SpecialComponentCard() { Id = 88, EnumName = SpecialComponentName.OlradinMechBack, FactionName = FactionName.TheOlradinLeague, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 89, EnumName = SpecialComponentName.PolicyThePeopleConnect, FactionName = FactionName.TheOlradinLeague, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 90, EnumName = SpecialComponentName.PolicyThePeopleControl, FactionName = FactionName.TheOlradinLeague, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 91, EnumName = SpecialComponentName.PolicyTheEnvironmentPreserve, FactionName = FactionName.TheOlradinLeague, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 92, EnumName = SpecialComponentName.PolicyTheEnvironmentPlunder, FactionName = FactionName.TheOlradinLeague, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 93, EnumName = SpecialComponentName.PolicyTheEconomyEmpower, FactionName = FactionName.TheOlradinLeague, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 94, EnumName = SpecialComponentName.PolicyTheEconomyExploit, FactionName = FactionName.TheOlradinLeague, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 95, EnumName = SpecialComponentName.OlradinLeagueCommodityTwo, FactionName = FactionName.TheOlradinLeague, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // Roh Dhna Mechatronics
        new SpecialComponentCard() { Id = 96, EnumName = SpecialComponentName.Terrafactory, FactionName = FactionName.RohDhnaMechatronics, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 97, EnumName = SpecialComponentName.OmniForgeWorld, FactionName = FactionName.RohDhnaMechatronics, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 98, EnumName = SpecialComponentName.AutomatonsToken, FactionName = FactionName.RohDhnaMechatronics, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // Tha Savages of Cymiae
        new SpecialComponentCard() { Id = 99, EnumName = SpecialComponentName.UnholyAbomination, FactionName = FactionName.TheSavagesOfCymiae, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Shipwrights of Axis
        new SpecialComponentCard() { Id = 100, EnumName = SpecialComponentName.AxisOrderDestroyer, FactionName = FactionName.TheShipwrightsofAxis, Count = 2, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 101, EnumName = SpecialComponentName.AxisOrderCruiser, FactionName = FactionName.TheShipwrightsofAxis, Count = 2, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 102, EnumName = SpecialComponentName.AxisOrderCarrier, FactionName = FactionName.TheShipwrightsofAxis, Count = 2, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 103, EnumName = SpecialComponentName.AxisOrderDreadnought, FactionName = FactionName.TheShipwrightsofAxis, Count = 2, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Tnelis Syndicate
        new SpecialComponentCard() { Id = 104, EnumName = SpecialComponentName.BlockadeRunner, FactionName = FactionName.TheTnelisSyndicate, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Vaylerian Scourge
        new SpecialComponentCard() { Id = 105, EnumName = SpecialComponentName.Raider, FactionName = FactionName.TheVaylerianScourge, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Veldyr Sovereignty
        new SpecialComponentCard() { Id = 106, EnumName = SpecialComponentName.LancerDreadnought, FactionName = FactionName.TheVeldyrSovereignty, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 107, EnumName = SpecialComponentName.BranchOfficeInfluenceToken, FactionName = FactionName.TheVeldyrSovereignty, Count = 2, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 108, EnumName = SpecialComponentName.BranchOfficeResourcesToken, FactionName = FactionName.TheVeldyrSovereignty, Count = 2, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 109, EnumName = SpecialComponentName.AuroraResearchBaseToken, FactionName = FactionName.TheVeldyrSovereignty, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Zelian Purifier
        new SpecialComponentCard() { Id = 110, EnumName = SpecialComponentName.Impactor, FactionName = FactionName.TheZelianPurifier, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Bentor Conglomerate
        new SpecialComponentCard() { Id = 111, EnumName = SpecialComponentName.CulturalFragmentToken, FactionName = FactionName.TheBentorConglomerate, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 112, EnumName = SpecialComponentName.IndustrialFragmentToken, FactionName = FactionName.TheBentorConglomerate, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 113, EnumName = SpecialComponentName.HazardousFragmentToken, FactionName = FactionName.TheBentorConglomerate, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 114, EnumName = SpecialComponentName.UnknownFragmentToken, FactionName = FactionName.TheBentorConglomerate, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 115, EnumName = SpecialComponentName.EncryptionKeyToken, FactionName = FactionName.TheBentorConglomerate, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 116, EnumName = SpecialComponentName.CmoRancToken, FactionName = FactionName.TheBentorConglomerate, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Cheiran Hordes
        new SpecialComponentCard() { Id = 117, EnumName = SpecialComponentName.ChitinHulk, FactionName = FactionName.TheCheiranHordes, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 118, EnumName = SpecialComponentName.CheiranAdditionalDreadnoughtToken, FactionName = FactionName.TheCheiranHordes, Count = 2, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 119, EnumName = SpecialComponentName.CheiranAdditionalMechToken, FactionName = FactionName.TheCheiranHordes, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Edyn Mandate
        new SpecialComponentCard() { Id = 120, EnumName = SpecialComponentName.EdynSigilToken, FactionName = FactionName.TheEdynMandate, Count = 4, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Ghoti Wayfarers
        new SpecialComponentCard() { Id = 121, EnumName = SpecialComponentName.GhotiCard, FactionName = FactionName.TheGhotiWayfarers, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 122, EnumName = SpecialComponentName.GhotiCardAbility, FactionName = FactionName.TheGhotiWayfarers, Count = 1, SpecialType = SpecialComponentType.HorizontalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The GLEdge Union
        new SpecialComponentCard() { Id = 123, EnumName = SpecialComponentName.OrionPlatform, FactionName = FactionName.TheGledgeUnion, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 124, EnumName = SpecialComponentName.GledgeDhonrazOne, FactionName = FactionName.TheGledgeUnion, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 125, EnumName = SpecialComponentName.GledgeDhonrazTwo, FactionName = FactionName.TheGledgeUnion, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 126, EnumName = SpecialComponentName.GledgeDhonrazThree, FactionName = FactionName.TheGledgeUnion, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 127, EnumName = SpecialComponentName.GLEdgeCoreToken, FactionName = FactionName.TheGledgeUnion, Count = 3, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 128, EnumName = SpecialComponentName.GLEdgeBaseToken, FactionName = FactionName.TheGledgeUnion, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Berserkers of Kjalengard
        new SpecialComponentCard() { Id = 129, EnumName = SpecialComponentName.StarDragon, FactionName = FactionName.TheBerserkersOfKjalengard, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 130, EnumName = SpecialComponentName.BannerhallOne, FactionName = FactionName.TheBerserkersOfKjalengard, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 131, EnumName = SpecialComponentName.BannerhallTwo, FactionName = FactionName.TheBerserkersOfKjalengard, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 132, EnumName = SpecialComponentName.BannerhallThree, FactionName = FactionName.TheBerserkersOfKjalengard, Count = 1, SpecialType = SpecialComponentType.VerticalCard, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 133, EnumName = SpecialComponentName.KjalengardGloryToken, FactionName = FactionName.TheBerserkersOfKjalengard, Count = 1, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },

        // The Nokar Sellships
        new SpecialComponentCard() { Id = 134, EnumName = SpecialComponentName.Sabre, FactionName = FactionName.TheNokarSellships, Count = 1, SpecialType = SpecialComponentType.FactionUnit, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
        new SpecialComponentCard() { Id = 135, EnumName = SpecialComponentName.NokarAdditionalDestroyerToken, FactionName = FactionName.TheNokarSellships, Count = 4, SpecialType = SpecialComponentType.Token, Type = CardType.SpecialComponent, GameVersion = GameVersion.DiscordantStars },
    };
}
