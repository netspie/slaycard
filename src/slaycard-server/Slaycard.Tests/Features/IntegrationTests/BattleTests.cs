using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Domain.Artifacts;

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
                Attack:     new Stat(1),
                Defence:    new Stat(1),
                Accuracy:   new Stat(1),
                Dodge:      new Stat(1),
                Critics:    new Stat(1),
                Speed:      new Stat(1)),
             artifacts: 
             [
                new AttackArtifact(new ArtifactId("attack"))
             ]);

        var unit2 = new Unit(
            new UnitId("unit-2"), 
            new CombatStatGroup(
                HP:         new Stat(1),
                Attack:     new Stat(1),
                Defence:    new Stat(1),
                Accuracy:   new Stat(1),
                Dodge:      new Stat(1),
                Critics:    new Stat(1),
                Speed:      new Stat(1)),
             artifacts:
             [
                new AttackArtifact(new ArtifactId("attack"))
             ]);

        var player1 = new Player(new PlayerId("player-1"), [unit1]);
        var player2 = new Player(new PlayerId("player-2"), [unit2]);

        var players = new[] { player1, player2 };
        var battle = new Battle(new BattleId("asd"), players);

        Assert.DoesNotThrow(() => battle.Start(player1.Id));
        Assert.DoesNotThrow(() => battle.Start(player2.Id));

        // Start
        Assert.Throws<Exception>(() => battle.Start(player1.Id));
        Assert.Throws<Exception>(() => battle.Start(player2.Id));

        // Unit 1 Attacks and Misses
        var attackByUnit1 = () => battle.ApplyArtifact(
            originPlayerId: player1.Id,
            originUnitId: unit1.Id,
            artifactId: new ArtifactId("attack"),
            targetPlayerId: player2.Id,
            targetUnitId: unit2.Id,
            random: new Random(8));

        Assert.DoesNotThrow(attackByUnit1.Invoke);
        Assert.Throws<Exception>(attackByUnit1.Invoke);

        Assert.IsFalse(battle.IsGameOver);

        // Unit 2 Attacks and Hits
        var attackByUnit2 = () => battle.ApplyArtifact(
            originPlayerId: player2.Id,
            originUnitId: unit2.Id,
            artifactId: new ArtifactId("attack"),
            targetPlayerId: player1.Id,
            targetUnitId: unit1.Id,
            random: new Random(1));

        Assert.DoesNotThrow(attackByUnit2.Invoke);
        Assert.Throws<Exception>(attackByUnit2.Invoke);

        Assert.IsTrue(battle.IsGameOver);
    }
}
