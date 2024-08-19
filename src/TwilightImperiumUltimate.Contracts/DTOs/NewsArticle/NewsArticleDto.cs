using TwilightImperiumUltimate.Contracts.DTOs.User;

namespace TwilightImperiumUltimate.Contracts.DTOs.NewsArticle;

public record NewsArticleDto(int Id, string Title, string Content, DateOnly CreatedAt, DateOnly UpdatedAt, TwilightImperiumUserDto User);
