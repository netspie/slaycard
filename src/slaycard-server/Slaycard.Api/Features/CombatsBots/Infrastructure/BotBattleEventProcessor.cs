using Mediator;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.Content;
using Slaycard.Api.Features.CombatsBots.Domain;

namespace Slaycard.Api.Features.CombatsBots.EventHandlers;

public class BotBattleEventProcessor(
    IBotBattleEventQueue eventQueue,
    IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IBotBattleEventQueue _eventQueue = eventQueue;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        while (await _timer.WaitForNextTickAsync(ct) &&
            !ct.IsCancellationRequested)
        {
            if (!_eventQueue.TryDequeue(out var ev) || ev is null)
                continue;

            if (ev.Notifications.Length == 0)
                continue;

            foreach(var n in ev.Notifications.Cast<Notification>())
            {
                await (n switch
                {
                    DamagedClientNotification dmgN => publisher.Publish(dmgN, ct),
                    BattleInstantiatedClientNotification dmgN => publisher.Publish(dmgN, ct),
                    _ => ValueTask.CompletedTask,
                });
            };
        }
    }
}

public record BattleInstantiatedNotificationHandler(
    IBotRepository BotRepository) : INotificationHandler<BattleInstantiatedClientNotification>
{
    async ValueTask INotificationHandler<BattleInstantiatedClientNotification>.Handle(
        BattleInstantiatedClientNotification n, CancellationToken ct)
    {
        await ValueTask.CompletedTask;
    }
}

public record DamagedClientNotificationHandler(
    IBotRepository BotRepository) : INotificationHandler<DamagedClientNotification>
{
    async ValueTask INotificationHandler<DamagedClientNotification>.Handle(
        DamagedClientNotification n, CancellationToken ct)
    {
        var bot = await BotRepository.Get(new PlayerId(n.TargetPlayerId));
    }
}
