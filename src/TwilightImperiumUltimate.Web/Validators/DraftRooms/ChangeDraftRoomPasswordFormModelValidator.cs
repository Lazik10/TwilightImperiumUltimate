using FluentValidation;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Validators.DraftRooms;

/// <summary>
/// Validates a room password change form.
/// </summary>
public sealed class ChangeDraftRoomPasswordFormModelValidator : AbstractValidator<ChangeDraftRoomPasswordFormModel>
{
    public ChangeDraftRoomPasswordFormModelValidator()
    {
        RuleFor(model => model.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ValidationMessages.DraftRooms_PasswordRequired)
            .Must(password => password.Length >= DraftRoomLimits.MinPasswordLength
                && password.Length <= DraftRoomLimits.MaxPasswordLength)
            .WithMessage(ValidationMessages.DraftRooms_PasswordLength);

        RuleFor(model => model.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ValidationMessages.DraftRooms_PasswordRequired)
            .Equal(model => model.Password)
            .WithMessage(ValidationMessages.ResetPassword__ConfirmPasswordMatch);
    }
}
