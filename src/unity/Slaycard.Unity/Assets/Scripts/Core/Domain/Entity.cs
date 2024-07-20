using Core.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain
{
    public class Entity<TId> : IEntity<TId>
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
    }

    public interface IEntity<TId>
    {
        TId Id { get; }
    }

    public static class EntityExtensions
    {
        public static TEntity GetOfId<TEntity, TId>(this IEnumerable<TEntity> source, TId id)
            where TEntity : IEntity<TId>
            where TId : class =>
            source.FirstOrDefault(x => x.Id == id) ?? default;
    }

    public interface IDomainEvent
    {
        string Id { get; }
        long Timestamp { get; }
    }
}
