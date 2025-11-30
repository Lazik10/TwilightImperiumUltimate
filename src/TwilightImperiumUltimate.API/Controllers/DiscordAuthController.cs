using FluentResults;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using TwilightImperiumUltimate.API.Options;
using TwilightImperiumUltimate.Business.Logic.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.DTOs.DiscordAuth;

namespace TwilightImperiumUltimate.API.Controllers;

[ApiController]
[Route("auth/discord")]
public partial class DiscordAuthController(
    IOptions<DiscordOptions> options,
    IHttpClientFactory httpClientFactory,
    ILogger<DiscordAuthController> logger,
    IOptions<FrontendOptions> frontendOptions,
    IMediator mediator) : ControllerBase
{
    private readonly DiscordOptions _options = options.Value;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger<DiscordAuthController> _logger = logger;
    private readonly FrontendOptions _frontendOptions = frontendOptions.Value;
    private readonly IMediator _mediator = mediator;

    [HttpGet("login")]
    public IActionResult Login([FromQuery] string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            returnUrl = _frontendOptions.Url.ToString();

        // raw state value only
        var stateValue = Guid.NewGuid().ToString("N");
        HttpContext.Session.SetString("discord_oauth_state", stateValue );
        HttpContext.Session.SetString("discord_return_url", returnUrl);

        var qs =
            $"{_options.DiscordAuthorizeUrl}?response_type=code" +
            $"&client_id={Uri.EscapeDataString(_options.ClientId!)}" +
            $"&redirect_uri={Uri.EscapeDataString(_options.RedirectUri!)}" +
            "&scope=identify" +
            $"&state={stateValue}";

        _logger.LogInformation("Discord login redirect: {Url} (returnUrl={ReturnUrl}, state={State})", qs, returnUrl, stateValue);

        return Redirect(qs);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
    {
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
        {
            logger.LogWarning("Callback missing code or state. code={Code}, state={State}", code, state);
            return BadRequest("Missing code or state.");
        }

        var expectedState = HttpContext.Session.GetString("discord_oauth_state");
        if (expectedState is null || !string.Equals(expectedState, state, StringComparison.Ordinal))
        {
            logger.LogWarning("State mismatch. expected={Expected}, received={Received}", expectedState, state);
            return BadRequest("Invalid OAuth state.");
        }

        HttpContext.Session.Remove("discord_oauth_state");

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
        logger.LogInformation("Discord token raw response: {Json}", tokenJson);

        if (!tokenResponse.IsSuccessStatusCode)
        {
            logger.LogWarning("Token exchange failed. Status={Status}", tokenResponse.StatusCode);
            return BadRequest("Failed to get Discord token.");
        }

        var tokenData = JsonSerializer.Deserialize<DiscordTokenResponse>(tokenJson);
        if (tokenData?.AccessToken is null)
        {
            logger.LogWarning("Token response missing access_token field. Parsed object: {@TokenData}", tokenData);
            return BadRequest("Discord token missing access token.");
        }

        using var userRequest = new HttpRequestMessage(HttpMethod.Get, _options.DiscordMeUrl);
        userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenData.AccessToken);
        userRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var userResponse = await httpClient.SendAsync(userRequest);
        var userJson = await userResponse.Content.ReadAsStringAsync();
        logger.LogInformation("Discord user raw response: {Json}", userJson);

        if (!userResponse.IsSuccessStatusCode)
        {
            logger.LogWarning("User fetch failed. Status={Status}", userResponse.StatusCode);
            return BadRequest("Failed to get Discord user.");
        }

        var discordUser = JsonSerializer.Deserialize<DiscordUserDto>(userJson);
        if (discordUser is null)
        {
            logger.LogWarning("Failed to deserialize Discord user.");
            return BadRequest("Unable to parse Discord user.");
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
            logger.LogWarning("Failed to register Tigl user for DiscordId={DiscordId}. Errors: {Errors}", discordUser.Id, string.Join(", ", errors));

            var redirectFailure = $"{returnUrl}?errorMessage={Uri.EscapeDataString(errors)}&playerId=-1";
            return Redirect(redirectFailure);
        }

        var redirectSuccess = $"{returnUrl}?name={Uri.EscapeDataString(discordUser.Username!)}&playerId={registerResult.Value}";

        logger.LogInformation("Redirecting back to frontend: {ReturnUrl} for DiscordId={DiscordId}", redirectSuccess, discordUser.Id);
        return Redirect(redirectSuccess);
    }

    private async Task<Result<int>> RegisterTiglUser(DiscordUserDto discordUser)
    {
        _logger.LogInformation("Registering new Tigl user with Discord ID: {DiscordId}, Username: {Username}", discordUser.Id, discordUser.Username);

        var discordIdLong = long.Parse(discordUser.Id, CultureInfo.InvariantCulture);
        var newTiglUserRequest = new NewTiglUserRequest() { DiscordId = discordIdLong, DiscordTag = discordUser.Username, TiglUserName = discordUser.Username };

        var response = await _mediator.Send(new RegisterNewTiglUserCommand(newTiglUserRequest));

        return response;
    }
}