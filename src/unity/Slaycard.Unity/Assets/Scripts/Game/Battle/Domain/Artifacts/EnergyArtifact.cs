#nullable enable

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

        public override void ApplyToTarget(StatGroup origin, StatGroup target, Random? random = null)
        {
            
        }
    }

    public enum EnergyLevel
    {
        Small,
        Medium,
        Big
    }
}
