namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>Builds type-specific room views from a fully loaded, authorized room snapshot.</summary>
public interface IDraftRoomDraftViewProvider
{
    /// <summary>Projects rules, lifecycle permissions and personalized draft state for the actor.</summary>
    Task<DraftRoomDraftView> GetAsync(DraftRoomActor actor, CancellationToken cancellationToken);
}
