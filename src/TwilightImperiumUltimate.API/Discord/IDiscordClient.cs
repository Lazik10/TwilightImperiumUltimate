namespace TwilightImperiumUltimate.API.Discord;

public interface IDiscordClient
{
    Task<bool> PostRankUpStandardAsync(string content, CancellationToken cancellationToken = default);

    Task<bool> PostRankUpFracturedAsync(string content, CancellationToken cancellationToken = default);

    Task<bool> PostLeadersStandardAsync(string content, CancellationToken cancellationToken = default);

    Task<bool> PostLeadersFracturedAsync(string content, CancellationToken cancellationToken = default);

    Task<bool> PostAchievementsAsync(string content, CancellationToken cancellationToken = default);

    Task<bool> PostTiglAdminAsync(string content, CancellationToken cancellationToken = default);

    Task<bool> AddRoleToUserAsync(ulong userId, ulong roleId, DiscordRoleType type, CancellationToken cancellationToken = default);

    Task<bool> RemoveRoleFromUserAsync(ulong userId, ulong roleId, DiscordRoleType type, CancellationToken cancellationToken = default);

    Task<bool> TransferRoleAsync(ulong fromUserId, ulong toUserId, ulong roleId, CancellationToken cancellationToken = default);
}
