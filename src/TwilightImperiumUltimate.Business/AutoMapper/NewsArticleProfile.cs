using TwilightImperiumUltimate.Contracts.DTOs.User;

namespace TwilightImperiumUltimate.Business.AutoMapperProfiles;

internal class NewsArticleProfile : Profile
{
    public NewsArticleProfile()
    {
        CreateMap<NewsArticle, NewsArticleDto>()
            .ConstructUsing((n, context) =>
            {
                var user = context.Mapper.Map<TwilightImperiumUserDto>(n.User);
                return new NewsArticleDto(
                    n.Id,
                    n.Title,
                    n.Content,
                    n.CreatedAt,
                    n.UpdatedAt,
                    user);
            });
    }
}
