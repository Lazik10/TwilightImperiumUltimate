using TwilightImperiumUltimate.Contracts.ApiContracts.User;
using TwilightImperiumUltimate.Contracts.DTOs.User;

namespace TwilightImperiumUltimate.Web.Components.Admin;

public partial class RoleAssignment
{
    private IReadOnlyCollection<TwilightImperiumUserDto> _users = new List<TwilightImperiumUserDto>();

    private IReadOnlyCollection<TwilightImperiumUserDto> _filteredUsers = new List<TwilightImperiumUserDto>();

    private IReadOnlyCollection<RoleDto> _roles = new List<RoleDto>();

    private IReadOnlyCollection<RoleDto> _specificUserRoles = new List<RoleDto>();

    private string _selectedUserEmail = string.Empty;

    private string _selectedRole = string.Empty;

    private bool _showRoleAddSuccess;

    private bool _showRoleRemoveSuccess;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadUsers();
        await LoadRoles();
    }

    private async Task LoadUsers()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<TwilightImperiumUserDto>>>(Paths.ApiPath_Users, default);
        if (statusCode == HttpStatusCode.OK)
        {
            _users = response!.Data!.Items;
            _filteredUsers = response!.Data.Items;
            _selectedUserEmail = _users.Select(x => x.Email).First() ?? string.Empty;
        }
    }

    private async Task LoadRoles()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<RoleDto>>>(Paths.ApiPath_Roles, default);

        if (statusCode == HttpStatusCode.OK)
        {
            var roles = new List<RoleDto>();
            foreach (var role in response!.Data!.Items)
            {
                roles.Add(role);
            }

            _roles = roles;
            _selectedRole = roles.Select(x => x.Name).First();
        }
    }

    private Task FilterUsersByEmail(string email)
    {
        if (email.Length < 3)
            _filteredUsers = _users;

        _filteredUsers = _users.Where(u => u.Email!.Contains(email)).ToList();
        _selectedUserEmail = _filteredUsers.Select(x => x.Email).First() ?? string.Empty;
        return Task.CompletedTask;
    }

    private async Task AddRoleToUser()
    {
        _showRoleAddSuccess = false;

        if (string.IsNullOrWhiteSpace(_selectedRole) || string.IsNullOrWhiteSpace(_selectedUserEmail))
            return;

        var userId = _users.Where(x => x.Email == _selectedUserEmail).Select(x => x.Id).First();
        var request = new AddRoleToUserRequest() { RoleName = _selectedRole, UserId = userId! };
        var (result, statusCode) = await HttpClient.PostAsync<AddRoleToUserRequest, AddRoleToUserResponse>(Paths.ApiPath_AddRole, request);

        if (statusCode == HttpStatusCode.OK)
        {
            _showRoleAddSuccess = true;
        }

        await GetUserRoles();
        StateHasChanged();
    }

    private async Task RemoveRoleFromUser()
    {
        _showRoleRemoveSuccess = false;

        if (string.IsNullOrWhiteSpace(_selectedRole) || string.IsNullOrWhiteSpace(_selectedUserEmail))
            return;

        var userId = _users.Where(x => x.Email == _selectedUserEmail).Select(x => x.Id).First();
        var request = new RemoveRoleFromUserRequest() { RoleName = _selectedRole, UserId = userId! };
        var (_, statusCode) = await HttpClient.PostAsync<RemoveRoleFromUserRequest, RemoveRoleFromUserResponse>(Paths.ApiPath_RemoveRole, request);

        if (statusCode == HttpStatusCode.OK)
        {
            _showRoleRemoveSuccess = true;
        }

        await GetUserRoles();
        StateHasChanged();
    }

    private async Task GetUserRoles()
    {
        _showRoleAddSuccess = false;
        _showRoleRemoveSuccess = false;

        var userDto = _users.First(x => x.Email == _selectedUserEmail);

        var (result, statusCode) = await HttpClient.PostAsync<TwilightImperiumUserDto, ApiResponse<ItemListDto<RoleDto>>>(Paths.ApiPath_SpecificUserRoles, userDto);

        if (statusCode == HttpStatusCode.OK)
        {
            _specificUserRoles = result!.Data!.Items;
        }

        StateHasChanged();
    }

    private bool IsAddRolePossible()
    {
        return _specificUserRoles.Select(x => x.Name).Contains(_selectedRole);
    }

    private bool IsRemoveRolePossible()
    {
        return !_specificUserRoles.Select(x => x.Name).Contains(_selectedRole);
    }
}