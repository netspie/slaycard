using Slaycard.Api.Features.Combats.Domain;
using System.Collections.Concurrent;

namespace Slaycard.Api.Features.CombatsTimeouts;

public record BattleTimeoutClock(int TimeoutSeconds)
{
    private readonly ConcurrentDictionary<BattleId, (BattleId id, DateTime lastTime)> _battles = new();

    public BattleId[] GetTimeoutBattles()
    {
        var battlesToRemove = new List<BattleId>();

        foreach (var battle in _battles.Values)
        {
            var offset = DateTime.UtcNow - battle.lastTime;
            if (offset < TimeSpan.FromSeconds(TimeoutSeconds))
                continue;

            battlesToRemove.Add(battle.id);
        }

        return battlesToRemove.ToArray();
    }

    public void Add(BattleId id)
    {
        _battles.TryAdd(id, (id, DateTime.UtcNow));
    }

    public void Update(BattleId id)
    {
        _battles[id] = (id, DateTime.UtcNow);
    }

    public void Remove(BattleId id)
    {
        _battles.Remove(id, out var _);
    }
}
