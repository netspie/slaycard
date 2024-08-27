using Core.Domain;
using FluentValidation;
using Mediator;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Domain.Artifacts;
using Slaycard.Api.Features.Combats.UseCases.Common;

namespace Slaycard.Api.Features.Combats.UseCases;

public record StartRandomPvEBattleApiCommand(
    string PlayerId);

public static class StartRandomPvEBattleRoute
{
    public static void InitStartRandomPvEBattleRoute(this IEndpointRouteBuilder app) => app.MapPost(
        pattern: "/battles/startRandomPvE",
        handler: 
            (IMediator mediator,
            StartRandomPvEBattleApiCommand command) =>
            {
                return mediator.Send(new StartRandomPvEBattleCommand(command.PlayerId));
            });
}

public record StartRandomBotBattleCommandHandler(
    IBattleRepository Repository) : ICommandHandler<StartRandomPvEBattleCommand>
{
    public async ValueTask<Mediator.Unit> Handle(
        StartRandomPvEBattleCommand command, CancellationToken ct)
    {
        var battle = new Battle(
            new BattleId("xyz"),
            [
                new Player(new PlayerId(command.PlayerId), CreateDefaultPlayerUnits()),
                new Player(new PlayerId("bot"), CreateDefaultBotUnits()),
            ]);
        
        await Repository.Add(battle);

        return new Mediator.Unit();
    }

    private static Domain.Unit[] CreateDefaultPlayerUnits() =>
    [
        new Domain.Unit(
            new UnitId(EntityId.NewGuid),
            new CombatStatGroup(
                HP: new Stat(5),
                Attack: new Stat(5),
                Defence: new Stat(5),
                Accuracy: new Stat(5),
                Dodge: new Stat(5),
                Critics: new Stat(5),
                Speed: new Stat(5)),
            artifacts:
            [
                new AttackArtifact(new ArtifactId("attack"))
            ])
    ];

    private static Domain.Unit[] CreateDefaultBotUnits() =>
    [
        new Domain.Unit(
            new UnitId(EntityId.NewGuid),
            new CombatStatGroup(
                HP: new Stat(5),
                Attack: new Stat(5),
                Defence: new Stat(5),
                Accuracy: new Stat(5),
                Dodge: new Stat(5),
                Critics: new Stat(5),
                Speed: new Stat(5)),
            artifacts:
            [
                new AttackArtifact(new ArtifactId("attack"))
            ])
    ];
}

public class StartRandomPvEBattleCommandHandlerValidator : AbstractValidator<StartRandomPvEBattleCommand>
{
    public StartRandomPvEBattleCommandHandlerValidator()
    {
        RuleFor(q => q.PlayerId)
            .MustBeGuid();
    }
}

public record StartRandomPvEBattleCommand(
    string PlayerId) : ICommand;
