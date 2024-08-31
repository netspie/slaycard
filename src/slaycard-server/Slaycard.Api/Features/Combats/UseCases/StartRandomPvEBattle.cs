using Core.Collections;
using Core.Domain;
using FluentValidation;
using Mediator;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Domain.Artifacts;
using Slaycard.Api.Features.Combats.Domain.Events;
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

public record StartRandomPvEBattleCommandHandler(
    IPublisher Publisher,
    IBattleRepository Repository,
    RandomizerConfiguration? RandomConfig = null) : ICommandHandler<StartRandomPvEBattleCommand>
{
    public async ValueTask<Mediator.Unit> Handle(
        StartRandomPvEBattleCommand command, CancellationToken ct)
    {
        var battle = new Battle(
            new BattleId(),
            [
                new Player(
                    new PlayerId(command.PlayerId), 
                    CreateDefaultPlayerUnits(RandomConfig?.FixedStatsValue ?? 5)),

                new Player(
                    new PlayerId(EntityId.NewGuid), 
                    CreateDefaultBotUnits(RandomConfig?.FixedStatsValue ?? 5)),
            ]);

        battle.Players.ForEach(player => battle.Start(player.Id));

        await Repository.Add(battle);
        if (battle.IsGameOver)
            await Repository.Delete(battle.Id);

        await Publisher.PublishEvents(
            battle,
            additionalEvents: [new BotCreatedEvent(battle.Id, battle.Players[1].Id)],
            ct);

        return new();
    }

    private static Domain.Unit[] CreateDefaultPlayerUnits(int statsValue = 5) =>
    [
        new Domain.Unit(
            new UnitId(EntityId.NewGuid),
            new CombatStatGroup(
                HP: new Stat(statsValue),
                Attack: new Stat(statsValue),
                Defence: new Stat(statsValue),
                Accuracy: new Stat(statsValue),
                Dodge: new Stat(statsValue),
                Critics: new Stat(statsValue),
                Speed: new Stat(statsValue)),
            artifacts:
            [
                new AttackArtifact(new ArtifactId("attack"))
            ])
    ];

    private static Domain.Unit[] CreateDefaultBotUnits(int statsValue = 5) =>
    [
        new Domain.Unit(
            new UnitId(EntityId.NewGuid),
            new CombatStatGroup(
                HP: new Stat(statsValue),
                Attack: new Stat(statsValue),
                Defence: new Stat(statsValue),
                Accuracy: new Stat(statsValue),
                Dodge: new Stat(statsValue),
                Critics: new Stat(statsValue),
                Speed: new Stat(statsValue)),
            artifacts:
            [
                new AttackArtifact(new ArtifactId("attack"))
            ])
    ];
}

public class StartRandomPvEBattleCommandValidator : AbstractValidator<StartRandomPvEBattleCommand>
{
    public StartRandomPvEBattleCommandValidator()
    {
        RuleFor(q => q.PlayerId)
            .MustBeGuid();
    }
}

public record StartRandomPvEBattleCommand(
    string PlayerId) : ICommand;
