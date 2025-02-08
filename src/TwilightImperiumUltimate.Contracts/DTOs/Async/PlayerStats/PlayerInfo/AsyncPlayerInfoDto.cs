namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.PlayerInfo;

public record AsyncPlayerInfoDto
{
    public AsyncPlayerInfoDto()
    {
        Id = 0;
        DiscordUserName = string.Empty;
    }

    public AsyncPlayerInfoDto(long discordUserId, string discordUserName)
    {
        Id = discordUserId;
        DiscordUserName = discordUserName;
    }

    public long Id { get; set; }

    public string DiscordUserName { get; set; }
}


