using Mediator;

namespace Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications;

public record ClientNotificationPublisher(
    IPublisher Publisher) :
    INotificationHandler<ClientBattleNotification>
{
    public ValueTask Handle(ClientBattleNotification ev, CancellationToken ct)
    {
        return ValueTask.CompletedTask;
    }
}
