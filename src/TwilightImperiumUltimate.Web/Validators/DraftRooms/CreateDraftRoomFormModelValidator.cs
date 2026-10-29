using FluentValidation;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Validators.DraftRooms;

/// <summary>
/// Validates the client-side create-room form.
/// </summary>
public sealed class CreateDraftRoomFormModelValidator : AbstractValidator<CreateDraftRoomFormModel>
{
    public CreateDraftRoomFormModelValidator()
    {
        RuleFor(model => model.Name)
            .Cascade(CascadeMode.Stop)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage(ValidationMessages.DraftRooms_RoomNameRequired)
            .Must(name => name.Trim().Length >= DraftRoomLimits.MinRoomNameLength
                && name.Trim().Length <= DraftRoomLimits.MaxRoomNameLength)
            .WithMessage(ValidationMessages.DraftRooms_RoomNameLength);

        RuleFor(model => model.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ValidationMessages.DraftRooms_PasswordRequired)
            .Must(password => password.Length >= DraftRoomLimits.MinPasswordLength
                && password.Length <= DraftRoomLimits.MaxPasswordLength)
            .WithMessage(ValidationMessages.DraftRooms_PasswordLength);

        When(model => model.IsGuest, () =>
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
