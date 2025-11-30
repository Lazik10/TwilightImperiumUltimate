using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System.Globalization;
using TwilightImperiumUltimate.API.Discord.Services;
using TwilightImperiumUltimate.API.Options;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Tigl.Discord;

namespace TwilightImperiumUltimate.API.Discord;

internal sealed class DiscordBotClient : IDiscordClient, IHostedService, IDisposable
{
    private readonly DiscordSocketClient _client;
    private readonly DiscordOptions _options;
    private readonly ILogger<DiscordBotClient> _logger;
    private readonly IDiscordRoleChangePublisher _discordRoleChangePublisher;
    private readonly TaskCompletionSource _readyTcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private volatile bool _ready;

    public DiscordBotClient(IOptions<DiscordOptions> options, ILogger<DiscordBotClient> logger, IDiscordRoleChangePublisher discordRoleChangePublisher)
    {
        _discordRoleChangePublisher = discordRoleChangePublisher;
        _options = options.Value;
        _logger = logger;
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMembers,
            AlwaysDownloadUsers = true,
            LogGatewayIntentWarnings = false,
            MessageCacheSize = 0,
        });

        _client.Log += msg =>
        {
            var level = msg.Severity switch
            {
                LogSeverity.Critical => Microsoft.Extensions.Logging.LogLevel.Critical,
                LogSeverity.Error => Microsoft.Extensions.Logging.LogLevel.Error,
                LogSeverity.Warning => Microsoft.Extensions.Logging.LogLevel.Warning,
                LogSeverity.Info => Microsoft.Extensions.Logging.LogLevel.Information,
                LogSeverity.Verbose => Microsoft.Extensions.Logging.LogLevel.Trace,
                LogSeverity.Debug => Microsoft.Extensions.Logging.LogLevel.Debug,
                _ => Microsoft.Extensions.Logging.LogLevel.Information,
            };
            _logger.Log(level, "[Discord.Net] {Message}", msg.ToString());
            return Task.CompletedTask;
        };

        _client.Ready += () =>
        {
            _ready = true;
            _readyTcs.TrySetResult();
            _logger.LogInformation("Discord bot ready. User: {User}, State={State}", _client.CurrentUser, _client.ConnectionState);
            return Task.CompletedTask;
        };

        _client.Disconnected += ex =>
        {
            _ready = false;
            _logger.LogWarning(ex, "Discord bot disconnected. State={State}", _client.ConnectionState);
            return Task.CompletedTask;
        };

        _client.LoggedOut += () =>
        {
            _ready = false;
            _logger.LogInformation("Discord bot logged out.");
            return Task.CompletedTask;
        };
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.BotToken))
        {
            _logger.LogWarning("Discord bot token missing. Bot will not start.");
            return;
        }

        await _client.LoginAsync(TokenType.Bot, _options.BotToken);
        await _client.StartAsync();

        _logger.LogInformation("Discord start requested. State={State}", _client.ConnectionState);
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(30));
            await _readyTcs.Task.WaitAsync(cts.Token);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Discord bot did not signal Ready within timeout; State={State}", _client.ConnectionState);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_client.ConnectionState != ConnectionState.Disconnected)
            await _client.StopAsync();
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public Task<bool> PostRankUpStandardAsync(string content, CancellationToken cancellationToken = default)
        => SendMessageAsync(_options.RankUpStandardChannelId, content, cancellationToken);

    public Task<bool> PostRankUpFracturedAsync(string content, CancellationToken cancellationToken = default)
        => SendMessageAsync(_options.RankUpFracturedChannelId, content, cancellationToken);

    public Task<bool> PostLeadersStandardAsync(string content, CancellationToken cancellationToken = default)
        => SendMessageAsync(_options.LeadersStandardChannelId, content, cancellationToken);

    public Task<bool> PostLeadersFracturedAsync(string content, CancellationToken cancellationToken = default)
        => SendMessageAsync(_options.LeadersFracturedChannelId, content, cancellationToken);

    public Task<bool> PostAchievementsAsync(string content, CancellationToken cancellationToken = default)
        => SendMessageAsync(_options.AchievementsChannelId, content, cancellationToken);

    public Task<bool> PostTiglAdminAsync(string content, CancellationToken cancellationToken = default)
        => SendMessageAsync(_options.TiglAdminChannelId, content, cancellationToken);

    public async Task<bool> AddRoleToUserAsync(ulong userId, ulong roleId, DiscordRoleType type, CancellationToken cancellationToken = default)
    {
        var discordRoleChangeLog = CreateDiscordRoleChangeLog(userId, roleId, type);
        discordRoleChangeLog.Operation = DiscordRoleChangeOperation.Add;

        try
        {
            var guild = _client.GetGuild(_options.GuildId);
            if (guild is null)
            {
                discordRoleChangeLog.Result = DiscordRoleChangeStatus.GuildNotFound;
                return false;
            }

            await guild.DownloadUsersAsync();

            var user = guild.GetUser(userId);
            if (user is null)
            {
                discordRoleChangeLog.Result = DiscordRoleChangeStatus.UserNotFound;
                return false;
            }

            var roles = user.Roles.ToList();
            if (roles.Any(r => r.Id == roleId))
            {
                discordRoleChangeLog.Result = DiscordRoleChangeStatus.AlreadyHasRole;
                return true;
            }

            await user.AddRoleAsync(roleId);

            await guild.DownloadUsersAsync();
            var updatedUser = guild.GetUser(userId);
            if (updatedUser is null)
            {
                discordRoleChangeLog.Result = DiscordRoleChangeStatus.UpdatedUserNotFound;
                return false;
            }

            var updatedRoles = updatedUser.Roles.ToList();
            if (!updatedRoles.Any(r => r.Id == roleId))
            {
                discordRoleChangeLog.Result = DiscordRoleChangeStatus.FailedToAddRole;
                return false;
            }

            discordRoleChangeLog.Result = DiscordRoleChangeStatus.Success;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add role {RoleId} to user {UserId}.", roleId, userId);
            discordRoleChangeLog.Result = DiscordRoleChangeStatus.UnknownError;
            return false;
        }
        finally
        {
            discordRoleChangeLog.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            await _discordRoleChangePublisher.PublishLogToDatabase(discordRoleChangeLog);
        }
    }

    public async Task<bool> RemoveRoleFromUserAsync(ulong userId, ulong roleId, DiscordRoleType type, CancellationToken cancellationToken = default)
    {
        var discordRoleChangeLog = CreateDiscordRoleChangeLog(userId, roleId, type);
        discordRoleChangeLog.Operation = DiscordRoleChangeOperation.Remove;

        try
        {
            var guild = _client.GetGuild(_options.GuildId);
            if (guild is null)
            {
                discordRoleChangeLog.Result = DiscordRoleChangeStatus.GuildNotFound;
                return false;
            }

            await guild.DownloadUsersAsync();

            var user = guild.GetUser(userId);
            if (user is null)
            {
                discordRoleChangeLog.Result = DiscordRoleChangeStatus.UserNotFound;
                return false;
            }

            var roles = user.Roles.ToList();
            if (!roles.Any(r => r.Id == roleId))
            {
                discordRoleChangeLog.Result = DiscordRoleChangeStatus.DoesNotHaveRole;
                return true;
            }

            await user.RemoveRoleAsync(roleId);

            await guild.DownloadUsersAsync();
            var updatedUser = guild.GetUser(userId);
            if (updatedUser is null)
            {
                discordRoleChangeLog.Result = DiscordRoleChangeStatus.UpdatedUserNotFound;
                return false;
            }

            var updatedRoles = updatedUser.Roles.ToList();
            if (updatedRoles.Any(r => r.Id == roleId))
            {
                discordRoleChangeLog.Result = DiscordRoleChangeStatus.FailedToRemoveRole;
                return false;
            }

            discordRoleChangeLog.Result = DiscordRoleChangeStatus.Success;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove role {RoleId} from user {UserId}", roleId, userId);
            discordRoleChangeLog.Result = DiscordRoleChangeStatus.UnknownError;
            return false;
        }
        finally
        {
            discordRoleChangeLog.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            await _discordRoleChangePublisher.PublishLogToDatabase(discordRoleChangeLog);
        }
    }

    public async Task<bool> TransferRoleAsync(ulong fromUserId, ulong toUserId, ulong roleId, CancellationToken cancellationToken = default)
    {
        try
        {
            var guild = _client.GetGuild(_options.GuildId);
            if (guild is null)
                return false;

            var fromUser = guild.GetUser(fromUserId);
            var toUser = guild.GetUser(toUserId);

            if (fromUser is null || toUser is null)
                return false;

            await fromUser.RemoveRoleAsync(roleId);
            await toUser.AddRoleAsync(roleId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to transfer role {RoleId} from {FromUserId} to {ToUserId}", roleId, fromUserId, toUserId);
            return false;
        }
    }

    private static DiscordRoleChangeLog CreateDiscordRoleChangeLog(ulong userId, ulong roleId, DiscordRoleType type)
    {
        var discordRoleChangeLog = new DiscordRoleChangeLog();
        var roleName = string.Empty;

        if (type == DiscordRoleType.Leader)
        {
            var leaderRole = DiscordRoleMappings.PrestigeRoles.FirstOrDefault(x => x.Value.RoleId == roleId.ToString(CultureInfo.InvariantCulture));
            roleName = leaderRole.Value.RoleName;
        }
        else if (type == DiscordRoleType.Rank)
        {
            var rankRole = DiscordRoleMappings.RankRoles.FirstOrDefault(x => x.Value.RoleId == roleId.ToString(CultureInfo.InvariantCulture));
            roleName = rankRole.Value.RoleName;
        }

        discordRoleChangeLog.RoleName = roleName;
        discordRoleChangeLog.RoleId = roleId;
        discordRoleChangeLog.UserId = (long)userId;

        return discordRoleChangeLog;
    }

    private async Task<bool> SendMessageAsync(string channelIdString, string content, CancellationToken cancellationToken)
    {
        try
        {
            if (!_ready || _client.ConnectionState != ConnectionState.Connected)
            {
                _logger.LogInformation("Discord not ready. State={State}. Waiting for Ready...", _client.ConnectionState);
                try
                {
                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    cts.CancelAfter(TimeSpan.FromSeconds(60));
                    await _readyTcs.Task.WaitAsync(cts.Token);
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogDebug(ex, "Discord client not ready; wait timed out. State={State}", _client.ConnectionState);
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(channelIdString))
                return false;

            if (!ulong.TryParse(channelIdString, out var channelId))
            {
                _logger.LogWarning("Invalid channel id: {ChannelIdString}", channelIdString);
                return false;
            }

            var channel = await _client.GetChannelAsync(channelId) as IMessageChannel;
            if (channel is null)
            {
                _logger.LogWarning("Channel not found: {ChannelId}", channelId);
                return false;
            }

            await channel.SendMessageAsync(content);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Discord message to channel {ChannelIdString}", channelIdString);
            return false;
        }
    }
}
