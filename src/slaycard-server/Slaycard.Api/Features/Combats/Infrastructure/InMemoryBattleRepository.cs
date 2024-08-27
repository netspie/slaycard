using Core.Domain;
using LanguageExt.Common;
using Slaycard.Api.Features.Combats.Domain;
using System.Collections.Concurrent;

namespace Slaycard.Api.Features.Combats.Infrastructure;

public class InMemoryBattleRepository : IBattleRepository
{
    private readonly ConcurrentDictionary<BattleId, Battle> _battles = new();

    private readonly Dictionary<BattleId, object> _locks = new();

    public Task Add(Battle battle)
    {
        if (!_battles.TryAdd(battle.Id, battle))
            throw new Exception();

        _locks[battle.Id] = new();

        return Task.CompletedTask;
    }

    public Task Update(Battle battle)
    {
        if (!_battles.TryGetValue(battle.Id, out var _))
            throw new FileNotFoundException("There is no battle of given id ongoing");

        var @lock = _locks[battle.Id];
        lock (@lock)
        {
            if (battle.Version != _battles[battle.Id].Version)
                throw new Exception();

            var root = battle as IAggregateRoot;
            root.Version++;

            _battles[battle.Id] = battle;
        }

        return Task.CompletedTask;
    }

    public Task Delete(BattleId id)
    {
        if (!_battles.TryGetValue(id, out var _))
            throw new FileNotFoundException("There is no battle of given id ongoing");

        var @lock = _locks[id];
        lock (@lock)
        {
            _battles.Remove(id, out var _);
            _locks.Remove(id, out var _);
        }

        return Task.CompletedTask;
    }

    public Task<Battle> Get(BattleId id)
    {
        if (!_battles.TryGetValue(id, out Battle? battle))
            throw new FileNotFoundException("There is no battle of given id ongoing");

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
