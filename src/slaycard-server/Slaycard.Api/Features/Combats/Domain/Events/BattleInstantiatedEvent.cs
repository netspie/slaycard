namespace Slaycard.Api.Features.Combats.Domain.Events;

public record BattleInstantiatedEvent(
    BattleId BattleId,
    Player[] Players,
    UnitId[] UnitsOrder) : BattleEvent(BattleId, null);
