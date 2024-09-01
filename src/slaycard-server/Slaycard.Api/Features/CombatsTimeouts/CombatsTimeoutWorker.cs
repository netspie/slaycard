using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Api.Features.CombatsTimeouts;

public class CombatsTimeoutWorker(
    IServiceProvider serviceProvider,
    BattleTimeoutClock TimeoutClock) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(TimeoutClock.TimeoutSeconds));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = _serviceProvider.CreateScope();    
        var battleRepository = scope.ServiceProvider.GetRequiredService<IBattleRepository>();

        while (await _timer.WaitForNextTickAsync(stoppingToken) &&
            !stoppingToken.IsCancellationRequested)
        {
            var battlesToRemove = TimeoutClock.GetTimeoutBattles();
            await battleRepository.DeleteMany(battlesToRemove);
        }
    }
}
