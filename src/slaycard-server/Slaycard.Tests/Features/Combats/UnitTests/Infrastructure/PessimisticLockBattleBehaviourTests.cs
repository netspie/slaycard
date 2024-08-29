using Mediator;
using Slaycard.Api.Features.Combats.Infrastructure.Behaviours;
using Slaycard.Api.Features.Combats.UseCases.Common;

namespace Slaycard.Tests.Features.Combats.UnitTests.Infrastructure;

internal class PessimisticLockBattleBehaviourTests
{
    public record TestCommand(string BattleId) : IBattleOngoingCommand, ICommand;

    [Test]
    public async Task Handle()
    {
        var behaviour = new PessimisticLockBattleBehaviour<TestCommand, Unit>();

        int value = 0;

        var task2 = Handle(async () =>
        {
            await Task.Delay(200);
            value = 1;
        });

        var task1 = Handle(async () =>
        {
            await Task.Delay(1);
            value = 2;
        });

        await Task.WhenAll([task1, task2]);

        Assert.That(value, Is.EqualTo(2));

        async Task Handle(Func<Task> action) =>
            await behaviour.Handle(
                new TestCommand("1"),
                CancellationToken.None,
                next: async (cmd, ct) =>
                {
                    await action();
                    return new Unit();
                });
    }
}
