using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Tigl.Discord;

public static class DiscordAchievementDescriptions
{
    private static readonly Dictionary<AchievementName, AchievementUiDescriptor> Map =
        new Dictionary<AchievementName, AchievementUiDescriptor>
        {
            [AchievementName.FactionAmateur] = new("Faction Amateur", "Win 5 games as the same faction"),
            [AchievementName.FactionEnthusiast] = new("Faction Enthusiast", "Win 10 games as the same faction"),
            [AchievementName.FactionExpert] = new("Faction Expert", "Win 15 games as the same faction"),
            [AchievementName.FlauntingIt] = new("Flaunting It", "Win a game in which you would gain a faction hero you already have"),
            [AchievementName.IAmAnOG] = new("I Am An OG", "Win a game with each TI-1 faction"),
            [AchievementName.BaseGameBoss] = new("Base Game Boss", "Win a game with each base game faction"),
            [AchievementName.TheProphesiedKing] = new("The Prophesied King", "Win a game with each Prophecy of Kings faction"),
            [AchievementName.TheTribunii] = new("The Tribunii", "Win a game with each Keleres variant"),
            [AchievementName.BringTheThunder] = new("Bring The Thunder", "Win a game with each Thunder’s Edge faction"),
            [AchievementName.ThirtyIsNotEnough] = new("Thirty Is Not Enough", "Win a game with each Discordant Stars faction"),
            [AchievementName.BlueDaBaDee] = new("Blue Da Ba Dee", "Win a game with each Blue Riverie faction"),
            [AchievementName.TyrantsGauntlet] = new("Tyrant's Gauntlet", "Win a game as each Mahact tyrant"),
            [AchievementName.DrFrankenstein] = new("Dr. Frankenstein", "Win a game as a Franken faction"),
            [AchievementName.ICookedThisMyself] = new("I Cooked This Myself", "Win a game as a homebrew faction"),
            [AchievementName.GottaCatchEmAllPartOne] = new("Gotta Catch'Em All (Part One)", "Win a game with all 30 factions"),
            [AchievementName.GottaCatchEmAllPartTwo] = new("Gotta Catch'Em All (Part Two)", "Win a game with all 80 factions"),
            [AchievementName.AllThatsSilverIsGold] = new("All That's Silver Is Gold", "Win a game with Silver Flame relic roll"),
            [AchievementName.HomeSystemWhoNeedsThat] = new("Home System? Who Needs That?", "Win a game where you failed to gain VP with Silver Flame roll"),
            [AchievementName.CrusherOfEnemies] = new("Crusher Of Enemies", "Win a game with 5 more points than any other player"),
            [AchievementName.MarrowEnjoyer] = new("Marrow Enjoyer", "Win a game with all other players at 9 points"),
            [AchievementName.BlinkAndYouWillMissIt] = new("Blink And You Will Miss It", "Win a game in round 3 or earlier"),
            [AchievementName.LongHauler] = new("Long Hauler", "Win a game in round 8 or later"),
            [AchievementName.IGotBetter] = new("I Got Better", "Win a game as a faction you have 5 losses with"),
            [AchievementName.BackToBack] = new("Back To Back", "Win 2 games in a row as the same faction"),
            [AchievementName.Inconceivable] = new("Inconceivable", "Win 5 games in a row"),
            [AchievementName.UnderdogStory] = new("Underdog Story", "Win against 3+ players with higher ranks than you"),
            [AchievementName.LiveInInterestingTimes] = new("Live In Interesting Times", "Win games that collectively included all Galactic Events"),
            [AchievementName.FirstSteps] = new("First Steps", "Finish 5 games"),
            [AchievementName.GettingWarmedUp] = new("Getting Warmed Up", "Finish 10 games"),
            [AchievementName.InTheGroove] = new("In The Groove", "Finish 25 games"),
            [AchievementName.SeasonedVeteran] = new("Seasoned Veteran", "Finish 50 games"),
            [AchievementName.Centurion] = new("Centurion", "Finish 100 games"),
            [AchievementName.SpeedDemon] = new("Speed Demon", "Win in the shortest game duration of the season"),
            [AchievementName.Marathoner] = new("Marathoner", "Win in the longest game duration of the season"),
            [AchievementName.Decathlon] = new("Decathlon", "Win a game to 10 VPs"),
            [AchievementName.DozenDone] = new("Dozen Done", "Win a game to 12 VPs"),
            [AchievementName.TheLongWar] = new("The Long War", "Win a game to 14 VPs"),
            [AchievementName.Versatile] = new("Versatile", "Win at least one game in each VP amount (10, 12, 14)"),
            [AchievementName.Collector] = new("Collector", "Earn 5 different achievements"),
            [AchievementName.Overachiever] = new("Overachiever", "Earn 15 different achievements"),
            [AchievementName.Completionist] = new("Completionist", "Earn 30 different achievements"),
        };

    public static string GetDetail(this AchievementName name)
    {
        return Map.TryGetValue(name, out var info) ? info.Details : string.Empty;
    }

    public static AchievementUiDescriptor GetDescriptor(this AchievementName name)
    {
        return Map.TryGetValue(name, out var info)
            ? info
            : new(name.ToString(), string.Empty);
    }
}
