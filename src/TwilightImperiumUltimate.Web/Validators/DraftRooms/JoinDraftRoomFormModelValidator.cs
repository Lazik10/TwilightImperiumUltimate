using FluentValidation;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Validators.DraftRooms;

/// <summary>
/// Validates the client-side join-room form.
/// </summary>
public sealed class JoinDraftRoomFormModelValidator : AbstractValidator<JoinDraftRoomFormModel>
{
    public JoinDraftRoomFormModelValidator()
    {
        RuleFor(model => model.Code)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ValidationMessages.DraftRooms_CodeRequired)
            .Length(DraftRoomLimits.CodeLength)
            .WithMessage(ValidationMessages.DraftRooms_CodeLength)
            .Must(code => code.All(DraftRoomLimits.CodeAlphabet.Contains))
            .WithMessage(ValidationMessages.DraftRooms_CodeCharacters);

        When(model => model.RequiresCredentials, () =>
        {
            RuleFor(model => model.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ValidationMessages.DraftRooms_PasswordRequired)
                .Must(password => password.Length >= DraftRoomLimits.MinPasswordLength
                    && password.Length <= DraftRoomLimits.MaxPasswordLength)
                .WithMessage(ValidationMessages.DraftRooms_PasswordLength);
        });

        When(model => model.RequiresGuestDisplayName, () =>
        {
            RuleFor(model => model.DisplayName)
                .Cascade(CascadeMode.Stop)
                .Must(displayName => !string.IsNullOrWhiteSpace(displayName))
                .WithMessage(ValidationMessages.DraftRooms_DisplayNameRequired)
                .Must(displayName => displayName.Trim().Length >= DraftRoomLimits.MinDisplayNameLength
                    && displayName.Trim().Length <= DraftRoomLimits.MaxDisplayNameLength)
                .WithMessage(ValidationMessages.DraftRooms_DisplayNameLength);
        });
    }
}
