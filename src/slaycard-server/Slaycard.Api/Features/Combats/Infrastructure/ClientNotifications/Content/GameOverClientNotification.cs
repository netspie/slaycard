using Mediator;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.Content;

public record GameOverClientNotification(
    string BattleId,
    string WinnerId,
    string[] PlayerIds) : Notification("GameOver"), INotification;

public static class GameOverEvent_To_ClientNotification_Converter
{
    public static GameOverClientNotification ToNotification(this GameOverEvent ev) =>
        new(ev.BattleId.Value,
            ev.WinnerId.Value ?? "",
            ev.PlayerIds.Map(id => id.Value).ToArray());
}
