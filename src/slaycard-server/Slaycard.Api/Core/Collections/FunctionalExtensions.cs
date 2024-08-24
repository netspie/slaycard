namespace Core.Collections;

public static class FunctionalExtensions
{
    public static T ThrowIfNull<T>(this T? @object, string message)
    {
        if (@object is null)
            throw new Exception(message);

        return @object;
    }
}
