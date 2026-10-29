using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Logic.Users;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Users;

public sealed class LinkDiscordPlayerProfilesCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenProfilesAreFound_ShouldPersistProfileIds()
    {
        var userRepository = new Mock<IUserRepository>();
        const string userId = "website-user";
        const int asyncProfileId = 42;
        const int tiglProfileId = 17;
        using var cancellationSource = new CancellationTokenSource();

        userRepository
            .Setup(repository => repository.UpdateLinkedPlayerProfiles(
                userId,
                "discord.account",
                asyncProfileId,
                tiglProfileId,
                cancellationSource.Token))
            .ReturnsAsync(true);

        var handler = new LinkDiscordPlayerProfilesCommandHandler(userRepository.Object);

        var result = await handler.Handle(
            new LinkDiscordPlayerProfilesCommand(userId, "discord.account", asyncProfileId, tiglProfileId),
            cancellationSource.Token);

        result.Should().BeTrue();
        userRepository.Verify(
            repository => repository.UpdateLinkedPlayerProfiles(
                userId,
                "discord.account",
                asyncProfileId,
                tiglProfileId,
                cancellationSource.Token),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProfilesAreMissing_ShouldPersistNullProfileIds()
    {
        var userRepository = new Mock<IUserRepository>();
        const string userId = "website-user";
        using var cancellationSource = new CancellationTokenSource();

        userRepository
            .Setup(repository => repository.UpdateLinkedPlayerProfiles(
                userId,
                "discord.account",
                null,
                null,
                cancellationSource.Token))
            .ReturnsAsync(true);

        var handler = new LinkDiscordPlayerProfilesCommandHandler(userRepository.Object);

        var result = await handler.Handle(
            new LinkDiscordPlayerProfilesCommand(userId, "discord.account", null, null),
            cancellationSource.Token);

        result.Should().BeTrue();
        userRepository.Verify(
            repository => repository.UpdateLinkedPlayerProfiles(
                userId,
                "discord.account",
                null,
                null,
                cancellationSource.Token),
            Times.Once);
    }
}
