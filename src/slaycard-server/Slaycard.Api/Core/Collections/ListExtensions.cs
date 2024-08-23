using System.Collections.Generic;

namespace Core.Collections
{
    public static class ListExtensions
    {
        public static bool Replace<T>(this IList<T> list, T oldItem, T newItem)
        {
            var i = list.IndexOf(oldItem);
            if (i == -1)
                return false;

            list[i] = newItem;

            return true;
        }
    }
}
