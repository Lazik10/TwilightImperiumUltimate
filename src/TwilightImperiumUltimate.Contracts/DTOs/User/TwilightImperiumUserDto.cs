using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.User;

public record TwilightImperiumUserDto(
    string Id,
    string? UserName,
    string FirstName,
    string LastName,
    string? Email,
    string? PhoneNumber,
    int? Age,
    FactionName FavoriteFaction,
    string UserInfo,
    string DiscordId,
    string SteamId);
