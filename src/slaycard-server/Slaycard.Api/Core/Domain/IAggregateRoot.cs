namespace Core.Domain;

public interface IAggregateRoot
{
    public int Version { get; set; }
}

public interface IAggregateRoot<TId> : IAggregateRoot
{
    public TId Id { get; }
}
