namespace Slaycard.Api.Features.Combats.Domain.Events;

public record BattleInstantiatedEvent(
    BattleId BattleId) : BattleEvent(BattleId, null);
