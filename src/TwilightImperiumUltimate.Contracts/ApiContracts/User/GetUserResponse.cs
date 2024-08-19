using TwilightImperiumUltimate.Contracts.DTOs.User;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.User;

public class GetUserResponse
{
    public IReadOnlyCollection<TwilightImperiumUserDto> Users { get; set; } = Array.Empty<TwilightImperiumUserDto>();
}
