using Slaycard.Api.Features.CombatsBots.Domain;

namespace Slaycard.Api.Features.CombatsBots.Infrastructure;

public class BotTimeoutWorker(
    IServiceProvider serviceProvider,
    BotTimeoutClock TimeoutClock) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(TimeoutClock.TimeoutSeconds));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();    
        var botRepository = scope.ServiceProvider.GetRequiredService<IBotRepository>();

        while (await _timer.WaitForNextTickAsync(stoppingToken) &&
            !stoppingToken.IsCancellationRequested)
        {
            var toRemove = TimeoutClock.GetTimeoutBots();
            await botRepository.DeleteMany(toRemove);
        }
    }
}
