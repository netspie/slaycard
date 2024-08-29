using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Domain.Artifacts;
using Slaycard.Api.Features.Combats.Domain.Events;
using Slaycard.Tests.Features.Combats.UnitTests.Domain;

namespace Slaycard.Tests.Features.Combats.UnitTests.Domain.Artifacts;

public class AttackArtifactTests
{
    [Test]
    public void ApplyToTarget()
    {
        var unit1 = UnitTests_.CreateUnit("unit-1", statValues: 1);
        var unit2 = UnitTests_.CreateUnit("unit-2", statValues: 1);

        var player1 = new Player(new PlayerId("player-1"), [unit1]);
        var player2 = new Player(new PlayerId("player-2"), [unit2]);

        var args = new ApplyArtifactArgs(
            new BattleId(),
            player1,
            unit1,
            player2,
            [unit2]);

        var artifact = new AttackArtifact(new ArtifactId("attack"));

        var events = artifact.ApplyToTarget(args, new Random(7));
        Assert.IsTrue(events.OfType<DamagedEvent>().Any());
        Assert.IsTrue(unit2.CombatStats.HP.CalculatedValue < unit2.CombatStats.HP.OriginalValue);
        Assert.IsFalse(events.OfType<MissedEvent>().Any());

        Assert.IsTrue(artifact.ApplyToTarget(args, new Random(8)).OfType<MissedEvent>().Any());
    }
}
