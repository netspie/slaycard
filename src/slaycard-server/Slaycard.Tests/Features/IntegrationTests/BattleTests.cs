using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Features.Combats.IntegrationTests;

public class BattleTests
{
    [Test]
    public void PerformBasicBattle()
    {
        var player1 = new Player(new PlayerId("player-1"), []);
        var player2 = new Player(new PlayerId("player-2"), []);

        var players = new[] { player1, player2 };
        var battle = new Battle(new BattleId("asd"), players);
    }
}
