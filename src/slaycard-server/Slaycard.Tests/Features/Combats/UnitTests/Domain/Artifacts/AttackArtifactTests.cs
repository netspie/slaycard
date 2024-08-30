using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Domain.Artifacts;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Tests.Features.Combats.UnitTests.Domain.Artifacts;

public class AttackArtifactTests
{
    [Test]
    public void ApplyToTarget()
    {
        var originUnit = UnitTests_.CreateUnit("unit-1", statValues: 1);
        var targetUnit = UnitTests_.CreateUnit("unit-2", statValues: 1);

        var originPlayer = new Player(new PlayerId("player-1"), [originUnit]);
        var targetPlayer = new Player(new PlayerId("player-2"), [targetUnit]);

        var args = new ApplyArtifactArgs(
            new BattleId(),
            originPlayer,
            originUnit,
            targetPlayer,
            [targetUnit]);

        var artifact = new AttackArtifact(new ArtifactId("attack"));

        var events = artifact.ApplyToTarget(args, random: new Random(7));
        Assert.IsTrue(events.OfType<DamagedEvent>().Any());
        Assert.IsTrue(targetUnit.CombatStats.HP.CalculatedValue < targetUnit.CombatStats.HP.OriginalValue);
        Assert.IsFalse(events.OfType<MissedEvent>().Any());

        Assert.IsTrue(artifact.ApplyToTarget(args, random: new Random(8)).OfType<MissedEvent>().Any());
    }
}
