using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Serilog;
using System.Security.Claims;

namespace TwilightImperiumUltimate.Web.Pages.Account;

public partial class UserClaims
{
    [CascadingParameter]
    public Task<AuthenticationState>? AuthenticationState { get; set; }

    private ClaimsPrincipal AuthenticatedUser { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (AuthenticationState is not null)
        {
            var state = await AuthenticationState;
            AuthenticatedUser = state.User;

            if (AuthenticatedUser.Identity?.IsAuthenticated == true)
            {
                // User is authenticated
                Log.Information("User is authenticated");
            }
            else
            {
                // User is not authenticated
                Log.Information("User is not authenticated");
            }
        }
    }
}