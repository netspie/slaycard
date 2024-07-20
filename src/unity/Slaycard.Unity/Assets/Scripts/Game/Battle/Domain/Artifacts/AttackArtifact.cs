#nullable enable

using Core.Domain;
using System;

namespace Game.Battle.Domain.Artifacts
{
    public class AttackArtifact : Artifact
    {
        public AttackArtifact(ArtifactId id, Chance chance) : base(id, chance) {}

        public override void ApplyToTarget(StatisticsGroup origin, StatisticsGroup target)
        {
            
        }

        public override bool CanAssemble => false;
    }
}
