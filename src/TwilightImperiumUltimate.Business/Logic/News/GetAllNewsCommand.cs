namespace TwilightImperiumUltimate.Business.Logic.News;

public class GetAllNewsCommand(int pageNumber, int pageSize) : IRequest<PagedItemListDto<NewsArticleDto>>
{
    public int PageNumber { get; } = pageNumber;

    public int PageSize { get; } = pageSize;
}
