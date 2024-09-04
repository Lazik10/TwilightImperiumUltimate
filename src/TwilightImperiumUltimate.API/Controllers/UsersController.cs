using TwilightImperiumUltimate.Business.Logic.Users;
using TwilightImperiumUltimate.Contracts.ApiContracts.User;
using TwilightImperiumUltimate.Contracts.DTOs.User;

namespace TwilightImperiumUltimate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(
    IMediator mediator)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IApiResponse<ItemListDto<TwilightImperiumUserDto>>>> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<TwilightImperiumUserDto>>() { Success = true, Data = users });
    }

    // GET: api/users/id
    [HttpPost]
    [Route("id")]
    public async Task<ActionResult<IApiResponse<TwilightImperiumUserDto>>> GetUserInfoById(TwilightImperiumUserDto user, CancellationToken cancellationToken)
    {
        var dbUser = await _mediator.Send(new GetUserByIdQuery(user.Id), cancellationToken);

        if (string.IsNullOrEmpty(dbUser.Id))
            return NotFound(new ApiResponse<TwilightImperiumUserDto>() { Success = false });

        return Ok(new ApiResponse<TwilightImperiumUserDto>() { Success = true, Data = dbUser });
    }

    // POST: api/users/email
    [HttpPost]
    [Route("email")]
    [AllowAnonymous]
    public async Task<ActionResult<IApiResponse<TwilightImperiumUserDto>>> GetUserInfoByEmail(UserByEmailRequest request, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserByEmailQuery(request.Email), cancellationToken);
        return Ok(new ApiResponse<TwilightImperiumUserDto>() { Success = true, Data = user });
    }

    // POST: api/users/login-and-email-check
    [HttpPost]
    [Route("login-and-email-check")]
    [AllowAnonymous]
    public async Task<ActionResult<IApiResponse<UserRegistrationPrecheckResponse>>> CheckRegistrationInformation(UserRegisterationPrecheckRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CheckRegistrationInfoQuery(request.Email, request.Username), cancellationToken);
        return Ok(new ApiResponse<UserRegistrationPrecheckResponse>() { Success = true, Data = response });
    }

    // PUT: api/users/update
    [HttpPut]
    [Route("update")]
    public async Task<ActionResult<IApiResponse<TwilightImperiumUserDto>>> UpdateUser(TwilightImperiumUserDto user, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new UpdateUserCommand(user), cancellationToken);

        if (response.Success)
            return Ok(response);
        else
            return Conflict(response);
    }

    // PUT: api/users/user-name-update
    [HttpPut]
    [Route("user-name-update")]
    [AllowAnonymous]
    public async Task<ActionResult<IApiResponse<TwilightImperiumUserDto>>> UpdateUserName(TwilightImperiumUserDto user, CancellationToken cancellationToken)
    {
        var dbUser = await _mediator.Send(new UpdateUserNameCommand(user.Email ?? string.Empty, user.UserName ?? string.Empty), cancellationToken);
        return Ok(new ApiResponse<TwilightImperiumUserDto>() { Success = true, Data = dbUser });
    }

    // GET: api/users/roles
    [HttpGet]
    [Route("roles")]
    public async Task<ActionResult<IApiResponse<ItemListDto<RoleDto>>>> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = await _mediator.Send(new GetAllRolesQuery(), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<RoleDto>>() { Success = true, Data = roles });
    }

    // POST: api/users/user-roles
    [HttpPost]
    [Route("user-roles")]
    public async Task<ActionResult<IApiResponse<ItemListDto<RoleDto>>>> GetAllSpecificUserRoles(TwilightImperiumUserDto twilightImperiumUserDto, CancellationToken cancellationToken)
    {
        var roles = await _mediator.Send(new GetAllUserRolesQuery(twilightImperiumUserDto.Id), cancellationToken);
        return Ok(new ApiResponse<ItemListDto<RoleDto>>() { Success = true, Data = roles });
    }

    // POST: api/users/add-role
    [HttpPost]
    [Route("add-role")]
    [AllowAnonymous]
    public async Task<ActionResult<IApiResponse<AddRoleToUserResponse>>> AddRoleToUser(AddRoleToUserRequest addUserRoleRequest, CancellationToken cancellationToken)
    {
        var success = await _mediator.Send(new AddUserToRoleCommand(addUserRoleRequest.UserId, addUserRoleRequest.RoleName), cancellationToken);
        return Ok(new ApiResponse<AddRoleToUserResponse>() { Success = success, Data = new AddRoleToUserResponse() { Success = success } });
    }

    // POST: api/users/remove-role
    [HttpPost]
    [Route("remove-role")]
    public async Task<ActionResult<IApiResponse<RemoveRoleFromUserResponse>>> RemoveRoleFromUser(RemoveRoleFromUserRequest removeRoleFromUser, CancellationToken cancellationToken)
    {
        var success = await _mediator.Send(new RemoveRoleFromUserCommand(removeRoleFromUser.UserId, removeRoleFromUser.RoleName), cancellationToken);
        return Ok(new ApiResponse<RemoveRoleFromUserResponse>() { Success = success, Data = new RemoveRoleFromUserResponse() { Success = success } });
    }
}
