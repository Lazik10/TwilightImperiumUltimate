using TwilightImperiumUltimate.Web.Models.Users;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class Admin
{
    [Inject]
    ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    private List<TwilightImperiumUser> Users { get; set; } = new List<TwilightImperiumUser>();

    protected override Task OnInitializedAsync()
    {
        /*        var users = HttpClient.GetAsync(Paths.ApiPath_Users);*/
        return base.OnInitializedAsync();
    }
}