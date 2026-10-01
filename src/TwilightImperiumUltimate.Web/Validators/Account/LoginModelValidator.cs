using FluentValidation;
using TwilightImperiumUltimate.Web.Models.Account;

namespace TwilightImperiumUltimate.Web.Validators.Account;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessages.Login_EmptyUsername);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.Login_EmptyPassword);
    }
}
