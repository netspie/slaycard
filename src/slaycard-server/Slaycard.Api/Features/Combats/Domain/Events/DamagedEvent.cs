namespace Slaycard.Api.Features.Combats.Domain.Events;

public record DamagedEvent(
    BattleId BattleId,
    PlayerId OriginPlayerId,
    UnitId OriginUnitId,
    PlayerId TargetPlayerId,
    UnitId TargetUnitId,
    double Damage,
    bool IsCritic = false) : BattleEvent(BattleId, OriginPlayerId);
