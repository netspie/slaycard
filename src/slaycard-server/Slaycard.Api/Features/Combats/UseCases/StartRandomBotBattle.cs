using Mediator;
using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Api.Features.Combats.UseCases;

public static class StartRandomBotBattleRoute
{
    public static void InitStartRandomBotBattleRoute(this IEndpointRouteBuilder app) => app.MapPost(
        pattern: "/battles/startRandomPvE",
        handler: 
            (IMediator mediator,
            StartRandomBotBattleApiCommand command) =>
            {
                mediator.Send(new StartRandomBotBattleCommand(command.PlayerId));
            });
}

public record StartRandomBotBattleApiCommand(
    string PlayerId);

public record StartRandomBotBattleCommand(
    string PlayerId) : ICommand;

public record StartRandomBotBattleCommandHandler(
    IBattleRepository Repository) : ICommandHandler<StartRandomBotBattleCommand>
{
    public async ValueTask<Mediator.Unit> Handle(
        StartRandomBotBattleCommand command, CancellationToken ct)
    {
        var battle = new Battle(
            new BattleId("xyz"),
            [
                new Player(new PlayerId(command.PlayerId), []),
                new Player(new PlayerId("bot"), []),
            ]);
        
        await Repository.Add(battle);

        return new();
    }
}
