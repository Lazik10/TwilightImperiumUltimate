using MediatR;
using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Business.Galaxy;

public class GetAllPlanetsCommand : IRequest<List<Planet>>
{
}
