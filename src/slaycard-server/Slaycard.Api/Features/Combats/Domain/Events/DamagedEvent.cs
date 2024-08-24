namespace Slaycard.Api.Features.Combats.Domain.Events;

public record DamagedEvent(
    BattleId BattleId,
    PlayerId PlayerId,
    UnitId OriginUnitId,
    UnitId TargetUnitId,
    double Damage,
    bool IsCritic = false) : BattleEvent(BattleId, PlayerId);
