using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Features.Combats.IntegrationTests;

public class BattleTests
{
    [Test]
    public void PerformBasicBattle()
    {
        var unit1 = new Unit(
            new UnitId("unit-1"), 
            new CombatStatGroup(
                HP:         new Stat(1),
                Damage:     new Stat(1),
                Defence:    new Stat(1),
                Accuracy:   new Stat(1),
                Dodge:      new Stat(1),
                Critics:    new Stat(1),
                Speed:      new Stat(1)));

        var unit2 = new Unit(
            new UnitId("unit-2"), 
            new CombatStatGroup(
                HP:         new Stat(1),
                Damage:     new Stat(1),
                Defence:    new Stat(1),
                Accuracy:   new Stat(1),
                Dodge:      new Stat(1),
                Critics:    new Stat(1),
                Speed:      new Stat(1)));

        var player1 = new Player(new PlayerId("player-1"), [unit1]);
        var player2 = new Player(new PlayerId("player-2"), [unit2]);

        var players = new[] { player1, player2 };
        var battle = new Battle(new BattleId("asd"), players);

        Assert.DoesNotThrow(() => battle.Start(player1.Id));
        Assert.DoesNotThrow(() => battle.Start(player2.Id));

        Assert.Throws<Exception>(() => battle.Start(player1.Id));
        Assert.Throws<Exception>(() => battle.Start(player2.Id));

        Assert.DoesNotThrow(() => battle.ApplyArtifact(
            originPlayerId: player1.Id,
            originUnitId: unit1.Id,
            artifactId: new ArtifactId(""),
            targetPlayerId: player2.Id,
            targetUnitId: unit2.Id));
    }
}
