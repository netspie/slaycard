using Mediator;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.Content;

public record BotCreatedClientNotification(
    string BattleId,
    string BotId) : Notification("BotCreated"), INotification;

public static class BotCreatedEvent_To_ClientNotification_Converter
{
    public static BotCreatedClientNotification ToNotification(this BotCreatedEvent ev) =>
        new(ev.BattleId.Value,
            ev.PlayerId?.Value ?? "");
}
