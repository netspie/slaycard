using FluentValidation;
using Mediator;

namespace Slaycard.Api.Infrastructure;

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
        await _validator.ValidateAsync(message);
        return await next(message, ct);
    }
}
