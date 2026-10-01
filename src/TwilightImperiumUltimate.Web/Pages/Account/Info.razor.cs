using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.User;
using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class Info
{
    private bool _infoSaved;
    private bool _updateSend;

    private int Age { get; set; }

    private FactionName FavoriteFaction { get; set; }

    private IReadOnlyCollection<KeyValuePair<FactionName, string>> FactionNames { get; } = EnumExtensions.GetFactionValuesWithDisplayNames();

    [Inject]
    private IUserService UserService { get; set; } = default!;

    private TwilightImperiumUser? User { get; set; } = new TwilightImperiumUser();

    protected override async Task OnInitializedAsync()
    {
        User = await UserService.GetCurrentUserAsync();
        if (User is not null)
        {
            Age = User.Age ?? 0;
            FavoriteFaction = User.FavoriteFaction;
        }
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
}
