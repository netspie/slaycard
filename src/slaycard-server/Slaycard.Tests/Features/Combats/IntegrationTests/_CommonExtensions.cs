namespace Slaycard.Tests.Features.Combats.IntegrationTests;

public static class CommonExtensions
{
    public static TImplementation? GetService<TInterface, TImplementation>(this IServiceProvider services)
        where TImplementation : class, TInterface =>
        services.GetService(typeof(TInterface)) as TImplementation;
}
