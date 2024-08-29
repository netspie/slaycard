using Mediator;
using Slaycard.Api.Features.Combats.UseCases.Common;
using System.Collections.Concurrent;

namespace Slaycard.Api.Features.Combats.Infrastructure.Behaviours;

public class PessimisticLockBattleBehaviour<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
    where TCommand : ICommand, IBattleOngoingCommand
{
    private readonly static ConcurrentDictionary<string, Unit> _battles = new();

    public async ValueTask<TResult> Handle(
        TCommand command, CancellationToken ct, MessageHandlerDelegate<TCommand, TResult> next)
    {
        while (_battles.TryGetValue(command.BattleId, out var _) && !ct.IsCancellationRequested);
        
        if (!_battles.TryAdd(command.BattleId, new()))
            throw new InvalidOperationException("");

        try
        {
            return await next(command, ct);
        }
        finally
        {
            _battles.Remove(command.BattleId, out var _);
        }
    }
}
