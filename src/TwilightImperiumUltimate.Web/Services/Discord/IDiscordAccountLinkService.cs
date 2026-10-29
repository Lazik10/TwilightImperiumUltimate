namespace TwilightImperiumUltimate.Web.Services.Discord;

public interface IDiscordAccountLinkService
{
    Task<Uri> GetAuthorizationUriAsync(CancellationToken cancellationToken = default);
}
