using Mediator;
using Slaycard.Api.Features.Combats.UseCases.Common;
using System.Collections.Concurrent;

namespace Slaycard.Api.Features.Combats.Infrastructure.Behaviours;

public class PessimisticLockBattleBehaviour<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
    where TCommand : ICommand, IBattleCommand
{
    private readonly static ConcurrentDictionary<string, Mutex> _mutexes = new();

    public async ValueTask<TResult> Handle(
        TCommand command, CancellationToken cancellationToken, MessageHandlerDelegate<TCommand, TResult> next)
    {
        if (!_mutexes.TryGetValue(command.BattleId, out var mutex))
            _mutexes.TryAdd(command.BattleId, mutex = new());

        mutex.WaitOne();
        try
        {
            return await next(command, cancellationToken);
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }
}
