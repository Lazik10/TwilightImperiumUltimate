using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TwilightImperiumUltimate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BrAndWftVFactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheArborec", "MagenDefenseGridOmega" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheCheiranHordes", "MagenDefenseGridOmega" });

            migrationBuilder.AlterColumn<string>(
                name: "PlanetTrait",
                schema: "Galaxy",
                table: "Planets",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.UpdateData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "X89BacterialWeaponOmega",
                column: "GameVersion",
                value: "Deprecated");

            migrationBuilder.UpdateData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "MagenDefenseGridOmega",
                column: "GameVersion",
                value: "Deprecated");

            migrationBuilder.InsertData(
                schema: "Card",
                table: "BreakthroughCards",
                columns: new[] { "Id", "EnumName", "FactionName", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 68, "ExperimentalMechanisms", "TheArvaxiCovenant", "WhispersFromTheVoid", "", "Breakthrough" },
                    { 69, "KineticProbeNetwork", "TheConstituantsOfXan", "WhispersFromTheVoid", "", "Breakthrough" },
                    { 70, "BypassOperations", "TheKaloraSwarm", "WhispersFromTheVoid", "", "Breakthrough" },
                    { 71, "DarkSideExploitation", "TheLunariumConspiracies", "WhispersFromTheVoid", "", "Breakthrough" },
                    { 72, "StyxAndStones", "TheOnyxxaComplex", "WhispersFromTheVoid", "", "Breakthrough" },
                    { 73, "NonLinearTimeProgression", "TheTyrisContinuum", "WhispersFromTheVoid", "", "Breakthrough" },
                    { 74, "MoraySystem", "TheVyserixAscendancy", "WhispersFromTheVoid", "", "Breakthrough" },
                    { 75, "SubdueChancellor", "TheZephyrionPursuit", "WhispersFromTheVoid", "", "Breakthrough" },
                    { 76, "PanoplyAnimus", "AtokeraLegacy", "BlueRiverie", "", "Breakthrough" },
                    { 77, "TrinityStockpile", "BelkoseaAlliedStates", "BlueRiverie", "", "Breakthrough" },
                    { 78, "HiddenVaults", "PharadnOrder", "BlueRiverie", "", "Breakthrough" },
                    { 79, "EchoOperatives", "PrincipalityOfKaltrim", "BlueRiverie", "", "Breakthrough" },
                    { 80, "DuraniumOutfitting", "QhetRepublic", "BlueRiverie", "", "Breakthrough" },
                    { 81, "Gravehold", "SarcosaDrift", "BlueRiverie", "", "Breakthrough" },
                    { 82, "ShameixsBaneHonor", "ToldarConcordat", "BlueRiverie", "", "Breakthrough" },
                    { 83, "ShameixsBaneDishonor", "ToldarConcordat", "BlueRiverie", "", "Breakthrough" },
                    { 84, "PrescientWarriors", "UydaiConclave", "BlueRiverie", "", "Breakthrough" },
                    { 85, "HarmoniousDiplomacy", "XinCourt", "BlueRiverie", "", "Breakthrough" }
                });

            migrationBuilder.InsertData(
                schema: "Faction",
                table: "FactionColorImportances",
                columns: new[] { "Id", "Color", "FactionName", "Importance" },
                values: new object[,]
                {
                    { 513, "Red", "TheArvaxiCovenant", 1 },
                    { 514, "Orange", "TheArvaxiCovenant", 2 },
                    { 515, "Yellow", "TheArvaxiCovenant", 7 },
                    { 516, "Green", "TheArvaxiCovenant", 9 },
                    { 517, "Blue", "TheArvaxiCovenant", 3 },
                    { 518, "Purple", "TheArvaxiCovenant", 0 },
                    { 519, "Pink", "TheArvaxiCovenant", 0 },
                    { 520, "Black", "TheArvaxiCovenant", 0 },
                    { 521, "Red", "TheConstituantsOfXan", 0 },
                    { 522, "Orange", "TheConstituantsOfXan", 8 },
                    { 523, "Yellow", "TheConstituantsOfXan", 1 },
                    { 524, "Green", "TheConstituantsOfXan", 9 },
                    { 525, "Blue", "TheConstituantsOfXan", 4 },
                    { 526, "Purple", "TheConstituantsOfXan", 0 },
                    { 527, "Pink", "TheConstituantsOfXan", 0 },
                    { 528, "Black", "TheConstituantsOfXan", 2 },
                    { 529, "Red", "TheKaloraSwarm", 0 },
                    { 530, "Orange", "TheKaloraSwarm", 9 },
                    { 531, "Yellow", "TheKaloraSwarm", 2 },
                    { 532, "Green", "TheKaloraSwarm", 5 },
                    { 533, "Blue", "TheKaloraSwarm", 8 },
                    { 534, "Purple", "TheKaloraSwarm", 0 },
                    { 535, "Pink", "TheKaloraSwarm", 0 },
                    { 536, "Black", "TheKaloraSwarm", 0 },
                    { 537, "Red", "TheLunariumConspiracies", 9 },
                    { 538, "Orange", "TheLunariumConspiracies", 3 },
                    { 539, "Yellow", "TheLunariumConspiracies", 0 },
                    { 540, "Green", "TheLunariumConspiracies", 0 },
                    { 541, "Blue", "TheLunariumConspiracies", 2 },
                    { 542, "Purple", "TheLunariumConspiracies", 0 },
                    { 543, "Pink", "TheLunariumConspiracies", 0 },
                    { 544, "Black", "TheLunariumConspiracies", 8 },
                    { 545, "Red", "TheOnyxxaComplex", 2 },
                    { 546, "Orange", "TheOnyxxaComplex", 9 },
                    { 547, "Yellow", "TheOnyxxaComplex", 3 },
                    { 548, "Green", "TheOnyxxaComplex", 0 },
                    { 549, "Blue", "TheOnyxxaComplex", 6 },
                    { 550, "Purple", "TheOnyxxaComplex", 0 },
                    { 551, "Pink", "TheOnyxxaComplex", 1 },
                    { 552, "Black", "TheOnyxxaComplex", 0 },
                    { 553, "Red", "TheTyrisContinuum", 0 },
                    { 554, "Orange", "TheTyrisContinuum", 1 },
                    { 555, "Yellow", "TheTyrisContinuum", 5 },
                    { 556, "Green", "TheTyrisContinuum", 9 },
                    { 557, "Blue", "TheTyrisContinuum", 4 },
                    { 558, "Purple", "TheTyrisContinuum", 0 },
                    { 559, "Pink", "TheTyrisContinuum", 0 },
                    { 560, "Black", "TheTyrisContinuum", 0 },
                    { 561, "Red", "TheVyserixAscendancy", 0 },
                    { 562, "Orange", "TheVyserixAscendancy", 7 },
                    { 563, "Yellow", "TheVyserixAscendancy", 1 },
                    { 564, "Green", "TheVyserixAscendancy", 0 },
                    { 565, "Blue", "TheVyserixAscendancy", 0 },
                    { 566, "Purple", "TheVyserixAscendancy", 4 },
                    { 567, "Pink", "TheVyserixAscendancy", 5 },
                    { 568, "Black", "TheVyserixAscendancy", 6 },
                    { 569, "Red", "TheZephyrionPursuit", 0 },
                    { 570, "Orange", "TheZephyrionPursuit", 0 },
                    { 571, "Yellow", "TheZephyrionPursuit", 3 },
                    { 572, "Green", "TheZephyrionPursuit", 0 },
                    { 573, "Blue", "TheZephyrionPursuit", 9 },
                    { 574, "Purple", "TheZephyrionPursuit", 7 },
                    { 575, "Pink", "TheZephyrionPursuit", 0 },
                    { 576, "Black", "TheZephyrionPursuit", 6 },
                    { 577, "Red", "AtokeraLegacy", 6 },
                    { 578, "Orange", "AtokeraLegacy", 9 },
                    { 579, "Yellow", "AtokeraLegacy", 5 },
                    { 580, "Green", "AtokeraLegacy", 1 },
                    { 581, "Blue", "AtokeraLegacy", 0 },
                    { 582, "Purple", "AtokeraLegacy", 0 },
                    { 583, "Pink", "AtokeraLegacy", 2 },
                    { 584, "Black", "AtokeraLegacy", 0 },
                    { 585, "Red", "BelkoseaAlliedStates", 0 },
                    { 586, "Orange", "BelkoseaAlliedStates", 0 },
                    { 587, "Yellow", "BelkoseaAlliedStates", 0 },
                    { 588, "Green", "BelkoseaAlliedStates", 4 },
                    { 589, "Blue", "BelkoseaAlliedStates", 6 },
                    { 590, "Purple", "BelkoseaAlliedStates", 7 },
                    { 591, "Pink", "BelkoseaAlliedStates", 0 },
                    { 592, "Black", "BelkoseaAlliedStates", 8 },
                    { 593, "Red", "PharadnOrder", 0 },
                    { 594, "Orange", "PharadnOrder", 9 },
                    { 595, "Yellow", "PharadnOrder", 8 },
                    { 596, "Green", "PharadnOrder", 2 },
                    { 597, "Blue", "PharadnOrder", 0 },
                    { 598, "Purple", "PharadnOrder", 0 },
                    { 599, "Pink", "PharadnOrder", 1 },
                    { 600, "Black", "PharadnOrder", 6 },
                    { 601, "Red", "PrincipalityOfKaltrim", 0 },
                    { 602, "Orange", "PrincipalityOfKaltrim", 0 },
                    { 603, "Yellow", "PrincipalityOfKaltrim", 1 },
                    { 604, "Green", "PrincipalityOfKaltrim", 10 },
                    { 605, "Blue", "PrincipalityOfKaltrim", 0 },
                    { 606, "Purple", "PrincipalityOfKaltrim", 0 },
                    { 607, "Pink", "PrincipalityOfKaltrim", 0 },
                    { 608, "Black", "PrincipalityOfKaltrim", 8 },
                    { 609, "Red", "QhetRepublic", 0 },
                    { 610, "Orange", "QhetRepublic", 0 },
                    { 611, "Yellow", "QhetRepublic", 3 },
                    { 612, "Green", "QhetRepublic", 7 },
                    { 613, "Blue", "QhetRepublic", 8 },
                    { 614, "Purple", "QhetRepublic", 0 },
                    { 615, "Pink", "QhetRepublic", 0 },
                    { 616, "Black", "QhetRepublic", 0 },
                    { 617, "Red", "SarcosaDrift", 0 },
                    { 618, "Orange", "SarcosaDrift", 2 },
                    { 619, "Yellow", "SarcosaDrift", 3 },
                    { 620, "Green", "SarcosaDrift", 0 },
                    { 621, "Blue", "SarcosaDrift", 6 },
                    { 622, "Purple", "SarcosaDrift", 7 },
                    { 623, "Pink", "SarcosaDrift", 9 },
                    { 624, "Black", "SarcosaDrift", 0 },
                    { 625, "Red", "ToldarConcordat", 5 },
                    { 626, "Orange", "ToldarConcordat", 3 },
                    { 627, "Yellow", "ToldarConcordat", 0 },
                    { 628, "Green", "ToldarConcordat", 9 },
                    { 629, "Blue", "ToldarConcordat", 0 },
                    { 630, "Purple", "ToldarConcordat", 1 },
                    { 631, "Pink", "ToldarConcordat", 0 },
                    { 632, "Black", "ToldarConcordat", 6 },
                    { 633, "Red", "UydaiConclave", 0 },
                    { 634, "Orange", "UydaiConclave", 8 },
                    { 635, "Yellow", "UydaiConclave", 2 },
                    { 636, "Green", "UydaiConclave", 0 },
                    { 637, "Blue", "UydaiConclave", 9 },
                    { 638, "Purple", "UydaiConclave", 1 },
                    { 639, "Pink", "UydaiConclave", 0 },
                    { 640, "Black", "UydaiConclave", 3 },
                    { 641, "Red", "XinCourt", 0 },
                    { 642, "Orange", "XinCourt", 5 },
                    { 643, "Yellow", "XinCourt", 7 },
                    { 644, "Green", "XinCourt", 8 },
                    { 645, "Blue", "XinCourt", 0 },
                    { 646, "Purple", "XinCourt", 0 },
                    { 647, "Pink", "XinCourt", 4 },
                    { 648, "Black", "XinCourt", 3 }
                });

            migrationBuilder.InsertData(
                schema: "Technology",
                table: "Technologies",
                columns: new[] { "TechnologyName", "FactionName", "GameVersion", "Id", "IsFactionTechnology", "Level", "Text", "Type" },
                values: new object[,]
                {
                    { "MagenDefenseGridOmegaOmega", "None", "CodexLiberation", 205, "false", "Level1", "", "Warfare" },
                    { "X89BacterialWeaponOmegaOmega", "None", "CodexLiberation", 204, "false", "Level3", "", "Biotic" }
                });

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionTechnology",
                columns: new[] { "FactionName", "TechnologyName", "StartingTechnology" },
                values: new object[,]
                {
                    { "TheArborec", "MagenDefenseGridOmegaOmega", "true" },
                    { "TheCheiranHordes", "MagenDefenseGridOmegaOmega", "true" }
                });

            migrationBuilder.InsertData(
                schema: "Faction",
                table: "Factions",
                columns: new[] { "FactionName", "Action", "Commodities", "ComplexityRating", "GameVersion", "History", "HomeSystem", "Id", "Quote", "SystemInfo", "SystemStats" },
                values: new object[,]
                {
                    { "AtokeraLegacy", "AtokeraLegacy_Action", 3, "Low", "BlueRiverie", "AtokeraLegacy_History", "TileBr01", 73, "AtokeraLegacy_Quote", "AtokeraLegacy_SystemInfo", "AtokeraLegacy_SystemStats" },
                    { "BelkoseaAlliedStates", "BelkoseaAlliedStates_Action", 1, "Low", "BlueRiverie", "BelkoseaAlliedStates_History", "TileBr02", 74, "BelkoseaAlliedStates_Quote", "BelkoseaAlliedStates_SystemInfo", "BelkoseaAlliedStates_SystemStats" },
                    { "PharadnOrder", "PharadnOrder_Action", 2, "Low", "BlueRiverie", "PharadnOrder_History", "TileBr03", 75, "PharadnOrder_Quote", "PharadnOrder_SystemInfo", "PharadnOrder_SystemStats" },
                    { "PrincipalityOfKaltrim", "PrincipalityOfKaltrim_Action", 3, "Low", "BlueRiverie", "PrincipalityOfKaltrim_History", "TileBr04", 76, "PrincipalityOfKaltrim_Quote", "PrincipalityOfKaltrim_SystemInfo", "PrincipalityOfKaltrim_SystemStats" },
                    { "QhetRepublic", "QhetRepublic_Action", 2, "Low", "BlueRiverie", "QhetRepublic_History", "TileBr05", 77, "QhetRepublic_Quote", "QhetRepublic_SystemInfo", "QhetRepublic_SystemStats" },
                    { "SarcosaDrift", "SarcosaDrift_Action", 3, "Low", "BlueRiverie", "SarcosaDrift_History", "TileBr06", 78, "SarcosaDrift_Quote", "SarcosaDrift_SystemInfo", "SarcosaDrift_SystemStats" },
                    { "TheArvaxiCovenant", "TheArvaxiCovenant_Action", 3, "Low", "WhispersFromTheVoid", "TheArvaxiCovenant_History", "TileWftV01", 65, "TheArvaxiCovenant_Quote", "TheArvaxiCovenant_SystemInfo", "TheArvaxiCovenant_SystemStats" },
                    { "TheConstituantsOfXan", "TheConstituantsOfXan_Action", 4, "Medium", "WhispersFromTheVoid", "TheConstituantsOfXan_History", "TileWftV02", 66, "TheConstituantsOfXan_Quote", "TheConstituantsOfXan_SystemInfo", "TheConstituantsOfXan_SystemStats" },
                    { "TheKaloraSwarm", "TheKaloraSwarm_Action", 2, "Medium", "WhispersFromTheVoid", "TheKaloraSwarm_History", "TileWftV03", 67, "TheKaloraSwarm_Quote", "TheKaloraSwarm_SystemInfo", "TheKaloraSwarm_SystemStats" },
                    { "TheLunariumConspiracies", "TheLunariumConspiracies_Action", 3, "High", "WhispersFromTheVoid", "TheLunariumConspiracies_History", "TileWftV04", 68, "TheLunariumConspiracies_Quote", "TheLunariumConspiracies_SystemInfo", "TheLunariumConspiracies_SystemStats" },
                    { "TheOnyxxaComplex", "TheOnyxxaComplex_Action", 3, "High", "WhispersFromTheVoid", "TheOnyxxaComplex_History", "TileWftV05", 69, "TheOnyxxaComplex_Quote", "TheOnyxxaComplex_SystemInfo", "TheOnyxxaComplex_SystemStats" },
                    { "TheTyrisContinuum", "TheTyrisContinuum_Action", 3, "High", "WhispersFromTheVoid", "TheTyrisContinuum_History", "TileWftV06", 70, "TheTyrisContinuum_Quote", "TheTyrisContinuum_SystemInfo", "TheTyrisContinuum_SystemStats" },
                    { "TheVyserixAscendancy", "TheVyserixAscendancy_Action", 3, "Medium", "WhispersFromTheVoid", "TheVyserixAscendancy_History", "TileWftV07", 71, "TheVyserixAscendancy_Quote", "TheVyserixAscendancy_SystemInfo", "TheVyserixAscendancy_SystemStats" },
                    { "TheZephyrionPursuit", "TheZephyrionPursuit_Action", 2, "Low", "WhispersFromTheVoid", "TheZephyrionPursuit_History", "TileWftV08", 72, "TheZephyrionPursuit_Quote", "TheZephyrionPursuit_SystemInfo", "TheZephyrionPursuit_SystemStats" },
                    { "ToldarConcordat", "ToldarConcordat_Action", 3, "Low", "BlueRiverie", "ToldarConcordat_History", "TileBr07", 79, "ToldarConcordat_Quote", "ToldarConcordat_SystemInfo", "ToldarConcordat_SystemStats" },
                    { "UydaiConclave", "UydaiConclave_Action", 5, "Low", "BlueRiverie", "UydaiConclave_History", "TileBr08", 80, "UydaiConclave_Quote", "UydaiConclave_SystemInfo", "UydaiConclave_SystemStats" },
                    { "XinCourt", "XinCourt_Action", 4, "Low", "BlueRiverie", "XinCourt_History", "TileBr09", 81, "XinCourt_Quote", "XinCourt_SystemInfo", "XinCourt_SystemStats" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "FlagshipCards",
                columns: new[] { "Id", "EnumName", "FactionName", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 76, "WrathOfTheCovenant", "TheArvaxiCovenant", "WhispersFromTheVoid", "", "Flagship" },
                    { 77, "AegisAscendant", "TheConstituantsOfXan", "WhispersFromTheVoid", "", "Flagship" },
                    { 78, "RazorsEdge", "TheKaloraSwarm", "WhispersFromTheVoid", "", "Flagship" },
                    { 79, "UmbraVas", "TheLunariumConspiracies", "WhispersFromTheVoid", "", "Flagship" },
                    { 80, "IgneousKhan", "TheOnyxxaComplex", "WhispersFromTheVoid", "", "Flagship" },
                    { 81, "Paradox", "TheTyrisContinuum", "WhispersFromTheVoid", "", "Flagship" },
                    { 82, "TheOblivion", "TheVyserixAscendancy", "WhispersFromTheVoid", "", "Flagship" },
                    { 83, "MarkOfTheHunter", "TheZephyrionPursuit", "WhispersFromTheVoid", "", "Flagship" },
                    { 84, "Kohi", "AtokeraLegacy", "BlueRiverie", "", "Flagship" },
                    { 85, "Ascendancy", "BelkoseaAlliedStates", "BlueRiverie", "", "Flagship" },
                    { 86, "Sedtra", "PharadnOrder", "BlueRiverie", "", "Flagship" },
                    { 87, "JewelOfTheRose", "PrincipalityOfKaltrim", "BlueRiverie", "", "Flagship" },
                    { 88, "Khage", "QhetRepublic", "BlueRiverie", "", "Flagship" },
                    { 89, "Nauclis", "SarcosaDrift", "BlueRiverie", "", "Flagship" },
                    { 90, "Errant", "ToldarConcordat", "BlueRiverie", "", "Flagship" },
                    { 91, "DesertSage", "UydaiConclave", "BlueRiverie", "", "Flagship" },
                    { 92, "CelestialDragon", "XinCourt", "BlueRiverie", "", "Flagship" }
                });

            migrationBuilder.InsertData(
                schema: "Tigl",
                table: "Leaders",
                columns: new[] { "Id", "ChangeCount", "CurrentOwnerId", "Faction", "FirstUpdate", "LastUpdate", "League", "LongestDuration", "LongestHolderId", "Name", "PreviousOwnerId", "PreviousUpdate", "ShortestDuration", "ShortestHolderId" },
                values: new object[,]
                {
                    { 116, 0, null, "TheArvaxiCovenant", null, null, "Fractured", null, null, "KaelenTheInvincibleSpearhead", null, null, null, null },
                    { 117, 0, null, "TheConstituentsOfXan", null, null, "Fractured", null, null, "FlaviusStellariusWarEconomist", null, null, null, null },
                    { 118, 0, null, "TheKaloraSwarm", null, null, "Fractured", null, null, "DreathosTheFinalGuard", null, null, null, null },
                    { 119, 0, null, "TheLunariumConspiracies", null, null, "Fractured", null, null, "AnimaScrutatorVoiceOfTheVeil", null, null, null, null },
                    { 120, 0, null, "TheOnyxxaComplex", null, null, "Fractured", null, null, "GrandmasterXelKorAncientOne", null, null, null, null },
                    { 121, 0, null, "TheTyrisContinuum", null, null, "Fractured", null, null, "FatebreakerAzhurakSelfOrdainedSavior", null, null, null, null },
                    { 122, 0, null, "TheVyserixAscendancy", null, null, "Fractured", null, null, "VaxeluundrTheProphet", null, null, null, null },
                    { 123, 0, null, "TheZephyrionPursuit", null, null, "Fractured", null, null, "MonturakHomotolApexPredator", null, null, null, null },
                    { 124, 0, null, "PrincipalityOfKaltrim", null, null, "Fractured", null, null, "JodekanJodarPrinceOfKaltrim", null, null, null, null },
                    { 125, 0, null, "SarcosaDrift", null, null, "Fractured", null, null, "CardossaFlightPirateQueen", null, null, null, null },
                    { 126, 0, null, "XinCourt", null, null, "Fractured", null, null, "TSaoGrandAdministrator", null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "Tigl",
                table: "PrestigeRanks",
                columns: new[] { "Id", "FactionName", "League", "Name" },
                values: new object[,]
                {
                    { 156, "TheArvaxiCovenant", "Fractured", "KaelenTheInvincibleSpearhead" },
                    { 157, "TheConstituentsOfXan", "Fractured", "FlaviusStellariusWarEconomist" },
                    { 158, "TheKaloraSwarm", "Fractured", "DreathosTheFinalGuard" },
                    { 159, "TheLunariumConspiracies", "Fractured", "AnimaScrutatorVoiceOfTheVeil" },
                    { 160, "TheOnyxxaComplex", "Fractured", "GrandmasterXelKorAncientOne" },
                    { 161, "TheTyrisContinuum", "Fractured", "FatebreakerAzhurakSelfOrdainedSavior" },
                    { 162, "TheVyserixAscendancy", "Fractured", "VaxeluundrTheProphet" },
                    { 163, "TheZephyrionPursuit", "Fractured", "MonturakHomotolApexPredator" },
                    { 164, "PrincipalityOfKaltrim", "Fractured", "JodekanJodarPrinceOfKaltrim" },
                    { 165, "SarcosaDrift", "Fractured", "CardossaFlightPirateQueen" },
                    { 166, "XinCourt", "Fractured", "TSaoGrandAdministrator" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "PromissaryNoteCards",
                columns: new[] { "Id", "EnumName", "Faction", "GameVersion", "Text", "Type" },
                values: new object[,]
                {
                    { 80, "AbsoluteSecrecy", "TheArvaxiCovenant", "WhispersFromTheVoid", "", "Promissary" },
                    { 81, "WarlordsDecree", "TheConstituantsOfXan", "WhispersFromTheVoid", "", "Promissary" },
                    { 82, "SharedTreasure", "TheKaloraSwarm", "WhispersFromTheVoid", "", "Promissary" },
                    { 83, "TheoryRenovation", "TheLunariumConspiracies", "WhispersFromTheVoid", "", "Promissary" },
                    { 84, "SilentAccord", "TheOnyxxaComplex", "WhispersFromTheVoid", "", "Promissary" },
                    { 85, "FatesBargain", "TheTyrisContinuum", "WhispersFromTheVoid", "", "Promissary" },
                    { 86, "LockdownProtocol", "TheVyserixAscendancy", "WhispersFromTheVoid", "", "Promissary" },
                    { 87, "MercenaryDefenseContract", "TheZephyrionPursuit", "WhispersFromTheVoid", "", "Promissary" },
                    { 88, "TriWeaveShielding", "AtokeraLegacy", "BlueRiverie", "", "Promissary" },
                    { 89, "MercenarySortie", "BelkoseaAlliedStates", "BlueRiverie", "", "Promissary" },
                    { 90, "DeathBinding", "PharadnOrder", "BlueRiverie", "", "Promissary" },
                    { 91, "KaltrimAmbassadors", "PrincipalityOfKaltrim", "BlueRiverie", "", "Promissary" },
                    { 92, "AlloyShipment", "QhetRepublic", "BlueRiverie", "", "Promissary" },
                    { 93, "Plunderers", "SarcosaDrift", "BlueRiverie", "", "Promissary" },
                    { 94, "ConcordatAllegiant", "ToldarConcordat", "BlueRiverie", "", "Promissary" },
                    { 95, "EeriePredictions", "UydaiConclave", "BlueRiverie", "", "Promissary" },
                    { 96, "StatecraftMentor", "XinCourt", "BlueRiverie", "", "Promissary" }
                });

            migrationBuilder.InsertData(
                schema: "Card",
                table: "SpecialComponentCards",
                columns: new[] { "Id", "Count", "EnumName", "FactionName", "GameVersion", "SpecialType", "Text", "Type" },
                values: new object[,]
                {
                    { 136, 1, "ArvaxiMobilizationEngineBoon", "TheArvaxiCovenant", "WhispersFromTheVoid", "Other", "", "SpecialComponent" },
                    { 137, 1, "ArvaxiMobilizationEngineCurse", "TheArvaxiCovenant", "WhispersFromTheVoid", "Other", "", "SpecialComponent" },
                    { 138, 1, "GalacticCitadel", "TheConstituantsOfXan", "WhispersFromTheVoid", "FactionUnit", "", "SpecialComponent" },
                    { 139, 1, "StellarArchitect", "TheConstituantsOfXan", "WhispersFromTheVoid", "FactionUnit", "", "SpecialComponent" },
                    { 140, 1, "Stinger", "TheKaloraSwarm", "WhispersFromTheVoid", "FactionUnit", "", "SpecialComponent" },
                    { 141, 1, "CrypticHauler", "TheLunariumConspiracies", "WhispersFromTheVoid", "FactionUnit", "", "SpecialComponent" },
                    { 142, 1, "PhaseTrooper", "TheTyrisContinuum", "WhispersFromTheVoid", "FactionUnit", "", "SpecialComponent" },
                    { 143, 1, "HarmonicBastion", "TheVyserixAscendancy", "WhispersFromTheVoid", "FactionUnit", "", "SpecialComponent" },
                    { 144, 1, "ZephyrHound", "TheZephyrionPursuit", "WhispersFromTheVoid", "FactionUnit", "", "SpecialComponent" },
                    { 145, 3, "BountyToken", "TheZephyrionPursuit", "WhispersFromTheVoid", "Token", "", "SpecialComponent" },
                    { 146, 1, "Mirrorshard", "AtokeraLegacy", "BlueRiverie", "FactionUnit", "", "SpecialComponent" },
                    { 147, 1, "Wyvern", "BelkoseaAlliedStates", "BlueRiverie", "FactionUnit", "", "SpecialComponent" },
                    { 148, 1, "SOLGIndustriesGroup", "BelkoseaAlliedStates", "BlueRiverie", "HorizontalCard", "", "SpecialComponent" },
                    { 149, 1, "AvailynToken", "BelkoseaAlliedStates", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 150, 1, "CaledToken", "BelkoseaAlliedStates", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 151, 1, "GlatisonToken", "BelkoseaAlliedStates", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 152, 1, "GromToken", "BelkoseaAlliedStates", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 153, 1, "MorsToken", "BelkoseaAlliedStates", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 154, 1, "Availyn", "BelkoseaAlliedStates", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 155, 1, "Caled", "BelkoseaAlliedStates", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 156, 1, "Glatison", "BelkoseaAlliedStates", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 157, 1, "Grom", "BelkoseaAlliedStates", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 158, 1, "Mors", "BelkoseaAlliedStates", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 159, 1, "Immortal", "PharadnOrder", "BlueRiverie", "FactionUnit", "", "SpecialComponent" },
                    { 160, 14, "TombToken", "PharadnOrder", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 161, 1, "Consulate", "PrincipalityOfKaltrim", "BlueRiverie", "FactionUnit", "", "SpecialComponent" },
                    { 162, 8, "ShrineCard", "PrincipalityOfKaltrim", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 163, 7, "GenericShrineToken", "PrincipalityOfKaltrim", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 164, 1, "KaltrimCrownShrineToken", "PrincipalityOfKaltrim", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 165, 1, "Commando", "QhetRepublic", "BlueRiverie", "FactionUnit", "", "SpecialComponent" },
                    { 166, 1, "Battlecruiser", "QhetRepublic", "BlueRiverie", "FactionUnit", "", "SpecialComponent" },
                    { 167, 1, "Ravager", "SarcosaDrift", "BlueRiverie", "FactionUnit", "", "SpecialComponent" },
                    { 168, 1, "Leviathan", "SarcosaDrift", "BlueRiverie", "HorizontalCard", "", "SpecialComponent" },
                    { 169, 8, "HonorToken", "ToldarConcordat", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 170, 8, "DishonorToken", "ToldarConcordat", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 171, 1, "Bestow", "ToldarConcordat", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 172, 1, "Thwart", "ToldarConcordat", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 173, 1, "Reflect", "ToldarConcordat", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 174, 1, "Deceive", "ToldarConcordat", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 175, 1, "Ascend", "ToldarConcordat", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 176, 1, "Scourge", "ToldarConcordat", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 177, 8, "PathToken", "UydaiConclave", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 178, 1, "PlanTactician", "UydaiConclave", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 179, 1, "PlanSpymaster", "UydaiConclave", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 180, 1, "Harvest", "UydaiConclave", "BlueRiverie", "HorizontalCard", "", "SpecialComponent" },
                    { 181, 1, "PlanStrategist", "UydaiConclave", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 182, 1, "DeathCommandos", "UydaiConclave", "BlueRiverie", "FactionUnit", "", "SpecialComponent" },
                    { 183, 1, "SentinelSteel", "XinCourt", "BlueRiverie", "FactionUnit", "", "SpecialComponent" },
                    { 184, 8, "StarToken", "XinCourt", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 185, 8, "SteelToken", "XinCourt", "BlueRiverie", "Token", "", "SpecialComponent" },
                    { 186, 1, "LevyTheSectors", "XinCourt", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 187, 1, "LogisticalIntervention", "XinCourt", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 188, 1, "OmenOfTheBlade", "XinCourt", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 189, 1, "RecruitSpies", "XinCourt", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 190, 1, "MeteorGambit", "XinCourt", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 191, 1, "CommissionTheArtisans", "XinCourt", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 192, 1, "DeftLegislation", "XinCourt", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 193, 1, "CallOfTheCosmos", "XinCourt", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 194, 1, "SecureFavor", "XinCourt", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 195, 1, "DeclareMandate", "XinCourt", "BlueRiverie", "VerticalCard", "", "SpecialComponent" },
                    { 196, 1, "ZGravEidolon", "TheNaazRokhaAlliance", "ProphecyOfKings", "FactionUnit", "", "SpecialComponent" }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "SystemTiles",
                columns: new[] { "SystemTileName", "Anomaly", "FactionName", "GameVersion", "Id", "SystemTileCode", "TileCategory" },
                values: new object[,]
                {
                    { "TileBr01", "None", "AtokeraLegacy", "BlueRiverie", 374, "Br01", "Green" },
                    { "TileBr02", "None", "BelkoseaAlliedStates", "BlueRiverie", 375, "Br02", "Green" },
                    { "TileBr03", "None", "PharadnOrder", "BlueRiverie", 376, "Br03", "Green" },
                    { "TileBr04", "None", "PrincipalityOfKaltrim", "BlueRiverie", 377, "Br04", "Green" },
                    { "TileBr05", "None", "QhetRepublic", "BlueRiverie", 378, "Br05", "Green" },
                    { "TileBr06", "None", "SarcosaDrift", "BlueRiverie", 379, "Br06", "Green" },
                    { "TileBr07", "None", "ToldarConcordat", "BlueRiverie", 380, "Br07", "Green" },
                    { "TileBr08", "None", "UydaiConclave", "BlueRiverie", 381, "Br08", "Green" },
                    { "TileBr09", "None", "XinCourt", "BlueRiverie", 382, "Br09", "Green" },
                    { "TileWftV01", "None", "TheArvaxiCovenant", "WhispersFromTheVoid", 366, "WftV01", "Green" },
                    { "TileWftV02", "None", "TheConstituantsOfXan", "WhispersFromTheVoid", 367, "WftV02", "Green" },
                    { "TileWftV03", "None", "TheKaloraSwarm", "WhispersFromTheVoid", 368, "WftV03", "Green" },
                    { "TileWftV04", "None", "TheLunariumConspiracies", "WhispersFromTheVoid", 369, "WftV04", "Green" },
                    { "TileWftV05", "None", "TheOnyxxaComplex", "WhispersFromTheVoid", 370, "WftV05", "Green" },
                    { "TileWftV06", "None", "TheTyrisContinuum", "WhispersFromTheVoid", 371, "WftV06", "Green" },
                    { "TileWftV07", "None", "TheVyserixAscendancy", "WhispersFromTheVoid", 372, "WftV07", "Green" },
                    { "TileWftV08", "None", "TheZephyrionPursuit", "WhispersFromTheVoid", 373, "WftV08", "Green" }
                });

            migrationBuilder.InsertData(
                schema: "Technology",
                table: "Technologies",
                columns: new[] { "TechnologyName", "FactionName", "GameVersion", "Id", "IsFactionTechnology", "Level", "Text", "Type" },
                values: new object[,]
                {
                    { "AstromanticCloakStar", "XinCourt", "BlueRiverie", 201, "true", "Level2", "", "Biotic" },
                    { "AstromanticCloakSteel", "XinCourt", "BlueRiverie", 200, "true", "Level2", "", "Biotic" },
                    { "BattlecruiserTwo", "QhetRepublic", "BlueRiverie", 192, "true", "Level3", "", "UnitUpgrade" },
                    { "CommandoTwo", "QhetRepublic", "BlueRiverie", 193, "true", "Level2", "", "UnitUpgrade" },
                    { "ConduitMatrix", "TheVyserixAscendancy", "WhispersFromTheVoid", 180, "true", "Level3", "", "Propulsion" },
                    { "ConsulateTwo", "PrincipalityOfKaltrim", "BlueRiverie", 190, "true", "Level2", "", "UnitUpgrade" },
                    { "CrypticHaulerTwo", "TheLunariumConspiracies", "WhispersFromTheVoid", 175, "true", "Level2", "", "UnitUpgrade" },
                    { "EmergencyModifications", "ToldarConcordat", "BlueRiverie", 196, "true", "Level1", "", "Propulsion" },
                    { "GalacticCitadelTwo", "TheConstituantsOfXan", "WhispersFromTheVoid", 171, "true", "Level2", "", "UnitUpgrade" },
                    { "HarmonicBastionTwo", "TheVyserixAscendancy", "WhispersFromTheVoid", 181, "true", "Level2", "", "UnitUpgrade" },
                    { "HuntingGrounds", "TheZephyrionPursuit", "WhispersFromTheVoid", 182, "true", "Level1", "", "Cybernetic" },
                    { "ChitinShielding", "TheKaloraSwarm", "WhispersFromTheVoid", 172, "true", "Level2", "", "Warfare" },
                    { "ImmortalTwo", "PharadnOrder", "BlueRiverie", 188, "true", "Level2", "", "UnitUpgrade" },
                    { "InfiltrationNetworks", "SarcosaDrift", "BlueRiverie", 195, "true", "Level2", "", "Biotic" },
                    { "KaldurArrestField", "PrincipalityOfKaltrim", "BlueRiverie", 191, "true", "Level2", "", "Warfare" },
                    { "MessiahProtocols", "UydaiConclave", "BlueRiverie", 198, "true", "Level2", "", "Biotic" },
                    { "MirrorshardTwo", "AtokeraLegacy", "BlueRiverie", 184, "true", "Level3", "", "UnitUpgrade" },
                    { "ModularDesigns", "AtokeraLegacy", "BlueRiverie", 185, "true", "Level3", "", "Cybernetic" },
                    { "NavigationRelays", "UydaiConclave", "BlueRiverie", 199, "true", "Level3", "", "Propulsion" },
                    { "PhaseTrooperTwo", "TheTyrisContinuum", "WhispersFromTheVoid", 179, "true", "Level2", "", "UnitUpgrade" },
                    { "RavagerTwo", "SarcosaDrift", "BlueRiverie", 194, "true", "Level2", "", "UnitUpgrade" },
                    { "RealityFieldImpactor", "PharadnOrder", "BlueRiverie", 189, "true", "Level1", "", "Propulsion" },
                    { "SacrificialCommand", "TheOnyxxaComplex", "WhispersFromTheVoid", 177, "true", "Level2", "", "Cybernetic" },
                    { "StellarArchitectTwo", "TheConstituantsOfXan", "WhispersFromTheVoid", 170, "true", "Level4", "", "UnitUpgrade" },
                    { "StingerTwo", "TheKaloraSwarm", "WhispersFromTheVoid", 173, "true", "Level3", "", "UnitUpgrade" },
                    { "StrategicInversion", "TheOnyxxaComplex", "WhispersFromTheVoid", 176, "true", "Level1", "", "Biotic" },
                    { "SubliminalIntuition", "TheLunariumConspiracies", "WhispersFromTheVoid", 174, "true", "Level2", "", "Biotic" },
                    { "SynchronyMatrix", "BelkoseaAlliedStates", "BlueRiverie", 187, "true", "Level2", "", "Cybernetic" },
                    { "TemporalDisplacement", "TheTyrisContinuum", "WhispersFromTheVoid", 178, "true", "Level1", "", "Cybernetic" },
                    { "UnyieldingWill", "TheArvaxiCovenant", "WhispersFromTheVoid", 168, "true", "Level2", "", "Biotic" },
                    { "VeiledNetworkingStar", "XinCourt", "BlueRiverie", 203, "true", "Level2", "", "Cybernetic" },
                    { "VeiledNetworkingSteel", "XinCourt", "BlueRiverie", 202, "true", "Level2", "", "Cybernetic" },
                    { "VIRTraining", "ToldarConcordat", "BlueRiverie", 197, "true", "Level1", "", "Warfare" },
                    { "WartimeInnovation", "TheArvaxiCovenant", "WhispersFromTheVoid", 169, "true", "Level2", "", "Cybernetic" },
                    { "WyvernTwo", "BelkoseaAlliedStates", "BlueRiverie", 186, "true", "Level2", "", "UnitUpgrade" },
                    { "ZephyrHoundTwo", "TheZephyrionPursuit", "WhispersFromTheVoid", 183, "true", "Level3", "", "UnitUpgrade" }
                });

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionTechnology",
                columns: new[] { "FactionName", "TechnologyName", "StartingTechnology" },
                values: new object[,]
                {
                    { "AtokeraLegacy", "GravitonLaserSystem", "true" },
                    { "AtokeraLegacy", "MagenDefenseGridOmegaOmega", "true" },
                    { "BelkoseaAlliedStates", "AIDevelopmentAlgorithm", "true" },
                    { "BelkoseaAlliedStates", "PlasmaScoring", "true" },
                    { "BelkoseaAlliedStates", "SarweenTools", "true" },
                    { "BelkoseaAlliedStates", "ScanlinkDroneNetwork", "true" },
                    { "PharadnOrder", "IntegratedEconomy", "true" },
                    { "PrincipalityOfKaltrim", "AssaultCannon", "true" },
                    { "QhetRepublic", "SelfAssemblyRoutines", "true" },
                    { "QhetRepublic", "SlingRelay", "true" },
                    { "SarcosaDrift", "LightWaveDeflector", "true" },
                    { "TheArvaxiCovenant", "AIDevelopmentAlgorithm", "true" },
                    { "TheArvaxiCovenant", "NeuralMotivator", "true" },
                    { "TheConstituantsOfXan", "AIDevelopmentAlgorithm", "true" },
                    { "TheConstituantsOfXan", "SarweenTools", "true" },
                    { "TheKaloraSwarm", "DarkEnergyTap", "true" },
                    { "TheKaloraSwarm", "PlasmaScoring", "true" },
                    { "TheLunariumConspiracies", "PredictiveIntelligence", "true" },
                    { "TheLunariumConspiracies", "Psychoarchaeology", "true" },
                    { "TheOnyxxaComplex", "PredictiveIntelligence", "true" },
                    { "TheTyrisContinuum", "DarkEnergyTap", "true" },
                    { "TheTyrisContinuum", "NeuralMotivator", "true" },
                    { "TheVyserixAscendancy", "MagenDefenseGridOmegaOmega", "true" },
                    { "TheVyserixAscendancy", "Psychoarchaeology", "true" },
                    { "TheZephyrionPursuit", "SarweenTools", "true" },
                    { "TheZephyrionPursuit", "SelfAssemblyRoutines", "true" },
                    { "ToldarConcordat", "AntimassDeflectors", "true" },
                    { "ToldarConcordat", "DarkEnergyTap", "true" },
                    { "ToldarConcordat", "NeuralMotivator", "true" },
                    { "ToldarConcordat", "Psychoarchaeology", "true" },
                    { "UydaiConclave", "DacxiveAnimators", "true" },
                    { "UydaiConclave", "GravityDrive", "true" },
                    { "XinCourt", "X89BacterialWeaponOmegaOmega", "true" }
                });

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionUnit",
                columns: new[] { "FactionName", "UnitName", "Count" },
                values: new object[,]
                {
                    { "AtokeraLegacy", "Carrier", 1 },
                    { "AtokeraLegacy", "Destroyer", 1 },
                    { "AtokeraLegacy", "Dreadnought", 1 },
                    { "AtokeraLegacy", "Fighter", 5 },
                    { "AtokeraLegacy", "Infantry", 3 },
                    { "AtokeraLegacy", "SpaceDock", 1 },
                    { "BelkoseaAlliedStates", "Carrier", 1 },
                    { "BelkoseaAlliedStates", "Cruiser", 1 },
                    { "BelkoseaAlliedStates", "Dreadnought", 1 },
                    { "BelkoseaAlliedStates", "Fighter", 5 },
                    { "BelkoseaAlliedStates", "Infantry", 3 },
                    { "BelkoseaAlliedStates", "Pds", 1 },
                    { "BelkoseaAlliedStates", "SpaceDock", 1 },
                    { "PharadnOrder", "Carrier", 1 },
                    { "PharadnOrder", "Cruiser", 1 },
                    { "PharadnOrder", "Dreadnought", 1 },
                    { "PharadnOrder", "Fighter", 2 },
                    { "PharadnOrder", "Infantry", 8 },
                    { "PharadnOrder", "SpaceDock", 1 },
                    { "PrincipalityOfKaltrim", "Carrier", 1 },
                    { "PrincipalityOfKaltrim", "Dreadnought", 1 },
                    { "PrincipalityOfKaltrim", "Fighter", 2 },
                    { "PrincipalityOfKaltrim", "Infantry", 3 },
                    { "PrincipalityOfKaltrim", "SpaceDock", 1 },
                    { "QhetRepublic", "Carrier", 1 },
                    { "QhetRepublic", "Cruiser", 2 },
                    { "QhetRepublic", "Fighter", 2 },
                    { "QhetRepublic", "Infantry", 3 },
                    { "QhetRepublic", "SpaceDock", 1 },
                    { "SarcosaDrift", "Carrier", 1 },
                    { "SarcosaDrift", "Cruiser", 2 },
                    { "SarcosaDrift", "Fighter", 4 },
                    { "SarcosaDrift", "Infantry", 3 },
                    { "SarcosaDrift", "SpaceDock", 1 },
                    { "TheArvaxiCovenant", "Carrier", 1 },
                    { "TheArvaxiCovenant", "Cruiser", 2 },
                    { "TheArvaxiCovenant", "Fighter", 2 },
                    { "TheArvaxiCovenant", "Infantry", 3 },
                    { "TheArvaxiCovenant", "Pds", 1 },
                    { "TheArvaxiCovenant", "SpaceDock", 1 },
                    { "TheConstituantsOfXan", "Carrier", 2 },
                    { "TheConstituantsOfXan", "Destroyer", 2 },
                    { "TheConstituantsOfXan", "Fighter", 2 },
                    { "TheConstituantsOfXan", "Infantry", 4 },
                    { "TheConstituantsOfXan", "SpaceDock", 1 },
                    { "TheKaloraSwarm", "Carrier", 1 },
                    { "TheKaloraSwarm", "Cruiser", 1 },
                    { "TheKaloraSwarm", "Dreadnought", 1 },
                    { "TheKaloraSwarm", "Fighter", 1 },
                    { "TheKaloraSwarm", "Infantry", 4 },
                    { "TheKaloraSwarm", "SpaceDock", 1 },
                    { "TheLunariumConspiracies", "Carrier", 1 },
                    { "TheLunariumConspiracies", "Destroyer", 1 },
                    { "TheLunariumConspiracies", "Dreadnought", 1 },
                    { "TheLunariumConspiracies", "Fighter", 2 },
                    { "TheLunariumConspiracies", "Infantry", 5 },
                    { "TheLunariumConspiracies", "SpaceDock", 1 },
                    { "TheOnyxxaComplex", "Carrier", 1 },
                    { "TheOnyxxaComplex", "Destroyer", 1 },
                    { "TheOnyxxaComplex", "Dreadnought", 1 },
                    { "TheOnyxxaComplex", "Fighter", 2 },
                    { "TheOnyxxaComplex", "Infantry", 3 },
                    { "TheOnyxxaComplex", "SpaceDock", 1 },
                    { "TheTyrisContinuum", "Carrier", 2 },
                    { "TheTyrisContinuum", "Destroyer", 1 },
                    { "TheTyrisContinuum", "Fighter", 3 },
                    { "TheTyrisContinuum", "Infantry", 3 },
                    { "TheTyrisContinuum", "SpaceDock", 1 },
                    { "TheVyserixAscendancy", "Carrier", 1 },
                    { "TheVyserixAscendancy", "Destroyer", 2 },
                    { "TheVyserixAscendancy", "Fighter", 2 },
                    { "TheVyserixAscendancy", "Infantry", 4 },
                    { "TheVyserixAscendancy", "Pds", 1 },
                    { "TheVyserixAscendancy", "SpaceDock", 1 },
                    { "TheZephyrionPursuit", "Carrier", 1 },
                    { "TheZephyrionPursuit", "Cruiser", 2 },
                    { "TheZephyrionPursuit", "Fighter", 2 },
                    { "TheZephyrionPursuit", "Infantry", 3 },
                    { "TheZephyrionPursuit", "Pds", 1 },
                    { "TheZephyrionPursuit", "SpaceDock", 1 },
                    { "ToldarConcordat", "Carrier", 1 },
                    { "ToldarConcordat", "Destroyer", 1 },
                    { "ToldarConcordat", "Dreadnought", 1 },
                    { "ToldarConcordat", "Fighter", 1 },
                    { "ToldarConcordat", "Infantry", 3 },
                    { "ToldarConcordat", "SpaceDock", 1 },
                    { "UydaiConclave", "Carrier", 3 },
                    { "UydaiConclave", "Fighter", 4 },
                    { "UydaiConclave", "Infantry", 5 },
                    { "XinCourt", "Carrier", 1 },
                    { "XinCourt", "Cruiser", 2 },
                    { "XinCourt", "Fighter", 5 },
                    { "XinCourt", "Infantry", 4 },
                    { "XinCourt", "SpaceDock", 1 }
                });

            migrationBuilder.InsertData(
                schema: "Galaxy",
                table: "Planets",
                columns: new[] { "PlanetName", "GameVersion", "Id", "Influence", "IsLegendary", "PlanetTrait", "Resources", "SystemTileName", "TechnologySkip" },
                values: new object[,]
                {
                    { "AvKHo", "BlueRiverie", 359, 2, "false", "None", 2, "TileBr04", "None" },
                    { "Conviction", "BlueRiverie", 356, 0, "true", "LegendarySpaceStation", 1, "TileBr02", "None" },
                    { "Drakkol", "BlueRiverie", 361, 1, "false", "None", 1, "TileBr05", "None" },
                    { "Dralius", "WhispersFromTheVoid", 354, 2, "false", "None", 2, "TileWftV08", "None" },
                    { "Dyrun", "BlueRiverie", 365, 1, "false", "None", 3, "TileBr07", "None" },
                    { "HexaraTwo", "WhispersFromTheVoid", 348, 3, "false", "None", 0, "TileWftV06", "None" },
                    { "Iterum", "BlueRiverie", 357, 2, "false", "None", 3, "TileBr02", "None" },
                    { "Kaltrim", "BlueRiverie", 360, 1, "false", "None", 3, "TileBr04", "None" },
                    { "Khodd", "BlueRiverie", 364, 4, "false", "None", 2, "TileBr07", "None" },
                    { "Krax", "WhispersFromTheVoid", 342, 1, "false", "None", 5, "TileWftV02", "None" },
                    { "Lirastra", "WhispersFromTheVoid", 340, 2, "false", "None", 2, "TileWftV01", "None" },
                    { "Luna", "WhispersFromTheVoid", 344, 2, "false", "None", 1, "TileWftV03", "None" },
                    { "Magna", "BlueRiverie", 355, 4, "false", "None", 4, "TileBr01", "None" },
                    { "Phar", "BlueRiverie", 358, 0, "false", "None", 1, "TileBr03", "None" },
                    { "Qhet", "BlueRiverie", 362, 2, "false", "None", 3, "TileBr05", "None" },
                    { "Sarcosa", "BlueRiverie", 363, 3, "false", "None", 3, "TileBr06", "None" },
                    { "TalaNine", "WhispersFromTheVoid", 350, 0, "false", "None", 1, "TileWftV06", "None" },
                    { "TerraNovus", "WhispersFromTheVoid", 343, 0, "false", "None", 3, "TileWftV03", "None" },
                    { "Thar", "WhispersFromTheVoid", 352, 1, "false", "None", 1, "TileWftV07", "None" },
                    { "Uikos", "BlueRiverie", 366, 1, "true", "Legendary", 1, "TileBr08", "None" },
                    { "VaelThunn", "WhispersFromTheVoid", 347, 1, "false", "None", 1, "TileWftV05", "None" },
                    { "Velarune", "WhispersFromTheVoid", 345, 4, "false", "None", 3, "TileWftV04", "None" },
                    { "Vraxil", "WhispersFromTheVoid", 341, 1, "false", "None", 1, "TileWftV01", "None" },
                    { "VyseraThree", "WhispersFromTheVoid", 349, 1, "false", "None", 2, "TileWftV06", "Propulsion" },
                    { "Xan", "WhispersFromTheVoid", 351, 4, "false", "None", 2, "TileWftV07", "None" },
                    { "XelVorr", "WhispersFromTheVoid", 346, 1, "false", "None", 3, "TileWftV05", "None" },
                    { "Yu", "BlueRiverie", 367, 5, "false", "None", 3, "TileBr09", "None" },
                    { "Zephyrax", "WhispersFromTheVoid", 353, 0, "false", "None", 2, "TileWftV08", "None" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "BreakthroughCards",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 513);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 514);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 515);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 516);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 517);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 518);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 519);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 520);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 521);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 522);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 523);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 524);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 525);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 526);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 527);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 528);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 529);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 530);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 531);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 532);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 533);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 534);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 535);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 536);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 537);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 538);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 539);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 540);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 541);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 542);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 543);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 544);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 545);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 546);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 547);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 548);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 549);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 550);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 551);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 552);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 553);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 554);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 555);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 556);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 557);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 558);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 559);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 560);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 561);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 562);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 563);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 564);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 565);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 566);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 567);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 568);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 569);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 570);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 571);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 572);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 573);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 574);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 575);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 576);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 577);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 578);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 579);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 580);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 581);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 582);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 583);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 584);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 585);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 586);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 587);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 588);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 589);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 590);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 591);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 592);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 593);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 594);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 595);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 596);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 597);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 598);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 599);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 600);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 601);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 602);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 603);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 604);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 605);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 606);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 607);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 608);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 609);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 610);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 611);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 612);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 613);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 614);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 615);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 616);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 617);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 618);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 619);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 620);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 621);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 622);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 623);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 624);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 625);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 626);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 627);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 628);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 629);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 630);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 631);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 632);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 633);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 634);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 635);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 636);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 637);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 638);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 639);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 640);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 641);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 642);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 643);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 644);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 645);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 646);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 647);

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "FactionColorImportances",
                keyColumn: "Id",
                keyValue: 648);

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "AtokeraLegacy", "GravitonLaserSystem" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "AtokeraLegacy", "MagenDefenseGridOmegaOmega" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "AIDevelopmentAlgorithm" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "PlasmaScoring" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "SarweenTools" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "ScanlinkDroneNetwork" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "PharadnOrder", "IntegratedEconomy" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "PrincipalityOfKaltrim", "AssaultCannon" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "QhetRepublic", "SelfAssemblyRoutines" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "QhetRepublic", "SlingRelay" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "SarcosaDrift", "LightWaveDeflector" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheArborec", "MagenDefenseGridOmegaOmega" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheArvaxiCovenant", "AIDevelopmentAlgorithm" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheArvaxiCovenant", "NeuralMotivator" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheConstituantsOfXan", "AIDevelopmentAlgorithm" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheConstituantsOfXan", "SarweenTools" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheCheiranHordes", "MagenDefenseGridOmegaOmega" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheKaloraSwarm", "DarkEnergyTap" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheKaloraSwarm", "PlasmaScoring" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheLunariumConspiracies", "PredictiveIntelligence" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheLunariumConspiracies", "Psychoarchaeology" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheOnyxxaComplex", "PredictiveIntelligence" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheTyrisContinuum", "DarkEnergyTap" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheTyrisContinuum", "NeuralMotivator" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheVyserixAscendancy", "MagenDefenseGridOmegaOmega" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheVyserixAscendancy", "Psychoarchaeology" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheZephyrionPursuit", "SarweenTools" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "TheZephyrionPursuit", "SelfAssemblyRoutines" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "ToldarConcordat", "AntimassDeflectors" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "ToldarConcordat", "DarkEnergyTap" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "ToldarConcordat", "NeuralMotivator" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "ToldarConcordat", "Psychoarchaeology" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "UydaiConclave", "DacxiveAnimators" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "UydaiConclave", "GravityDrive" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionTechnology",
                keyColumns: new[] { "FactionName", "TechnologyName" },
                keyValues: new object[] { "XinCourt", "X89BacterialWeaponOmegaOmega" });

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "MagenDefenseGridOmegaOmega");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "X89BacterialWeaponOmegaOmega");

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "AtokeraLegacy", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "AtokeraLegacy", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "AtokeraLegacy", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "AtokeraLegacy", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "AtokeraLegacy", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "AtokeraLegacy", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "Cruiser" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "Pds" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "BelkoseaAlliedStates", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PharadnOrder", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PharadnOrder", "Cruiser" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PharadnOrder", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PharadnOrder", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PharadnOrder", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PharadnOrder", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PrincipalityOfKaltrim", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PrincipalityOfKaltrim", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PrincipalityOfKaltrim", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PrincipalityOfKaltrim", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "PrincipalityOfKaltrim", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "QhetRepublic", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "QhetRepublic", "Cruiser" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "QhetRepublic", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "QhetRepublic", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "QhetRepublic", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "SarcosaDrift", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "SarcosaDrift", "Cruiser" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "SarcosaDrift", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "SarcosaDrift", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "SarcosaDrift", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheArvaxiCovenant", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheArvaxiCovenant", "Cruiser" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheArvaxiCovenant", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheArvaxiCovenant", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheArvaxiCovenant", "Pds" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheArvaxiCovenant", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituantsOfXan", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituantsOfXan", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituantsOfXan", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituantsOfXan", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheConstituantsOfXan", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheKaloraSwarm", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheKaloraSwarm", "Cruiser" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheKaloraSwarm", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheKaloraSwarm", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheKaloraSwarm", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheKaloraSwarm", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheLunariumConspiracies", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheLunariumConspiracies", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheLunariumConspiracies", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheLunariumConspiracies", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheLunariumConspiracies", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheLunariumConspiracies", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheOnyxxaComplex", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheOnyxxaComplex", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheOnyxxaComplex", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheOnyxxaComplex", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheOnyxxaComplex", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheOnyxxaComplex", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheTyrisContinuum", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheTyrisContinuum", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheTyrisContinuum", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheTyrisContinuum", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheTyrisContinuum", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheVyserixAscendancy", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheVyserixAscendancy", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheVyserixAscendancy", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheVyserixAscendancy", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheVyserixAscendancy", "Pds" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheVyserixAscendancy", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheZephyrionPursuit", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheZephyrionPursuit", "Cruiser" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheZephyrionPursuit", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheZephyrionPursuit", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheZephyrionPursuit", "Pds" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "TheZephyrionPursuit", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "ToldarConcordat", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "ToldarConcordat", "Destroyer" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "ToldarConcordat", "Dreadnought" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "ToldarConcordat", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "ToldarConcordat", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "ToldarConcordat", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "UydaiConclave", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "UydaiConclave", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "UydaiConclave", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "XinCourt", "Carrier" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "XinCourt", "Cruiser" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "XinCourt", "Fighter" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "XinCourt", "Infantry" });

            migrationBuilder.DeleteData(
                schema: "Relationships",
                table: "FactionUnit",
                keyColumns: new[] { "FactionName", "UnitName" },
                keyValues: new object[] { "XinCourt", "SpaceDock" });

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "FlagshipCards",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "Leaders",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "AvKHo");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Conviction");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Drakkol");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Dralius");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Dyrun");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "HexaraTwo");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Iterum");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Kaltrim");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Khodd");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Krax");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Lirastra");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Luna");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Magna");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Phar");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Qhet");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Sarcosa");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "TalaNine");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "TerraNovus");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Thar");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Uikos");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "VaelThunn");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Velarune");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Vraxil");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "VyseraThree");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Xan");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "XelVorr");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Yu");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "Planets",
                keyColumn: "PlanetName",
                keyValue: "Zephyrax");

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                schema: "Tigl",
                table: "PrestigeRanks",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "PromissaryNoteCards",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                schema: "Card",
                table: "SpecialComponentCards",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "AstromanticCloakStar");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "AstromanticCloakSteel");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "BattlecruiserTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "CommandoTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "ConduitMatrix");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "ConsulateTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "CrypticHaulerTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "EmergencyModifications");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "GalacticCitadelTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "HarmonicBastionTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "HuntingGrounds");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "ChitinShielding");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "ImmortalTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "InfiltrationNetworks");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "KaldurArrestField");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "MessiahProtocols");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "MirrorshardTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "ModularDesigns");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "NavigationRelays");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "PhaseTrooperTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "RavagerTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "RealityFieldImpactor");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "SacrificialCommand");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "StellarArchitectTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "StingerTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "StrategicInversion");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "SubliminalIntuition");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "SynchronyMatrix");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "TemporalDisplacement");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "UnyieldingWill");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "VeiledNetworkingStar");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "VeiledNetworkingSteel");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "VIRTraining");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "WartimeInnovation");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "WyvernTwo");

            migrationBuilder.DeleteData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "ZephyrHoundTwo");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "AtokeraLegacy");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "BelkoseaAlliedStates");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "PharadnOrder");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "PrincipalityOfKaltrim");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "QhetRepublic");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "SarcosaDrift");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheArvaxiCovenant");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheConstituantsOfXan");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheKaloraSwarm");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheLunariumConspiracies");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheOnyxxaComplex");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheTyrisContinuum");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheVyserixAscendancy");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "TheZephyrionPursuit");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "ToldarConcordat");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "UydaiConclave");

            migrationBuilder.DeleteData(
                schema: "Faction",
                table: "Factions",
                keyColumn: "FactionName",
                keyValue: "XinCourt");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileBr01");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileBr02");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileBr03");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileBr04");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileBr05");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileBr06");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileBr07");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileBr08");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileBr09");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileWftV01");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileWftV02");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileWftV03");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileWftV04");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileWftV05");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileWftV06");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileWftV07");

            migrationBuilder.DeleteData(
                schema: "Galaxy",
                table: "SystemTiles",
                keyColumn: "SystemTileName",
                keyValue: "TileWftV08");

            migrationBuilder.AlterColumn<string>(
                name: "PlanetTrait",
                schema: "Galaxy",
                table: "Planets",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.UpdateData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "SelfAssemblyRoutines",
                column: "GameVersion",
                value: "ProphecyOfKings");

            migrationBuilder.UpdateData(
                schema: "Technology",
                table: "Technologies",
                keyColumn: "TechnologyName",
                keyValue: "X89BacterialWeaponOmega",
                column: "GameVersion",
                value: "BaseGame");

            migrationBuilder.InsertData(
                schema: "Relationships",
                table: "FactionTechnology",
                columns: new[] { "FactionName", "TechnologyName", "StartingTechnology" },
                values: new object[,]
                {
                    { "TheArborec", "MagenDefenseGridOmega", "true" },
                    { "TheCheiranHordes", "MagenDefenseGridOmega", "true" }
                });
        }
    }
}
