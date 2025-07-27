using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class ChangeUserNameCommand(
    ChangeTiglUserNameRequest changeTiglUserNameRequest)
    : IRequest<NewTiglUserResponse>
{
    public long DiscordId { get; set; } = changeTiglUserNameRequest.DiscordId;

    public string NewTiglUserName { get; set; } = changeTiglUserNameRequest.NewTiglUserName;
}
