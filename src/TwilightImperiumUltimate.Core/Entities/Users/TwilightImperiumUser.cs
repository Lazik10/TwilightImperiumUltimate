using Microsoft.AspNetCore.Identity;
using TwilightImperiumUltimate.Contracts.Enums;

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
}
