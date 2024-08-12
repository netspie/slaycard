using Microsoft.AspNetCore.Mvc;
using Slaycard.Api.Features.Combats.Domain;
using static Slaycard.Api.Features.Combats.UseCases.GetBattleQueryResponse;

namespace Slaycard.Api.Features.Combats.UseCases;

public static class GetBattleRoute
{
    public static void InitStartRandomBotBattleRoute(this IEndpointRouteBuilder app) => app.MapPost(
        pattern: "/battles/{battleId}", 
        handler: (
            HttpContext context,
            GetBattleApiQuery query,
            IBattleRepository respository) =>

            new GetBattleQueryHandler(respository)
                .Handle(new GetBattleQuery(query.BattleId))
    );
}

public record GetBattleQueryHandler(
    IBattleRepository Repository)
{
    public async Task<GetBattleQueryResponse> Handle(
        GetBattleQuery query)
    {
        var battle = await Repository.Get(
            new BattleId(query.BattleId));
        
        var players = battle.Players.Map(p => 
            new PlayerDTO(p.Id)).ToArray();

        return new GetBattleQueryResponse(  
            new BattleDTO(
                battle.Id,
                players));
    }
}

public record GetBattleApiQuery(
    [FromRoute] string BattleId);

public record GetBattleQuery(string BattleId);

public record GetBattleQueryResponse(
    BattleDTO DTO)
{
    public record BattleDTO(
        string Id,
        PlayerDTO[] Players);

    public record PlayerDTO(
        string Id);
};
