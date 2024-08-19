using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwilightImperiumUltimate.Contracts.ApiContracts.User;
using TwilightImperiumUltimate.Core.Entities.Users;

namespace TwilightImperiumUltimate.API.API.User;

[Route("api/[controller]")]
[ApiController]
public class RoleController(
    IRoleStore<IdentityRole> roleStore,
    IUserStore<TwilightImperiumUser> userStore,
    IUserRoleStore<TwilightImperiumUser> userRoleStore) : ControllerBase
{
    private readonly IRoleStore<IdentityRole> _roleStore = roleStore;
    private readonly IUserStore<TwilightImperiumUser> _userStore = userStore;
    private readonly IUserRoleStore<TwilightImperiumUser> _userRoleStore = userRoleStore;

    [HttpPost]
    [Route("adduserrole")]
    public async Task<IActionResult> AddUserRole([FromBody] AddUserRoleRequest model, CancellationToken ct)
    {
        var user = await _userStore.FindByIdAsync(model.UserId, ct);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        else
        {
            await _userRoleStore.AddToRoleAsync(user, model.RoleName, ct);
            return Ok($"Role {model.RoleName} added to user {model.UserId} successfully.");
        }
    }

    [HttpPost]
    [Route("addrole")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(roleName))
        {
            return BadRequest("Role name cannot be empty.");
        }

        var roleExist = await _roleStore.FindByNameAsync(roleName, ct);
        if (roleExist is not null)
        {
            var result = await _roleStore.CreateAsync(new IdentityRole(roleName), ct);
            if (result.Succeeded)
            {
                return Ok($"Role {roleName} created successfully.");
            }

            return BadRequest(result.Errors);
        }

        return BadRequest("Role already exists.");
    }
}
