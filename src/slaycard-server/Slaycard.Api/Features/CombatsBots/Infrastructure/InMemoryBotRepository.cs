using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.CombatsBots.Domain;
using System.Collections.Concurrent;

namespace Slaycard.Api.Features.CombatsBots.Infrastructure;

public class InMemoryBotRepository : IBotRepository
{
    private readonly ConcurrentDictionary<PlayerId, Bot> _bots = new();

    public Task Add(Bot battle)
    {
        if (!_bots.TryAdd(battle.Id, battle))
            throw new Exception();

        return Task.CompletedTask;
    }

    public Task Update(Bot battle)
    {
        if (!_bots.TryGetValue(battle.Id, out var _))
            throw new FileNotFoundException("There is no bot of given id ongoing");

        _bots[battle.Id] = battle;

        return Task.CompletedTask;
    }

    public Task Delete(PlayerId id)
    {
        if (!_bots.TryGetValue(id, out var _))
            throw new FileNotFoundException("There is no bot of given id ongoing");

        _bots.Remove(id, out var _);

        return Task.CompletedTask;
    }

    public Task DeleteMany(IEnumerable<PlayerId> ids)
    {
        foreach (var id in ids)
            _bots.Remove(id, out var _);

        return Task.CompletedTask;
    }

    public Task<Bot> Get(PlayerId id)
    {
        if (!_bots.TryGetValue(id, out Bot? battle))
            throw new FileNotFoundException("There is no bot of given id ongoing");

        return Task.FromResult(battle);
    }

    public Task<Bot[]> GetMany(IEnumerable<PlayerId> ids) =>
        Task.FromResult(
            ids.Select(id =>
            {
                _bots.TryGetValue(id, out Bot? battle);
                return battle;
            })
            .Where(b => b is not null)
            .Select(b => b!)
            .ToArray());
}
