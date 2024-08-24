using Core.Domain;

namespace Slaycard.Api.Features.Combats.Domain;

public class Unit : IEntity<UnitId>
{
    public UnitId Id { get; init; }

    public CombatStatGroup CombatStats { get; private set; }

    public Artifact[] Artifacts { get; private set; }

    public Unit(
        UnitId id, 
        CombatStatGroup combatStats, 
        IEnumerable<Artifact> artifacts)
    {
        Id = id;
        CombatStats = combatStats;
        Artifacts = artifacts.ToArray();
    }

    public Artifact? GetArtifact(ArtifactId id) =>
        Artifacts.GetOfId(id);

    public bool IsAlive() =>
        CombatStats.HP.CalculatedValue > 0;

    public override bool Equals(object? obj) =>
        obj is Unit unit ?
            unit.Id == Id : 
            false;

    public override int GetHashCode() => Id.GetHashCode();
}

public record UnitId(string Value) : EntityId(Value);
