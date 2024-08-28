using Core.Domain;
using Mediator;

namespace Slaycard.Api.Features.Combats.Domain.Events;

public abstract record BattleEvent(
    BattleId BattleId,
    PlayerId? PlayerId) : IDomainEvent, INotification
{
    public string Id => Guid.NewGuid().ToString();
    public long Timestamp => DateTime.UtcNow.Ticks;
}
