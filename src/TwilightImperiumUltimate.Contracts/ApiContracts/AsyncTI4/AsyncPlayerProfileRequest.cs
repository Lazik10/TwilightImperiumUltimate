namespace TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;

public record AsyncPlayerProfileRequest(
    long DiscordId,
    string Name,
    int PlayerId);
