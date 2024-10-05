using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Text;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.User;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class ConfirmEmail
{
    private bool _confirmSuccess;

    [Parameter]
    [SupplyParameterFromQuery(Name = "userId")]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "code")]
    public string Code { get; set; } = string.Empty;

    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _confirmSuccess = await UserService.ConfirmEmailAsync(Id, Code);
        StateHasChanged();

        if (_confirmSuccess)
        {
            await NavigateToIndexPage();
        }
    }

    private string UserIdConfirmingMessage()
    {
        CompositeFormat userIdMessage = CompositeFormat.Parse(Strings.ConfirmEmail_UserId);
        return string.Format(CultureInfo.InvariantCulture, userIdMessage, Id);
    }

    private async Task NavigateToIndexPage()
    {
        await Task.Delay(7000);
        NavigationManager.NavigateTo(Pages.Index, false);
    }
}