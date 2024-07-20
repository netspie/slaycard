#nullable enable

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

        public override void ApplyToTarget(StatisticsGroup origin, StatisticsGroup target)
        {
            
        }

        public override AssembleArtifactResult Assemble(StatisticsGroup origin, Artifact target)
        {
            return AssembleArtifactResult.Default;
        }

        public virtual bool CanAssemble(Artifact target) =>
            false;
    }

    public enum EnergyLevel
    {
        Small,
        Medium,
        Big
    }
}
