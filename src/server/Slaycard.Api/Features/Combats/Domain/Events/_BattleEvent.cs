using Core.Domain;

namespace Game.Battle.Domain.Events;

public abstract record BattleEvent(
    BattleId BattleId,
    PlayerId PlayerId) : IDomainEvent
{
    public string Id => Guid.NewGuid().ToString();
    public long Timestamp => DateTime.UtcNow.Ticks;
}
