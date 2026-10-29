using TwilightImperiumUltimate.IntegrationTests.Infrastructure;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

public sealed class DraftRoomsModelTestsFixture : IAsyncLifetime
{
    internal TestDatabase Database { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        this.Database = await TestDatabase.CreateAsync();
    }

    public async Task DisposeAsync()
    {
        await this.Database.DisposeAsync();
    }
}
