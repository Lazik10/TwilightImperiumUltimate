using MediatR;
using TwilightImperiumUltimate.Core.Entities.News;

namespace TwilightImperiumUltimate.Business.News;

public class GetAllNewsCommand : IRequest<List<NewsArticle>>
{
}
