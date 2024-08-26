namespace Slaycard.Api.Core.Domain;

public class DomainException(string? message) : Exception(message);

public static class DomainExceptionExtensions
{
    public static T ThrowIfNull<T>(this T? @object, string message)
    {
        if (@object is null)
            throw new DomainException(message);

        return @object;
    }
}
