using Mediator;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications;

public record DamagedClientNotification(
    string BattleId,
    string OriginPlayerId,
    string OriginUnitId,
    string TargetPlayerId,
    string TargetUnitId,
    double Damage,
    bool IsCritic) : INotification;

public class ClientNotificationPublisher : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}

public class BattleEventToClientNotificationProcessor(
    IPublisher publisher) : 
    INotificationHandler<DamagedEvent>
{
    private readonly IPublisher _publisher = publisher;

    public ValueTask Handle(DamagedEvent ev, CancellationToken ct)
    {
        return _publisher.Publish(
            new ClientBattleNotification(
                new ClientBattleNotification.MetaData(""),
                new DamagedClientNotification(
                    ev.BattleId.Value,
                    ev.OriginPlayerId.Value,
                    ev.OriginUnitId.Value,
                    ev.TargetPlayerId.Value,
                    ev.TargetUnitId.Value,
                    ev.Damage,
                    ev.IsCritic)));
    }
}
