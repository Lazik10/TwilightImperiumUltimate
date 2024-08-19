using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwilightImperiumUltimate.Contracts.ApiContracts.User;
using TwilightImperiumUltimate.Core.Entities.Users;

namespace TwilightImperiumUltimate.API.API.User;

[Route("api/[controller]")]
[ApiController]
public class ClaimsController(
    IUserStore<TwilightImperiumUser> userStore,
    IUserClaimStore<TwilightImperiumUser> userClaimStore)
    : ControllerBase
{
    private readonly IUserStore<TwilightImperiumUser> _userStore = userStore;
    private readonly IUserClaimStore<TwilightImperiumUser> _userClaimStore = userClaimStore;

    [HttpPost]
    [Route("addclaim")]
    public async Task<IActionResult> AddClaim([FromBody] AddClaimRequest model, CancellationToken ct)
    {
        var user = await _userStore.FindByIdAsync(model.UserId, ct);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var claim = new Claim(model.ClaimType, model.ClaimValue);
        IEnumerable<Claim> claims = [claim];
        await _userClaimStore.AddClaimsAsync(user, claims, ct);

        return Ok("Claim added successfully.");
    }

    [HttpPost]
    [Route("adduserclaim")]
    public async Task<IActionResult> AddUserClaim([FromBody] AddUserClaimRequest model, CancellationToken ct)
    {
        var user = await _userStore.FindByIdAsync(model.UserId, ct);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var claim = new Claim(model.ClaimType, model.ClaimValue);
        IEnumerable<Claim> claims = [claim];
        await _userClaimStore.AddClaimsAsync(user, claims, ct);

        return Ok("Claim added successfully.");
    }
}
