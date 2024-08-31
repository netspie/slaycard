using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Api.Features.CombatsTimeouts;

public record CombatsTimeoutWorkerConfiguration(int TimeoutSeconds);

public class CombatsTimeoutWorker(
    IServiceProvider serviceProvider,
    CombatsTimeoutWorkerConfiguration configuration) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly CombatsTimeoutWorkerConfiguration _configuration = configuration;

    private readonly Dictionary<BattleId, (BattleId id, DateTime lastTime)> _battles = new();

    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(5));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = _serviceProvider.CreateScope();    
        var battleRepository = scope.ServiceProvider.GetRequiredService<IBattleRepository>();

        while (await _timer.WaitForNextTickAsync(stoppingToken) &&
            !stoppingToken.IsCancellationRequested)
        {
            var battlesToRemove = new List<BattleId>();

            foreach (var battle in _battles.Values)
            {
                var offset = DateTime.UtcNow - battle.lastTime;
                if (offset < TimeSpan.FromSeconds(_configuration.TimeoutSeconds))
                    continue;

                battlesToRemove.Add(battle.id);
            }

            await battleRepository.DeleteMany(battlesToRemove);
        }
    }
}
