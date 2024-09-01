using Slaycard.Api.Features.Combats.Domain;
using System.Collections.Concurrent;

namespace Slaycard.Api.Features.CombatsBots.Infrastructure;

public record BotTimeoutClock(int TimeoutSeconds)
{
    private readonly ConcurrentDictionary<BattleId, (PlayerId[] ids, DateTime lastTime)> _bots = new();

    public PlayerId[] GetTimeoutBots()
    {
        var timeoutBots = new List<PlayerId>();

        foreach (var bot in _bots.Values)
        {
            var offset = DateTime.UtcNow - bot.lastTime;
            if (offset < TimeSpan.FromSeconds(TimeoutSeconds))
                continue;

            timeoutBots.AddRange(bot.ids);
        }

        return timeoutBots.ToArray();
    }

    public void Add(BattleId battleId, PlayerId[] botIds)
    {
        _bots.TryAdd(battleId, (botIds, DateTime.UtcNow));
    }

    public void Update(BattleId id)
    {
        _bots[id] = (_bots[id].ids, DateTime.UtcNow);
    }

    public void Remove(BattleId id)
    {
        _bots.Remove(id, out var _);
    }
}
