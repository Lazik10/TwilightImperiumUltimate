using TwilightImperiumUltimate.Contracts.ApiContracts.User;
using TwilightImperiumUltimate.Contracts.DTOs.User;

namespace TwilightImperiumUltimate.Web.Components.Admin;

public partial class RoleAssignment
{
    private IReadOnlyCollection<TwilightImperiumUserDto> _users = new List<TwilightImperiumUserDto>();

    private IReadOnlyCollection<RoleDto> _roles = new List<RoleDto>();

    private IReadOnlyCollection<RoleDto> _specificUserRoles = new List<RoleDto>();

    /// <summary>
    /// Every user's email and login (username), combined into a single alphabetically sorted
    /// lookup list, so the autocomplete can offer both at once -- useful "just in case" the
    /// exact email address isn't known. Computed once per <see cref="LoadUsers"/> call (NOT a
    /// property recomputed on every render): RadzenAutoComplete's own internal filtering/open
    /// state got unreliable when handed a freshly-materialized IEnumerable on every keystroke.
    /// </summary>
    private List<string> _userLookupSuggestions = [];

    private string _userSearchText = string.Empty;

    private string _selectedUserEmail = string.Empty;

    private string _selectedRole = string.Empty;

    private bool _showRoleAddSuccess;

    private bool _showRoleRemoveSuccess;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    private bool IsUserSelected => !string.IsNullOrEmpty(_selectedUserEmail);

    protected override async Task OnInitializedAsync()
    {
        await LoadUsers();
        await LoadRoles();
    }

    private async Task LoadUsers()
    {
        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<TwilightImperiumUserDto>>>(Paths.ApiPath_Users);
        var response = result.Response;
        var statusCode = result.StatusCode;
        if (statusCode == HttpStatusCode.OK)
        {
            _users = response!.Data!.Items;
            _userLookupSuggestions = _users
                .SelectMany(u => new[] { u.Email, u.UserName })
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(value => value, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }

    private async Task LoadRoles()
    {
        var result = await HttpClient.GetAsync<ApiResponse<ItemListDto<RoleDto>>>(Paths.ApiPath_Roles);
        var response = result.Response;
        var statusCode = result.StatusCode;

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

    /// <summary>
    /// Handles typing/selection in the player lookup autocomplete. Only resolves and switches
    /// the selected player once the text exactly matches a known email or login -- while the
    /// user is still typing a partial value, the previously selected player is left untouched.
    /// </summary>
    private async Task OnUserSearchChanged(string value)
    {
        _userSearchText = value;

        var user = _users.FirstOrDefault(x =>
            string.Equals(x.Email, value, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(x.UserName, value, StringComparison.OrdinalIgnoreCase));

        if (user is null)
        {
            _selectedUserEmail = string.Empty;
            _specificUserRoles = [];
            return;
        }

        _selectedUserEmail = user.Email ?? string.Empty;

        await GetUserRoles();
    }

    private async Task OnRoleSelected(string role)
    {
        _selectedRole = role;
        await GetUserRoles();
    }

    private async Task AddRoleToUser()
    {
        _showRoleAddSuccess = false;

        if (string.IsNullOrWhiteSpace(_selectedRole) || string.IsNullOrWhiteSpace(_selectedUserEmail))
            return;

        var userId = _users.Where(x => x.Email == _selectedUserEmail).Select(x => x.Id).First();
        var request = new AddRoleToUserRequest() { RoleName = _selectedRole, UserId = userId! };
        var result = await HttpClient.PostAsync<AddRoleToUserRequest, AddRoleToUserResponse>(Paths.ApiPath_AddRole, request);
        var statusCode = result.StatusCode;

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
        var result = await HttpClient.PostAsync<RemoveRoleFromUserRequest, RemoveRoleFromUserResponse>(Paths.ApiPath_RemoveRole, request);
        var statusCode = result.StatusCode;

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

        var userDto = _users.FirstOrDefault(x => x.Email == _selectedUserEmail);

        if (userDto is null)
            return;

        var result = await HttpClient.PostAsync<TwilightImperiumUserDto, ApiResponse<ItemListDto<RoleDto>>>(Paths.ApiPath_SpecificUserRoles, userDto);
        var response = result.Response;
        var statusCode = result.StatusCode;

        if (statusCode == HttpStatusCode.OK)
        {
            _specificUserRoles = response!.Data!.Items;
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
