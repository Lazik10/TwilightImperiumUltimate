using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.API.Helpers;
using TwilightImperiumUltimate.Contracts.ApiContracts.User;
using TwilightImperiumUltimate.Contracts.DTOs.User;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.API.API.User;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(
    IDbContextFactory<TwilightImperiumDbContext> dbContext,
    IUserRoleStore<TwilightImperiumUser> userRoleStore)
    : ControllerBase
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = dbContext;
    private readonly IUserRoleStore<TwilightImperiumUser> _userRoleStore = userRoleStore;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TwilightImperiumUser>>> GetAllUsers()
    {
        using var context = await _context.CreateDbContextAsync();

        return await context.Users.ToListAsync();
    }

    // GET: api/user/byemail
    [HttpPost]
    [Route("byemail")]
    public async Task<TwilightImperiumUserDto> GetUserInfoByEmail(UserByEmailRequest request, CancellationToken ct)
    {
        using var context = await _context.CreateDbContextAsync(ct);

        var user = await context.Users.FirstAsync(x => x.Email == request.Email, ct);
        var roles = await _userRoleStore.GetRolesAsync(user, ct);
        var userRoles = RoleHelper.GetValidUserRoles([.. roles]);

        return new TwilightImperiumUserDto(
            user!.Id,
            user.UserName,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Age,
            user.FavoriteFaction,
            user.UserInfo,
            user.DiscordId,
            user.SteamId);
    }

    // PUT: api/user/update
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateUser(TwilightImperiumUserDto tiUser, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(tiUser);
        using var context = await _context.CreateDbContextAsync(ct);

        if (!UserExists(tiUser.Email!))
        {
            return NotFound(tiUser);
        }

        try
        {
            var dbUser = await context.Users.FirstAsync(x => x.Id == tiUser.Id, ct);
            var updatedUser = UpdatedUser(dbUser, tiUser);

            context.Entry(updatedUser).State = EntityState.Modified;

            await context.SaveChangesAsync(ct);

            return Ok(tiUser);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }

    private static TwilightImperiumUser UpdatedUser(TwilightImperiumUser dbUser, TwilightImperiumUserDto user)
    {
        dbUser.FirstName = user.FirstName;
        dbUser.LastName = user.LastName;
        dbUser.Age = user.Age;
        dbUser.FavoriteFaction = user.FavoriteFaction;
        dbUser.UserInfo = user.UserInfo;
        dbUser.UserName = user.UserName;
        dbUser.Email = user.Email;
        dbUser.PhoneNumber = user.PhoneNumber;
        dbUser.DiscordId = user.DiscordId;
        dbUser.SteamId = user.SteamId;

        return dbUser;
    }

    private bool UserExists(string email)
    {
        using var context = _context.CreateDbContext();

        return context.Users.Any(e => e.Email == email);
    }
}
