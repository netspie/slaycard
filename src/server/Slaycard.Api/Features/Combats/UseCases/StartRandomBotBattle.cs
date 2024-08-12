using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Api.Features.Combats.UseCases;

public static class StartRandomBotBattleRoute
{
    public static void InitStartRandomBotBattleRoute(this IEndpointRouteBuilder app) => app.MapPost(
        pattern: "/battles/startPvERandom",
        (HttpContext context) =>
        {

        });
}

public record StartRandomBotBattleApiCommand();

public record StartRandomBotBattleCommand(
    string PlayerId);

public record StartRandomBotBattleCommandHandler(
    IBattleRepository Repository)
{
    public async Task Handle(StartRandomBotBattleCommand command)
    {
        //var battle = new Battle(
        //null,)

        var battle = new Battle(
            new BattleId(""),
            Enumerable.Empty<Player>());
        
        await Repository.Add(battle);
    }
}
