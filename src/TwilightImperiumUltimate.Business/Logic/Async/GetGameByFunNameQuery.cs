using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetGameByFunNameQuery(string funName) : IRequest<AsyncGameDto>
{
    public string FunName { get; set; } = funName;
}
