using Microsoft.Extensions.Localization;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.Authentication;
using TwilightImperiumUltimate.Web.Services.Discord;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class Info
{
    private bool _isLoading = true;
    private bool _isLinkingDiscord;
    private string? _discordLinkMessage;
    private string? _discordLinkError;
    private bool _infoSaved;
    private bool _updateSend;

    [Parameter]
    [SupplyParameterFromQuery(Name = "discordLink")]
    public string? DiscordLinkStatus { get; set; }

    private int Age { get; set; }

    private FactionName FavoriteFaction { get; set; }

    private IReadOnlyCollection<KeyValuePair<FactionName, string>> FactionNames { get; } = EnumExtensions.GetFactionValuesWithDisplayNames();

    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private IStringLocalizer<Strings> LinkStrings { get; set; } = default!;

    [Inject]
    private IDiscordAccountLinkService DiscordAccountLinkService { get; set; } = default!;

    [Inject]
    private ICurrentUserState CurrentUserState { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ILogger<Info> Logger { get; set; } = default!;

    private TwilightImperiumUser? User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await CurrentUserState.InitializeAsync();
        User = CurrentUserState.User;
        if (User is not null)
        {
            Age = User.Age ?? 0;
            FavoriteFaction = User.FavoriteFaction;
        }

        _isLoading = false;
    }

    protected override void OnParametersSet()
    {
        _discordLinkMessage = DiscordLinkStatus is "success" or "partial"
            ? Strings.Account_LinkDiscordSuccess
            : null;
        _discordLinkError = DiscordLinkStatus switch
        {
            "failed" => Strings.Account_LinkDiscordFailure,
            "profiles-not-found" => LinkStrings["Account_DiscordProfilesNotFound"].Value,
            _ => null,
        };
    }

    private async Task SaveInfo()
    {
        _updateSend = false;
        _infoSaved = false;

        if (User is not null)
        {
            User.Age = Age;
            User.FavoriteFaction = FavoriteFaction;
        }

        var result = await UserService.UpdateUserInfoAsync(User, default);

        _infoSaved = result;
        _updateSend = true;
    }

    private async Task LinkDiscordAsync()
    {
        if (_isLinkingDiscord)
            return;

        _isLinkingDiscord = true;
        _discordLinkMessage = null;
        _discordLinkError = null;

        try
        {
            var authorizationUri = await DiscordAccountLinkService.GetAuthorizationUriAsync();
            NavigationManager.NavigateTo(authorizationUri.ToString(), forceLoad: true);
            return;
        }
        catch (HttpRequestException exception)
        {
            Logger.LogError(exception, "Unable to start Discord profile linking.");
            _discordLinkError = Strings.Account_LinkDiscordFailure;
        }
        catch (InvalidDataException exception)
        {
            Logger.LogError(exception, "The Discord profile-link authorization response was invalid.");
            _discordLinkError = Strings.Account_LinkDiscordFailure;
        }
        catch (TaskCanceledException exception)
        {
            Logger.LogError(exception, "Discord profile-link authorization timed out.");
            _discordLinkError = Strings.Account_LinkDiscordFailure;
        }

        _isLinkingDiscord = false;
    }
}
