using Core.Domain;
using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Combats.Domain.Events;

public abstract record BattleEvent(
    BattleId BattleId,
    PlayerId PlayerId) : IDomainEvent
{
    public string Id => Guid.NewGuid().ToString();
    public long Timestamp => DateTime.UtcNow.Ticks;
}
