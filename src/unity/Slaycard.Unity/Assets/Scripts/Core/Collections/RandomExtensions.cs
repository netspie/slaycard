#nullable enable

using System;

namespace Core.Collections
{
    public static class RandomExtensions
    {
        public static double Percent(this Random? random) =>
            (random ?? new()).Next(0, 100) / (double) 100;
    }
}
