using Core.Domain;
using Mediator;

namespace Slaycard.Api.Features.Combats.UseCases.Common;

public record BattleEventBatch(
    string BattleId,
    string NextPlayerId,
    string NextUnitId,
    IDomainEvent[] Events) : INotification;
