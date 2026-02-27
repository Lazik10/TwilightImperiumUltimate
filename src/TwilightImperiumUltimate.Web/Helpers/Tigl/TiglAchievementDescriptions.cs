using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Helpers.Tigl;

public static class TiglAchievementDescriptions
{
    private static readonly Dictionary<AchievementName, (string DisplayName, string Description)> Map = new()
    {
        [AchievementName.FactionAmateur] = ("Faction Amateur", "Win 5 games as the same faction"),
        [AchievementName.FactionEnthusiast] = ("Faction Enthusiast", "Win 10 games as the same faction"),
        [AchievementName.FactionExpert] = ("Faction Expert", "Win 15 games as the same faction"),
        [AchievementName.FlauntingIt] = ("Flaunting It", "Win a game in which you would gain a faction hero you already have"),
        [AchievementName.IAmAnOG] = ("I Am An OG", "Win a game with each TI-1 faction"),
        [AchievementName.BaseGameBoss] = ("Base Game Boss", "Win a game with each base game faction"),
        [AchievementName.TheProphesiedKing] = ("The Prophesied King", "Win a game with each Prophecy of Kings faction"),
        [AchievementName.TheTribunii] = ("The Tribunii", "Win a game with each Keleres variant"),
        [AchievementName.BringTheThunder] = ("Bring The Thunder", "Win a game with each Thunder's Edge faction"),
        [AchievementName.ThirtyIsNotEnough] = ("Thirty Is Not Enough", "Win a game with each Discordant Stars faction"),
        [AchievementName.BlueDaBaDee] = ("Blue Da Ba Dee", "Win a game with each Blue Riverie faction"),
        [AchievementName.TyrantsGauntlet] = ("Tyrant's Gauntlet", "Win a game as each Mahact tyrant"),
        [AchievementName.DrFrankenstein] = ("Dr. Frankenstein", "Win a game as a Franken faction"),
        [AchievementName.ICookedThisMyself] = ("I Cooked This Myself", "Win a game as a homebrew faction"),
        [AchievementName.GottaCatchEmAllPartOne] = ("Gotta Catch'Em All (Part One)", "Win a game with all 30 factions"),
        [AchievementName.GottaCatchEmAllPartTwo] = ("Gotta Catch'Em All (Part Two)", "Win a game with all 80 factions"),
        [AchievementName.AllThatsSilverIsGold] = ("All That's Silver Is Gold", "Win a game with Silver Flame relic roll"),
        [AchievementName.HomeSystemWhoNeedsThat] = ("Home System? Who Needs That?", "Win a game where you failed to gain VP with Silver Flame roll"),
        [AchievementName.CrusherOfEnemies] = ("Crusher Of Enemies", "Win a game with 5 more points than any other player"),
        [AchievementName.MarrowEnjoyer] = ("Marrow Enjoyer", "Win a game with all other players at 9 points"),
        [AchievementName.BlinkAndYouWillMissIt] = ("Blink And You Will Miss It", "Win a game in round 3 or earlier"),
        [AchievementName.LongHauler] = ("Long Hauler", "Win a game in round 8 or later"),
        [AchievementName.IGotBetter] = ("I Got Better", "Win a game as a faction you have 5 losses with"),
        [AchievementName.BackToBack] = ("Back To Back", "Win 2 games in a row as the same faction"),
        [AchievementName.Inconceivable] = ("Inconceivable", "Win 5 games in a row"),
        [AchievementName.UnderdogStory] = ("Underdog Story", "Win against 3+ players with higher ranks than you"),
        [AchievementName.LiveInInterestingTimes] = ("Live In Interesting Times", "Win games that collectively included all Galactic Events"),
        [AchievementName.FirstSteps] = ("First Steps", "Finish 5 games"),
        [AchievementName.GettingWarmedUp] = ("Getting Warmed Up", "Finish 10 games"),
        [AchievementName.InTheGroove] = ("In The Groove", "Finish 25 games"),
        [AchievementName.SeasonedVeteran] = ("Seasoned Veteran", "Finish 50 games"),
        [AchievementName.Centurion] = ("Centurion", "Finish 100 games"),
        [AchievementName.SpeedDemon] = ("Speed Demon", "Win in the shortest game duration of the season"),
        [AchievementName.Marathoner] = ("Marathoner", "Win in the longest game duration of the season"),
        [AchievementName.Decathlon] = ("Decathlon", "Win a game to 10 VPs"),
        [AchievementName.DozenDone] = ("Dozen Done", "Win a game to 12 VPs"),
        [AchievementName.TheLongWar] = ("The Long War", "Win a game to 14 VPs"),
        [AchievementName.Versatile] = ("Versatile", "Win at least one game in each VP amount (10, 12, 14)"),
        [AchievementName.Collector] = ("Collector", "Earn 5 different achievements"),
        [AchievementName.Overachiever] = ("Overachiever", "Earn 15 different achievements"),
        [AchievementName.Completionist] = ("Completionist", "Earn 30 different achievements"),
    };

    public static string GetDisplayName(AchievementName name)
    {
        return Map.TryGetValue(name, out var info) ? info.DisplayName : name.ToString();
    }

    public static string GetDescription(AchievementName name)
    {
        return Map.TryGetValue(name, out var info) ? info.Description : string.Empty;
    }
}
