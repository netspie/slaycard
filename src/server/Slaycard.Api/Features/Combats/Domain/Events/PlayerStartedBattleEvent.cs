namespace Game.Battle.Domain.Events;

public record PlayerStartedBattleEvent(
    BattleId BattleId,
    PlayerId PlayerId) : BattleEvent(BattleId, PlayerId);
