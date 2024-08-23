using Slaycard.Api.Features.Combats.Domain;
using System.Collections.Concurrent;

namespace Slaycard.Api.Features.Combats.Infrastructure;

public class InMemoryBattleRepository : IBattleRepository
{
    private readonly ConcurrentDictionary<BattleId, Battle> _battles = new();

    public Task<bool> Add(Battle battle)
    {
        var result = _battles.TryAdd(battle.Id, battle);
        return Task.FromResult(result);
    }

    public Task<Battle> Get(BattleId id)
    {
        if (!_battles.TryGetValue(id, out Battle? battle))  
            throw new Exception("There is no battle of given id ongoing");

        return Task.FromResult(battle);
    }

    public Task<Battle[]> GetMany(int Offset, int Limit)
    {
        return Task.FromResult(
            _battles.Values
                .Skip(Offset)
                .Take(Limit)
                .ToArray());
    }
}
