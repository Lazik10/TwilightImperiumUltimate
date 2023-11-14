using MediatR;
using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.News;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.Business.News;

public class GetAllNewsCommandHandler : IRequestHandler<GetAllNewsCommand, List<NewsArticle>>
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _dbContextFactory;

    public GetAllNewsCommandHandler(IDbContextFactory<TwilightImperiumDbContext> dbContextFactory)
    {
       _dbContextFactory = dbContextFactory;
    }

    public async Task<List<NewsArticle>> Handle(GetAllNewsCommand request, CancellationToken cancellationToken)
    {
        using var context = _dbContextFactory.CreateDbContext();

        return await context.NewsArticles
            .Include(x => x.User)
            .OrderByDescending(x => x.Id)
            .ToListAsync(cancellationToken);
    }
}
