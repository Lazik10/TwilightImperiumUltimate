using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class TiglParametersData
{
    internal static List<TiglParameter> TiglParameters => new List<TiglParameter>
    {
        new TiglParameter
        {
            Id = 1,
            Name = TiglParameterName.RankingUp,
            Enabled = true,
        },
        new TiglParameter
        {
            Id = 2,
            Name = TiglParameterName.EvaluateGames,
            Enabled = true,
        },
    };
}
