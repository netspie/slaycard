using Mediator;
using Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications;

namespace Slaycard.Api.Features.CombatsBots.EventHandlers;

public record BattleEventsHandler(
    IBotBattleEventQueue EventQueue) :
    INotificationHandler<ClientBattleNotification>
{
    public ValueTask Handle(ClientBattleNotification notification, CancellationToken ct)
    {
        EventQueue.Enqueue(notification);
        return ValueTask.CompletedTask;
    }
}
