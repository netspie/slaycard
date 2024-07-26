#nullable enable

namespace Game.Battle.Domain.Events
{
    public record DamagedEvent(
        BattleId BattleId,
        PlayerId PlayerId,
        UnitId OriginUnitId,
        UnitId TargetUnitId,
        double Damage) : BattleEvent(BattleId, PlayerId);
}
