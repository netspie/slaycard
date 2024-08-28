using Mediator;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.CombatsBots.EventHandlers;

public class BattleEventsHandler :
    INotificationHandler<BattleInstantiatedEvent>
{
    public ValueTask Handle(BattleInstantiatedEvent ev, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
