using Slaycard.Api.Features.CombatsBots.Domain;
using Slaycard.Api.Features.CombatsBots.Infrastructure;

namespace Slaycard.Api.Features.CombatsBots;

public static class SlaycardCombatsBotsModule
{
    public static IServiceCollection AddCombatsBotsModule(this IServiceCollection services)
    {
        var botRepository = new InMemoryBotRepository();
        services.AddSingleton<IBotRepository>(botRepository);

        return services;
    }
}
