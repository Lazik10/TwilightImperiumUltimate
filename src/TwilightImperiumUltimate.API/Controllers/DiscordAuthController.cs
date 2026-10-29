using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using FluentResults;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using TwilightImperiumUltimate.API.Options;
using TwilightImperiumUltimate.Business.Logic.Tigl;
using TwilightImperiumUltimate.Business.Logic.Users;
using TwilightImperiumUltimate.Contracts.ApiContracts.DiscordAuth;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.DTOs.DiscordAuth;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.API.Controllers;

[ApiController]
[Route("auth/discord")]
public partial class DiscordAuthController(
    IOptions<DiscordOptions> options,
    IHttpClientFactory httpClientFactory,
    ILogger<DiscordAuthController> logger,
    IOptions<FrontendOptions> frontendOptions,
    IMediator mediator,
    IDistributedCache distributedCache) : ControllerBase
{
    private const string DiscordLinkStatePrefix = "link_";
    private static readonly TimeSpan DiscordLinkStateLifetime = TimeSpan.FromMinutes(10);

    private readonly DiscordOptions _options = options.Value;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger<DiscordAuthController> _logger = logger;
    private readonly FrontendOptions _frontendOptions = frontendOptions.Value;
    private readonly IMediator _mediator = mediator;
    private readonly IDistributedCache _distributedCache = distributedCache;

    [HttpGet("login")]
    public IActionResult Login([FromQuery] string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            returnUrl = _frontendOptions.Url.ToString();

        // raw state value only
        var stateValue = Guid.NewGuid().ToString("N");
        HttpContext.Session.SetString("discord_oauth_state", stateValue);
        HttpContext.Session.SetString("discord_return_url", returnUrl);

        _logger.LogInformation("Starting Discord authorization.");
        return Redirect(BuildDiscordAuthorizationUrl(stateValue));
    }

    [Authorize]
    [HttpGet("link")]
    public async Task<ActionResult<DiscordLinkAuthorizationResponse>> StartProfileLinking(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var state = DiscordLinkStatePrefix + Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        await _distributedCache.SetStringAsync(
            GetDiscordLinkStateCacheKey(state),
            userId,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = DiscordLinkStateLifetime,
            },
            cancellationToken);

        return Ok(new DiscordLinkAuthorizationResponse
        {
            AuthorizationUrl = BuildDiscordAuthorizationUrl(state),
        });
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback(
        [FromQuery] string? code,
        [FromQuery] string? state,
        [FromQuery] string? error,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(error) && state?.StartsWith(DiscordLinkStatePrefix, StringComparison.Ordinal) is true)
        {
            var userId = await ConsumeDiscordLinkStateAsync(state, cancellationToken);
            return userId is null
                ? BadRequest("Invalid or expired OAuth state.")
                : Redirect(GetDiscordLinkResultUrl("failed"));
        }

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
        {
            _logger.LogWarning("Discord callback is missing its authorization code or state.");
            return BadRequest("Missing code or state.");
        }

        string? linkedWebsiteUserId = null;
        if (state.StartsWith(DiscordLinkStatePrefix, StringComparison.Ordinal))
        {
            linkedWebsiteUserId = await ConsumeDiscordLinkStateAsync(state, cancellationToken);

            if (string.IsNullOrWhiteSpace(linkedWebsiteUserId))
                return BadRequest("Invalid or expired OAuth state.");
        }
        else
        {
            var expectedState = HttpContext.Session.GetString("discord_oauth_state");
            if (expectedState is null || !string.Equals(expectedState, state, StringComparison.Ordinal))
            {
                _logger.LogWarning("Discord callback state validation failed.");
                return BadRequest("Invalid OAuth state.");
            }

            HttpContext.Session.Remove("discord_oauth_state");
        }

        using var httpClient = _httpClientFactory.CreateClient();

        using var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _options.DiscordTokenUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", _options.ClientId! },
                { "client_secret", _options.ClientSecret! },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", _options.RedirectUri! },
            }),
        };

        // Optional: explicit Accept
        tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var tokenResponse = await httpClient.SendAsync(tokenRequest);
        var tokenJson = await tokenResponse.Content.ReadAsStringAsync();

        if (!tokenResponse.IsSuccessStatusCode)
        {
            _logger.LogWarning("Token exchange failed. Status={Status}", tokenResponse.StatusCode);
            if (linkedWebsiteUserId is not null)
                return Redirect(GetDiscordLinkResultUrl("failed"));

            return BadRequest("Failed to get Discord token.");
        }

        var tokenData = JsonSerializer.Deserialize<DiscordTokenResponse>(tokenJson);
        if (tokenData?.AccessToken is null)
        {
            _logger.LogWarning("Discord token response did not contain an access token.");
            if (linkedWebsiteUserId is not null)
                return Redirect(GetDiscordLinkResultUrl("failed"));

            return BadRequest("Discord token missing access token.");
        }

        using var userRequest = new HttpRequestMessage(HttpMethod.Get, _options.DiscordMeUrl);
        userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenData.AccessToken);
        userRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var userResponse = await httpClient.SendAsync(userRequest);
        var userJson = await userResponse.Content.ReadAsStringAsync();

        if (!userResponse.IsSuccessStatusCode)
        {
            _logger.LogWarning("User fetch failed. Status={Status}", userResponse.StatusCode);
            if (linkedWebsiteUserId is not null)
                return Redirect(GetDiscordLinkResultUrl("failed"));

            return BadRequest("Failed to get Discord user.");
        }

        var discordUser = JsonSerializer.Deserialize<DiscordUserDto>(userJson);
        if (discordUser is null)
        {
            _logger.LogWarning("Failed to deserialize Discord user.");
            if (linkedWebsiteUserId is not null)
                return Redirect(GetDiscordLinkResultUrl("failed"));

            return BadRequest("Unable to parse Discord user.");
        }

        if (linkedWebsiteUserId is not null)
        {
            return await LinkDiscordProfilesAsync(linkedWebsiteUserId, discordUser, cancellationToken);
        }

        HttpContext.Session.SetString("discord_id", discordUser.Id);
        HttpContext.Session.SetString("discord_username", discordUser.Username ?? string.Empty);
        HttpContext.Session.SetString("discord_global_name", discordUser.GlobalName ?? string.Empty);
        HttpContext.Session.SetString("discord_avatar", discordUser.Avatar ?? string.Empty);

        var returnUrl = HttpContext.Session.GetString("discord_return_url") ?? $"{_frontendOptions.Url}/community/tigl/register";
        HttpContext.Session.Remove("discord_return_url");

        var registerResult = await RegisterTiglUser(discordUser);
        if (registerResult.IsFailed)
        {
            var errors = string.Join(", ", registerResult.Errors.Select(e => e.Message));
            _logger.LogWarning("Failed to register Tigl user for DiscordId={DiscordId}. Errors: {Errors}", discordUser.Id, string.Join(", ", errors));

            var redirectFailure = $"{returnUrl}?errorMessage={Uri.EscapeDataString(errors)}&playerId=-1";
            return Redirect(redirectFailure);
        }

        var redirectSuccess = $"{returnUrl}?name={Uri.EscapeDataString(discordUser.Username!)}&playerId={registerResult.Value}";

        _logger.LogInformation("Redirecting back to frontend: {ReturnUrl} for DiscordId={DiscordId}", redirectSuccess, discordUser.Id);
        return Redirect(redirectSuccess);
    }

    private static string GetDiscordLinkStateCacheKey(string state) => $"discord-link-state:{state}";

    private async Task<IActionResult> LinkDiscordProfilesAsync(string userId, DiscordUserDto discordUser, CancellationToken cancellationToken)
    {
        if (!long.TryParse(discordUser.Id, NumberStyles.None, CultureInfo.InvariantCulture, out var discordId)
            || string.IsNullOrWhiteSpace(discordUser.Username))
        {
            _logger.LogWarning("Discord returned an invalid user identifier during account linking.");
            return Redirect(GetDiscordLinkResultUrl("failed"));
        }

        var profileIds = await _mediator.Send(new GetPlayerIdsByDiscordIdQuery(discordId), cancellationToken);
        var asyncProfileFound = profileIds.AsyncPlayerProfileId > 0;
        var tiglProfileFound = profileIds.TiglPlayerId > 0;
        _logger.LogInformation(
            "Discord profile lookup completed. AsyncProfileFound={AsyncProfileFound}, TiglProfileFound={TiglProfileFound}",
            asyncProfileFound,
            tiglProfileFound);

        if (!asyncProfileFound && !tiglProfileFound)
        {
            _logger.LogWarning("Discord profile linking found no existing Async or TIGL profiles.");
        }

        int? asyncPlayerProfileId = asyncProfileFound ? profileIds.AsyncPlayerProfileId : null;
        int? tiglPlayerProfileId = tiglProfileFound ? profileIds.TiglPlayerId : null;
        var linked = await _mediator.Send(
            new LinkDiscordPlayerProfilesCommand(userId, discordUser.Username, asyncPlayerProfileId, tiglPlayerProfileId),
            cancellationToken);

        if (!linked)
        {
            _logger.LogWarning("Discord profile linking failed because the website user was not found.");
            return Redirect(GetDiscordLinkResultUrl("failed"));
        }

        _logger.LogInformation("Discord profiles were linked to a website account.");
        var resultStatus = asyncProfileFound && tiglProfileFound
            ? "success"
            : asyncProfileFound || tiglProfileFound ? "partial" : "profiles-not-found";
        return Redirect(GetDiscordLinkResultUrl(resultStatus));
    }

    private async Task<string?> ConsumeDiscordLinkStateAsync(string state, CancellationToken cancellationToken)
    {
        var stateCacheKey = GetDiscordLinkStateCacheKey(state);
        var userId = await _distributedCache.GetStringAsync(stateCacheKey, cancellationToken);
        await _distributedCache.RemoveAsync(stateCacheKey, cancellationToken);

        return userId;
    }

    private string BuildDiscordAuthorizationUrl(string state) =>
        $"{_options.DiscordAuthorizeUrl}?response_type=code" +
        $"&client_id={Uri.EscapeDataString(_options.ClientId!)}" +
        $"&redirect_uri={Uri.EscapeDataString(_options.RedirectUri!)}" +
        "&scope=identify" +
        $"&state={Uri.EscapeDataString(state)}";

    private string GetDiscordLinkResultUrl(string status) =>
        new Uri(_frontendOptions.Url, $"/account/info?discordLink={Uri.EscapeDataString(status)}").ToString();

    private async Task<Result<int>> RegisterTiglUser(DiscordUserDto discordUser)
    {
        _logger.LogInformation("Registering a TIGL user through Discord authorization.");

        var discordIdLong = long.Parse(discordUser.Id, CultureInfo.InvariantCulture);
        var newTiglUserRequest = new NewTiglUserRequest() { DiscordId = discordIdLong, DiscordTag = discordUser.Username, TiglUserName = discordUser.Username };

        var response = await _mediator.Send(new RegisterNewTiglUserCommand(newTiglUserRequest));

        return response;
    }
}
