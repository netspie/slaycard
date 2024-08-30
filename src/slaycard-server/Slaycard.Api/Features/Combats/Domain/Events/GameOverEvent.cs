namespace Slaycard.Api.Features.Combats.Domain.Events;

public record GameOverEvent(
    BattleId BattleId,
    PlayerId WinnerId,
    PlayerId[] PlayerIds) : BattleEvent(BattleId, WinnerId);
