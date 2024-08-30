using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Tests.Features.Combats.E2ETests;

public class BattleTests
{
    private WebApplicationFactory<Program> _factory;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(
                builder => 
                    builder.ConfigureServices(services =>
                    {
                        services.AddScoped(sp => 
                            new RandomizerConfiguration(AlwaysHit: true));
                    }));
    }

    [TearDown]
    public void TearDown()
    {
        _factory?.Dispose();
    }

    [Test]
    public async Task PerformBattle()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/battles/startRandomPvE",
            new { playerId = Guid.NewGuid().ToString() });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
