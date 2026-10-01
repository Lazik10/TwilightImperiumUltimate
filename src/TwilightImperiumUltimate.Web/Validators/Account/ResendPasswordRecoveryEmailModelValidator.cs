using FluentValidation;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Validators.Account;

public class ResendPasswordRecoveryEmailModelValidator : AbstractValidator<ResendPasswordRecoveryEmailModel>
{
    public ResendPasswordRecoveryEmailModelValidator()
    {
        RuleFor(model => model.Email)
            .NotEmpty().WithMessage(ValidationMessages.Register_EmptyEmail)
            .EmailAddress().WithMessage(ValidationMessages.Register_EmailNotValid);
    }
}