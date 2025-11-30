using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class AchievementsData
{
    internal static List<Achievement> Achievements => new List<Achievement>()
    {
        new Achievement() { Id = 1, Name = AchievementName.FactionAmateur, Category = AchievementCategory.Faction },
        new Achievement() { Id = 2, Name = AchievementName.FactionEnthusiast, Category = AchievementCategory.Faction },
        new Achievement() { Id = 3, Name = AchievementName.FactionExpert, Category = AchievementCategory.Faction },
        new Achievement() { Id = 4, Name = AchievementName.FlauntingIt, Category = AchievementCategory.Faction },

        new Achievement() { Id = 5, Name = AchievementName.IAmAnOG, Category = AchievementCategory.Wins },
        new Achievement() { Id = 6, Name = AchievementName.BaseGameBoss, Category = AchievementCategory.Wins },
        new Achievement() { Id = 7, Name = AchievementName.TheProphesiedKing, Category = AchievementCategory.Wins },
        new Achievement() { Id = 8, Name = AchievementName.TheTribunii, Category = AchievementCategory.Wins },
        new Achievement() { Id = 9, Name = AchievementName.BringTheThunder, Category = AchievementCategory.Wins },
        new Achievement() { Id = 10, Name = AchievementName.ThirtyIsNotEnough, Category = AchievementCategory.Wins },
        new Achievement() { Id = 11, Name = AchievementName.BlueDaBaDee, Category = AchievementCategory.Wins },
        new Achievement() { Id = 12, Name = AchievementName.TyrantsGauntlet, Category = AchievementCategory.Wins },
        new Achievement() { Id = 13, Name = AchievementName.DrFrankenstein, Category = AchievementCategory.Wins },
        new Achievement() { Id = 14, Name = AchievementName.ICookedThisMyself, Category = AchievementCategory.Wins },
        new Achievement() { Id = 15, Name = AchievementName.AllThatsSilverIsGold, Category = AchievementCategory.Wins },
        new Achievement() { Id = 16, Name = AchievementName.HomeSystemWhoNeedsThat, Category = AchievementCategory.Wins },

        new Achievement() { Id = 17, Name = AchievementName.GottaCatchEmAllPartOne, Category = AchievementCategory.Wins },
        new Achievement() { Id = 18, Name = AchievementName.GottaCatchEmAllPartTwo, Category = AchievementCategory.Wins },

        new Achievement() { Id = 19, Name = AchievementName.CrusherOfEnemies, Category = AchievementCategory.Wins },
        new Achievement() { Id = 20, Name = AchievementName.MarrowEnjoyer, Category = AchievementCategory.Wins },
        new Achievement() { Id = 21, Name = AchievementName.BlinkAndYouWillMissIt, Category = AchievementCategory.Wins },
        new Achievement() { Id = 22, Name = AchievementName.LongHauler, Category = AchievementCategory.Wins },
        new Achievement() { Id = 23, Name = AchievementName.IGotBetter, Category = AchievementCategory.Wins },
        new Achievement() { Id = 24, Name = AchievementName.BackToBack, Category = AchievementCategory.Wins },
        new Achievement() { Id = 25, Name = AchievementName.Inconceivable, Category = AchievementCategory.Wins },
        new Achievement() { Id = 26, Name = AchievementName.UnderdogStory, Category = AchievementCategory.Wins },

        new Achievement() { Id = 27, Name = AchievementName.LiveInInterestingTimes, Category = AchievementCategory.GameMode },

        new Achievement() { Id = 28, Name = AchievementName.FirstSteps, Category = AchievementCategory.Games },
        new Achievement() { Id = 29, Name = AchievementName.GettingWarmedUp, Category = AchievementCategory.Games },
        new Achievement() { Id = 30, Name = AchievementName.InTheGroove, Category = AchievementCategory.Games },
        new Achievement() { Id = 31, Name = AchievementName.SeasonedVeteran, Category = AchievementCategory.Games },
        new Achievement() { Id = 32, Name = AchievementName.Centurion, Category = AchievementCategory.Games },

        new Achievement() { Id = 33, Name = AchievementName.SpeedDemon, Category = AchievementCategory.Season },
        new Achievement() { Id = 34, Name = AchievementName.Marathoner, Category = AchievementCategory.Season },

        new Achievement() { Id = 35, Name = AchievementName.Decathlon, Category = AchievementCategory.Wins },
        new Achievement() { Id = 36, Name = AchievementName.DozenDone, Category = AchievementCategory.Wins },
        new Achievement() { Id = 37, Name = AchievementName.TheLongWar, Category = AchievementCategory.Wins },
        new Achievement() { Id = 38, Name = AchievementName.Versatile, Category = AchievementCategory.Wins },

        new Achievement() { Id = 39, Name = AchievementName.Collector, Category = AchievementCategory.Summary },
        new Achievement() { Id = 40, Name = AchievementName.Overachiever, Category = AchievementCategory.Summary },
        new Achievement() { Id = 41, Name = AchievementName.Completionist, Category = AchievementCategory.Summary },
    };
}
