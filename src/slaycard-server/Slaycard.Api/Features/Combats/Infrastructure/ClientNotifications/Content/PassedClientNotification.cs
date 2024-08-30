using Mediator;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.Content;

public record PassedClientNotification(
    string BattleId,
    string PlayerId) : Notification("Passed"), INotification;

public static class PassedClientEvent_To_ClientNotification_Converter
{
    public static PassedClientNotification ToNotification(this PassedEvent ev) =>
        new(ev.BattleId.Value, 
            ev.PlayerId?.Value ?? "");
}
