using Core.Domain;

namespace Slaycard.Api.Features.Combats.Domain;

/// <summary>
/// Artifact is an abstract creation. It can be an attack, heal, skill or anything really.
/// </summary>
/// <param name="Id"></param>
public abstract record Artifact(
    ArtifactId Id) : IEntity<ArtifactId>
{
    public abstract IDomainEvent[] ApplyToTarget(
        ApplyArtifactArgs args,
        RandomizerConfiguration? randomConfig = null,
        Random? random = null);
}

public record RandomizerConfiguration(
    bool AlwaysHit = false,
    bool AlwaysCritic = false);

public record ArtifactId(string Value) : EntityId(Value);

public record ApplyArtifactArgs(
    BattleId BattleId,
    Player OriginPlayer,
    Unit OriginUnit,
    Player TargetPlayer,
    Unit[] TargetUnits);