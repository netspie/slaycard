using Slaycard.Api.Features.Combats.Domain;
using System.Collections.Concurrent;

namespace Slaycard.Api.Features.CombatsBots.Infrastructure;

public record BotTimeoutClock(int TimeoutSeconds)
{
    private readonly ConcurrentDictionary<PlayerId, (PlayerId id, DateTime lastTime)> _bots = new();

    public PlayerId[] GetTimeoutBattles()
    {
        var timeoutBots = new List<PlayerId>();

        foreach (var bot in _bots.Values)
        {
            var offset = DateTime.UtcNow - bot.lastTime;
            if (offset < TimeSpan.FromSeconds(TimeoutSeconds))
                continue;

            timeoutBots.Add(bot.id);
        }

        return timeoutBots.ToArray();
    }

    public void Add(PlayerId id)
    {
        _bots.TryAdd(id, (id, DateTime.UtcNow));
    }

    public void Update(PlayerId id)
    {
        _bots[id] = (id, DateTime.UtcNow);
    }

    public void Remove(PlayerId id)
    {
        _bots.Remove(id, out var _);
    }
}
