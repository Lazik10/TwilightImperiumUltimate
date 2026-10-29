using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Validators.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Validators.DraftRooms;

public sealed class JoinDraftRoomFormModelValidatorTests
{
    private readonly JoinDraftRoomFormModelValidator _validator = new();

    [Fact]
    public void Validate_WhenRoomCodeAndCredentialsAreValid_ShouldPass()
    {
        var model = new JoinDraftRoomFormModel
        {
            Code = "abc234",
            Password = "secret1",
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
        model.Code.Should().Be("ABC234");
    }

    [Theory]
    [InlineData("ABC23")]
    [InlineData("ABC2345")]
    public void Validate_WhenRoomCodeLengthIsInvalid_ShouldFail(string code)
    {
        var model = new JoinDraftRoomFormModel
        {
            Code = code,
            Password = "secret1",
        };

        var result = _validator.Validate(model);

        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.Code));
    }

    [Fact]
    public void Validate_WhenRoomCodeContainsDisallowedCharacter_ShouldFail()
    {
        var model = new JoinDraftRoomFormModel
        {
            Code = "ABC20I",
            Password = "secret1",
        };

        var result = _validator.Validate(model);

        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.Code));
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("12345678901234567890123456789012345678901234567890123456789012345")]
    public void Validate_WhenPasswordIsOutsideAllowedLength_ShouldFail(string password)
    {
        var model = new JoinDraftRoomFormModel
        {
            Code = "ABC234",
            Password = password,
        };

        var result = _validator.Validate(model);

        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.Password));
    }

    [Fact]
    public void Validate_WhenReturningParticipantDoesNotProvideCredentials_ShouldPass()
    {
        var model = new JoinDraftRoomFormModel
        {
            Code = "ABC234",
            RequiresCredentials = false,
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenGuestDisplayNameIsMissing_ShouldFail()
    {
        var model = new JoinDraftRoomFormModel
        {
            Code = "ABC234",
            Password = "secret1",
            RequiresGuestDisplayName = true,
        };

        var result = _validator.Validate(model);

        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.DisplayName));
    }

    [Theory]
    [InlineData("A")]
    [InlineData("A display name that is too long")]
    public void Validate_WhenGuestDisplayNameIsOutsideAllowedLength_ShouldFail(string displayName)
    {
        var model = new JoinDraftRoomFormModel
        {
            Code = "ABC234",
            Password = "secret1",
            DisplayName = displayName,
            RequiresGuestDisplayName = true,
        };

        var result = _validator.Validate(model);

        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.DisplayName));
    }
}
