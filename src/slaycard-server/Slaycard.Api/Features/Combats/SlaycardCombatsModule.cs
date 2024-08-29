using FluentValidation;
using FluentValidation.AspNetCore;
using Mediator;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Infrastructure;
using Slaycard.Api.Features.Combats.Infrastructure.Behaviours;
using Slaycard.Api.Features.Combats.UseCases;

namespace Slaycard.Api.Features.Combats;

public static class SlaycardCombatsModule
{
    public static IServiceCollection AddCombatsModule(this IServiceCollection services)
    {
        services.AddSingleton<IBattleRepository, InMemoryBattleRepository>();

        services.AddMediator(opts => opts.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PessimisticLockBattleBehaviour<,>));

        return services;
    }

    public static IEndpointRouteBuilder UseCombatsModule(this IEndpointRouteBuilder app)
    {
        app.InitStartRandomPvEBattleRoute();
        app.InitApplyArtifactRoute();
        app.InitGetBattleRoute();
        app.InitGetBattlesRoute();

        return app;
    }
}
