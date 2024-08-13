using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Infrastructure;
using Slaycard.Api.Features.Combats.UseCases;

namespace Slaycard.Api.Features.Combats;

public static class SlaycardCombatsModule
{
    public static IServiceCollection AddCombatsModule(this IServiceCollection services)
    {
        services.AddSingleton<IBattleRepository, InMemoryBattleRepository>();

        return services;
    }

    public static IEndpointRouteBuilder UseCombatsModule(this IEndpointRouteBuilder app)
    {
        app.InitStartRandomBotBattleRoute();
        app.InitGetBattleRoute();

        return app;
    }
}
