using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Tests.Features.Combats.UnitTests;

internal class BattleTests
{
    [Test]
    public void _Throws_IfHasLessThanTwoPlayers()
    {
        Assert.Throws<Exception>(() => new Battle(new BattleId(), []));
    }
}
