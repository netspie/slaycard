namespace Slaycard.Api.Features.Combats.Domain.Events;

public record PassedEvent(
    BattleId BattleId,
    PlayerId PlayerId) : BattleEvent(BattleId, PlayerId);
