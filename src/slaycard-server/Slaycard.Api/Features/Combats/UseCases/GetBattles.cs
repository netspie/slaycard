using FluentValidation;
using Mediator;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.UseCases.Common;
using static Slaycard.Api.Features.Combats.UseCases.GetBattlesQueryResponse;

namespace Slaycard.Api.Features.Combats.UseCases;

public record GetBattlesApiQuery(
    int Offset = 0,
    int Limit = 25,
    string? PlayerId = null);

public static class GetBattlesRoute
{
    public static void InitGetBattlesRoute(this IEndpointRouteBuilder app) => app.MapGet(
        pattern: "/battles",
        handler: 
            (IMediator mediator,
            [AsParameters] GetBattlesApiQuery query) =>
            {
                return mediator.Send(
                    new GetBattlesQuery(query.Offset, query.Limit, query.PlayerId));
            });
}

public record GetBattlesQueryHandler(
    IBattleRepository Repository) : IQueryHandler<GetBattlesQuery, GetBattlesQueryResponse>
{
    public async ValueTask<GetBattlesQueryResponse> Handle(
        GetBattlesQuery query, CancellationToken ct)
    {
        if (query.PlayerId is not null)
        {
            var battle = await Repository.Get(new PlayerId(query.PlayerId));
            return new GetBattlesQueryResponse(
            [
                new BattleDTO(battle.Id.Value, battle.TimeCreated)
            ]);
        }

        var battles = await Repository.GetMany(
            query.Offset, 
            query.Limit);

        var battleDTOs = battles
            .Map(b => new BattleDTO(b.Id.Value, b.TimeCreated))
            .ToArray();

        return new GetBattlesQueryResponse(battleDTOs);
    }
}

public class GetBattlesQueryValidator : AbstractValidator<GetBattlesQuery>
{
    public GetBattlesQueryValidator() 
    {
        RuleFor(q => q.Offset)
            .GreaterThanOrEqualTo(0);

        RuleFor(q => q.Limit)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(25);

        RuleFor(q => q.PlayerId)
            .MustBeGuid()
            .When(q => q.PlayerId is not null);
    }
}

public record GetBattlesQuery(
    int Offset = 0,
    int Limit = 25,
    string? PlayerId = null) : IQuery<GetBattlesQueryResponse>;

public record GetBattlesQueryResponse(
    BattleDTO[] Battles)
{
    public record BattleDTO(
        string Id,
        DateTime TimeCreated);
};
