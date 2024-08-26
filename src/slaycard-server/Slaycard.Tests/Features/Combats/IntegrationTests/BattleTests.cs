using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Net;
using System.Net.Http.Json;
namespace Slaycard.Tests.Features.Combats.IntegrationTests;

public class BattleTests
{
    private WebApplicationFactory<Program> _app;

    [SetUp]
    public void SetUp()
    {
        _app = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => {});
    }

    [TearDown]
    public void TearDown()
    {
        _app?.Dispose();
    }

    [Test]
    public async Task StartBotBattle()
    {
        var client = _app.CreateClient();

        var response = await client.PostAsJsonAsync("/battles", new { });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
