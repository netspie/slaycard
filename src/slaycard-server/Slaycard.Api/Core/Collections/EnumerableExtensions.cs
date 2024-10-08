﻿namespace Core.Collections
{
    public static class EnumerableExtensions
    {
        public static T[] AsArray<T>(this T @object) => [@object];

        public static T? FirstOfType<T>(this IEnumerable<object> enumerable) =>
            enumerable.OfType<T>().FirstOrDefault();

        public static T? SingleOfType<T>(this IEnumerable<object> enumerable) =>
            enumerable.OfType<T>().SingleOrDefault();

        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable) =>
            enumerable is null || enumerable.Count() <= 0;

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> itemAction)
        {
            foreach (var item in enumerable)
            {
                if (item != null)
                    itemAction(item);
            }

            return enumerable;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> itemAction)
        {
            int i = 0;
            foreach (var item in enumerable)
            {
                if (item != null)
                    itemAction(item, i);

                i++;
            }

            return enumerable;
        }

        public static TOut[] SelectToArray<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TOut> selector) =>
            source.Select(selector).ToArray();

        public static TOut[] SelectToArray<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, int, TOut> selector) =>
            source.Select(selector).ToArray();

        public static TOut[] SelectToArray<TOut>(this int count, Func<int, TOut> selector) =>
            Enumerable.Range(0, count).SelectToArray(selector);
    }
}
