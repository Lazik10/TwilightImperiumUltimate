using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.API.Discord.Services;

public class DiscordRoleChangePublisher(
    IDbContextFactory<TwilightImperiumDbContext> contextFactory,
    ILogger<DiscordRoleChangePublisher> logger)
    : IDiscordRoleChangePublisher
{
    public async Task<bool> PublishLogToDatabase(DiscordRoleChangeLog log)
    {
        using var db = contextFactory.CreateDbContext();

        var user = await db.TiglUsers.FirstOrDefaultAsync(x => x.DiscordId == log.UserId);
        if (user is not null)
        {
            log.UserTag = user.DiscordTag;
        }

        try
        {
            db.DiscordRoleChangeLogs.Add(log);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add role change log to database for User ID: {UserId} with Discord tag: {DiscordTag}!", log.UserId, log.UserTag);
        }

        return true;
    }
}
