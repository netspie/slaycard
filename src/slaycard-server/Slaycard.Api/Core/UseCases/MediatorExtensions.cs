using Core.Domain;
using Mediator;

namespace Slaycard.Api.Core.UseCases;

public static class MediatorExtensions
{
    public static Task PublishEvents(
        this IPublisher publisher,
        IEventContainer container)
    {
        var events = container.Events.ToList();
        container.Clear();

        return Task.WhenAll(events.Select(async n => await publisher.Publish(n)).ToArray());
    }
}
