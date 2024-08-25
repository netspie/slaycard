using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Tests.Features.Combats.UnitTests;

public class PlayerTests
{
    [Test]
    public void _Throws_IfHasZeroUnits()
    {
        Assert.Throws<Exception>(() => new Player(new PlayerId("id-1"), []));
    }
}
