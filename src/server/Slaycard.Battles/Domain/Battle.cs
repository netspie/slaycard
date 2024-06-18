


using System;

namespace Slaycard.Battles.Domain;

public class Battle
{
    public Player[] Players { get; private set; }

    public void AssembleActionCard(string cardId, string? targetCardId = null)
    {
        //Players.AssembleCard(cardId, targetCardId);
    }

    // Change so it supports area/group applies
    public void ApplyActionCard(
        string originPlayerId,
        string originUnitId,
        string actionCardId, 
        string targetPlayerId,
        string targetUnitId)
    {
        Players.ApplyActionCard(
            originPlayerId, 
            originUnitId,
            targetPlayerId, 
            actionCardId, 
            targetUnitId);
    }

    public void Pass()
    {
        //Players.GenerateActionCards();
    }
}

public class Player
{
    public Unit[] Units { get; private set; }

    public ActionCard TakeActionCard(
        string unitId,
        string actionCardId)
    {
        return default;
    }

    public void ApplyActionCard(
        Player originPlayer,
        string originUnitId,
        string actionCardId, 
        string targetUnitId)
    {
        var actionCard = originPlayer.TakeActionCard(originUnitId, actionCardId);
        var originUnit = originPlayer.GetUnit(originUnitId);

        Units.ApplyActionCard(actionCard, originUnit, targetUnitId);
    }

    public Unit GetUnit(string unitId) =>
        Units.OfId(unitId);
}
public static class PlayerExtensions
{
    public static void ApplyActionCard(
        this IEnumerable<Player> players,
        string originPlayerId,
        string originUnitId,
        string targetPlayerId,
        string actionCardId, 
        string targetUnitCardId)
    {
        var originPlayer = players.OfId(originPlayerId);

        var targetPlayer = players.OfId(targetPlayerId);
        targetPlayer.ApplyActionCard(
            originPlayer, originUnitId, actionCardId, targetUnitCardId);
    }

    public static Player OfId(this IEnumerable<Player> players, string playerId) =>
        default;
        //players.FirstOrDefault();
}

public class Unit
{
    public StatisticsGroup BaseStatistics { get; private set; }
    public StatisticsGroup DerivativeStatistics { get; private set; }

    public AssemblySpace AssemblySpace { get; private set; }
    public ActionRow ActionRow { get; private set; }

    public void AssembleCard(string cardId, string? targetCardId = null)
    {
        var card = ActionRow.TakeCard(cardId);
    }

    public void GenerateActionCards()
    {
        ActionRow.GenerateCards();
    }

    public void ApplyActionCard(
        Unit originUnit,
        ActionCard actionCard)
    {
        actionCard.ApplyToTarget(
            originUnit.DerivativeStatistics,
            DerivativeStatistics);
    }
}

public static class UnitExtensions
{
    public static void ApplyActionCard(
        this IEnumerable<Unit> units,
        ActionCard actionCard,
        Unit originUnit,
        string targetUnitId)
    {
        var targetUnit = units.OfId(targetUnitId);
        targetUnit.ApplyActionCard(originUnit, actionCard);
    }

    public static Unit OfId(this IEnumerable<Unit> unit, string unitId) =>
       default;
}

public class StatisticsGroup
{

}

public class Statistic
{

}

public class AssemblySpace
{
    public void AssembleCard(string actionCardId, string? targetCardId = null)
    {

    }
}

public abstract class ActionCard
{
    public abstract void ApplyToTarget(
        StatisticsGroup origin,
        StatisticsGroup target);
}

public class MeleeAttackAction : ActionCard
{
    public int Damage { get; private set; }

    public override void ApplyToTarget(
        StatisticsGroup origin,
        StatisticsGroup target)
    {
        //origin.DamageTarget(target, Damage)
    }
}

public class MagicMissileAttackAction : ActionCard
{
    public int Damage { get; private set; }

    public override void ApplyToTarget(
        StatisticsGroup origin,
        StatisticsGroup target)
    {
        //origin.DamageTarget(target, Damage)
    }
}

public class ActionRow
{
    public void GenerateCards()
    {

    }

    public ActionCard TakeCard(string cardId)
    {
        return default;
    }
}