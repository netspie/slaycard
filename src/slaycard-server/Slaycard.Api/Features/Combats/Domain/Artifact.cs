using Core.Domain;

namespace Slaycard.Api.Features.Combats.Domain;

public abstract class Artifact : IEntity<ArtifactId>
{
    public ArtifactId Id { get; init; }
    public Chance Chance { get; init; }

    public Artifact(ArtifactId id, Chance chance)
    {
        Id = id;
        Chance = chance;
    }

    public abstract IDomainEvent[] ApplyToTarget(
        ApplyArtifactArgs args,
        Random? random = null);
}

public record ArtifactId(string Value);

public record ApplyArtifactArgs(
    Player OriginPlayer,
    Unit OriginUnit,
    Player TargetPlayer,
    Unit[] TargetUnits);