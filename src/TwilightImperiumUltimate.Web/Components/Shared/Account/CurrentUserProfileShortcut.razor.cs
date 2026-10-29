using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Services.Authentication;

namespace TwilightImperiumUltimate.Web.Components.Shared.Account;

public partial class CurrentUserProfileShortcut : IDisposable
{
    private bool _disposed;

    [Parameter]
    public bool IsTiglProfile { get; set; }

    [Parameter]
    public IReadOnlyCollection<TiglUserLiteDto> TiglPlayers { get; set; } = [];

    [Parameter]
    public IReadOnlyCollection<AsyncPlayerProfileDto> AsyncPlayerProfiles { get; set; } = [];

    [Inject]
    private ICurrentUserState CurrentUserState { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private int? ProfileId => IsTiglProfile
        ? CurrentUserState.User?.TiglPlayerProfileId
        : CurrentUserState.User?.AsyncPlayerProfileId;

    private string ButtonText
    {
        get
        {
            if (ProfileId is int profileId)
            {
                var profileName = IsTiglProfile
                    ? TiglPlayers.FirstOrDefault(player => player.Id == profileId)?.TiglUserName
                    : AsyncPlayerProfiles.FirstOrDefault(player => player.Id == profileId)?.DiscordUsername;
                if (!string.IsNullOrWhiteSpace(profileName))
                    return profileName;
            }

            return IsTiglProfile
                ? Strings.Account_ViewMyTiglProfile
                : Strings.Account_ViewMyAsyncProfile;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            CurrentUserState.UserChanged -= OnUserChanged;

        _disposed = true;
    }

    protected override async Task OnInitializedAsync()
    {
        CurrentUserState.UserChanged += OnUserChanged;
        await CurrentUserState.InitializeAsync();
    }

    private void OnUserChanged() => InvokeAsync(StateHasChanged);

    private void NavigateToProfile(int playerId)
    {
        var profilePage = IsTiglProfile ? Pages.Pages.TiglPlayerProfile : Pages.Pages.AsyncProfile;
        NavigationManager.NavigateTo($"{profilePage}?playerId={playerId}");
    }
}
