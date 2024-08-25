using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Tests.Features.Combats.UnitTests;

namespace Slaycard.Tests.Features.Combats.IntegrationTests;

public class BattleTests
{
    [Test]
    public void PerformBattle_WithUnitsStats_EqualTo_1()
    {
        var unit1 = UnitTests_.CreateUnit("unit-1", statValues: 1);
        var unit2 = UnitTests_.CreateUnit("unit-2", statValues: 1);

        var player1 = new Player(new PlayerId("player-1"), [unit1]);
        var player2 = new Player(new PlayerId("player-2"), [unit2]);

        var players = new[] { player1, player2 };
        var battle = new Battle(new BattleId("battle-1"), players);

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

    [Test]
    public void PerformBattle_WithUnitsStats_EqualTo_10()
    {
        var unit1 = UnitTests_.CreateUnit("unit-1", statValues: 10);
        var unit2 = UnitTests_.CreateUnit("unit-2", statValues: 10);

        var player1 = new Player(new PlayerId("player-1"), [unit1]);
        var player2 = new Player(new PlayerId("player-2"), [unit2]);

        var players = new[] { player1, player2 };
        var battle = new Battle(new BattleId("battle-1"), players);

        Assert.DoesNotThrow(() => battle.Start(player1.Id));
        Assert.DoesNotThrow(() => battle.Start(player2.Id));

        // Start
        Assert.Throws<Exception>(() => battle.Start(player1.Id));
        Assert.Throws<Exception>(() => battle.Start(player2.Id));

        // Unit 1 Attacks
        var attackByUnit1 = () => battle.ApplyArtifact(
            originPlayerId: player1.Id,
            originUnitId: unit1.Id,
            artifactId: new ArtifactId("attack"),
            targetPlayerId: player2.Id,
            targetUnitId: unit2.Id,
            random: new Random(7));

        Assert.DoesNotThrow(attackByUnit1.Invoke);
        Assert.Throws<Exception>(attackByUnit1.Invoke);

        Assert.IsTrue(unit2.CombatStats.HP.CalculatedValue == 8);
        Assert.IsFalse(battle.IsGameOver);

        // Unit 2 Attacks
        var attackByUnit2 = () => battle.ApplyArtifact(
            originPlayerId: player2.Id,
            originUnitId: unit2.Id,
            artifactId: new ArtifactId("attack"),
            targetPlayerId: player1.Id,
            targetUnitId: unit1.Id,
            random: new Random(7));

        Assert.DoesNotThrow(attackByUnit2.Invoke);
        Assert.Throws<Exception>(attackByUnit2.Invoke);

        Assert.IsTrue(unit1.CombatStats.HP.CalculatedValue == 8);
        Assert.IsFalse(battle.IsGameOver);
    }
}
