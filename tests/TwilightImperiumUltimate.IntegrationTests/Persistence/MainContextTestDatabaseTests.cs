using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.Website;
using TwilightImperiumUltimate.IntegrationTests.Infrastructure;

namespace TwilightImperiumUltimate.IntegrationTests.Persistence;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class MainContextTestDatabaseTests
{
    [Fact]
    public async Task MainContextTestDatabase_WhenUsed_ShouldInsertAndReadWebsite()
    {
        await using var database = await TestDatabase.CreateAsync();
        var website = new Website
        {
            Title = "Draft Rooms test website",
            Description = "SQL Server test database",
            WebsitePath = "/draft-rooms-test",
        };

        await using (var context = await database.ContextFactory.CreateDbContextAsync())
        {
            context.Websites.Add(website);
            await context.SaveChangesAsync();
        }

        await using (var context = await database.ContextFactory.CreateDbContextAsync())
        {
            var persistedWebsite = await context.Websites.SingleAsync(item => item.Id == website.Id);

            persistedWebsite.Title.Should().Be(website.Title);
        }
    }
}
