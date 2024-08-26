using Core.Domain;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.Combats.Domain.Artifacts;

public record AttackArtifact(ArtifactId Id) : Artifact(Id)
{
    public override IDomainEvent[] ApplyToTarget(
        ApplyArtifactArgs args, Random? random = null)
    {
        var origin = args.OriginUnit.CombatStats;
        var targets = args.TargetUnits.Map(u => u.CombatStats).ToArray();

        return args.TargetUnits
            .Map<Unit, IDomainEvent>(targetUnit =>
            {
                var target = targetUnit.CombatStats;

                var isHit = CombatFormulas.CalculateIfHit(origin.Accuracy, target.Dodge, random);
                if (!isHit) 
                    return new MissedEvent(args.BattleId, args.OriginPlayer.Id, args.OriginUnit.Id, targetUnit.Id);

                var damage = CombatFormulas.CalculateDamage(origin.Attack, target.Defence, out var damageRange, random);

                bool isCriticalHit = CombatFormulas.CalculateIfCriticHit(origin.Critics, target.Critics, out var criticalHitChance, random);
                damage = isCriticalHit ? CombatFormulas.CalculateCriticalDamage(damage, random) : damage;

                target.HP.Modify(-damage, Id);

                return new DamagedEvent(args.BattleId, args.OriginPlayer.Id, args.OriginUnit.Id, targetUnit.Id, damage, isCriticalHit);
            })
            .ToArray();
    }
}

public record HealArtifact(ArtifactId Id) : Artifact(Id)
{
    public override IDomainEvent[] ApplyToTarget(
        ApplyArtifactArgs args, Random? random = null)
    {
        throw new InvalidOperationException("");
    }
}
