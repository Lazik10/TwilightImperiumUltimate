using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class TiglParametersData
{
    internal static List<TiglParameter> TiglParameters => new List<TiglParameter>
    {
        new TiglParameter
        {
            Id = 10,
            Name = TiglParameterName.OnlyThundersEdgeEra,
            Enabled = true,
        },
        new TiglParameter
        {
            Id = 2,
            Name = TiglParameterName.EvaluateGames,
            Enabled = true,
        },
        new TiglParameter
        {
            Id = 6,
            Name = TiglParameterName.OnlyImportReports,
            Enabled = false,
        },
        new TiglParameter
        {
            Id = 3,
            Name = TiglParameterName.ManualGameReview,
            Enabled = true,
        },
        new TiglParameter
        {
            Id = 5,
            Name = TiglParameterName.TiglTestUserRegistrations,
            Enabled = false,
        },
        new TiglParameter
        {
            Id = 1,
            Name = TiglParameterName.RankingUp,
            Enabled = true,
        },
        new TiglParameter
        {
            Id = 4,
            Name = TiglParameterName.RankingUpProphecyOfKings,
            Enabled = false,
        },
        new TiglParameter
        {
            Id = 7,
            Name = TiglParameterName.AchievementsPublish,
            Enabled = false,
        },
        new TiglParameter
        {
            Id = 8,
            Name = TiglParameterName.HeroPublish,
            Enabled = false,
        },
        new TiglParameter
        {
            Id = 9,
            Name = TiglParameterName.RankUpPublish,
            Enabled = false,
        },
        new TiglParameter
        {
            Id = 11,
            Name = TiglParameterName.DiscordInteraction,
            Enabled = false,
        },
    };
}
