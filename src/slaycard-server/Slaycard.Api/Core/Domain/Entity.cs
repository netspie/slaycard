using Core.Collections;

namespace Core.Domain;

public class Entity<TId> : IEntity<TId>
    where TId : class
{
    public TId Id { get; }
    public Entity(TId id) => Id = id;

    private List<IDomainEvent> _events = new();
    public IEnumerable<IDomainEvent> Events => _events;
    public void ClearEvents() => _events.Clear();

    protected void AddEvent(IDomainEvent ev) => _events.Add(ev);
    protected void AddEvents(IEnumerable<IDomainEvent> events) => events.ForEach(AddEvent);

    public override string ToString()
    {
        var result = Id.ToString();

        var type = GetType();
        var nameProperty = type.GetProperty("Name");
        if (nameProperty != null)
        {
            var name = nameProperty.GetValue(this) as string;
            if (!name.IsNullOrEmpty())
                result = $"{name} - {Id}";
        }

        return result;
    }

    public static implicit operator bool(Entity<TId> v) => v is not null;

    public override bool Equals(object? obj) =>
        obj is Entity<TId> entity ?
            entity.Id == Id :
            false;

    public override int GetHashCode() => Id.GetHashCode();
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
        source.FirstOrDefault(x => x.Id == id) ?? default;

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
    public static implicit operator string(EntityId id) => id.Value;

    public static string NewGuid => Guid.NewGuid().ToString();
}
