#nullable enable

using Core.Collections;
using Core.Domain;
using Game.Battle.Domain.Events;
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

        public override IDomainEvent[] ApplyToTarget(
            ApplyArtifactArgs args, Random? random = null)
        {
            var origin = args.OriginUnit;
            var originStats = origin.CombatStats;

            var target = args.TargetUnits[0];
            var targetStats = target.CombatStats;

            var isHit = CombatFormulas.CalculateIfHit(Accuracy, targetStats.Dodge, random);
            if (!isHit)
                return new MissedEvent(
                    args.BattleId, args.OriginPlayer.Id, origin.Id, target.Id).AsArray();

            var damage = CombatFormulas.CalculateDamage(Accuracy, targetStats.Dodge, out var damageRange, random);
            var isCritic = CombatFormulas.CalculateIfCriticHit(Critics, targetStats.Critics, out var criticsChance, random);
            damage = isCritic ? CombatFormulas.CalculateCriticalDamage(damage, random) : damage;

            var damagePercent = damage / targetStats.Defence.CalculatedValue;

            originStats.Energy.Modify(EnergyRequired.CalculatedValue, nameof(AttackArtifact));
            targetStats.HP.Modify(damagePercent, nameof(AttackArtifact));

            return new DamagedEvent(
                args.BattleId, args.OriginPlayer.Id, origin.Id, target.Id, damagePercent).AsArray();
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
