using FluentValidation;
using Mediator;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.UseCases.Common;
using static Slaycard.Api.Features.Combats.UseCases.GetBattleChangedQueryResponse;

namespace Slaycard.Api.Features.Combats.UseCases;

public record GetBattleChangedApiQuery(
    string BattleId, 
    int Version);

public static class GetBattleChangedRoute
{
    public static void InitGetBattleChangedRoute(this IEndpointRouteBuilder app) => app.MapGet(
        pattern: "/battles/{battleId}/changed",
        handler:
            (IMediator mediator,
            [AsParameters] GetBattleChangedApiQuery query) =>
            {
                return mediator.Send(
                    new GetBattleChangedQuery(
                        query.BattleId,
                        query.Version));
            });
}

public record GetBattleChangedQueryHandler(
    IBattleRepository Repository) : IQueryHandler<GetBattleChangedQuery, GetBattleChangedQueryResponse>
{
    public async ValueTask<GetBattleChangedQueryResponse> Handle(
        GetBattleChangedQuery query, CancellationToken ct)
    {
        var battle = await Repository.Get(
            new BattleId(query.BattleId));

        return new GetBattleChangedQueryResponse(
            new ChangedDTO(
                Changed: query.Version != battle.Version));
    }
}

public class GetBattleChangedQueryValidator : AbstractValidator<GetBattleChangedQuery>
{
    public GetBattleChangedQueryValidator()
    {
        RuleFor(q => q.BattleId)
            .MustBeGuid();
    }
}

public record GetBattleChangedQuery(string BattleId, int Version) : IQuery<GetBattleChangedQueryResponse>;
public record GetBattleChangedQueryResponse(ChangedDTO DTO)
{
    public record struct ChangedDTO(bool Changed);
}
