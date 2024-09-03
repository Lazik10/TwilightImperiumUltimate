using Microsoft.AspNetCore.Identity;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

namespace TwilightImperiumUltimate.Core.Entities.Users;

public class TwilightImperiumUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public int? Age { get; set; }

    public FactionName FavoriteFaction { get; set; }

    public string UserInfo { get; set; } = string.Empty;

    public string DiscordId { get; set; } = string.Empty;

    public string SteamId { get; set; } = string.Empty;

    public IReadOnlyCollection<Map> Maps { get; set; } = new List<Map>();

    public IReadOnlyCollection<MapRating> MapRatings { get; set; } = new List<MapRating>();

    public IReadOnlyCollection<SliceDraft> SliceDrafts { get; set; } = new List<SliceDraft>();

    public IReadOnlyCollection<SliceDraftRating> SliceDraftRatings { get; set; } = new List<SliceDraftRating>();
}
