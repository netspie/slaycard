using Core.Domain;
using Mediator;
using Slaycard.Api.Features.Combats.Domain.Events;
using Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.Content;
using Slaycard.Api.Features.Combats.UseCases.Common;

namespace Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications;

public record BattleEventToClientNotificationProcessor(
    IPublisher Publisher) : 
    INotificationHandler<BattleEventBatch>
{
    public ValueTask Handle(BattleEventBatch ev, CancellationToken ct)
    {
        var notifications = ev.Events.Map(MapSingle).ToArray();

        notifications = notifications.Where(n => n is not Unit).ToArray();

        return Publisher.Publish(
            new ClientBattleNotification(
                new ClientBattleNotification.MetaData(ev.NextUnitId),
                notifications));
    }

    private static object MapSingle(IDomainEvent ev) => ev switch
    {
        BattleInstantiatedEvent instEv => instEv.ToNotification(),
        DamagedEvent dmgEv => dmgEv.ToNotification(),
        _ => new Unit(),
    };
}
