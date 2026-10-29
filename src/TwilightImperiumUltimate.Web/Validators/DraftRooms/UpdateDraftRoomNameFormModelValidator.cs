using FluentValidation;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Validators.DraftRooms;

/// <summary>
/// Validates a room name update form.
/// </summary>
public sealed class UpdateDraftRoomNameFormModelValidator : AbstractValidator<UpdateDraftRoomNameFormModel>
{
    public UpdateDraftRoomNameFormModelValidator()
    {
        RuleFor(model => model.Name)
            .Cascade(CascadeMode.Stop)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage(ValidationMessages.DraftRooms_RoomNameRequired)
            .Must(name => name.Trim().Length >= DraftRoomLimits.MinRoomNameLength
                && name.Trim().Length <= DraftRoomLimits.MaxRoomNameLength)
            .WithMessage(ValidationMessages.DraftRooms_RoomNameLength);
    }
}
