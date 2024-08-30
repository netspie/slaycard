using Core.Collections;

namespace Core.Domain;

public class Entity<TId> : IEntity<TId>, IEventContainer
{
    public TId Id { get; }

    public Entity(TId id) => Id = id;

    private List<IDomainEvent> _events = new();
    IEnumerable<IDomainEvent> IEventContainer.Events => _events;
    void IEventContainer.Clear() => _events.Clear();

    protected void AddEvent(IDomainEvent ev) => _events.Add(ev);
    protected void AddEvents(IEnumerable<IDomainEvent> events) => events.ForEach(AddEvent);

    public override string? ToString()
    {
        if (Id is null)
            return base.ToString();

        var result = Id.ToString();
        if (result is null)
            return $"Error occured while .ToString()";

        var type = GetType();
        var nameProperty = type.GetProperty("Name");
        if (nameProperty is null)
            return $"Error occured while .ToString()";
        
        var name = nameProperty.GetValue(this) as string;
        if (!name.IsNullOrEmpty())
            result = $"{name} - {Id}";

        return result;
    }

    public static implicit operator bool(Entity<TId> v) => v is not null;

    public override bool Equals(object? obj) =>
        obj is Entity<TId> entity && entity.Id is not null ?
            entity.Id.Equals(Id) :
            false;

    public override int GetHashCode() =>
        Id is not null ?
            Id.GetHashCode() : default;
}

public interface IEventContainer
{
    IEnumerable<IDomainEvent> Events { get; }
    void Clear();
}

public interface IEntity<TId>
{
    TId Id { get; }
}

public static class EntityExtensions
{
    public static TEntity? GetOfId<TEntity, TId>(this IEnumerable<TEntity> source, TId id)
        where TEntity : IEntity<TId>
        where TId : class =>
        source.FirstOrDefault(x => x.Id.Equals(id));

    public static TEntity GetNotOfId<TEntity, TId>(this IEnumerable<TEntity> source, TId id)
        where TEntity : IEntity<TId>
        where TId : class =>
        source.First(x => !x.Id.Equals(id));

    public static TId[] GetIds<TId>(this IEnumerable<IEntity<TId>> source) =>
        source.Select(item => item.Id).ToArray();
}

public interface IDomainEvent
{
    string Id { get; }
    long Timestamp { get; }
}

public abstract record EntityId(string Value)
{
    public override int GetHashCode() => Value.GetHashCode();
    public static string NewGuid => Guid.NewGuid().ToString();
}
