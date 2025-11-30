namespace TwilightImperiumUltimate.Contracts.Enums;

public enum AchievementName
{
    FactionAmateur, // Win 5 games as [Faction]
    FactionEnthusiast, // Win 10 games as [Faction]
    FactionExpert, // Win 15 games as [Faction]
    FlauntingIt, // Win a game in which you would gain a Faction hero you already have

    IAmAnOG, // Win a game with each TI-1 faction (Hacan, Sol, Xxcha, Barony, Jol-Nar, Sardakk)
    BaseGameBoss, // Win a game with each base game faction
    TheProphesiedKing, // Win a game with each PoK faction
    TheTribunii, // Win a game with each Keleres variant
    BringTheThunder, // Win a game with each Thunder's Edge faction
    ThirtyIsNotEnough, // Win a game with each Discordant Stars faction
    BlueDaBaDee, // Win a game with each Blue Riverie faction
    TyrantsGauntlet, // Win a game as each Mahact tyrant
    DrFrankenstein, // Win a game as franken faction
    ICookedThisMyself, // Win a game as homebrew faction
    GottaCatchEmAllPartOne, // Win a game with all 30 factions
    GottaCatchEmAllPartTwo, // Win a game with all 80 factions (30 official, 34 ds, 
    AllThatsSilverIsGold, // Win a game with Silver Flame relic roll
    HomeSystemWhoNeedsThat, // Win a game Win a game where you failed to gain VP with Silver Flame roll

    CrusherOfEnemies, // Win a game with 5 more points than any other player
    MarrowEnjoyer, // Win a game with all other players at 9 points
    BlinkAndYouWillMissIt, // Win a game in round 3 or earlier
    LongHauler, // Win a game in round 8 or later
    IGotBetter, // Win a game as a faction you have 5 losses with
    BackToBack, // Win 2 games in a row as the same faction (games not as that faction can be played in between)
    Inconceivable, // Win 5 games in a row
    UnderdogStory, // Win against 3+ players with higher ranks than you

    LiveInInterestingTimes, // Win games that collectively included all Galactic Events (not currently possible to log, but once it is, should work retroactively

    FirstSteps, // Finish 5 games
    GettingWarmedUp, // Finish 10 games
    InTheGroove, // Finish 25 games
    SeasonedVeteran, // Finish 50 games
    Centurion, // Finish 100 games

    SpeedDemon, // Win in the shortest game duration of the season
    Marathoner, // Win in the longest game duration of the season

    Decathlon, // Win a game to 10 VPs
    DozenDone, // Win a game to 12 VPs
    TheLongWar, // Win a game to 14 VPs
    Versatile, // Win at least one game in each VP amount (10, 12, 14)

    Collector, // Earn 5 different achievements
    Overachiever, // Earn 15 different achievements
    Completionist, // Earn 30 different achievements
}
