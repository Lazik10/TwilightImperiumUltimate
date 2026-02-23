using System.Globalization;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Tigl.Discord;

public static class DiscordRoleMappings
{
    public static readonly IReadOnlyDictionary<TiglPrestigeRank, DiscordRoleInfo> PrestigeRoles =
        new Dictionary<TiglPrestigeRank, DiscordRoleInfo>
        {
            // Leaders
            { TiglPrestigeRank.LetaniMiasmiala, Make("TIGL - Letani Miasmiala", "1276264769409781911", "#816de6", "LetaniMiasmiala:1443389203722539018") },
            { TiglPrestigeRank.DarktalonTreilla, Make("TIGL - Darktalon Treilla", "1288168814797787210", "#816de6", "DarktalonTreilla:1443389336744759386") },
            { TiglPrestigeRank.GurnoAggero, Make("TIGL - Gurno Aggero", "1288168773144023132", "#816de6", "GurnoAggero:1443389509474582619") },
            { TiglPrestigeRank.AdjudicatorBaal, Make("TIGL - Adjudicator Ba'al", "1272999332978626703", "#816de6", "AdjudicatorBaal:1443390234480869497") },
            { TiglPrestigeRank.HarrughGefhara, Make("TIGL - Harrugh Gefhara", "1288169692699164772", "#816de6", "HarrughGefhara:1443390372863676653") },
            { TiglPrestigeRank.JaceX4thAirLegion, Make("TIGL - Jace X, 4th Air Legion", "1252648922745278514", "#816de6", "JaceX4thAirLegion:1443390473594212432") },
            { TiglPrestigeRank.RiftwalkerMeian, Make("TIGL - Riftwalker Meian", "1288169578488135730", "#816de6", "RiftwalkerMeian:1443390599444303872") },
            { TiglPrestigeRank.TheHelmsman, Make("TIGL - The Helmsman", "1288169673396977725", "#816de6", "TheHelmsman:1443390691714662461") },
            { TiglPrestigeRank.IpswitchLooseCannon, Make("TIGL - Ipswitch, Loose Cannon", "1288169681148182569", "#816de6", "IpswitchLooseCannon:1443390792558186556") },
            { TiglPrestigeRank.TheOracle, Make("TIGL - The Oracle", "1275461047163289630", "#816de6", "TheOracle:1443390887496388723") },
            { TiglPrestigeRank.UNITDSGNFLAYESH, Make("TIGL - UNIT.DSGN.FLAYESH", "1285084515752087736", "#816de6", "UNITDSGNFLAYESH:1443390993691840512") },
            { TiglPrestigeRank.ShvalHarbinger, Make("TIGL - Sh'val, Harbinger", "1254824548096933929", "#816de6", "ShvalHarbinger:1443391248210854018") },
            { TiglPrestigeRank.RinTheMastersLegacy, Make("TIGL - Rin, The Master's Legacy", "1283794468008493122", "#816de6", "RinTheMastersLegacy:1443391362144665681") },
            { TiglPrestigeRank.MathisMathinus, Make("TIGL - Mathis Mathinus", "1234988559438581861", "#816de6", "MathisMathinus:1443391475420364890") },
            { TiglPrestigeRank.XxekirGrom, Make("TIGL - Xxekir Grom", "1288169695299502142", "#816de6", "XxekirGrom:1443391567812362421") },
            { TiglPrestigeRank.DannelOfTheTenth, Make("TIGL - Dannel of the Tenth", "1287035313256136785", "#816de6", "DannelOfTheTenth:1443391652457877524") },
            { TiglPrestigeRank.KyverBladeAndKey, Make("TIGL - Kyver, Blade and Key", "1242217137850810439", "#816de6", "KyverBladeAndKey:1443391771882033417") },

            // Argent / PoK extra
            { TiglPrestigeRank.MirikAunSissiri, Make("TIGL - Mirik Aun Sissiri", "1282864241174642793", "#816de6", "MirikAunSissiri:1443391863976624219") },
            { TiglPrestigeRank.ConservatorProcyon, Make("TIGL - Conservator Procyon", "1277729896935723081", "#816de6", "ConservatorProcyon:1443391960353345546") },
            { TiglPrestigeRank.AiroShirAur, Make("TIGL - Airo Shir Aur", "1254488587001663569", "#816de6", "AiroShirAur:1443392094168289300") },
            { TiglPrestigeRank.HeshAndPrit, Make("TIGL - Hesh and Prit", "1263960652175900838", "#816de6", "HeshAndPrit:1443392175734788116") },
            { TiglPrestigeRank.AhkSylSiven, Make("TIGL - Ahk-Syl Siven", "1272999680879362100", "#816de6", "AhkSylSiven:1443392268231770233") },
            { TiglPrestigeRank.UlTheProgenitor, Make("TIGL - Ul the Progenitor", "1288169683735806075", "#816de6", "UlTheProgenitor:1443392370606477322") },
            { TiglPrestigeRank.ItFeedsOnCarrion, Make("TIGL - It Feeds on Carrion", "1288169687355625772", "#816de6", "ItFeedsOnCarrion:1443392475669463190") },

            // Council Keleres
            { TiglPrestigeRank.KuuasiAunJalatai, Make("TIGL - Kuuasi Aun Jalatai", "1252648551943639132", "#816de6", "KuuasiAunJalatai:1443392960396787792") },
            { TiglPrestigeRank.HarkaLeeds, Make("TIGL - Harka Leeds", "1272999113729900554", "#816de6", "HarkaLeeds:1443393052386525185") },
            { TiglPrestigeRank.OdlynnMyrr, Make("TIGL - Odlynn Myrr", "1288169690094506005", "#816de6", "OdlynnMyrr:1443393126109544539") },

            // Thunder's Edge (partial mapping entries available)
            { TiglPrestigeRank.Entity4X41AApollo, Make("TIGL - Entity 4X41A Apollo", "1441463763441291426", "#816de6", string.Empty) },
            { TiglPrestigeRank.DirectorNel, Make("TIGL - Director Nel", "1441206005253017630", "#816de6", string.Empty) },
            { TiglPrestigeRank.TaZern, Make("TIGL - Ta Zern", "1441205182540021781", "#816de6", string.Empty) },
            { TiglPrestigeRank.HomesickPhantom, Make("TIGL - Homesick Phantom", "1441463759490121959", "#816de6", string.Empty) },
            { TiglPrestigeRank.Sharsiss, Make("TIGL - Sharsiss", "1441463524416163982", "#816de6", string.Empty) },

            // Twilight's Fall
            { TiglPrestigeRank.TheRubyMonarch, Make("TIGL - The Ruby Monarch", "0", "#000000", "TheRubyMonarch:1443398164605440010") },
            { TiglPrestigeRank.RadiantAur, Make("TIGL - Radiant Aur", "0", "#000000", "RadiantAur:1443398258197139477") },
            { TiglPrestigeRank.AvariceRex, Make("TIGL - Avarice Rex", "0", "#000000", "AvariceRex:1443398332801089548") },
            { TiglPrestigeRank.IlSaiLakoeHeraldOfThorns, Make("TIGL - Il Sai Lakoe, Herald Of Thorns", "0", "#000000", "IlSaiLakoeHeraldOfThorns:1443398404871819274") },
            { TiglPrestigeRank.TheSaintOfSwords, Make("TIGL - The Saint Of Swords", "0", "#000000", "TheSaintOfSwords:1443398489424924864") },
            { TiglPrestigeRank.IlNaViroset, Make("TIGL - Il Na Viroset", "0", "#000000", "IlNaViroset:1443398562783428660") },
            { TiglPrestigeRank.ElNenJanovet, Make("TIGL - El Nen Janovet", "0", "#000000", "ElNenJanovet:1443398649026449601") },
            { TiglPrestigeRank.ASickeningLurch, Make("TIGL - A Sickening Lurch", "0", "#000000", "ASickeningLurch:1443398719461396601") },

            // Discordant Stars
            { TiglPrestigeRank.AtrophaWeaver, Make("TIGL - Atropha - Weaver", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.TitusFlaviusCouncilman, Make("TIGL - Titus Flavius - Councilman", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.VerrisusYpruFormerAdmiralOfTheUnrelentingBattlegroup, Make("TIGL - Verrisus Ypru - Former Admiral of the Unrelenting Battlegroup", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.BanuaGowenAdministratorOfMinds, Make("TIGL - Banua Gowen - Administrator of Minds", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.CountOttoPmayInspiringRhetorician, Make("TIGL - Count Otto P'may - Inspiring Rhetorician", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.KorelaTheLadyAndKantrusTheLord, Make("TIGL - Korela - The Lady and Kantrus - The Lord", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.BayanDeepMagenta, Make("TIGL - Bayan - Deep Magenta", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.DorrahnGriphynTheCollector, Make("TIGL - Dorrahn Griphyn - The Collector", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.QueenNadaliaLifeAndDeath, Make("TIGL - Queen Nadalia - Life and Death", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.KhazRinLiZhoEmpress, Make("TIGL - Khaz-Rin Li-Zho - Empress", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.VehlTikarArchDruid, Make("TIGL - Vehl-Tikar - Arch Druid", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.WrathMachinaAIMainframe, Make("TIGL - Wrath Machina - AI Mainframe", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.CoprinusComatusNecromancer, Make("TIGL - Coprinus Comatus - Necromancer", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.KrillDrakkonStarCrownedKing, Make("TIGL - Krill Drakkon - Star-Crowned King", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.PahnSilverfurCouncilSpeaker, Make("TIGL - Pahn Silverfur - Council Speaker", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.RohVhinDhnaMK4RuthlessExecutive, Make("TIGL - Roh'Vhin Dhna MK4 - Ruthless Executive", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.TheVoiceUnitedPsionicMaelstrom, Make("TIGL - The Voice United - Psionic Maelstrom", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.DemiQueenMdckssskCommissionerOfProfits, Make("TIGL - Demi-Queen Mdcksssk - Commissioner of Profits", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.TurraSveyarShadowCouncilor, Make("TIGL - Turra Sveyar - Shadow Councilor", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.PutrivSirvonskClanmasterPrime, Make("TIGL - Putriv Sirvonsk - Clanmaster Prime", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.DylnHarthuulViceAdmiralOfFleetGroup15, Make("TIGL - Dyln Harthuul - Vice Admiral of Fleet Group 15", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.AuberonElyrinChairman, Make("TIGL - Auberon Elyrin - Chairman", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.SaintBinalTheProphet, Make("TIGL - Saint Binal - The Prophet", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.ZelianRTheDestroyer, Make("TIGL - Zelian R - The Destroyer", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.CEOKenTuccVisionaryExplorer, Make("TIGL - C.E.O. Ken Tucc - Visionary Explorer", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.ThaktClquaPolemarch, Make("TIGL - Thakt Clqua - Polemarch", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.MidirLivingWill, Make("TIGL - Midir - Living Will", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.NmenmedeGhotiAllMother, Make("TIGL - Nmenmede - Ghoti All Mother", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.GorthrimChiefOfExpeditions, Make("TIGL - Gorthrim - Chief of Expeditions", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.YgegnadTheThunderHonorarySkald, Make("TIGL - Ygegnad, The Thunder - Honorary Skald", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.WonellTheSilentGrandmasterOfTheOrder, Make("TIGL - Wonell The Silent - Grandmaster of the Order", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.SpeyghBlightmaster, Make("TIGL - Speygh - Blightmaster", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.TheVenerableKeeperOfMyths, Make("TIGL - The Venerable - Keeper of Myths", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.StarsailsMercenaryKing, Make("TIGL - Starsails - Mercenary King", "0", "#000000", string.Empty) },

            // Blue Riverie
            { TiglPrestigeRank.KapokoVuiGreatSpirit, Make("TIGL - Kapoko Vui - Great Spirit", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.MobiusSpikeTheReaper, Make("TIGL - Mobius Spike - The Reaper", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.PharadnTheImmortalImperishableUnifier, Make("TIGL - Pharad'n The Immortal - Imperishable Unifier", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.TvorKhageShieldOfQhet, Make("TIGL - Tvor Khage - Shield of Qhet", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.CadhAcamontConcordatMarshall, Make("TIGL - Cadh Acamont - Concordat Marshall", "0", "#000000", string.Empty) },
            { TiglPrestigeRank.LondorIIGodEmperor, Make("TIGL - Londor II - God Emperor", "0", "#000000", string.Empty) },

            // Galactic Emperor winner
            { TiglPrestigeRank.DeposedEmperor, Make("TIGL - Deposed Emperor", "0", "#000000", string.Empty) },

            // Standard/Shattered prestige ranks
            { TiglPrestigeRank.GalacticThreat, Make("TIGL - Galactic Threat", "1307040803935223868", "#ba6de6", string.Empty) },
            { TiglPrestigeRank.PaxMagnificaBellumGloriosum, Make("TIGL - Pax Magnifica Bellum Gloriosum", "1441463529411575990", "#e91e63", string.Empty) },
            { TiglPrestigeRank.Tyrant, Make("TIGL - Tyrant", "1441466170115883038", "#f65904", string.Empty) },

            // Other
            { TiglPrestigeRank.Homebrew, Make("TIGL - Homebrew", "0", "#f65904", string.Empty) },
            { TiglPrestigeRank.Franken, Make("TIGL - Franken", "0", "#f65904", string.Empty) },
        };

    public static readonly IReadOnlyDictionary<TiglRankName, DiscordRoleInfo> RankRoles =
        new Dictionary<TiglRankName, DiscordRoleInfo>
        {
            { TiglRankName.Unranked, Make("TIGL - Unranked", "0", "#000000", string.Empty) },

            { TiglRankName.Minister, Make("TIGL - Minister", "1171907111110844416", "#3498db", string.Empty) },
            { TiglRankName.Agent, Make("TIGL - Agent", "1171907881096970271", "#206694", string.Empty) },
            { TiglRankName.Commander, Make("TIGL - Commander", "1199486443928686592", "#b2a8e4", string.Empty) },
            { TiglRankName.Hero, Make("TIGL - Hero", "1234988242391273603", "#816de6", string.Empty) },

            { TiglRankName.Thrall, Make("TIGL - Thrall", "1441466188688261181", "#df966e", string.Empty) },
            { TiglRankName.Acolyte, Make("TIGL - Acolyte", "1441466159554887831", "#ce8863", string.Empty) },
            { TiglRankName.Legionnaire, Make("TIGL - Legionnaire", "1441466181679583303", "#c3764c", string.Empty) },
            { TiglRankName.Starlancer, Make("TIGL - Starlancer", "1441466166093680672", "#c26b3d", string.Empty) },
            { TiglRankName.GeneSorcerer, Make("TIGL - Gene-Sorcerer", "1441466177883734147", "#d6682d", string.Empty) },
            { TiglRankName.IxthLord, Make("TIGL - Ixth-Lord", "1441466163082297544", "#d66223", string.Empty) },
            { TiglRankName.Archon, Make("TIGL - Archon", "1441466185425223681", "#ec651c", string.Empty) },
        };

    private static DiscordRoleInfo Make(string roleName, string roleId, string colorHex, string emojiId)
    {
        return new DiscordRoleInfo
        {
            RoleName = roleName,
            RoleId = roleId,
            ColorHex = colorHex,
            ColorRgb = ParseHexColor(colorHex),
            EmojiId = emojiId,
        };
    }

    private static int ParseHexColor(string hex)
    {
        var clean = hex.Trim().TrimStart('#');
        if (clean.Length != 6)
            return 0;

        return int.Parse(clean, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    }
}
