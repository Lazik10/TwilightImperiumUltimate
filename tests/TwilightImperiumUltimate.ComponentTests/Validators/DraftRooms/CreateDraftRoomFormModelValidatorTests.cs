using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Validators.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Validators.DraftRooms;

public sealed class CreateDraftRoomFormModelValidatorTests
{
    private readonly CreateDraftRoomFormModelValidator _validator = new();

    [Fact]
    public void Validate_WhenRoomNameAndPasswordAreValid_ShouldPass()
    {
        var model = new CreateDraftRoomFormModel
        {
            Name = "Room One",
            Password = "secret1",
            DisplayName = "Guest One",
            IsGuest = true,
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("AB")]
    [InlineData("A room name that is deliberately longer than sixty characters")]
    public void Validate_WhenRoomNameIsOutsideAllowedLength_ShouldFail(string roomName)
    {
        var model = new CreateDraftRoomFormModel
        {
            Name = roomName,
            Password = "secret1",
        };

        var result = _validator.Validate(model);

        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.Name));
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("12345678901234567890123456789012345678901234567890123456789012345")]
    public void Validate_WhenPasswordIsOutsideAllowedLength_ShouldFail(string password)
    {
        var model = new CreateDraftRoomFormModel
        {
            Name = "Room One",
            Password = password,
        };

        var result = _validator.Validate(model);

        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.Password));
    }

    [Fact]
    public void Validate_WhenGuestDisplayNameIsMissing_ShouldFail()
    {
        var model = new CreateDraftRoomFormModel
        {
            Name = "Room One",
            Password = "secret1",
            IsGuest = true,
        };

        var result = _validator.Validate(model);

        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.DisplayName));
    }

    [Theory]
    [InlineData("A")]
    [InlineData("A display name that is too long")]
    public void Validate_WhenGuestDisplayNameIsOutsideAllowedLength_ShouldFail(string displayName)
    {
        var model = new CreateDraftRoomFormModel
        {
            Name = "Room One",
            Password = "secret1",
            DisplayName = displayName,
            IsGuest = true,
        };

        var result = _validator.Validate(model);

        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.DisplayName));
    }

    [Fact]
    public void Validate_WhenAccountDisplayNameIsMissing_ShouldPass()
    {
        var model = new CreateDraftRoomFormModel
        {
            Name = "Room One",
            Password = "secret1",
        };

        var result = _validator.Validate(model);

        result.Errors.Should().NotContain(error => error.PropertyName == nameof(model.DisplayName));
    }
}
