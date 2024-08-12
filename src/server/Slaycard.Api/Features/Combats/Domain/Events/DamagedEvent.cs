using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Api.Features.Combats.Domain.Events;

public record DamagedEvent(
    BattleId BattleId,
    PlayerId PlayerId,
    UnitId OriginUnitId,
    UnitId TargetUnitId,
    double Damage) : BattleEvent(BattleId, PlayerId);
