using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.Users;

public class UpdateUserCommand(
    TwilightImperiumUserDto userDto) : IRequest<ApiResponse<TwilightImperiumUserDto>>
{
    public TwilightImperiumUserDto User { get; set; } = userDto;
}
