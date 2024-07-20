#nullable enable

using Core.Domain;
using Game.Battle.Domain.Artifacts;

namespace Game.Battle.Domain
{
    public abstract class Artifact : IEntity<ArtifactId>
    {
        public ArtifactId Id { get; init; }
        public Chance Chance { get; init; }

        public Artifact(ArtifactId id, Chance chance)
        {
            Id = id;
            Chance = chance;
        }

        public abstract void ApplyToTarget(
            StatisticsGroup origin,
            StatisticsGroup target);

        public virtual AssembleArtifactResult Assemble(Artifact target) =>
            AssembleArtifactResult.Default;

        public virtual bool CanAssemble(Artifact target) =>
            false;
    }

    public record ArtifactId(string Value);

    public enum ArtifactType
    {
        Skill,
        Item,
    }
}
