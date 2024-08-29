using Core.Domain;
using Mediator;

namespace Slaycard.Api.Features.Combats.UseCases.Common;

public record BattleEventBatch(
    string NextUnitId,
    IDomainEvent[] Events) : INotification;
