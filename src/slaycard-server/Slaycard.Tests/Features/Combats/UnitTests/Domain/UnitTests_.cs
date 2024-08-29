using Slaycard.Api.Features.Combats.Domain.Artifacts;
using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Tests.Features.Combats.UnitTests.Domain;

public class UnitTests_
{
    public static Unit CreateUnit(string unitId, int statValues = 1) => new Unit(
        new UnitId(unitId),
        new CombatStatGroup(
            HP: new Stat(statValues),
            Attack: new Stat(statValues),
            Defence: new Stat(statValues),
            Accuracy: new Stat(statValues),
            Dodge: new Stat(statValues),
            Critics: new Stat(statValues),
            Speed: new Stat(statValues)),
        artifacts:
        [
            new AttackArtifact(new ArtifactId("attack"))
        ]);
}
