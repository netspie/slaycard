using Slaycard.Api.Features.CombatsBots.Domain;

namespace Slaycard.Api.Features.CombatsBots.EventHandlers;

public class BotBattleEventProcessor(
    IBotRepository botRepository,
    IBotBattleEventQueue eventQueue) : BackgroundService
{
    private readonly IBotRepository _botRepository = botRepository;
    private readonly IBotBattleEventQueue _eventQueue = eventQueue;

    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (await _timer.WaitForNextTickAsync(ct) &&
            !ct.IsCancellationRequested)
        {
            if (!_eventQueue.TryDequeue(out var ev))
                continue;

            Console.WriteLine(ev);
        }
    }
}
