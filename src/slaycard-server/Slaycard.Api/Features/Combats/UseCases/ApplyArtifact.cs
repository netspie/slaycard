using FluentValidation;
using Mediator;
using Slaycard.Api.Core.Auth;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.UseCases.Common;

namespace Slaycard.Api.Features.Combats.UseCases;

public record ApplyArtifactApiCommand(
    string OriginUnitId,
    string ArtifactId,
    string TargetPlayerId,
    string TargetUnitId);

public static class ApplyArtifactRoute
{
    public static void InitApplyArtifactRoute(this IEndpointRouteBuilder app) => app.MapPost(
        pattern: "/battles/{battleId}/applyArtifact",
        handler:
            (HttpContext context,
            string battleId,
            IMediator mediator,
            ApplyArtifactApiCommand command) =>
            {
                var playerId = context.Request.Headers.Authorization.FirstOrDefault();
                if (playerId is null)
                    throw new NotAuthorizedException();

                return mediator.Send(
                    new ApplyArtifactCommand(
                        battleId,
                        playerId,
                        command.OriginUnitId,
                        command.ArtifactId,
                        command.TargetPlayerId,
                        command.TargetUnitId));
            });
}

public record ApplyArtifactCommandHandler(
    IBattleRepository Repository,
    IPublisher Publisher,
    RandomizerConfiguration? RandomConfig = null) : ICommandHandler<ApplyArtifactCommand>
{
    public async ValueTask<Mediator.Unit> Handle(
        ApplyArtifactCommand command, CancellationToken ct)
    {
        var battle = await Repository.Get(new BattleId(command.BattleId));

        battle.ApplyArtifact(
            new PlayerId(command.OriginPlayerId),
            new UnitId(command.OriginUnitId),
            new ArtifactId(command.ArtifactId),
            new PlayerId(command.TargetPlayerId),
            new UnitId(command.TargetUnitId),
            RandomConfig);

        await Repository.Update(battle);
        await Publisher.PublishEvents(battle, ct);

        return new();
    }
}

public class ApplyArtifactCommandValidator : AbstractValidator<ApplyArtifactCommand>
{
    public ApplyArtifactCommandValidator()
    {
        RuleFor(q => q.BattleId)
            .MustBeGuid();

        RuleFor(q => q.OriginPlayerId)
            .MustBeGuid();

        RuleFor(q => q.OriginUnitId)
            .MustBeGuid();

        RuleFor(q => q.ArtifactId)
            .Must(id => id is not null && id.All(char.IsLetter))
            .WithMessage("'ArtifactId' can contain only letters");

        RuleFor(q => q.TargetPlayerId)
            .MustBeGuid();

        RuleFor(q => q.TargetUnitId)
            .MustBeGuid();
    }
}

public record ApplyArtifactCommand(
    string BattleId,
    string OriginPlayerId,
    string OriginUnitId,
    string ArtifactId,
    string TargetPlayerId,
    string TargetUnitId) : ICommand, IBattleOngoingCommand;
