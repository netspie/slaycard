using Core.Domain;
using Mediator;
using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Api.Features.Combats.UseCases.Common;

public static class BattlePublisherExtensions
{
    public static Task PublishEvents(
        this IPublisher publisher,
        Battle battle,
        CancellationToken ct) => publisher.PublishEvents(battle, container: battle, ct);

    public static async Task PublishEvents(
        this IPublisher publisher,
        Battle battle,
        IEventContainer container,
        CancellationToken ct)
    {
        var batch = new BattleEventBatch(
            battle.NextUnitId.Value, container.Events.ToArray());

        container.Clear();

        await publisher.Publish(batch, ct);
    }
}
