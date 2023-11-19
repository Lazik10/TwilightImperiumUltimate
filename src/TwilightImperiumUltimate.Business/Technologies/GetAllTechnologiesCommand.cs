using MediatR;
using TwilightImperiumUltimate.Core.Entities.Technologies;

namespace TwilightImperiumUltimate.Business.Technologies;

public class GetAllTechnologiesCommand : IRequest<List<Technology>>
{
}
