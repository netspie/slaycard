#nullable enable

using System;

namespace Game.Battle.Domain.Artifacts
{
    public class AttackArtifact : Artifact
    {
        public AttackType Type { get; }
        public Stat Damage { get; }
        public Stat Accuracy { get; }
        public Stat Critics { get; }
        public Stat EnergyRequired { get; }

        public AttackArtifact(ArtifactId id, Chance chance) : base(id, chance) {}

        public override void ApplyToTarget(StatGroup origin, StatGroup target, Random? random = null)
        {
            var isHit = CombatFormulas.CalculateIfHit(Accuracy, target.Dodge, random);
            if (!isHit)
                return; // return missed event

            var damage = CombatFormulas.CalculateDamage(Accuracy, target.Dodge, out var damageRange, random);
            var isCritic = CombatFormulas.CalculateIfCriticHit(Critics, target.Critics, out var criticsChance, random);
            damage = isCritic ? CombatFormulas.CalculateCriticalDamage(damage, random) : damage;

            var damagePercent = damage / target.Defence.CalculatedValue;

            origin.Energy.Modify(EnergyRequired.CalculatedValue, "attack");
            target.HP.Modify(damagePercent, "attack");

            // return damaged/dealt/ event
        }

        public override AssembleArtifactResult Assemble(Artifact artifact)
        {
            if (artifact is EnergyArtifact energyArtifact)
            {
                var a = new AttackArtifact(Id, Chance);
            }

            return AssembleArtifactResult.Default;
        }
    }

    public enum AttackType
    {
        Quick,
        Heavy,
    }
}
