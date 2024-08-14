using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Api.Features.Combats.UseCases;

public static class StartRandomBotBattleRoute
{
    public static void InitStartRandomBotBattleRoute(this IEndpointRouteBuilder app) => app.MapPost(
        pattern: "/battles/startRandomPvE",
        (HttpContext context,
        StartRandomBotBattleApiCommand command,
        IBattleRepository respository) =>
        {
            var handler = new StartRandomBotBattleCommandHandler(respository);
            return handler.Handle(
                new StartRandomBotBattleCommand(command.PlayerId));
        });
}

public record StartRandomBotBattleApiCommand(
    string PlayerId);

public record StartRandomBotBattleCommand(
    string PlayerId);

public record StartRandomBotBattleCommandHandler(
    IBattleRepository Repository)
{
    public async Task Handle(StartRandomBotBattleCommand command)
    {
        var battle = new Battle(
            new BattleId("xyz"),
            [
                new Player(new PlayerId(command.PlayerId)),
                new Player(new PlayerId("bot")),
            ]);
        
        await Repository.Add(battle);
    }
}
