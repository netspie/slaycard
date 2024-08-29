using Slaycard.Api.Features.CombatsBots.Domain;
using Slaycard.Api.Features.CombatsBots.EventHandlers;
using Slaycard.Api.Features.CombatsBots.Infrastructure;

namespace Slaycard.Api.Features.CombatsBots;

public static class SlaycardCombatsBotsModule
{
    public static IServiceCollection AddCombatsBotsModule(this IServiceCollection services)
    {
        var botRepository = new InMemoryBotRepository();
        services.AddSingleton<IBotRepository>(botRepository);

        var eventQueue = new BotBattleEventQueue();
        services.AddSingleton<IBotBattleEventQueue>(eventQueue);

        services.AddHostedService(sp => 
            new BotBattleEventProcessor(eventQueue, sp.GetRequiredService<IServiceProvider>()));

        return services;
    }
}
