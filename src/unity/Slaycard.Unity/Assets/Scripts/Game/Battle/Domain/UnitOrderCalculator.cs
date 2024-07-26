using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Battle.Domain
{
    public static class UnitOrderCalculator
    {
        public const int MaxOrderLimit = 50;

        public static int[] CalculateActionOrder(Unit[] Units) =>
            CalculateActionOrder(Units.Select(u => u.CombatStats.Speed.CalculatedValue).ToArray());

        public static int[] CalculateActionOrder(int[] speeds)
        {
            var sum = speeds.Sum();
            var divisor = sum > MaxOrderLimit ? sum / (double) MaxOrderLimit : 1;

            if (divisor > 1)
            {
                for (int i = 0; i < speeds.Length; i++)
                    speeds[i] = (int) Math.Floor(speeds[i] / divisor);

                sum = speeds.Sum();
            }

            // (optional) - shrink down extreme speed values, so one unit does not overtake the order space - more of the game design issue, for later
            //..

            // Find greatest common divisor, shrink speeds, in the result shrink the output order array - not a big deal though
            var gcd = GCD(speeds);
            if (gcd > 1)
            {
                for (int i = 0; i < speeds.Length; i++)
                    speeds[i] /= gcd;

                sum = speeds.Sum();
            }

            var order = new int[sum];
            for (int i = 0; i < sum; i++)
                order[i] = -1;

            var speedsSorted = speeds.Select((s, i) => new Pair(s, i)).ToList();
            speedsSorted.Sort(new Comparer());

            // Spread indexes evenly by its occurence value
            // Units/indexes with the same speed value should be in ascending order (by input array index) - although eventually will be randomized
            var firstFree = 0;
            for (int si = 0; si < speeds.Length; si++)
            {
                firstFree = GetFirstFreeIndex(firstFree);

                var speed = speedsSorted[si];
                for (int vi = 0; vi < speed.v; vi++)
                {
                    var d = sum / (double) speed.v;
                    var oi = (int) (vi * d) + (vi == 0 ? firstFree : 0);
                    oi = GetFirstFreeIndex(oi);

                    order[oi] = speed.i;
                }
            }

            int GetFirstFreeIndex(int firstFree)
            {
                for (int fi = firstFree; fi < order.Length; fi++)
                {
                    if (order[fi] != -1)
                        continue;

                    firstFree = fi;
                    return firstFree;
                }

                return -1;
            }

            return order;
        }

        public record Pair(int v, int i);
        public class Comparer : IComparer<Pair>
        {
            int IComparer<Pair>.Compare(Pair x, Pair y) =>
                y.v - x.v;
        }

        private static int GCD(params int[] v) =>
            v.Aggregate((x, y) => GCDx(x, y));

        private static int GCDx(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }
    }
}