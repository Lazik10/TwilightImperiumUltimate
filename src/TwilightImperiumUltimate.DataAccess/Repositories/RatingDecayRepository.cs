using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class RatingDecayRepository(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<RatingDecayRepository> logger)
    : IRatingDecayRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;
    private readonly ILogger<RatingDecayRepository> _logger = logger;

    public async Task InsertRatingDecays(IReadOnlyCollection<TiglUser> inactivePlayers, TiglLeague league, int season, CancellationToken cancellationToken)
    {
        if (inactivePlayers.Count == 0)
            return;

        var decays = new List<RatingDecay>();
        var ratingDelta = 0.0;

        foreach (var player in inactivePlayers)
        {
            var asyncRating = player.AsyncStats!.Single(x => x.League == league).Rating;
            if (asyncRating?.Rating > 1000.9)
            {
                ratingDelta = asyncRating!.Rating * 0.02; // Apply 2% decay
                asyncRating.Rating -= ratingDelta;

                decays.Add(new RatingDecay { TiglUserId = player.Id, Amount = ratingDelta, League = league, Season = season, RatingSystem = RankingSystem.Async });
            }

            var glicko2Rating = player.GlickoStats!.Single(x => x.League == league).Rating;
            if (glicko2Rating?.Rating > 1500)
            {
                ratingDelta = glicko2Rating!.Rating * 0.02; // Apply 2% decay
                glicko2Rating.Rating -= ratingDelta;

                decays.Add(new RatingDecay { TiglUserId = player.Id, Amount = ratingDelta, League = league, Season = season, RatingSystem = RankingSystem.Glicko2 });
            }

            var trueSkillRating = player.TrueSkillStats!.Single(x => x.League == league).TrueSkillRating;
            if (trueSkillRating?.Rating > 25.0)
            {
                ratingDelta = trueSkillRating!.Rating * 0.02; // Apply 2% decay
                trueSkillRating.Mu -= ratingDelta;

                decays.Add(new RatingDecay { TiglUserId = player.Id, Amount = ratingDelta, League = league, Season = season, RatingSystem = RankingSystem.TrueSkill });
            }
        }

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        try
        {
            dbContext.RatingDecays.AddRange(decays);
            dbContext.TiglUsers.UpdateRange(inactivePlayers);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to insert RatingDecay records.");
        }
    }
}
