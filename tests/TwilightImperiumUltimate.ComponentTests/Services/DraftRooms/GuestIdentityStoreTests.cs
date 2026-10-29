using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Services.DraftRooms;

public sealed class GuestIdentityStoreTests
{
    private const string StorageKey = "draftRooms.guests.v1";

    [Fact]
    public async Task GetAsync_WhenCodeDiffersOnlyByCase_ShouldReturnSavedIdentity()
    {
        var expectedIdentity = CreateIdentity("abc234");
        var localStorage = new Mock<ILocalStorageService>();
        localStorage
            .Setup(storage => storage.GetItemAsync<Dictionary<string, GuestIdentity>>(StorageKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, GuestIdentity>
            {
                ["abc234"] = expectedIdentity,
            });
        var store = CreateStore(localStorage.Object);

        var result = await store.GetAsync("ABC234");

        result.Should().BeEquivalentTo(expectedIdentity with { Code = "ABC234" });
    }

    [Fact]
    public async Task SaveAsync_WhenIdentityIsSaved_ShouldNormalizeTheRoomCode()
    {
        var localStorage = new Mock<ILocalStorageService>();
        localStorage
            .Setup(storage => storage.GetItemAsync<Dictionary<string, GuestIdentity>>(StorageKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Dictionary<string, GuestIdentity>?)null);
        Dictionary<string, GuestIdentity>? savedIdentities = null;
        localStorage
            .Setup(storage => storage.SetItemAsync(
                StorageKey,
                It.IsAny<Dictionary<string, GuestIdentity>>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, Dictionary<string, GuestIdentity>, CancellationToken>((_, identities, _) =>
                savedIdentities = identities)
            .Returns(ValueTask.CompletedTask);
        var store = CreateStore(localStorage.Object);

        await store.SaveAsync(CreateIdentity("abc234"));

        savedIdentities.Should().ContainKey("ABC234");
        savedIdentities!["ABC234"].Code.Should().Be("ABC234");
    }

    [Fact]
    public async Task RemoveAsync_WhenCodeDiffersOnlyByCase_ShouldRemoveSavedIdentity()
    {
        var localStorage = new Mock<ILocalStorageService>();
        localStorage
            .Setup(storage => storage.GetItemAsync<Dictionary<string, GuestIdentity>>(StorageKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, GuestIdentity>
            {
                ["abc234"] = CreateIdentity("abc234"),
            });
        Dictionary<string, GuestIdentity>? savedIdentities = null;
        localStorage
            .Setup(storage => storage.SetItemAsync(
                StorageKey,
                It.IsAny<Dictionary<string, GuestIdentity>>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, Dictionary<string, GuestIdentity>, CancellationToken>((_, identities, _) =>
                savedIdentities = identities)
            .Returns(ValueTask.CompletedTask);
        var store = CreateStore(localStorage.Object);

        await store.RemoveAsync("ABC234");

        savedIdentities.Should().NotContainKey("abc234");
    }

    [Fact]
    public async Task ListAsync_WhenIdentitiesExist_ShouldReturnAllIdentities()
    {
        var firstIdentity = CreateIdentity("ABC234");
        var secondIdentity = CreateIdentity("DEF567");
        var localStorage = new Mock<ILocalStorageService>();
        localStorage
            .Setup(storage => storage.GetItemAsync<Dictionary<string, GuestIdentity>>(StorageKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, GuestIdentity>
            {
                [firstIdentity.Code] = firstIdentity,
                [secondIdentity.Code] = secondIdentity,
            });
        var store = CreateStore(localStorage.Object);

        var result = await store.ListAsync();

        result.Should().BeEquivalentTo([firstIdentity, secondIdentity]);
    }

    [Fact]
    public async Task ListAsync_WhenStoredJsonIsCorrupt_ShouldReturnEmptyList()
    {
        var localStorage = new Mock<ILocalStorageService>();
        localStorage
            .Setup(storage => storage.GetItemAsync<Dictionary<string, GuestIdentity>>(StorageKey, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new JsonException());
        var store = CreateStore(localStorage.Object);

        var result = await store.ListAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public void GuestIdentity_WhenDeserializedFromOlderStoredJson_ShouldHaveNoRoomName()
    {
        var legacyIdentity = new
        {
            RoomId = Guid.NewGuid(),
            Code = "ABC234",
            ParticipantId = Guid.NewGuid(),
            Secret = "guest-secret",
            DisplayName = "Guest Player",
            SavedAtUtc = DateTimeOffset.UtcNow,
        };
        var json = JsonSerializer.Serialize(legacyIdentity);

        var result = JsonSerializer.Deserialize<GuestIdentity>(json);

        result.Should().NotBeNull();
        result!.RoomName.Should().BeNull();
    }

    private static GuestIdentityStore CreateStore(ILocalStorageService localStorage) =>
        new(localStorage, NullLogger<GuestIdentityStore>.Instance);

    private static GuestIdentity CreateIdentity(string code) =>
        new(Guid.NewGuid(), code, Guid.NewGuid(), "guest-secret", "Guest Player", DateTimeOffset.UtcNow);
}
