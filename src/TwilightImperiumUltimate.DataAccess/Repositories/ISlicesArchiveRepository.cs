namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface ISlicesArchiveRepository
{
    Task<IEnumerable<SliceDraft>> GetAllSliceDrafts(CancellationToken cancellationToken);

    Task<SliceDraft?> GetSliceDraftById(int id, CancellationToken cancellationToken);

    Task<bool> AddNewSliceDraft(SliceDraft sliceDraft, CancellationToken cancellationToken);

    Task<bool> DeleteSliceDraft(int id, CancellationToken cancellationToken);

    Task<SliceDraftRating?> GetSliceDraftRatingFromUser(string userId, int sliceDraftId, CancellationToken cancellationToken);

    Task<SliceDraftRating> AddOrUpdateUserSliceDraftRating(string userId, int sliceDraftId, float rating, CancellationToken cancellationToken);
}
