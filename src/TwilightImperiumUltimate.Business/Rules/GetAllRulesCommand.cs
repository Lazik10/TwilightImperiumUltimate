using MediatR;
using TwilightImperiumUltimate.Core.Entities.Rules;

namespace TwilightImperiumUltimate.Business.Rules;

public class GetAllRulesCommand : IRequest<List<Rule>>
{
}
