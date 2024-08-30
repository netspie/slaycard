using Core.Domain;
using Mediator;
using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Api.Features.Combats.UseCases.Common;

public static class BattlePublisherExtensions
{
    public static Task PublishEvents(
        this IPublisher publisher,
        Battle battle,
        IDomainEvent[] additionalEvents,
        CancellationToken ct) => publisher.PublishEvents(battle, container: battle, additionalEvents, ct);

    public static Task PublishEvents(
        this IPublisher publisher,
        Battle battle,
        CancellationToken ct) => publisher.PublishEvents(battle, container: battle, additionalEvents: [], ct);

    public static async Task PublishEvents(
        this IPublisher publisher,
        Battle battle,
        IEventContainer container,
        IDomainEvent[] additionalEvents,
        CancellationToken ct)
    {
        var batch = new BattleEventBatch(
            battle.Id.Value, 
            NextPlayerId: battle.CurrentPlayerId.Value, 
            NextUnitId: battle.CurrentUnitId.Value,
            container.Events.Concat(additionalEvents).ToArray());

        container.Clear();

        await publisher.Publish(batch, ct);
    }
}