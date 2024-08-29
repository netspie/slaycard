using Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications;
using System.Collections.Concurrent;

namespace Slaycard.Api.Features.CombatsBots.EventHandlers;

public class BotBattleEventQueue : IBotBattleEventQueue
{
    private readonly ConcurrentQueue<ClientBattleNotification> _events = new();

    public void Enqueue(ClientBattleNotification notification) =>
        _events.Enqueue(notification);

    public bool TryDequeue(out ClientBattleNotification? result) =>
        _events.TryDequeue(out result);
}

public interface IBotBattleEventQueue
{
    void Enqueue(ClientBattleNotification notification);
    bool TryDequeue(out ClientBattleNotification? result);
}
