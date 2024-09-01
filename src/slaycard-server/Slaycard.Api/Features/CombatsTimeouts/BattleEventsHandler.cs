using Mediator;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications;
using Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.Content;

namespace Slaycard.Api.Features.CombatsTimeouts;

public record BattleEventsHandler(
    BattleTimeoutClock BattleTimer) : INotificationHandler<ClientBattleNotification>
{
    public ValueTask Handle(ClientBattleNotification n, CancellationToken ct)
    {
        var battleId = new BattleId(n.Metadata.BattleId);

        BattleTimer.Update(battleId);

        foreach (var subN in n.Notifications)
        {
            if (subN is BattleInstantiatedClientNotification instN)
            {
                BattleTimer.Add(battleId);
                break;
            }

            if (subN is GameOverClientNotification overN)
            {
                BattleTimer.Remove(battleId);
                break;
            }
        }

        return ValueTask.CompletedTask;
    }
}
