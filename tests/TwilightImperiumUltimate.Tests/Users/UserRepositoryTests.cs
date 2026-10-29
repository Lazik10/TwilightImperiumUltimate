using FluentAssertions;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.Users;

public sealed class UserRepositoryTests
{
    [Fact]
    public void UpdateDbUser_WhenLinkedProfileIdsDiffer_ShouldPreservePersistedIds()
    {
        var dbUser = new TwilightImperiumUser
        {
            Id = "website-user",
            AsyncPlayerProfileId = 42,
            TiglPlayerProfileId = 17,
        };
        var update = new TwilightImperiumUser
        {
            Id = "website-user",
            AsyncPlayerProfileId = 99,
            TiglPlayerProfileId = 88,
        };

        UserRepository.UpdateDbUser(dbUser, update);

        dbUser.AsyncPlayerProfileId.Should().Be(42);
        dbUser.TiglPlayerProfileId.Should().Be(17);
    }
}
