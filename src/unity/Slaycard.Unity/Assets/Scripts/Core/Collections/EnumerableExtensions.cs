using System.Collections.Generic;
using System;
using System.Linq;

namespace Core.Collections
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> itemAction)
        {
            if (enumerable == null)
                return null;

            foreach (var item in enumerable)
            {
                if (item != null)
                    itemAction(item);
            }

            return enumerable;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> itemAction)
        {
            if (enumerable == null)
                return null;

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
