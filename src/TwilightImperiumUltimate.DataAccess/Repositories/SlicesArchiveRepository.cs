using Microsoft.Extensions.Logging;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class SlicesArchiveRepository(
    IDbContextFactory<TwilightImperiumDbContext> context,
    ILogger<SlicesArchiveRepository> logger)
    : ISlicesArchiveRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;
    private readonly ILogger<SlicesArchiveRepository> _logger = logger;

    public async Task<bool> AddNewSliceDraft(SliceDraft sliceDraft, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        try
        {
            dbContext.SliceDrafts.Add(sliceDraft);
            await dbContext.SaveChangesAsync(cancellationToken);
            sliceDraft.SliceDraftArchiveLink = string.Concat(sliceDraft.SliceDraftArchiveLink, sliceDraft.Id);

            dbContext.SliceDrafts.Update(sliceDraft);
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add new slice draft");
            return false;
        }
    }

    public async Task<bool> DeleteSliceDraft(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        var sliceDraft = await dbContext.SliceDrafts.FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken);

        if (sliceDraft is null)
        {
            return false;
        }

        dbContext.SliceDrafts.Remove(sliceDraft);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<SliceDraft>> GetAllSliceDrafts(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.SliceDrafts
            .Include(x => x.SliceDraftRatings)
            .ToListAsync(cancellationToken);
    }

    public async Task<SliceDraft?> GetSliceDraftById(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.SliceDrafts
            .Include(x => x.SliceDraftRatings)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<SliceDraftRating?> GetSliceDraftRatingFromUser(string userId, int sliceDraftId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.SliceDraftRatings
            .Where(x => x.UserId == userId && x.SliceDraftId == sliceDraftId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<SliceDraftRating> AddOrUpdateUserSliceDraftRating(string userId, int sliceDraftId, float rating, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var sliceDraftRating = await dbContext.SliceDraftRatings
            .Where(x => x.UserId == userId && x.SliceDraftId == sliceDraftId)
            .FirstOrDefaultAsync(cancellationToken);

        if (sliceDraftRating is null)
        {
            sliceDraftRating = new SliceDraftRating
            {
                UserId = userId,
                SliceDraftId = sliceDraftId,
                Rating = rating,
            };

            dbContext.SliceDraftRatings.Add(sliceDraftRating);
        }
        else
        {
            sliceDraftRating.Rating = rating;
            dbContext.SliceDraftRatings.Update(sliceDraftRating);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return sliceDraftRating;
    }
}
