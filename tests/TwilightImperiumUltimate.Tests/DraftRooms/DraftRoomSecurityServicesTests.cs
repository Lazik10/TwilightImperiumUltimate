using System.Security.Cryptography;
using FluentAssertions;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomSecurityServicesTests
{
    private readonly GuestSecretService guestSecretService = new();
    private readonly RoomCodeGenerator roomCodeGenerator = new();
    private readonly RoomPasswordHasher roomPasswordHasher = new();

    [Fact]
    public void Generate_WhenCreatingManyRoomCodes_ShouldUseConfiguredLengthAndAlphabet()
    {
        var codes = Enumerable.Range(0, 10_000)
            .Select(_ => this.roomCodeGenerator.Generate())
            .ToArray();

        codes.Should().OnlyContain(code =>
            code.Length == DraftRoomLimits.CodeLength
            && code.All(DraftRoomLimits.CodeAlphabet.Contains));
    }

    [Fact]
    public void Generate_WhenCreatingGuestSecrets_ShouldReturnUniqueBase64UrlSecrets()
    {
        var secrets = Enumerable.Range(0, 1_000)
            .Select(_ => this.guestSecretService.Generate())
            .ToArray();

        secrets.Should().OnlyContain(secret =>
            secret.Length == 43
            && secret.All(character =>
                char.IsAsciiLetterOrDigit(character) || character == '-' || character == '_'));
        secrets.Distinct().Should().HaveCount(secrets.Length);
    }

    [Fact]
    public void Verify_WhenGuestSecretMatchesStoredHash_ShouldReturnTrue()
    {
        var secret = this.guestSecretService.Generate();
        var hash = this.guestSecretService.Hash(secret);

        hash.Should().HaveCount(32);
        this.guestSecretService.Verify(secret, hash).Should().BeTrue();
    }

    [Fact]
    public void Hash_WhenHashingGuestSecret_ShouldReturnSha256Digest()
    {
        var hash = this.guestSecretService.Hash("abc");

        Convert.ToHexString(hash)
            .Should()
            .Be("BA7816BF8F01CFEA414140DE5DAE2223B00361A396177A9CB410FF61F20015AD");
    }

    [Fact]
    public void Verify_WhenGuestSecretIsChanged_ShouldReturnFalse()
    {
        var secret = this.guestSecretService.Generate();
        var hash = this.guestSecretService.Hash(secret);
        var changedSecret = string.Concat(secret, "changed");

        this.guestSecretService.Verify(changedSecret, hash).Should().BeFalse();
    }

    [Fact]
    public void Hash_WhenHashingSameRoomPasswordTwice_ShouldUseDifferentSalts()
    {
        var room = new DraftRoom();
        const string password = "room-password";

        var firstHash = this.roomPasswordHasher.Hash(room, password);
        var secondHash = this.roomPasswordHasher.Hash(room, password);

        firstHash.Should().NotBe(secondHash);
    }

    [Fact]
    public void Verify_WhenRoomPasswordIsCorrect_ShouldReturnTrue()
    {
        var room = new DraftRoom();
        var passwordHash = this.roomPasswordHasher.Hash(room, "room-password");

        this.roomPasswordHasher.Verify(room, passwordHash, "room-password").Should().BeTrue();
    }

    [Fact]
    public void Verify_WhenRoomPasswordIsIncorrect_ShouldReturnFalse()
    {
        var room = new DraftRoom();
        var passwordHash = this.roomPasswordHasher.Hash(room, "room-password");

        this.roomPasswordHasher.Verify(room, passwordHash, "incorrect-password").Should().BeFalse();
    }

    [Fact]
    public void Verify_WhenPasswordHashIsInvalid_ShouldReturnFalse()
    {
        var room = new DraftRoom();
        var invalidHash = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        this.roomPasswordHasher.Verify(room, invalidHash, "password").Should().BeFalse();
    }

    [Fact]
    public void VerifyDummyPassword_WhenCandidateDoesNotMatch_ShouldReturnFalse()
    {
        this.roomPasswordHasher.VerifyDummyPassword("room-password").Should().BeFalse();
    }
}
