namespace Game.Battle.Domain.Events
{
    public record MissedEvent(
        BattleId BattleId,
        PlayerId PlayerId,
        UnitId OriginUnitId,
        UnitId TargetUnitId) : BattleEvent(BattleId, PlayerId);
}
