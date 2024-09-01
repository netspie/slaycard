using Mediator;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.Content;

public record BattleInstantiatedClientNotification(
    string BattleId,
    string[] PlayerIds) : Notification("BattleInstantiated"), INotification;

public static class BattleInstantiatedEvent_To_ClientNotification_Converter
{
    public static BattleInstantiatedClientNotification ToNotification(this BattleInstantiatedEvent ev) =>
        new(ev.BattleId.Value,
            ev.Players.Map(p => p.Id.Value).ToArray());
}
