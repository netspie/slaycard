using LanguageExt.ClassInstances;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Slaycard.Api.Features.Combats.UseCases;
using Slaycard.Api.Middleware;
using System.Net;
using System.Net.Http.Json;
namespace Slaycard.Tests.Features.Combats.IntegrationTests;

public class BattleTests
{
    private WebApplicationFactory<Program> _factory;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => {});
    }

    [TearDown]
    public void TearDown()
    {
        _factory?.Dispose();
    }

    [Test]
    public async Task StartBotBattle_IsSuccess()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/battles/startRandomPvE", 
            new { playerId = Guid.NewGuid().ToString() });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task StartRandomPvE_WhenMissingPlayerId_IsError()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/battles/startRandomPvE", new { });
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        var responseJson = await response.Content.ReadFromJsonAsync<ApiError>();
        Assert.NotNull(responseJson);
        Assert.That(responseJson.Status, Is.EqualTo(400));
        Assert.That(responseJson.Message, Is.EqualTo("'Player Id' must not be empty."));
    }
}
