#nullable enable

using Core.Domain;
using System;

namespace Game.Battle.Domain.Artifacts
{
    public class EnergyArtifact : Artifact
    {
        public EnergyLevel Level { get; }

        public EnergyArtifact(
            ArtifactId id, 
            Chance chance, 
            EnergyLevel level) : base(id, chance)
        {
            Level = level;
        }

        public override IDomainEvent[] ApplyToTarget(ApplyArtifactArgs args, Random? random = null)
        {
            return Array.Empty<IDomainEvent>();
        }
    }

    public enum EnergyLevel
    {
        Small,
        Medium,
        Big
    }
}
