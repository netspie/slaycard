namespace Slaycard.Api.Features.Combats.Domain.Events;

public record BotCreatedEvent(BattleId BattleId, PlayerId PlayerId) : BattleEvent(BattleId, PlayerId);
