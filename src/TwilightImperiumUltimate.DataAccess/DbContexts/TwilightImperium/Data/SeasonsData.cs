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
            Name = string.Empty,
            StartDate = DateOnly.FromDateTime(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
            EndDate = DateOnly.FromDateTime(new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)),
            IsActive = true,
        },
    };
}
