using Slaycard.Api.Features.Combats.Domain;
using static Slaycard.Api.Features.Combats.UseCases.GetBattlesQueryResponse;

namespace Slaycard.Api.Features.Combats.UseCases;

public static class GetBattlesRoute
{
    public static void InitGetBattlesRoute(this IEndpointRouteBuilder app) => app.MapGet(
        pattern: "/battles",
        handler: 
            (HttpContext context,
            [AsParameters] GetBattlesApiQuery query,
            IBattleRepository repository) =>
            {
                var handler = new GetBattlesQueryHandler(repository);
                return handler.Handle(new GetBattlesQuery(query.Offset, query.Limit));
            });
}

public record GetBattlesQueryHandler(
    IBattleRepository Repository)
{
    public async Task<GetBattlesQueryResponse> Handle(
        GetBattlesQuery query)
    {
        var battles = await Repository.GetMany(
            query.Offset, 
            query.Limit);

        var battleDTOs = battles
            .Map(b => new BattleDTO(b.Id, b.TimeCreated))
            .ToArray();

        return new GetBattlesQueryResponse(battleDTOs);
    }
}

public record GetBattlesApiQuery(
    int Offset = 0,
    int Limit = 25);

public record GetBattlesQuery(
    int Offset = 0,
    int Limit = 25);

public record GetBattlesQueryResponse(
    BattleDTO[] Battles)
{
    public record BattleDTO(
        string Id,
        DateTime TimeCreated);
};
