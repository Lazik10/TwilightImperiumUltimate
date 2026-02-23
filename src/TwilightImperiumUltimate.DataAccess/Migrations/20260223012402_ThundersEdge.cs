using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ThundersEdge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BreakthroughCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreakthroughCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlagshipCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlagshipCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialComponentCards",
                schema: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FactionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    SpecialType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialComponentCards", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "ActionCards",
                columns: new[] { "Id", "EnumName", "GameVersion", "Text", "TimingWindow", "Type" },
                values: new object[,]
                {
                    { 122, "Salvage", "BaseGame", "", "Combat", "Action" },
                    { 123, "WarEffort", "BaseGame", "", "Action", "Action" },
                    { 124, "BlackMarketDealings", "ThundersEdge", "", "Transaction", "Action" },
                    { 125, "Brilliance", "ThundersEdge", "", "Action", "Action" },
                    { 126, "CrashLanding", "ThundersEdge", "", "Combat", "Action" },
                    { 127, "Crisis", "ThundersEdge", "", "Anytime", "Action" },
                    { 128, "ExchangeProgram", "ThundersEdge", "", "Action", "Action" },
                    { 129, "ExtremeDuress", "ThundersEdge", "", "TurnStart", "Action" },
                    { 130, "LieInWait", "ThundersEdge", "", "Transaction", "Action" },
                    { 131, "MercenaryContract", "ThundersEdge", "", "Action", "Action" },
                    { 132, "PirateContract", "ThundersEdge", "", "Action", "Action" },
                    { 133, "PirateFleet", "ThundersEdge", "", "Action", "Action" },
                    { 134, "PuppetsOnAString", "ThundersEdge", "", "TurnEnd", "Action" },
                    { 135, "Rescue", "ThundersEdge", "", "Anytime", "Action" },
                    { 136, "Strategize", "ThundersEdge", "", "Action", "Action" },
                    { 137, "Overrule", "ThundersEdge", "", "Action", "Action" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "BreakthroughCards",
                columns: new[] { "Id", "EnumName", "FactionName", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 1, "Psychospore", "TheArborec", "ThundersEdge", "", "Breakthrough" },
                    { 2, "WingTransfer", "TheArgentFlight", "ThundersEdge", "", "Breakthrough" },
                    { 3, "GravleashManeuvers", "TheBaronyOfLetnev", "ThundersEdge", "", "Breakthrough" },
                    { 4, "DeorbitBarrage", "TheClanOfSaar", "ThundersEdge", "", "Breakthrough" },
                    { 5, "IIHQModernization", "TheCouncilKeleres", "ThundersEdge", "", "Breakthrough" },
                    { 6, "ResonanceGenerator", "TheCrimsonRebellion", "ThundersEdge", "", "Breakthrough" },
                    { 7, "VisionariaSelect", "TheDeepwroughtScholarate", "ThundersEdge", "", "Breakthrough" },
                    { 8, "StellarGenesis", "TheEmbersOfMuaat", "ThundersEdge", "", "Breakthrough" },
                    { 9, "AutoFactories", "TheEmiratesOfHacan", "ThundersEdge", "", "Breakthrough" },
                    { 10, "VoidTether", "TheEmpyrean", "ThundersEdge", "", "Breakthrough" },
                    { 11, "BellumGloriosum", "TheFederationOfSol", "ThundersEdge", "", "Breakthrough" },
                    { 12, "TheSowing", "TheFirmamentTheObsidian", "ThundersEdge", "", "Breakthrough" },
                    { 13, "TheReaping", "TheFirmamentTheObsidian", "ThundersEdge", "", "Breakthrough" },
                    { 14, "ParticleSynthesis", "TheGhostsOfCreuss", "ThundersEdge", "", "Breakthrough" },
                    { 15, "FealtyUplink", "TheL1z1xMindnet", "ThundersEdge", "", "Breakthrough" },
                    { 16, "TheIcon", "LastBastion", "ThundersEdge", "", "Breakthrough" },
                    { 17, "VaultsOfTheHeir", "TheMahactGeneSorcerers", "ThundersEdge", "", "Breakthrough" },
                    { 18, "TheTablesGrace", "TheMentakCoalition", "ThundersEdge", "", "Breakthrough" },
                    { 19, "Corsair", "TheMentakCoalition", "ThundersEdge", "", "Breakthrough" },
                    { 20, "Mindsieve", "TheNaaluCollective", "ThundersEdge", "", "Breakthrough" },
                    { 21, "AbsoluteSynergy", "TheNaazRokhaAlliance", "ThundersEdge", "", "Breakthrough" },
                    { 22, "EidolonMaximum", "TheNaazRokhaAlliance", "ThundersEdge", "", "Breakthrough" },
                    { 23, "ValefarAssimilatorZ", "TheNekroVirus", "ThundersEdge", "", "Breakthrough" },
                    { 24, "ThundersParadox", "TheNomad", "ThundersEdge", "", "Breakthrough" },
                    { 25, "DataSkimmer", "TheRalNelConsortium", "ThundersEdge", "", "Breakthrough" },
                    { 26, "NorrSupremacy", "SardakkNorr", "ThundersEdge", "", "Breakthrough" },
                    { 27, "SlumberstateComputing", "TheTitansOfUl", "ThundersEdge", "", "Breakthrough" },
                    { 28, "SpecializedCompounds", "TheUniversitiesOfJolNar", "ThundersEdge", "", "Breakthrough" },
                    { 29, "AlRaithIxIanovar", "TheVuilRaithCabal", "ThundersEdge", "", "Breakthrough" },
                    { 30, "Imperator", "TheWinnu", "ThundersEdge", "", "Breakthrough" },
                    { 31, "ArchonsGift", "TheXxchaKingdom", "ThundersEdge", "", "Breakthrough" },
                    { 32, "YinAscendant", "TheYinBrotherhood", "ThundersEdge", "", "Breakthrough" },
                    { 33, "DeepgloomExecutable", "TheYssarilTribes", "ThundersEdge", "", "Breakthrough" },
                    { 34, "ArmsBrokerage", "TheShipwrightsofAxis", "ThundersEdge", "", "Breakthrough" },
                    { 35, "TradeProtectorate", "TheCeldauriTradeConfederation", "ThundersEdge", "", "Breakthrough" },
                    { 36, "CyberneticArmoring", "TheSavagesOfCymiae", "ThundersEdge", "", "Breakthrough" },
                    { 37, "ExodusEngineering", "TheDihMohnFlotilla", "ThundersEdge", "", "Breakthrough" },
                    { 38, "ReverieImplants", "TheFlorzenProfiteers", "ThundersEdge", "", "Breakthrough" },
                    { 39, "GalacticMovement", "TheFreeSystemsCompact", "ThundersEdge", "", "Breakthrough" },
                    { 40, "RaidingParties", "TheGheminaRaiders", "ThundersEdge", "", "Breakthrough" },
                    { 41, "CrypticInsights", "TheAugursOfIlyxum", "ThundersEdge", "", "Breakthrough" },
                    { 42, "TheCollectorsMuseum", "TheKolleccSociety", "ThundersEdge", "", "Breakthrough" },
                    { 43, "TheQueensWrath", "TheKortaliTribunal", "ThundersEdge", "", "Breakthrough" },
                    { 44, "ProfessionalIntrigue", "TheLiZhoDynasty", "ThundersEdge", "", "Breakthrough" },
                    { 45, "MendedGrove", "TheLTokkKhrask", "ThundersEdge", "", "Breakthrough" },
                    { 46, "StabilizationArrays", "TheMirvedaProtectorate", "ThundersEdge", "", "Breakthrough" },
                    { 47, "MirageCraf", "TheGlimmerOfMortheus", "ThundersEdge", "", "Breakthrough" },
                    { 48, "Dreamwalkers", "TheMykoMentori", "ThundersEdge", "", "Breakthrough" },
                    { 49, "AnomalyStabilization", "TheNivynStarKings", "ThundersEdge", "", "Breakthrough" },
                    { 50, "InsurrectionistNetworking", "TheOlradinLeague", "ThundersEdge", "", "Breakthrough" },
                    { 51, "RhodunsReliquary", "TheZealotsOfRhodun", "ThundersEdge", "", "Breakthrough" },
                    { 52, "TheProdigysTriumph", "RohDhnaMechatronics", "ThundersEdge", "", "Breakthrough" },
                    { 53, "HiddenHardpoints", "TheTnelisSyndicate", "ThundersEdge", "", "Breakthrough" },
                    { 54, "StrongarmBanking", "TheVadenBankingClans", "ThundersEdge", "", "Breakthrough" },
                    { 55, "GraviticDisruption", "TheVaylerianScourge", "ThundersEdge", "", "Breakthrough" },
                    { 56, "AuroraResearchBase", "TheVeldyrSovereignty", "ThundersEdge", "", "Breakthrough" },
                    { 57, "AwakenedPlanetoids", "TheZelianPurifier", "ThundersEdge", "", "Breakthrough" },
                    { 58, "HistorianConclave", "TheBentorConglomerate", "ThundersEdge", "", "Breakthrough" },
                    { 59, "Matriphagy", "TheCheiranHordes", "ThundersEdge", "", "Breakthrough" },
                    { 60, "CelestialAmbassadors", "TheEdynMandate", "ThundersEdge", "", "Breakthrough" },
                    { 61, "CultOfTheAllMother", "TheGhotiWayfarers", "ThundersEdge", "", "Breakthrough" },
                    { 62, "DhonrazInstallations", "TheGledgeUnion", "ThundersEdge", "", "Breakthrough" },
                    { 63, "Bannerhalls", "TheBerserkersOfKjalengard", "ThundersEdge", "", "Breakthrough" },
                    { 64, "SynchronicitySix", "TheMonksOfKolume", "ThundersEdge", "", "Breakthrough" },
                    { 65, "StrainDispersa", "TheKyroSodality", "ThundersEdge", "", "Breakthrough" },
                    { 66, "ErasureCorps", "TheLanefirRemnants", "ThundersEdge", "", "Breakthrough" },
                    { 67, "MercenaryCaptains", "TheNokarSellships", "ThundersEdge", "", "Breakthrough" }
                });

            migrationBuilder.InsertData(
                schema: "Faction",
                table: "FactionColorImportances",
                columns: new[] { "Id", "Color", "FactionName", "Importance" },
                values: new object[,]
                {
                    { 473, "Red", "TheCrimsonRebellion", 9 },
                    { 474, "Orange", "TheCrimsonRebellion", 3 },
                    { 475, "Yellow", "TheCrimsonRebellion", 0 },
                    { 476, "Green", "TheCrimsonRebellion", 0 },
                    { 477, "Blue", "TheCrimsonRebellion", 2 },
                    { 478, "Purple", "TheCrimsonRebellion", 0 },
                    { 479, "Pink", "TheCrimsonRebellion", 0 },
                    { 480, "Black", "TheCrimsonRebellion", 8 },
                    { 481, "Red", "LastBastion", 2 },
                    { 482, "Orange", "LastBastion", 9 },
                    { 483, "Yellow", "LastBastion", 3 },
                    { 484, "Green", "LastBastion", 0 },
                    { 485, "Blue", "LastBastion", 6 },
                    { 486, "Purple", "LastBastion", 0 },
                    { 487, "Pink", "LastBastion", 1 },
                    { 488, "Black", "LastBastion", 0 },
                    { 489, "Red", "TheFirmamentTheObsidian", 0 },
                    { 490, "Orange", "TheFirmamentTheObsidian", 7 },
                    { 491, "Yellow", "TheFirmamentTheObsidian", 1 },
                    { 492, "Green", "TheFirmamentTheObsidian", 0 },
                    { 493, "Blue", "TheFirmamentTheObsidian", 0 },
                    { 494, "Purple", "TheFirmamentTheObsidian", 4 },
                    { 495, "Pink", "TheFirmamentTheObsidian", 5 },
                    { 496, "Black", "TheFirmamentTheObsidian", 6 },
                    { 497, "Red", "TheDeepwroughtScholarate", 0 },
                    { 498, "Orange", "TheDeepwroughtScholarate", 0 },
                    { 499, "Yellow", "TheDeepwroughtScholarate", 3 },
                    { 500, "Green", "TheDeepwroughtScholarate", 0 },
                    { 501, "Blue", "TheDeepwroughtScholarate", 9 },
                    { 502, "Purple", "TheDeepwroughtScholarate", 7 },
                    { 503, "Pink", "TheDeepwroughtScholarate", 0 },
                    { 504, "Black", "TheDeepwroughtScholarate", 6 },
                    { 505, "Red", "TheRalNelConsortium", 0 },
                    { 506, "Orange", "TheRalNelConsortium", 1 },
                    { 507, "Yellow", "TheRalNelConsortium", 5 },
                    { 508, "Green", "TheRalNelConsortium", 9 },
                    { 509, "Blue", "TheRalNelConsortium", 4 },
                    { 510, "Purple", "TheRalNelConsortium", 0 },
                    { 511, "Pink", "TheRalNelConsortium", 0 },
                    { 512, "Black", "TheRalNelConsortium", 0 }
                });

            migrationBuilder.InsertData(
                schema: "Faction",
                table: "Factions",
                columns: new[] { "FactionName", "Action", "Commodities", "ComplexityRating", "GameVersion", "History", "HomeSystem", "Id", "Quote", "SystemInfo", "SystemStats" },
                values: new object[,]
                {
                    { "LastBastion", "LastBastion_Action", 1, "Low", "ThundersEdge", "LastBastion_History", "TileTE92", 63, "LastBastion_Quote", "LastBastion_SystemInfo", "LastBastion_SystemStats" },
                    { "TheCrimsonRebellion", "TheCrimsonRebellion_Action", 2, "High", "ThundersEdge", "TheCrimsonRebellion_History", "TileTE94", 60, "TheCrimsonRebellion_Quote", "TheCrimsonRebellion_SystemInfo", "TheCrimsonRebellion_SystemStats" },
                    { "TheDeepwroughtScholarate", "TheDeepwroughtScholarate_Action", 3, "Medium", "ThundersEdge", "TheDeepwroughtScholarate_History", "TileTE95", 61, "TheDeepwroughtScholarate_Quote", "TheDeepwroughtScholarate_SystemInfo", "TheDeepwroughtScholarate_SystemStats" },
                    { "TheFirmamentTheObsidian", "TheFirmamentTheObsidian_Action", 3, "High", "ThundersEdge", "TheFirmamentTheObsidian_History", "TileTE96A", 62, "TheFirmamentTheObsidian_Quote", "TheFirmamentTheObsidian_SystemInfo", "TheFirmamentTheObsidian_SystemStats" },
                    { "TheRalNelConsortium", "TheRalNelConsortium_Action", 4, "Low", "ThundersEdge", "TheRalNelConsortium_History", "TileTE93", 64, "TheRalNelConsortium_Quote", "TheRalNelConsortium_SystemInfo", "TheRalNelConsortium_SystemStats" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "FlagshipCards",
                columns: new[] { "Id", "EnumName", "FactionName", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 1, "DuhaMenaimon", "TheArborec", "BaseGame", "", "Flagship" },
                    { 2, "ArcSecundus", "TheBaronyOfLetnev", "BaseGame", "", "Flagship" },
                    { 3, "SonOfRagh", "TheClanOfSaar", "BaseGame", "", "Flagship" },
                    { 4, "TheInferno", "TheEmbersOfMuaat", "BaseGame", "", "Flagship" },
                    { 5, "WrathOfKenara", "TheEmiratesOfHacan", "BaseGame", "", "Flagship" },
                    { 6, "Genesis", "TheFederationOfSol", "BaseGame", "", "Flagship" },
                    { 7, "HilColish", "TheGhostsOfCreuss", "BaseGame", "", "Flagship" },
                    { 8, "ZeroZeroOne", "TheL1z1xMindnet", "BaseGame", "", "Flagship" },
                    { 9, "FourthMoon", "TheMentakCoalition", "BaseGame", "", "Flagship" },
                    { 10, "Matriarch", "TheNaaluCollective", "BaseGame", "", "Flagship" },
                    { 11, "TheAlastor", "TheNekroVirus", "BaseGame", "", "Flagship" },
                    { 12, "CMorranNorr", "SardakkNorr", "BaseGame", "", "Flagship" },
                    { 13, "JNSHylarim", "TheUniversitiesOfJolNar", "BaseGame", "", "Flagship" },
                    { 14, "SalaiSaiCorian", "TheWinnu", "BaseGame", "", "Flagship" },
                    { 15, "LoncarraSsodu", "TheXxchaKingdom", "BaseGame", "", "Flagship" },
                    { 16, "VanHauge", "TheYinBrotherhood", "BaseGame", "", "Flagship" },
                    { 17, "YsiaYssrila", "TheYssarilTribes", "BaseGame", "", "Flagship" },
                    { 18, "Quetzecoatl", "TheArgentFlight", "ProphecyOfKings", "", "Flagship" },
                    { 19, "Dynamo", "TheEmpyrean", "ProphecyOfKings", "", "Flagship" },
                    { 20, "ArviconRex", "TheMahactGeneSorcerers", "ProphecyOfKings", "", "Flagship" },
                    { 21, "ViszElVir", "TheNaazRokhaAlliance", "ProphecyOfKings", "", "Flagship" },
                    { 22, "Memoria", "TheNomad", "ProphecyOfKings", "", "Flagship" },
                    { 23, "Ouranos", "TheTitansOfUl", "ProphecyOfKings", "", "Flagship" },
                    { 24, "TheTerrorBetween", "TheVuilRaithCabal", "ProphecyOfKings", "", "Flagship" },
                    { 25, "Artemiris", "TheCouncilKeleres", "CodexVigil", "", "Flagship" },
                    { 26, "Quietus", "TheCrimsonRebellion", "ThundersEdge", "", "Flagship" },
                    { 27, "DWSLuminous", "TheDeepwroughtScholarate", "ThundersEdge", "", "Flagship" },
                    { 28, "HeavensEye", "TheFirmamentTheObsidian", "ThundersEdge", "", "Flagship" },
                    { 29, "HeavensHollow", "TheFirmamentTheObsidian", "ThundersEdge", "", "Flagship" },
                    { 30, "TheEgeiro", "LastBastion", "ThundersEdge", "", "Flagship" },
                    { 31, "LastDispatch", "TheRalNelConsortium", "ThundersEdge", "", "Flagship" },
                    { 32, "TheScarletKnife", "TheRubyMonarch", "ThundersEdge", "", "Flagship" },
                    { 33, "AiroShirRex", "RadiantAur", "ThundersEdge", "", "Flagship" },
                    { 34, "Scintillia", "AvariceRex", "ThundersEdge", "", "Flagship" },
                    { 35, "Nightbloom", "IlSaiLakoeHeraldOfThorns", "ThundersEdge", "", "Flagship" },
                    { 36, "Tizona", "TheSaintOfSwords", "ThundersEdge", "", "Flagship" },
                    { 37, "Enigma", "IlNaViroset", "ThundersEdge", "", "Flagship" },
                    { 38, "TheFaceOfJanovet", "ElNenJanovet", "ThundersEdge", "", "Flagship" },
                    { 39, "AStrangledWhisper", "ASickeningLurch", "ThundersEdge", "", "Flagship" },
                    { 40, "BearerOfHeavens", "TheShipwrightsofAxis", "DiscordantStars", "", "Flagship" },
                    { 41, "Supremacy", "TheCeldauriTradeConfederation", "DiscordantStars", "", "Flagship" },
                    { 42, "Celagrom", "TheCeldauriTradeConfederation", "DiscordantStars", "", "Flagship" },
                    { 43, "ReprocessorAlpha", "TheSavagesOfCymiae", "DiscordantStars", "", "Flagship" },
                    { 44, "Maximus", "TheDihMohnFlotilla", "DiscordantStars", "", "Flagship" },
                    { 45, "ManOWar", "TheFlorzenProfiteers", "DiscordantStars", "", "Flagship" },
                    { 46, "Vox", "TheFreeSystemsCompact", "DiscordantStars", "", "Flagship" },
                    { 47, "TheLady", "TheGheminaRaiders", "DiscordantStars", "", "Flagship" },
                    { 48, "TheLord", "TheGheminaRaiders", "DiscordantStars", "", "Flagship" },
                    { 49, "Nemsys", "TheAugursOfIlyxum", "DiscordantStars", "", "Flagship" },
                    { 50, "NightingaleV", "TheKolleccSociety", "DiscordantStars", "", "Flagship" },
                    { 51, "Magistrate", "TheKortaliTribunal", "DiscordantStars", "", "Flagship" },
                    { 52, "SilenceOfStars", "TheLiZhoDynasty", "DiscordantStars", "", "Flagship" },
                    { 53, "SplinteringGale", "TheLTokkKhrask", "DiscordantStars", "", "Flagship" },
                    { 54, "Nexus", "TheMirvedaProtectorate", "DiscordantStars", "", "Flagship" },
                    { 55, "ParticleSieve", "TheGlimmerOfMortheus", "DiscordantStars", "", "Flagship" },
                    { 56, "PsyclobeaQarnyx", "TheMykoMentori", "DiscordantStars", "", "Flagship" },
                    { 57, "Eradica", "TheNivynStarKings", "DiscordantStars", "", "Flagship" },
                    { 58, "Rallypoint", "TheOlradinLeague", "DiscordantStars", "", "Flagship" },
                    { 59, "Reckoning", "TheZealotsOfRhodun", "DiscordantStars", "", "Flagship" },
                    { 60, "Kyvir", "RohDhnaMechatronics", "DiscordantStars", "", "Flagship" },
                    { 61, "PrincipiaAneris", "TheTnelisSyndicate", "DiscordantStars", "", "Flagship" },
                    { 62, "AurumVadra", "TheVadenBankingClans", "DiscordantStars", "", "Flagship" },
                    { 63, "LostCause", "TheVaylerianScourge", "DiscordantStars", "", "Flagship" },
                    { 64, "Richtyrian", "TheVeldyrSovereignty", "DiscordantStars", "", "Flagship" },
                    { 65, "WorldCracker", "TheZelianPurifier", "DiscordantStars", "", "Flagship" },
                    { 66, "Wayfinder", "TheBentorConglomerate", "DiscordantStars", "", "Flagship" },
                    { 67, "Lithodax", "TheCheiranHordes", "DiscordantStars", "", "Flagship" },
                    { 68, "Kaliburn", "TheEdynMandate", "DiscordantStars", "", "Flagship" },
                    { 69, "AllMother", "TheGhotiWayfarers", "DiscordantStars", "", "Flagship" },
                    { 70, "BegBersha", "TheGledgeUnion", "DiscordantStars", "", "Flagship" },
                    { 71, "HulgadesHammer", "TheBerserkersOfKjalengard", "DiscordantStars", "", "Flagship" },
                    { 72, "Halberd", "TheMonksOfKolume", "DiscordantStars", "", "Flagship" },
                    { 73, "Auriga", "TheKyroSodality", "DiscordantStars", "", "Flagship" },
                    { 74, "MemoryOfDusk", "TheLanefirRemnants", "DiscordantStars", "", "Flagship" },
                    { 75, "AnnahRegia", "TheNokarSellships", "DiscordantStars", "", "Flagship" }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "Planets",
                columns: new[] { "PlanetName", "GameVersion", "Id", "Influence", "IsLegendary", "PlanetTrait", "Resources", "SystemTileName", "TechnologySkip" },
                values: new object[,]
                {
                    { "Avernus", "ThundersEdge", 338, 0, "true", "Legendary", 2, "TileEmpty", "None" },
                    { "Mirage", "ProphecyOfKings", 338, 2, "true", "Cultural", 1, "TileEmpty", "None" },
                    { "ThundersEdge", "ThundersEdge", 338, 0, "true", "Legendary", 5, "TileEmpty", "None" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "PromissaryNoteCards",
                columns: new[] { "Id", "EnumName", "Faction", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 74, "RaiseTheStandard", "LastBastion", "ThundersEdge", "", "Promissary" },
                    { 75, "NanoLinkPermit", "TheRalNelConsortium", "ThundersEdge", "", "Promissary" },
                    { 76, "Sever", "TheCrimsonRebellion", "ThundersEdge", "", "Promissary" },
                    { 77, "BlackOps", "TheFirmamentTheObsidian", "ThundersEdge", "", "Promissary" },
                    { 78, "Malevolency", "TheFirmamentTheObsidian", "ThundersEdge", "", "Promissary" },
                    { 79, "ShareKnowledge", "TheDeepwroughtScholarate", "ThundersEdge", "", "Promissary" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "RelicCards",
                columns: new[] { "Id", "EnumName", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 21, "CircletOfTheVoid", "CodexLiberation", "", "Relic" },
                    { 22, "BookOfLatvinia", "CodexLiberation", "", "Relic" },
                    { 23, "Neuraloop", "CodexLiberation", "", "Relic" },
                    { 24, "MetaliVoidArmaments", "ThundersEdge", "", "Relic" },
                    { 25, "TheQuantumcore", "ThundersEdge", "", "Relic" },
                    { 26, "TheSilverFlame", "ThundersEdge", "", "Relic" },
                    { 27, "LightrailOrdnance", "ThundersEdge", "", "Relic" },
                    { 28, "MetaliVoidShielding", "ThundersEdge", "", "Relic" },
                    { 29, "TheTriad", "ThundersEdge", "", "Relic" },
                    { 30, "HeartOfIxth", "ThundersEdge", "", "Relic" }
                });

            migrationBuilder.UpdateData(
                schema: "Tigl",
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 2, 23), new DateOnly(2026, 2, 23) });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "SpecialComponentCards",
                columns: new[] { "Id", "Count", "EnumName", "FactionName", "GameVersion", "SpecialType", "Text", "Type" },
                values: new object[,]
                {
                    { 1, 1, "LetaniWarrior", "TheArborec", "BaseGame", "FactionUnit", "", "SpecialComponent" },
                    { 2, 1, "FloatingFactory", "TheClanOfSaar", "BaseGame", "FactionUnit", "", "SpecialComponent" },
                    { 3, 1, "PrototypeWarSun", "TheEmbersOfMuaat", "BaseGame", "FactionUnit", "", "SpecialComponent" },
                    { 4, 1, "AvernusCard", "TheEmbersOfMuaat", "BaseGame", "VerticalCard", "", "SpecialComponent" },
                    { 5, 1, "AvernusCardAbility", "TheEmbersOfMuaat", "BaseGame", "HorizontalCard", "", "SpecialComponent" },
                    { 6, 1, "AvernusToken", "TheEmbersOfMuaat", "BaseGame", "PlanetToken", "", "SpecialComponent" },
                    { 7, 1, "AdvancedCarrier", "TheFederationOfSol", "BaseGame", "FactionUnit", "", "SpecialComponent" },
                    { 8, 1, "SpecOps", "TheFederationOfSol", "BaseGame", "FactionUnit", "", "SpecialComponent" },
                    { 9, 1, "AlphaCreussToken", "TheGhostsOfCreuss", "BaseGame", "Token", "", "SpecialComponent" },
                    { 10, 1, "BetaCreussToken", "TheGhostsOfCreuss", "BaseGame", "Token", "", "SpecialComponent" },
                    { 11, 1, "GammaCreussToken", "TheGhostsOfCreuss", "BaseGame", "Token", "", "SpecialComponent" },
                    { 12, 1, "Corsair", "TheMentakCoalition", "ThundersEdge", "FactionUnit", "", "SpecialComponent" },
                    { 13, 1, "HybridCrystalFighter", "TheNaaluCollective", "BaseGame", "FactionUnit", "", "SpecialComponent" },
                    { 14, 1, "NaaluZeroToken", "TheNaaluCollective", "BaseGame", "Token", "", "SpecialComponent" },
                    { 15, 1, "SuperDreadnought", "TheL1z1xMindnet", "BaseGame", "FactionUnit", "", "SpecialComponent" },
                    { 16, 1, "AssimilationTokenX", "TheNekroVirus", "BaseGame", "Token", "", "SpecialComponent" },
                    { 17, 1, "AssimilationTokenY", "TheNekroVirus", "BaseGame", "Token", "", "SpecialComponent" },
                    { 18, 1, "AssimilationTokenZ", "TheNekroVirus", "BaseGame", "Token", "", "SpecialComponent" },
                    { 19, 3, "DimensionalTearTokenNekro", "TheNekroVirus", "ProphecyOfKings", "Token", "", "SpecialComponent" },
                    { 20, 3, "HeliosTokenNekro", "TheNekroVirus", "ThundersEdge", "Token", "", "SpecialComponent" },
                    { 21, 1, "Exotrireme", "SardakkNorr", "BaseGame", "FactionUnit", "", "SpecialComponent" },
                    { 22, 1, "StrikeWingAlpha", "TheArgentFlight", "ProphecyOfKings", "FactionUnit", "", "SpecialComponent" },
                    { 23, 2, "VoidTetherToken", "TheEmpyrean", "ProphecyOfKings", "Token", "", "SpecialComponent" },
                    { 24, 2, "CrimsonLegionnaire", "TheMahactGeneSorcerers", "ProphecyOfKings", "FactionUnit", "", "SpecialComponent" },
                    { 25, 1, "EidolonMaximum", "TheNaazRokhaAlliance", "ThundersEdge", "FactionUnit", "", "SpecialComponent" },
                    { 26, 1, "HellTitan", "TheTitansOfUl", "ProphecyOfKings", "FactionUnit", "", "SpecialComponent" },
                    { 27, 1, "SaturnEngine", "TheTitansOfUl", "ProphecyOfKings", "FactionUnit", "", "SpecialComponent" },
                    { 28, 5, "SleeperToken", "TheTitansOfUl", "ProphecyOfKings", "Token", "", "SpecialComponent" },
                    { 29, 1, "TerraformToken", "TheTitansOfUl", "ProphecyOfKings", "Token", "", "SpecialComponent" },
                    { 30, 1, "GeoformToken", "TheTitansOfUl", "ProphecyOfKings", "Token", "", "SpecialComponent" },
                    { 31, 3, "DimensionalTear", "TheVuilRaithCabal", "ProphecyOfKings", "FactionUnit", "", "SpecialComponent" },
                    { 32, 3, "DimensionalTearToken", "TheVuilRaithCabal", "ProphecyOfKings", "Token", "", "SpecialComponent" },
                    { 33, 1, "CustodiaVigiliaCard", "TheCouncilKeleres", "CodexVigil", "VerticalCard", "", "SpecialComponent" },
                    { 34, 1, "CustodiaVigiliaCardAbility", "TheCouncilKeleres", "CodexVigil", "HorizontalCard", "", "SpecialComponent" },
                    { 35, 1, "CustodiaVigiliaToken", "TheCouncilKeleres", "CodexVigil", "Token", "", "SpecialComponent" },
                    { 36, 1, "Exile", "TheCrimsonRebellion", "ThundersEdge", "FactionUnit", "", "SpecialComponent" },
                    { 37, 7, "BreachToken", "TheCrimsonRebellion", "ThundersEdge", "Token", "", "SpecialComponent" },
                    { 38, 1, "SeverToken", "TheCrimsonRebellion", "ThundersEdge", "Token", "", "SpecialComponent" },
                    { 39, 1, "DeepAbyss", "TheDeepwroughtScholarate", "ThundersEdge", "Ocean", "", "SpecialComponent" },
                    { 40, 1, "BrinePool", "TheDeepwroughtScholarate", "ThundersEdge", "Ocean", "", "SpecialComponent" },
                    { 41, 1, "CoralReef", "TheDeepwroughtScholarate", "ThundersEdge", "Ocean", "", "SpecialComponent" },
                    { 42, 1, "IceShelf", "TheDeepwroughtScholarate", "ThundersEdge", "Ocean", "", "SpecialComponent" },
                    { 43, 1, "LostFleet", "TheDeepwroughtScholarate", "ThundersEdge", "Ocean", "", "SpecialComponent" },
                    { 44, 1, "EnervatePlot", "TheFirmamentTheObsidian", "ThundersEdge", "Plot", "", "SpecialComponent" },
                    { 45, 1, "SyphonPlot", "TheFirmamentTheObsidian", "ThundersEdge", "Plot", "", "SpecialComponent" },
                    { 46, 1, "SeethePlot", "TheFirmamentTheObsidian", "ThundersEdge", "Plot", "", "SpecialComponent" },
                    { 47, 1, "AssailPlot", "TheFirmamentTheObsidian", "ThundersEdge", "Plot", "", "SpecialComponent" },
                    { 48, 1, "ExtractPlot", "TheFirmamentTheObsidian", "ThundersEdge", "Plot", "", "SpecialComponent" },
                    { 49, 1, "FourXFourOneCHeliosSix", "LastBastion", "ThundersEdge", "FactionUnit", "", "SpecialComponent" },
                    { 50, 1, "OrdinianCard", "LastBastion", "ThundersEdge", "VerticalCard", "", "SpecialComponent" },
                    { 51, 1, "OrdinianCardAbility", "LastBastion", "ThundersEdge", "HorizontalCard", "", "SpecialComponent" },
                    { 52, 3, "HeliosOneCard", "LastBastion", "ThundersEdge", "VerticalCard", "", "SpecialComponent" },
                    { 53, 3, "HeliosTwoCard", "LastBastion", "ThundersEdge", "VerticalCard", "", "SpecialComponent" },
                    { 54, 3, "HeliosThreeCard", "LastBastion", "ThundersEdge", "VerticalCard", "", "SpecialComponent" },
                    { 55, 3, "HeliosToken", "LastBastion", "ThundersEdge", "Token", "", "SpecialComponent" },
                    { 56, 7, "GalvanizeToken", "LastBastion", "ThundersEdge", "Token", "", "SpecialComponent" },
                    { 57, 1, "Linkship", "TheRalNelConsortium", "ThundersEdge", "FactionUnit", "", "SpecialComponent" },
                    { 58, 1, "TradePort", "TheCeldauriTradeConfederation", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 59, 1, "CeldauriDockToken", "TheCeldauriTradeConfederation", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 60, 1, "CelagromToken", "TheCeldauriTradeConfederation", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 61, 1, "Aegis", "TheDihMohnFlotilla", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 62, 1, "CorsairFlorzen", "TheFlorzenProfiteers", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 63, 1, "HeartOfRebellionToken", "TheFreeSystemsCompact", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 64, 1, "CombatTransport", "TheGheminaRaiders", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 65, 1, "GeminaRaidersAdditionalFlagshipToken", "TheGheminaRaiders", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 66, 1, "Tribune", "TheKortaliTribunal", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 67, 1, "HeavyBomber", "TheLiZhoDynasty", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 68, 1, "TrapInterferenceGrid", "TheLiZhoDynasty", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 69, 1, "TrapMinefields", "TheLiZhoDynasty", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 70, 1, "TrapAccountSiphon", "TheLiZhoDynasty", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 71, 1, "TrapGraviticInhibitor", "TheLiZhoDynasty", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 72, 1, "TrapFeint", "TheLiZhoDynasty", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 73, 1, "TrapSaboteurs", "TheLiZhoDynasty", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 74, 1, "CardInterferenceGrid", "TheLiZhoDynasty", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 75, 1, "CardMinefields", "TheLiZhoDynasty", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 76, 1, "CardAccountSiphon", "TheLiZhoDynasty", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 77, 1, "CardGraviticInhibitor", "TheLiZhoDynasty", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 78, 1, "CardFeint", "TheLiZhoDynasty", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 79, 1, "CardSaboteurs", "TheLiZhoDynasty", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 80, 1, "ShatteredSky", "TheLTokkKhrask", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 81, 1, "Grove", "TheLTokkKhrask", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 82, 1, "GaussCannon", "TheMirvedaProtectorate", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 83, 1, "MyceliumRing", "TheMykoMentori", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 84, 1, "MykoMentoriCommodityTwo", "TheMykoMentori", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 85, 1, "MykoMentoriCommodityThree", "TheMykoMentori", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 86, 1, "MykoMentoriCommodityFour", "TheMykoMentori", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 87, 1, "WoundToken", "TheNivynStarKings", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 88, 1, "OlradinMechBack", "TheOlradinLeague", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 89, 1, "PolicyThePeopleConnect", "TheOlradinLeague", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 90, 1, "PolicyThePeopleControl", "TheOlradinLeague", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 91, 1, "PolicyTheEnvironmentPreserve", "TheOlradinLeague", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 92, 1, "PolicyTheEnvironmentPlunder", "TheOlradinLeague", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 93, 1, "PolicyTheEconomyEmpower", "TheOlradinLeague", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 94, 1, "PolicyTheEconomyExploit", "TheOlradinLeague", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 95, 1, "OlradinLeagueCommodityTwo", "TheOlradinLeague", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 96, 1, "Terrafactory", "RohDhnaMechatronics", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 97, 1, "OmniForgeWorld", "RohDhnaMechatronics", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 98, 1, "AutomatonsToken", "RohDhnaMechatronics", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 99, 1, "UnholyAbomination", "TheSavagesOfCymiae", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 100, 2, "AxisOrderDestroyer", "TheShipwrightsofAxis", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 101, 2, "AxisOrderCruiser", "TheShipwrightsofAxis", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 102, 2, "AxisOrderCarrier", "TheShipwrightsofAxis", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 103, 2, "AxisOrderDreadnought", "TheShipwrightsofAxis", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 104, 1, "BlockadeRunner", "TheTnelisSyndicate", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 105, 1, "Raider", "TheVaylerianScourge", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 106, 1, "LancerDreadnought", "TheVeldyrSovereignty", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 107, 2, "BranchOfficeInfluenceToken", "TheVeldyrSovereignty", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 108, 2, "BranchOfficeResourcesToken", "TheVeldyrSovereignty", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 109, 1, "AuroraResearchBaseToken", "TheVeldyrSovereignty", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 110, 1, "Impactor", "TheZelianPurifier", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 111, 1, "CulturalFragmentToken", "TheBentorConglomerate", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 112, 1, "IndustrialFragmentToken", "TheBentorConglomerate", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 113, 1, "HazardousFragmentToken", "TheBentorConglomerate", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 114, 1, "UnknownFragmentToken", "TheBentorConglomerate", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 115, 1, "EncryptionKeyToken", "TheBentorConglomerate", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 116, 1, "CmoRancToken", "TheBentorConglomerate", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 117, 1, "ChitinHulk", "TheCheiranHordes", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 118, 2, "CheiranAdditionalDreadnoughtToken", "TheCheiranHordes", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 119, 1, "CheiranAdditionalMechToken", "TheCheiranHordes", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 120, 4, "EdynSigilToken", "TheEdynMandate", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 121, 1, "GhotiCard", "TheGhotiWayfarers", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 122, 1, "GhotiCardAbility", "TheGhotiWayfarers", "DiscordantStars", "HorizontalCard", "", "SpecialComponent" },
                    { 123, 1, "OrionPlatform", "TheGledgeUnion", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 124, 1, "GledgeDhonrazOne", "TheGledgeUnion", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 125, 1, "GledgeDhonrazTwo", "TheGledgeUnion", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 126, 1, "GledgeDhonrazThree", "TheGledgeUnion", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 127, 3, "GLEdgeCoreToken", "TheGledgeUnion", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 128, 1, "GLEdgeBaseToken", "TheGledgeUnion", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 129, 1, "StarDragon", "TheBerserkersOfKjalengard", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 130, 1, "BannerhallOne", "TheBerserkersOfKjalengard", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 131, 1, "BannerhallTwo", "TheBerserkersOfKjalengard", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 132, 1, "BannerhallThree", "TheBerserkersOfKjalengard", "DiscordantStars", "VerticalCard", "", "SpecialComponent" },
                    { 133, 1, "KjalengardGloryToken", "TheBerserkersOfKjalengard", "DiscordantStars", "Token", "", "SpecialComponent" },
                    { 134, 1, "Sabre", "TheNokarSellships", "DiscordantStars", "FactionUnit", "", "SpecialComponent" },
                    { 135, 4, "NokarAdditionalDestroyerToken", "TheNokarSellships", "DiscordantStars", "Token", "", "SpecialComponent" }
                });

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "StrategyCards",
                keyColumn: "Id",
                keyValue: 6,
                column: "GameVersion",
                value: "Deprecated");

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "StrategyCards",
                keyColumn: "Id",
                keyValue: 10,
                column: "GameVersion",
                value: "Deprecated");

            migrationBuilder.InsertData(
                schema: "Card",
                table: "StrategyCards",
                columns: new[] { "Id", "EnumName", "GameVersion", "InitiativeOrder", "PrimaryAbilityText", "SecondaryAbilityText", "Text", "Type" },
                values: new object[,]
                {
                    { 11, "ConstructionThundersEdge", "ThundersEdge", "Fourth", "", "", "", "Strategy" },
                    { 12, "WarfareThundersEdge", "ThundersEdge", "Sixth", "", "", "", "Strategy" }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "SystemTiles",
                columns: new[] { "SystemTileName", "Anomaly", "FactionName", "GameVersion", "Id", "SystemTileCode", "TileCategory" },
                values: new object[,]
                {
                    { "TileTE100", "None", "None", "ThundersEdge", 328, "100", "Blue" },
                    { "TileTE101", "None", "None", "ThundersEdge", 329, "101", "Blue" },
                    { "TileTE102", "None", "None", "ThundersEdge", 330, "102", "Blue" },
                    { "TileTE103", "None", "None", "ThundersEdge", 331, "103", "Blue" },
                    { "TileTE104", "None", "None", "ThundersEdge", 332, "104", "Blue" },
                    { "TileTE105", "None", "None", "ThundersEdge", 333, "105", "Blue" },
                    { "TileTE106", "None", "None", "ThundersEdge", 334, "106", "Blue" },
                    { "TileTE107", "None", "None", "ThundersEdge", 335, "107", "Blue" },
                    { "TileTE108", "None", "None", "ThundersEdge", 336, "108", "Blue" },
                    { "TileTE109", "None", "None", "ThundersEdge", 337, "109", "Blue" },
                    { "TileTE110", "None", "None", "ThundersEdge", 338, "110", "Blue" },
                    { "TileTE111", "None", "None", "ThundersEdge", 339, "111", "Blue" },
                    { "TileTE112", "None", "None", "ThundersEdge", 340, "112", "MecatolRex" },
                    { "TileTE113", "GravityRift", "None", "ThundersEdge", 341, "113", "Red" },
                    { "TileTE114", "EntropicScar", "None", "ThundersEdge", 342, "114", "Red" },
                    { "TileTE115", "AsteroidField", "None", "ThundersEdge", 343, "115", "Red" },
                    { "TileTE116", "EntropicScar", "None", "ThundersEdge", 344, "116", "Red" },
                    { "TileTE117", "AsteroidField", "None", "ThundersEdge", 345, "117", "Red" },
                    { "TileTE118", "None", "TheCrimsonRebellion", "ThundersEdge", 346, "118", "ExternalMapTile" },
                    { "TileTE119A", "None", "None", "ThundersEdge", 347, "119A", "Hyperlane" },
                    { "TileTE119B", "None", "None", "ThundersEdge", 348, "119B", "Hyperlane" },
                    { "TileTE120A", "None", "None", "ThundersEdge", 349, "120A", "Hyperlane" },
                    { "TileTE120B", "None", "None", "ThundersEdge", 350, "120B", "Hyperlane" },
                    { "TileTE121A", "None", "None", "ThundersEdge", 351, "121A", "Hyperlane" },
                    { "TileTE121B", "None", "None", "ThundersEdge", 352, "121B", "Hyperlane" },
                    { "TileTE122A", "None", "None", "ThundersEdge", 353, "122A", "Hyperlane" },
                    { "TileTE122B", "None", "None", "ThundersEdge", 354, "122B", "Hyperlane" },
                    { "TileTE123A", "None", "None", "ThundersEdge", 355, "123A", "Hyperlane" },
                    { "TileTE123B", "None", "None", "ThundersEdge", 356, "123B", "Hyperlane" },
                    { "TileTE124A", "None", "None", "ThundersEdge", 357, "124A", "Hyperlane" },
                    { "TileTE124B", "None", "None", "ThundersEdge", 358, "124B", "Hyperlane" },
                    { "TileTE125A", "None", "None", "ThundersEdge", 359, "Fracture1", "ExternalMapTile" },
                    { "TileTE125B", "None", "None", "ThundersEdge", 360, "Fracture2", "ExternalMapTile" },
                    { "TileTE126A", "None", "None", "ThundersEdge", 361, "Fracture3", "ExternalMapTile" },
                    { "TileTE126B", "None", "None", "ThundersEdge", 362, "Fracture4", "ExternalMapTile" },
                    { "TileTE126C", "None", "None", "ThundersEdge", 363, "Fracture5", "ExternalMapTile" },
                    { "TileTE127A", "None", "None", "ThundersEdge", 364, "Fracture6", "ExternalMapTile" },
                    { "TileTE127B", "None", "None", "ThundersEdge", 365, "Fracture7", "ExternalMapTile" },
                    { "TileTE92", "Nebula", "LastBastion", "ThundersEdge", 319, "92", "Green" },
                    { "TileTE93", "None", "TheRalNelConsortium", "ThundersEdge", 320, "93", "Green" },
                    { "TileTE94", "None", "TheCrimsonRebellion", "ThundersEdge", 321, "94", "Green" },
                    { "TileTE95", "None", "TheDeepwroughtScholarate", "ThundersEdge", 322, "95", "Green" },
                    { "TileTE96A", "None", "TheFirmamentTheObsidian", "ThundersEdge", 323, "96A", "Green" },
                    { "TileTE96B", "None", "TheFirmamentTheObsidian", "ThundersEdge", 324, "96B", "ExternalMapTile" },
                    { "TileTE97", "None", "None", "ThundersEdge", 325, "97", "Blue" },
                    { "TileTE98", "None", "None", "ThundersEdge", 326, "98", "Blue" },
                    { "TileTE99", "None", "None", "ThundersEdge", 327, "99", "Blue" }
                });

            migrationBuilder.InsertData(
                schema: "Technology",
                table: "Technologies",
                columns: new[] { "TechnologyName", "FactionName", "GameVersion", "Id", "IsFactionTechnology", "Level", "Text", "Type" },
                values: new object[,]
                {
                    { "AgencySupplyNetworkThundersEdge", "TheCouncilKeleres", "ThundersEdge", 167, "true", "Level2", "", "Cybernetic" },
                    { "ExecutiveOrder", "TheCouncilKeleres", "ThundersEdge", 166, "true", "Level1", "", "Cybernetic" },
                    { "ExileTwo", "TheCrimsonRebellion", "ThundersEdge", 160, "true", "Level2", "", "UnitUpgrade" },
                    { "FourXFourOneCHELIOSVTwo", "LastBastion", "ThundersEdge", 162, "true", "Level2", "", "UnitUpgrade" },
                    { "HydrothermalMining", "TheDeepwroughtScholarate", "ThundersEdge", 158, "true", "Level1", "", "Biotic" },
                    { "LinkshipTwo", "TheRalNelConsortium", "ThundersEdge", 164, "true", "Level2", "", "UnitUpgrade" },
                    { "Nanomachines", "TheRalNelConsortium", "ThundersEdge", 163, "true", "Level1", "", "Warfare" },
                    { "NeuralParasiteFirmament", "TheFirmamentTheObsidian", "ThundersEdge", 166, "true", "Level2", "", "Biotic" },
                    { "NeuralParasiteObsidian", "TheFirmamentTheObsidian", "ThundersEdge", 166, "true", "Level2", "", "Biotic" },
                    { "PlanesplitterFirmament", "TheFirmamentTheObsidian", "ThundersEdge", 165, "true", "Level2", "", "Cybernetic" },
                    { "PlanesplitterObsidian", "TheFirmamentTheObsidian", "ThundersEdge", 165, "true", "Level2", "", "Cybernetic" },
                    { "ProximaTargetingSix", "LastBastion", "ThundersEdge", 161, "true", "Level1", "", "Warfare" },
                    { "RadicalAdvancement", "TheDeepwroughtScholarate", "ThundersEdge", 157, "true", "Level1", "", "Biotic" },
                    { "SubatomicSplicer", "TheCrimsonRebellion", "ThundersEdge", 159, "true", "Level1", "", "Cybernetic" }
                });

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionTechnology",
                columns: new[] { "FactionName", "TechnologyName", "StartingTechnology" },
                values: new object[,]
                {
                    { "LastBastion", "AntimassDeflectors", "true" },
                    { "LastBastion", "DarkEnergyTap", "true" },
                    { "LastBastion", "SarweenTools", "true" },
                    { "LastBastion", "ScanlinkDroneNetwork", "true" },
                    { "TheCrimsonRebellion", "AIDevelopmentAlgorithm", "true" },
                    { "TheCrimsonRebellion", "AntimassDeflectors", "true" },
                    { "TheCrimsonRebellion", "DarkEnergyTap", "true" },
                    { "TheCrimsonRebellion", "PlasmaScoring", "true" },
                    { "TheFirmamentTheObsidian", "NeuralMotivator", "true" },
                    { "TheFirmamentTheObsidian", "Psychoarchaeology", "true" },
                    { "TheFirmamentTheObsidian", "SarweenTools", "true" },
                    { "TheFirmamentTheObsidian", "ScanlinkDroneNetwork", "true" },
                    { "TheRalNelConsortium", "AIDevelopmentAlgorithm", "true" },
                    { "TheRalNelConsortium", "NeuralMotivator", "true" },
                    { "TheRalNelConsortium", "PlasmaScoring", "true" },
                    { "TheRalNelConsortium", "Psychoarchaeology", "true" }
                });

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionUnit",
                columns: new[] { "FactionName", "UnitName", "Count" },
                values: new object[,]
                {
                    { "LastBastion", "Carrier", 1 },
                    { "LastBastion", "Cruiser", 1 },
                    { "LastBastion", "Dreadnought", 1 },
                    { "LastBastion", "Fighter", 2 },
                    { "LastBastion", "Infantry", 3 },
                    { "LastBastion", "SpaceDock", 1 },
                    { "TheCrimsonRebellion", "Carrier", 1 },
                    { "TheCrimsonRebellion", "Destroyer", 2 },
                    { "TheCrimsonRebellion", "Fighter", 3 },
                    { "TheCrimsonRebellion", "Infantry", 4 },
                    { "TheCrimsonRebellion", "Pds", 1 },
                    { "TheCrimsonRebellion", "SpaceDock", 1 },
                    { "TheDeepwroughtScholarate", "Carrier", 1 },
                    { "TheDeepwroughtScholarate", "Dreadnought", 1 },
                    { "TheDeepwroughtScholarate", "Fighter", 4 },
                    { "TheDeepwroughtScholarate", "Infantry", 3 },
                    { "TheDeepwroughtScholarate", "SpaceDock", 1 },
                    { "TheFirmamentTheObsidian", "Carrier", 1 },
                    { "TheFirmamentTheObsidian", "Cruiser", 1 },
                    { "TheFirmamentTheObsidian", "Destroyer", 1 },
                    { "TheFirmamentTheObsidian", "Fighter", 3 },
                    { "TheFirmamentTheObsidian", "Infantry", 3 },
                    { "TheFirmamentTheObsidian", "SpaceDock", 1 },
                    { "TheRalNelConsortium", "Carrier", 1 },
                    { "TheRalNelConsortium", "Destroyer", 1 },
                    { "TheRalNelConsortium", "Dreadnought", 1 },
                    { "TheRalNelConsortium", "Fighter", 2 },
                    { "TheRalNelConsortium", "Infantry", 4 },
                    { "TheRalNelConsortium", "Pds", 2 },
                    { "TheRalNelConsortium", "SpaceDock", 1 }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "Planets",
                columns: new[] { "PlanetName", "GameVersion", "Id", "Influence", "IsLegendary", "PlanetTrait", "Resources", "SystemTileName", "TechnologySkip" },
                values: new object[,]
                {
                    { "AhkCreuxx", "ThundersEdge", 333, 2, "false", "None", 4, "TileTE118", "None" },
                    { "Andeara", "ThundersEdge", 311, 1, "false", "Industrial", 1, "TileTE102", "Propulsion" },
                    { "Bellatrix", "ThundersEdge", 322, 2, "false", "Cultural", 1, "TileTE109", "None" },
                    { "Capha", "ThundersEdge", 321, 0, "false", "Hazardous", 3, "TileTE108", "None" },
                    { "Cocytus", "ThundersEdge", 334, 0, "false", "Relic", 3, "TileTE125A", "None" },
                    { "Cresius", "ThundersEdge", 316, 1, "false", "Hazardous", 0, "TileTE106", "None" },
                    { "Cronos", "ThundersEdge", 302, 1, "false", "None", 2, "TileTE96A", "None" },
                    { "CronosHollow", "ThundersEdge", 304, 0, "false", "None", 3, "TileTE96B", "None" },
                    { "Elnath", "ThundersEdge", 325, 0, "false", "Hazardous", 2, "TileTE110", "None" },
                    { "Emelpar", "ThundersEdge", 308, 2, "true", "Cultural", 0, "TileTE99", "None" },
                    { "Faunus", "ThundersEdge", 306, 3, "true", "Industrial", 1, "TileTE97", "Biotic" },
                    { "Garbozia", "ThundersEdge", 307, 1, "true", "Hazardous", 2, "TileTE98", "None" },
                    { "Hercalor", "ThundersEdge", 319, 0, "false", "Industrial", 1, "TileTE107", "None" },
                    { "Horizon", "ThundersEdge", 324, 2, "false", "Cultural", 1, "TileTE110", "None" },
                    { "Ikatena", "ThundersEdge", 301, 4, "false", "None", 4, "TileTE95", "None" },
                    { "Industrex", "ThundersEdge", 330, 0, "true", "Industrial", 2, "TileTE115", "Warfare" },
                    { "Kostboth", "ThundersEdge", 320, 1, "false", "Cultural", 0, "TileTE108", "None" },
                    { "LazulRex", "ThundersEdge", 317, 2, "false", "IndustrialCultural", 2, "TileTE106", "None" },
                    { "Lemox", "ThundersEdge", 331, 3, "false", "Industrial", 0, "TileTE116", "None" },
                    { "Lesab", "ThundersEdge", 313, 1, "false", "HazardousIndustrial", 2, "TileTE104", "None" },
                    { "Lethe", "ThundersEdge", 336, 2, "false", "Relic", 0, "TileTE127B", "None" },
                    { "LuthienSix", "ThundersEdge", 326, 1, "false", "Hazardous", 3, "TileTE110", "None" },
                    { "MecatolRexLegendary", "ThundersEdge", 329, 6, "true", "Legendary", 1, "TileTE112", "None" },
                    { "MezLoOrzPeiZsha", "ThundersEdge", 299, 1, "false", "None", 2, "TileTE93", "None" },
                    { "NewTerra", "ThundersEdge", 314, 1, "false", "Industrial", 1, "TileTE105", "Biotic" },
                    { "Olergodt", "ThundersEdge", 310, 1, "false", "CulturalHazardous", 2, "TileTE101", "CyberneticWarfare" },
                    { "OluzStation", "ThundersEdge", 328, 1, "false", "SpaceStation", 1, "TileTE111", "None" },
                    { "Ordinian", "ThundersEdge", 297, 0, "true", "Legendary", 0, "TileTE92", "None" },
                    { "Phlegethon", "ThundersEdge", 337, 2, "false", "Relic", 1, "TileTE127B", "None" },
                    { "RepLoOrzQet", "ThundersEdge", 300, 3, "false", "None", 1, "TileTE93", "None" },
                    { "Revelation", "ThundersEdge", 298, 2, "false", "SpaceStation", 1, "TileTE92", "None" },
                    { "Styx", "ThundersEdge", 335, 0, "false", "Relic", 4, "TileTE126B", "None" },
                    { "Tallin", "ThundersEdge", 303, 2, "false", "None", 1, "TileTE96A", "None" },
                    { "TallinHollow", "ThundersEdge", 305, 0, "false", "None", 3, "TileTE96B", "None" },
                    { "Tarana", "ThundersEdge", 327, 2, "false", "IndustrialCultural", 1, "TileTE111", "None" },
                    { "Tempesta", "ThundersEdge", 309, 1, "true", "Hazardous", 1, "TileTE100", "Propulsion" },
                    { "TheWatchtower", "ThundersEdge", 332, 1, "false", "SpaceStation", 1, "TileTE117", "None" },
                    { "Tiamat", "ThundersEdge", 318, 2, "false", "Cultural", 1, "TileTE107", "CyberneticCybernetic" },
                    { "Tinnes", "ThundersEdge", 315, 1, "false", "HazardousIndustrial", 2, "TileTE105", "Biotic" },
                    { "TsionStation", "ThundersEdge", 323, 1, "false", "SpaceStation", 1, "TileTE109", "None" },
                    { "ViraPicsThree", "ThundersEdge", 312, 3, "false", "CulturalHazardous", 2, "TileTE103", "None" }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "Wormholes",
                columns: new[] { "Id", "GameVersion", "SystemTileName", "WormholeName" },
                values: new object[,]
                {
                    { 71, "ThundersEdge", "TileTE94", "Epsilon" },
                    { 72, "ThundersEdge", "TileTE102", "Alpha" },
                    { 73, "ThundersEdge", "TileTE113", "Beta" },
                    { 74, "ThundersEdge", "TileTE118", "Epsilon" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreakthroughCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "FlagshipCards",
                schema: "Card");

            migrationBuilder.DropTable(
                name: "SpecialComponentCards",
                schema: "Card");

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "ActionCards",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 473);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 474);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 475);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 476);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 477);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 478);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 479);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 480);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 481);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 482);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 483);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 484);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 485);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 486);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 487);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 488);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 489);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 490);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 491);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 492);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 493);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 494);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 495);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 496);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 497);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 498);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 499);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 500);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 501);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 502);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 503);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 504);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 505);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 506);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 507);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 508);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 509);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 510);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 511);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 512);

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "LastBastion", "AntimassDeflectors" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "LastBastion", "DarkEnergyTap" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "LastBastion", "SarweenTools" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "LastBastion", "ScanlinkDroneNetwork" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheCrimsonRebellion", "AIDevelopmentAlgorithm" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheCrimsonRebellion", "AntimassDeflectors" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheCrimsonRebellion", "DarkEnergyTap" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheCrimsonRebellion", "PlasmaScoring" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheFirmamentTheObsidian", "NeuralMotivator" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheFirmamentTheObsidian", "Psychoarchaeology" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheFirmamentTheObsidian", "SarweenTools" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheFirmamentTheObsidian", "ScanlinkDroneNetwork" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheRalNelConsortium", "AIDevelopmentAlgorithm" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheRalNelConsortium", "NeuralMotivator" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheRalNelConsortium", "PlasmaScoring" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheRalNelConsortium", "Psychoarchaeology" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "LastBastion", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "LastBastion", "Cruiser" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "LastBastion", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "LastBastion", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "LastBastion", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "LastBastion", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheCrimsonRebellion", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheCrimsonRebellion", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheCrimsonRebellion", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheCrimsonRebellion", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheCrimsonRebellion", "Pds" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheCrimsonRebellion", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheDeepwroughtScholarate", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheDeepwroughtScholarate", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheDeepwroughtScholarate", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheDeepwroughtScholarate", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheDeepwroughtScholarate", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheFirmamentTheObsidian", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheFirmamentTheObsidian", "Cruiser" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheFirmamentTheObsidian", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheFirmamentTheObsidian", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheFirmamentTheObsidian", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheFirmamentTheObsidian", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheRalNelConsortium", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheRalNelConsortium", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheRalNelConsortium", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheRalNelConsortium", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheRalNelConsortium", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheRalNelConsortium", "Pds" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheRalNelConsortium", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "AhkCreuxx");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Andeara");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Avernus");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Bellatrix");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Capha");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Cocytus");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Cresius");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Cronos");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "CronosHollow");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Elnath");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Emelpar");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Faunus");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Garbozia");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Hercalor");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Horizon");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Ikatena");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Industrex");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Kostboth");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "LazulRex");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Lemox");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Lesab");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Lethe");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "LuthienSix");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "MecatolRexLegendary");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "MezLoOrzPeiZsha");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Mirage");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "NewTerra");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Olergodt");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "OluzStation");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Ordinian");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Phlegethon");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "RepLoOrzQet");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Revelation");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Styx");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Tallin");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "TallinHollow");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Tarana");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Tempesta");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "TheWatchtower");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "ThundersEdge");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Tiamat");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Tinnes");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "TsionStation");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "ViraPicsThree");

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "RelicCards",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "RelicCards",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "RelicCards",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "RelicCards",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "RelicCards",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "RelicCards",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "RelicCards",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "RelicCards",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "RelicCards",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "RelicCards",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "StrategyCards",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "StrategyCards",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE114");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE119A");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE119B");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE120A");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE120B");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE121A");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE121B");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE122A");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE122B");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE123A");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE123B");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE124A");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE124B");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE125B");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE126A");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE126C");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE127A");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "AgencySupplyNetworkThundersEdge");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "ExecutiveOrder");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "ExileTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "FourXFourOneCHELIOSVTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "HydrothermalMining");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "LinkshipTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "Nanomachines");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "NeuralParasiteFirmament");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "NeuralParasiteObsidian");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "PlanesplitterFirmament");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "PlanesplitterObsidian");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "ProximaTargetingSix");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "RadicalAdvancement");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "SubatomicSplicer");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Wormholes",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Wormholes",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Wormholes",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Wormholes",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "LastBastion");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheCrimsonRebellion");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheDeepwroughtScholarate");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheFirmamentTheObsidian");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheRalNelConsortium");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE100");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE101");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE102");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE103");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE104");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE105");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE106");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE107");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE108");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE109");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE110");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE111");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE112");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE113");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE115");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE116");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE117");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE118");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE125A");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE126B");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE127B");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE92");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE93");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE94");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE95");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE96A");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE96B");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE97");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE98");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileTE99");

            migrationBuilder.UpdateData(
                schema: "Tigl",
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2025, 12, 14), new DateOnly(2025, 12, 14) });

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "StrategyCards",
                keyColumn: "Id",
                keyValue: 6,
                column: "GameVersion",
                value: "BaseGame");

            migrationBuilder.UpdateData(
                schema: "Card",
                table: "StrategyCards",
                keyColumn: "Id",
                keyValue: 10,
                column: "GameVersion",
                value: "ProphecyOfKings");
        }
    }
}
