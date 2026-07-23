using FluentValidation;
using TwilightImperiumUltimate.Web.Models.Website;

namespace TwilightImperiumUltimate.Web.Validators.Website;

public class WebsiteFormModelValidator : AbstractValidator<WebsiteFormModel>
{
    public WebsiteFormModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ValidationMessages.WebsiteAdmin_EmptyTitle)
            .MaximumLength(100).WithMessage(ValidationMessages.WebsiteAdmin_TitleTooLong);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationMessages.WebsiteAdmin_EmptyDescription)
            .MaximumLength(500).WithMessage(ValidationMessages.WebsiteAdmin_DescriptionTooLong);

        RuleFor(x => x.WebsitePath)
            .NotEmpty().WithMessage(ValidationMessages.WebsiteAdmin_EmptyWebsitePath)
            .Must(path => Uri.TryCreate(path, UriKind.Absolute, out _)).WithMessage(ValidationMessages.WebsiteAdmin_WebsitePathNotValid);
    }
}
