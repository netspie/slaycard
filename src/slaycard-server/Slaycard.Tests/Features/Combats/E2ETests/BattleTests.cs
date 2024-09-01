using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.UseCases;
using Slaycard.Api.Features.CombatsBots.Domain;
using Slaycard.Api.Features.CombatsBots.Infrastructure;
using Slaycard.Api.Features.CombatsTimeouts;
using System.Net;
using System.Net.Http.Json;

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
                        services.AddSingleton(sp => 
                            new RandomizerConfiguration(
                                FixedStatsValue: 1,
                                AlwaysHit: true));

                        services.AddSingleton(sp =>
                            new BattleTimeoutClock(TimeoutSeconds: 1));

                        services.AddSingleton(sp =>
                            new BotTimeoutClock(TimeoutSeconds: 1));
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
        var playerId = Guid.NewGuid().ToString();

        // Start Battle
        var startBattleResponse = await client.PostAsJsonAsync("/battles/startRandomPvE", new { playerId });
        Assert.That(startBattleResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        // Get Battles
        var getBattlesResponse = await client.GetAsync($"/battles?playerId={playerId}");
        Assert.That(getBattlesResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var getBattlesObject = await getBattlesResponse.Content.ReadFromJsonAsync<GetBattlesQueryResponse>();
        Assert.IsNotNull(getBattlesObject);
        Assert.That(getBattlesObject.Battles.Length, Is.EqualTo(1));

        // Get Battle
        var battleId = getBattlesObject.Battles[0].Id;

        var getBattleResponse = await client.GetAsync($"/battles/{battleId}");
        Assert.That(getBattleResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getBattleObject = await getBattleResponse.Content.ReadFromJsonAsync<GetBattleQueryResponse>();
        Assert.IsNotNull(getBattleObject);

        var battle = getBattleObject.DTO;

        // Apply Artifact
        var applyArtifactResponse = await client.PostAsJsonAsync(
            $"/battles/{battleId}/applyArtifact",
            new
            {
                OriginPlayerId = battle.Players[0].Id,
                OriginUnitId = battle.Players[0].Units[0].Id,
                ArtifactId = "attack",
                TargetPlayerId = battle.Players[1].Id,
                TargetUnitId = battle.Players[1].Units[0].Id
            });

        Assert.That(applyArtifactResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        // Get Battle Changed
        var getBattleChangedResponse = await client.GetAsync($"/battles/{battle.Id}/changed?version={battle.Version}");
        Assert.That(getBattleChangedResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        // Get Battle
        getBattleResponse = await client.GetAsync($"/battles/{battleId}");
        Assert.That(getBattleResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Verify_That_BattleIsDeleted_When_Timeout()
    {
        var client = _factory.CreateClient();
        var playerId = Guid.NewGuid().ToString();

        // Start Battle
        var startBattleResponse = await client.PostAsJsonAsync("/battles/startRandomPvE", new { playerId });
        Assert.That(startBattleResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        await Task.Delay(2500);

        // Get Battles
        var getBattlesResponse = await client.GetAsync($"/battles?playerId={playerId}");
        Assert.That(getBattlesResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Verify_That_BotsAreDeleted_When_Timeout()
    {
        using var scope = _factory.Services.CreateScope();
        var client = _factory.CreateClient();
        var playerId = Guid.NewGuid().ToString();

        // Start Battle
        var startBattleResponse = await client.PostAsJsonAsync("/battles/startRandomPvE", new { playerId });
        Assert.That(startBattleResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        // Get Battles
        var getBattlesResponse = await client.GetAsync($"/battles?playerId={playerId}");
        Assert.That(getBattlesResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var getBattlesObject = await getBattlesResponse.Content.ReadFromJsonAsync<GetBattlesQueryResponse>();
        Assert.IsNotNull(getBattlesObject);
        Assert.That(getBattlesObject.Battles.Length, Is.EqualTo(1));

        // Get Battle
        var battleId = getBattlesObject.Battles[0].Id;

        var getBattleResponse = await client.GetAsync($"/battles/{battleId}");
        Assert.That(getBattleResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getBattleObject = await getBattleResponse.Content.ReadFromJsonAsync<GetBattleQueryResponse>();
        Assert.IsNotNull(getBattleObject);

        var botRepository = scope.ServiceProvider.GetRequiredService<IBotRepository>();
        Assert.DoesNotThrowAsync(() => botRepository.Get(new PlayerId(getBattleObject.DTO.Players[1].Id)));

        await Task.Delay(2500);

        Assert.Throws<FileNotFoundException>(() => botRepository.Get(new PlayerId(getBattleObject.DTO.Players[1].Id)));
    }
}
