using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Combats.Domain.Events;

public record PlayerStartedBattleEvent(
    BattleId BattleId,
    PlayerId PlayerId) : BattleEvent(BattleId, PlayerId);
