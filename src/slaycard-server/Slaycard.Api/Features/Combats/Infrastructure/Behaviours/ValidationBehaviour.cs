using FluentValidation;
using LanguageExt.Common;
using Mediator;

namespace Slaycard.Api.Features.Combats.Infrastructure.Behaviours;

public class ValidationBehaviour<T, TResult> : IPipelineBehavior<T, TResult>
    where T : IMessage
{
    private readonly IValidator<T> _validator;

    public ValidationBehaviour(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<TResult> Handle(
        T message,
        CancellationToken ct,
        MessageHandlerDelegate<T, TResult> next)
    {
        var result = await _validator.ValidateAsync(message);
        if (!result.IsValid)
            throw new ValidationException(result.ToString());

        return await next(message, ct);
    }
}

public class BattleEventPublisherBehaviour<T, TResult> : IPipelineBehavior<T, TResult>
    where T : IMessage
{
    private readonly IValidator<T> _validator;

    public BattleEventPublisherBehaviour(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<TResult> Handle(
        T message,
        CancellationToken ct,
        MessageHandlerDelegate<T, TResult> next)
    {
        var result = await _validator.ValidateAsync(message);
        if (!result.IsValid)
            throw new ValidationException(result.ToString());

        return await next(message, ct);
    }
}
