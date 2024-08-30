namespace Slaycard.Api.Core.Exceptions;

public static class Ex
{
    public static async Task<T?> TryCatch<T>(Func<Task<T>> action) 
    {
        try
        {
            return await action();
        }
        catch (Exception)
        {
            return default;
        }
    }
}
