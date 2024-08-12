using Slaycard.Api.Features.Combats.UseCases;

namespace Slaycard.Api.Features.Combats;

public static class SlaycardCombats
{
    public static IEndpointRouteBuilder InitCombatsModule(this IEndpointRouteBuilder app)
    {
        app.InitStartRandomBotBattleRoute();

        return app;
    }
}
