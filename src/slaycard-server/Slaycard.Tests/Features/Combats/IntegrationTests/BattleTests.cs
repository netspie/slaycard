﻿using Microsoft.AspNetCore.Mvc.Testing;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Infrastructure;
using Slaycard.Api.Middleware;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
namespace Slaycard.Tests.Features.Combats.IntegrationTests;

public class BattleTests
{
    private WebApplicationFactory<Program> _factory;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => {});
    }

    [TearDown]
    public void TearDown()
    {
        _factory?.Dispose();
    }

    [Test]
    public void TestX()
    {
        var battleId = new BattleId("xyz");

        var map = new ConcurrentDictionary<BattleId, int?>();

        map.TryAdd(battleId, 1);
        Assert.IsTrue(map.TryGetValue(battleId, out var v1));
        Assert.IsTrue(map.TryGetValue(new BattleId(battleId.Value), out var v2));
    }

    [Test]
    public async Task StartRandomPvE()
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
        Assert.That(responseJson.Message, Is.EqualTo(
            "The id must be a valid guid."));
    }

    [Test]
    public async Task GetBattle()
    {
        var battle = UnitTests.BattleTests.CreateBattle(id: Guid.NewGuid().ToString());

        _factory.Services.GetService<IBattleRepository, InMemoryBattleRepository>()?.Add(battle);

        var client = _factory.CreateClient();

        var response = await client.GetAsync($"/battles/{battle.Id.Value}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetBattle_WhenMissingBattleId_IsError()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/battles/xyz");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        var responseJson = await response.Content.ReadFromJsonAsync<ApiError>();
        Assert.NotNull(responseJson);
        Assert.That(responseJson.Status, Is.EqualTo(400));
        Assert.That(responseJson.Message, Is.EqualTo(
            "The id must be a valid guid."));
    }
}
