using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class RegisterNewTiglUserCommand(
    NewTiglUserRequest newTiglUserRequest)
    : IRequest<Result<int>>
{
    public long DiscordId { get; set; } = newTiglUserRequest.DiscordId;

    public string DiscordTag { get; set; } = newTiglUserRequest.DiscordTag;

    public string TiglUserName { get; set; } = newTiglUserRequest.TiglUserName;
}
