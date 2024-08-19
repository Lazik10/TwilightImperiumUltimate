using FluentValidation;
using TwilightImperiumUltimate.Web.Models.Account;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Validators.Account;

public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
{
    public ResetPasswordModelValidator()
    {
        RuleFor(x => x.ResetCode)
            .NotEmpty().WithMessage(ValidationMessages.ResetPassword_EmptyResetCode);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(ValidationMessages.ResetPassword_EmptyPassword)
            .MinimumLength(6).WithMessage(ValidationMessages.ResetPassword_PasswordSixCharacters)
            .Matches(@"[A-Z]").WithMessage(ValidationMessages.ResetPassword_PasswordOneUpperLetter)
            .Matches(@"[a-z]").WithMessage(ValidationMessages.ResetPassword_PasswordOneLowerLetter)
            .Matches(@"[0-9]").WithMessage(ValidationMessages.ResetPassword_PasswordOneDigit)
            .Matches(@"[\W_]").WithMessage(ValidationMessages.ResetPassword_PasswordOneNonAlphaNumeric);

        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword).WithMessage(ValidationMessages.ResetPassword__ConfirmPasswordMatch);
    }
}
