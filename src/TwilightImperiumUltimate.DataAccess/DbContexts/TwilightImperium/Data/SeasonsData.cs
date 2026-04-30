using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class SeasonsData
{
    internal static List<Season> Seasons => new List<Season>
    {
        new Season
        {
            Id = 1,
            SeasonNumber = 1,
            Name = "Season 1",
            StartDate = new DateOnly(2025, 12, 1),
            EndDate = new DateOnly(2025, 12, 1),
            IsActive = true,
        },
    };
}
