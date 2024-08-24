namespace Slaycard.Api.Features.Combats.Domain.Events;

public record GameOverEvent(
    BattleId BattleId,
    PlayerId WinnerId) : BattleEvent(BattleId, WinnerId);
