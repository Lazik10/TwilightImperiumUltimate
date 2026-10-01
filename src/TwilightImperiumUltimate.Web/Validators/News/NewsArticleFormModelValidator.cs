using FluentValidation;
using TwilightImperiumUltimate.Web.Models.News;

namespace TwilightImperiumUltimate.Web.Validators.News;

public class NewsArticleFormModelValidator : AbstractValidator<NewsArticleFormModel>
{
    public NewsArticleFormModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ValidationMessages.NewsAdmin_EmptyTitle)
            .MaximumLength(255).WithMessage(ValidationMessages.NewsAdmin_TitleTooLong);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage(ValidationMessages.NewsAdmin_EmptyContent);
    }
}
