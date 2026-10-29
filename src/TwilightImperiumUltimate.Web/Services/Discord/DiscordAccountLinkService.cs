using TwilightImperiumUltimate.Contracts.ApiContracts.DiscordAuth;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Services.Discord;

public sealed class DiscordAccountLinkService(
    ITwilightImperiumApiHttpClient apiHttpClient)
    : IDiscordAccountLinkService
{
    private readonly ITwilightImperiumApiHttpClient _apiHttpClient = apiHttpClient;

    public async Task<Uri> GetAuthorizationUriAsync(CancellationToken cancellationToken = default)
    {
        var (response, statusCode) = await _apiHttpClient.GetAsync<DiscordLinkAuthorizationResponse>(
            Paths.ApiPath_DiscordLinkStart,
            cancellationToken: cancellationToken);

        if (statusCode != HttpStatusCode.OK)
        {
            throw new HttpRequestException("Discord authorization could not be started.", null, statusCode);
        }

        if (response is null ||
            !Uri.TryCreate(response.AuthorizationUrl, UriKind.Absolute, out var authorizationUri) ||
            authorizationUri.Scheme != Uri.UriSchemeHttps ||
            !string.Equals(authorizationUri.Host, "discord.com", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException("Discord returned an invalid authorization URL.");
        }

        return authorizationUri;
    }
}
