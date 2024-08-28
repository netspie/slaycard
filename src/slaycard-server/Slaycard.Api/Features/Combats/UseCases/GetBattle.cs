using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.UseCases.Common;
using static Slaycard.Api.Features.Combats.UseCases.GetBattleQueryResponse;

namespace Slaycard.Api.Features.Combats.UseCases;

public record GetBattleApiQuery(
    [FromRoute] string BattleId);

public static class GetBattleRoute
{
    public static void InitGetBattleRoute(this IEndpointRouteBuilder app) => app.MapGet(
        pattern: "/battles/{battleId}",
        handler: 
            (IMediator mediator,
            [AsParameters] GetBattleApiQuery query) =>
            {
                return mediator.Send(new GetBattleQuery(query.BattleId));
            });
}

public record GetBattleQueryHandler(
    IBattleRepository Repository) : IQueryHandler<GetBattleQuery, GetBattleQueryResponse>
{
    public async ValueTask<GetBattleQueryResponse> Handle(
        GetBattleQuery query, CancellationToken ct)
    {
        var battle = await Repository.Get(
            new BattleId(query.BattleId));
        
        var players = battle.Players.Map(p => 
            new PlayerDTO(p.Id.Value)).ToArray();

        return new GetBattleQueryResponse(  
            new BattleDTO(
                battle.Id.Value,
                players));
    }
}

public class GetBattleQueryValidator : AbstractValidator<GetBattleQuery>
{
    public GetBattleQueryValidator()
    {
        RuleFor(q => q.BattleId)
            .MustBeGuid();
    }
}

public record GetBattleQuery(string BattleId) : IQuery<GetBattleQueryResponse>;

public record GetBattleQueryResponse(
    BattleDTO DTO)
{
    public record BattleDTO(
        string Id,
        PlayerDTO[] Players);

    public record PlayerDTO(
        string Id);
};
