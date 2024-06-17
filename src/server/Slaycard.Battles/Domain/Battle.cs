


using System;

namespace Slaycard.Battles.Domain;

public class Battle
{
    public Player[] Players { get; private set; }

    public void AssembleCard(string cardId, string? targetCardId = null)
    {
        //Players.AssembleCard(cardId, targetCardId);
    }

    public void UseActionCardOnUnit(string actionCardId, string? unitCardId = null)
    {
        //Players.Act(actionCardId, targetCardId);
    }

    public void Pass()
    {
        //Players.GenerateActionCards();
    }
}

public class Player
{
    public Unit[] Units { get; private set; }

    public ActionCard TakeActionCard(string actionCardId)
    {
        return default;
    }

    public void ApplyActionCard(ActionCard actionCard, string unitId)
    {
        Units.ApplyActionCard(actionCard, unitId);
    }
}

public static class PlayerExtensions
{
    public static void UseActionCardOnUnit(
        this IEnumerable<Player> players,
        string sourcePlayerId,
        string targetPlayerId,
        string actionCardId, 
        string unitCardId)
    {
        var sourcePlayer = players.OfId(sourcePlayerId);
        var actionCard = sourcePlayer.TakeActionCard(actionCardId);

        var targetPlayer = players.OfId(targetPlayerId);
        targetPlayer.ApplyActionCard(actionCard, unitCardId);
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

    public void ApplyActionCard(ActionCard actionCard)
    {
        actionCard.ApplyToStatistics(DerivativeStatistics);
    }
}

public class StatisticsGroup
{

}

public class Statistic
{

}

public static class UnitExtensions
{
    public static void ApplyActionCard(this IEnumerable<Unit> units, ActionCard actionCard, string unitId)
    {
        units.OfId(unitId).ApplyActionCard(actionCard);
    }

    public static Unit OfId(this IEnumerable<Unit> unit, string unitId) =>
       default;
}

public class AssemblySpace
{
    public void AssembleCard(string actionCardId, string? targetCardId = null)
    {

    }
}

public class ActionCard
{
    public void ApplyToStatistics(StatisticsGroup derivativeStatistics)
    {
        //if (meleeAttack || punchAttack || kickAttack || headBang || magicMissile.. quick or strong?)
        //    derivativeStatistics.Damage(attack);

        //if (potion)
        //    derivativeStatistics.ModifyHp();

        //if (scroll)
        //    derivativeStatistics.ModifyHp();
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