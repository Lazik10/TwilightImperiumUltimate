using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomDisplayNameValidatorTests
{
    private readonly Mock<IUserRepository> userRepository = new(MockBehavior.Strict);
    private readonly DraftRoomDisplayNameValidator validator;

    public DraftRoomDisplayNameValidatorTests()
    {
        this.validator = new DraftRoomDisplayNameValidator(this.userRepository.Object);
    }

    [Fact]
    public void Normalize_WhenNameHasLeadingTrailingAndRepeatedSpaces_ShouldReturnCollapsedName()
    {
        var normalizedName = this.validator.Normalize("  Alice   Smith  ");

        normalizedName.Should().Be("Alice Smith");
    }

    [Fact]
    public void MakeKey_WhenNameContainsMixedCaseAndExtraSpaces_ShouldReturnNormalizedUppercase()
    {
        var nameKey = this.validator.MakeKey("  Alice   Smith  ");

        nameKey.Should().Be("ALICE SMITH");
    }

    [Theory]
    [InlineData("A")]
    [InlineData("ThisDisplayNameIsTooLongA")]
    [InlineData("Alice!")]
    [InlineData("Alice\tSmith")]
    [InlineData("Alice\nSmith")]
    public async Task ValidateGuestName_WhenNameViolatesDisplayRules_ShouldReturnFalse(string displayName)
    {
        var isValid = await this.validator.ValidateGuestName(displayName);

        isValid.Should().BeFalse();
        this.userRepository.Verify(
            repository => repository.GetUserByUserName(It.IsAny<string>()),
            Times.Never);
    }

    [Theory]
    [InlineData("AB", "AB")]
    [InlineData("123456789012345678901234", "123456789012345678901234")]
    [InlineData("  Alice   Smith ", "Alice Smith")]
    [InlineData(" O'Neil_1-A.B ", "O'Neil_1-A.B")]
    public async Task ValidateGuestName_WhenNameMeetsLengthAndCharacterRules_ShouldReturnTrue(
        string displayName,
        string normalizedName)
    {
        this.userRepository
            .Setup(repository => repository.GetUserByUserName(normalizedName))
            .ReturnsAsync((TwilightImperiumUser?)null);

        var isValid = await this.validator.ValidateGuestName(displayName);

        isValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateGuestName_WhenNameMatchesRegisteredUsernameIgnoringCase_ShouldReturnFalse()
    {
        this.userRepository
            .Setup(repository => repository.GetUserByUserName("alice"))
            .ReturnsAsync(new TwilightImperiumUser { UserName = "Alice" });

        var isValid = await this.validator.ValidateGuestName("alice");

        isValid.Should().BeFalse();
    }
}
