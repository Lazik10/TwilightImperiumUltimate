using Microsoft.AspNetCore.Components;
using System.Net;
using TwilightImperiumUltimate.Contracts.ApiContracts.User;
using TwilightImperiumUltimate.Contracts.DTOs.User;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Components.Admin;

public partial class RoleAssignment
{
    private IReadOnlyCollection<TwilightImperiumUserDto> _users = [];

    private TwilightImperiumUserDto _selectedUser = default!;

    private UserRole _selectedRole;

    private List<UserRole> UserRoles => GetUserRoles();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadUsers();
    }

    private static List<UserRole> GetUserRoles() => [.. Enum.GetValues<UserRole>()];

    private async Task LoadUsers()
    {
        var (response, statusCode) = await HttpClient.GetAsync<GetUserResponse>(Paths.ApiPath_Users, default);

        if (statusCode == HttpStatusCode.OK)
        {
            _users = response!.Users;
        }
    }
}