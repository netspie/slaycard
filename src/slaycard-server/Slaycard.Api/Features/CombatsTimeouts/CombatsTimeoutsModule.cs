namespace Slaycard.Api.Features.CombatsTimeouts;

public static class CombatsTimeoutsModule
{
    public static IServiceCollection AddCombatsTimeoutsModule(
        this IServiceCollection services)
    {
        services.AddSingleton(new CombatsTimeoutWorkerConfiguration(TimeoutSeconds: 120));
        services.AddHostedService(sp => 
            new CombatsTimeoutWorker(
                sp,
                sp.GetRequiredService<CombatsTimeoutWorkerConfiguration>()));

        return services;
    }
}
