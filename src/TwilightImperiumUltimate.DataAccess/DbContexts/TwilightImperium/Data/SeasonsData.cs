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
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow),
            IsActive = true,
        },
    };
}
