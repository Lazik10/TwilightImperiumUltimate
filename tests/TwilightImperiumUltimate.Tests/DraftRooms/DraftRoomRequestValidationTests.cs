using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomRequestValidationTests
{
    [Fact]
    public void CreateRequest_WhenValuesAreWithinLimits_ShouldBeValid()
    {
        var request = new CreateDraftRoomRequest("Friday TI", "friday1", "Alice");

        IsValid(request).Should().BeTrue();
    }

    [Theory]
    [InlineData("TI", "friday1", "Alice")]
    [InlineData(
        "1234567890123456789012345678901234567890123456789012345678901",
        "friday1",
        "Alice")]
    [InlineData("Friday TI", "short", "Alice")]
    [InlineData("Friday TI", "12345678901234567890123456789012345678901234567890123456789012345", "Alice")]
    [InlineData("Friday TI", "friday1", "A")]
    [InlineData("Friday TI", "friday1", "1234567890123456789012345")]
    public void CreateRequest_WhenAValueIsOutsideLimits_ShouldBeInvalid(
        string name,
        string password,
        string displayName)
    {
        var request = new CreateDraftRoomRequest(name, password, displayName);

        IsValid(request).Should().BeFalse();
    }

    [Fact]
    public void CreateRequest_WhenRequiredValuesAreMissing_ShouldBeInvalid()
    {
        var request = new CreateDraftRoomRequest(null!, null!, null);

        IsValid(request).Should().BeFalse();
    }

    [Fact]
    public void JoinRequest_WhenValuesAreWithinLimits_ShouldBeValid()
    {
        var request = new JoinDraftRoomRequest("ABC234", "friday1", "Alice");

        IsValid(request).Should().BeTrue();
    }

    [Fact]
    public void JoinRequest_WhenOptionalValuesAreMissing_ShouldBeValid()
    {
        var request = new JoinDraftRoomRequest("ABC234", null, null);

        IsValid(request).Should().BeTrue();
    }

    [Theory]
    [InlineData(null, "friday1", "Alice")]
    [InlineData("AB234", "friday1", "Alice")]
    [InlineData("ABC2345", "friday1", "Alice")]
    [InlineData("ABC234", "short", "Alice")]
    [InlineData("ABC234", "12345678901234567890123456789012345678901234567890123456789012345", "Alice")]
    [InlineData("ABC234", "friday1", "A")]
    [InlineData("ABC234", "friday1", "1234567890123456789012345")]
    public void JoinRequest_WhenAValueIsOutsideLimits_ShouldBeInvalid(
        string? code,
        string? password,
        string? displayName)
    {
        var request = new JoinDraftRoomRequest(code!, password, displayName);

        IsValid(request).Should().BeFalse();
    }

    [Fact]
    public void SetParticipantRoleRequest_WhenRoleIsDefined_ShouldBeValid()
    {
        var request = new SetParticipantRoleRequest(DraftRoomRole.CoHost);

        IsValid(request).Should().BeTrue();
    }

    [Fact]
    public void SetParticipantRoleRequest_WhenRoleIsUndefined_ShouldBeInvalid()
    {
        var request = new SetParticipantRoleRequest((DraftRoomRole)int.MaxValue);

        IsValid(request).Should().BeFalse();
    }

    [Fact]
    public void UpdateRoomSettingsRequest_WhenValuesAreValid_ShouldBeValid()
    {
        var request = new UpdateRoomSettingsRequest("Room", true);

        IsValid(request).Should().BeTrue();
    }

    [Fact]
    public void UpdateRoomSettingsRequest_WhenNameIsAtMaximumLength_ShouldBeValid()
    {
        var request = new UpdateRoomSettingsRequest(
            new string('R', DraftRoomLimits.MaxRoomNameLength),
            true);

        IsValid(request).Should().BeTrue();
    }

    [Theory]
    [InlineData("TI", true)]
    [InlineData(
        "1234567890123456789012345678901234567890123456789012345678901",
        false)]
    public void UpdateRoomSettingsRequest_WhenNameIsOutsideLimits_ShouldBeInvalid(
        string name,
        bool isLocked)
    {
        var request = new UpdateRoomSettingsRequest(name, isLocked);

        IsValid(request).Should().BeFalse();
    }

    [Fact]
    public void UpdateRoomSettingsRequest_WhenNameIsMissing_ShouldBeInvalid()
    {
        var request = new UpdateRoomSettingsRequest(null!, false);

        IsValid(request).Should().BeFalse();
    }

    [Fact]
    public void ChangeRoomPasswordRequest_WhenPasswordIsWithinLimits_ShouldBeValid()
    {
        var request = new ChangeRoomPasswordRequest("123456");

        IsValid(request).Should().BeTrue();
    }

    [Fact]
    public void ChangeRoomPasswordRequest_WhenPasswordIsAtMaximumLength_ShouldBeValid()
    {
        var request = new ChangeRoomPasswordRequest(
            new string('P', DraftRoomLimits.MaxPasswordLength));

        IsValid(request).Should().BeTrue();
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("12345678901234567890123456789012345678901234567890123456789012345")]
    public void ChangeRoomPasswordRequest_WhenPasswordIsOutsideLimits_ShouldBeInvalid(string password)
    {
        var request = new ChangeRoomPasswordRequest(password);

        IsValid(request).Should().BeFalse();
    }

    [Fact]
    public void ChangeRoomPasswordRequest_WhenPasswordIsMissing_ShouldBeInvalid()
    {
        var request = new ChangeRoomPasswordRequest(null!);

        IsValid(request).Should().BeFalse();
    }

    private static bool IsValid(object instance)
    {
        return Validator.TryValidateObject(
            instance,
            new ValidationContext(instance),
            new List<ValidationResult>(),
            validateAllProperties: true);
    }
}
