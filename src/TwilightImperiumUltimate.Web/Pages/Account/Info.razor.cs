using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Models.Users;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class Info
{
    private int Age { get; set; }

    private int FavoriteFactionNumber { get; set; }

    private FactionName FavoriteFaction { get; set; }

    private IReadOnlyCollection<FactionName> FactionNames { get; } = Enum.GetValues<FactionName>().ToList();

    [Inject]
    private IUserService UserService { get; set; } = default!;

    private TwilightImperiumUser? User { get; set; } = new TwilightImperiumUser();

    protected override async Task OnInitializedAsync()
    {
        User = await UserService.GetCurrentUserAsync();
        if (User is not null)
        {
            Age = User.Age ?? 0;
            FavoriteFactionNumber = (int)User.FavoriteFaction;
        }
    }

    private void ShowNextFaction()
    {
        if (FavoriteFactionNumber < FactionNames.Count - 1)
            FavoriteFactionNumber++;

        FavoriteFaction = (FactionName)FavoriteFactionNumber;
    }

    private void ShowPreviousFaction()
    {
        if (FavoriteFactionNumber > 0)
            FavoriteFactionNumber--;

        FavoriteFaction = (FactionName)FavoriteFactionNumber;

        StateHasChanged();
    }

    private void IncreaaeAge() => Age++;

    private void DecreaseAge()
    {
        if (Age > 0)
            Age--;
    }

    private async Task SaveInfo()
    {
        await UserService.UpdateUserInfoAsync(User, default);
    }
}