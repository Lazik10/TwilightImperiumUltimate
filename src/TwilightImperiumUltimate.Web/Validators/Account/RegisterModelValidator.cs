using FluentValidation;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Validators.Account;

public class RegisterModelValidator : AbstractValidator<RegistrationModel>
{
    public RegisterModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessages.Register_EmptyEmail)
            .EmailAddress().WithMessage(ValidationMessages.Register_EmailNotValid);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.Register_EmptyPassword)
            .MinimumLength(6).WithMessage(ValidationMessages.Register_PasswordSixCharacters)
            .Matches(@"[A-Z]").WithMessage(ValidationMessages.Register_PasswordOneUpperLetter)
            .Matches(@"[a-z]").WithMessage(ValidationMessages.Register_PasswordOneLowerLetter)
            .Matches(@"[0-9]").WithMessage(ValidationMessages.Register_PasswordOneDigit)
            .Matches(@"[\W_]").WithMessage(ValidationMessages.Register_PasswordOneNonAlphaNumeric);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage(ValidationMessages.Register_ConfirmPasswordMatch);
    }
}