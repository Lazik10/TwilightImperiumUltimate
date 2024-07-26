using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Models.Users;

public class TwilightImperiumUser
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public int? Age { get; set; }

    public string Email { get; set; } = string.Empty;

    public string UserInfo { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public FactionName FavoriteFaction { get; set; }

    public string DiscordId { get; set; } = string.Empty;

    public string SteamId { get; set; } = string.Empty;

    public IReadOnlyList<string> Roles { get; set; } = [];
}
