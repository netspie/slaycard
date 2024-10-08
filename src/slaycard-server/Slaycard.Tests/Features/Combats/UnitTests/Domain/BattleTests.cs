﻿using Core.Domain;
using Slaycard.Api.Core.Domain;
using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Tests.Features.Combats.UnitTests.Domain;

internal class BattleTests
{
    [Test]
    public void _Throws_IfHasLessThanTwoPlayers()
    {
        Assert.Throws<DomainException>(() => new Battle(new BattleId(), []));
    }

    [Test]
    public void Start()
    {
        var battle = CreateBattle(unitStatValues: 1, start: false);

        Assert.DoesNotThrow(() => battle.Start(battle.Players[0].Id));
        Assert.DoesNotThrow(() => battle.Start(battle.Players[1].Id));
        Assert.Throws<DomainException>(() => battle.Start(battle.Players[0].Id));
        Assert.Throws<DomainException>(() => battle.Start(battle.Players[1].Id));
    }

    [Test]
    public void ApplyArtifact_Attack_WhenUnitStatsAre_1()
    {
        var battle = CreateBattle(unitStatValues: 1);

        var attackByUnit1 = () => battle.ApplyArtifact(
            originPlayerId: battle.Players[0].Id,
            originUnitId: battle.Players[0].Units[0].Id,
            artifactId: new ArtifactId("attack"),
            targetPlayerId: battle.Players[1].Id,
            targetUnitId: battle.Players[1].Units[0].Id,
            random: new Random(1));

        Assert.DoesNotThrow(attackByUnit1.Invoke);
        Assert.Throws<DomainException>(attackByUnit1.Invoke);

        Assert.IsTrue(battle.Players[1].Units[0].CombatStats.HP.CalculatedValue == 0);
        Assert.IsTrue(battle.IsGameOver);
    }

    [Test]
    public void ApplyArtifact_Attack_ButMiss_WhenUnitStatsAre_1()
    {
        var battle = CreateBattle(unitStatValues: 1);

        // Unit 1 Attacks and Misses
        var attackByUnit1 = () => battle.ApplyArtifact(
            originPlayerId: battle.Players[0].Id,
            originUnitId: battle.Players[0].Units[0].Id,
            artifactId: new ArtifactId("attack"),
            targetPlayerId: battle.Players[1].Id,
            targetUnitId: battle.Players[1].Units[0].Id,
            random: new Random(8));

        Assert.DoesNotThrow(attackByUnit1.Invoke);
        Assert.Throws<DomainException>(attackByUnit1.Invoke);
        Assert.IsTrue(battle.Players[1].Units[0].CombatStats.HP.CalculatedValue == 1);
        Assert.IsFalse(battle.IsGameOver);
    }

    [Test]
    public void ApplyArtifact_Attack_WhenUnitStatsAre_10()
    {
        var battle = CreateBattle(unitStatValues: 10);

        // Unit 1 Attacks
        var attackByUnit1 = () => battle.ApplyArtifact(
            originPlayerId: battle.Players[0].Id,
            originUnitId: battle.Players[0].Units[0].Id,
            artifactId: new ArtifactId("attack"),
            targetPlayerId: battle.Players[1].Id,
            targetUnitId: battle.Players[1].Units[0].Id,
            random: new Random(7));

        Assert.DoesNotThrow(attackByUnit1.Invoke);
        Assert.Throws<DomainException>(attackByUnit1.Invoke);

        Assert.IsTrue(battle.Players[1].Units[0].CombatStats.HP.CalculatedValue == 8);
        Assert.IsFalse(battle.IsGameOver);
    }

    public static Battle CreateBattle(
        int unitStatValues = 1,
        bool start = true,
        bool guidIds = false)
    {
        var unit1 = UnitTests_.CreateUnit(guidIds ? EntityId.NewGuid : "unit-1", unitStatValues);
        var unit2 = UnitTests_.CreateUnit(guidIds ? EntityId.NewGuid : "unit-2", unitStatValues);

        var player1 = new Player(new PlayerId(guidIds ? EntityId.NewGuid : "player-1"), [unit1]);
        var player2 = new Player(new PlayerId(guidIds ? EntityId.NewGuid : "player-2"), [unit2]);

        var battle = new Battle(
            new BattleId(guidIds ? EntityId.NewGuid : "battle-1"), [player1, player2]);

        if (start)
        {
            battle.Start(battle.Players[0].Id);
            battle.Start(battle.Players[1].Id);
        }

        return battle;
    }
}
