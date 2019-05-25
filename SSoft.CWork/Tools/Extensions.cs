using System;
using System.Collections.Generic;
using System.Linq;

namespace SSoft.CWork.Tools {
    public static class Extensions {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> function) {
            foreach (var x in enumerable) {
                function(x);
                yield return x;
            }
        }

        public static string Repeat(this char c, int amount) {
            return new string(Enumerable.Repeat(c, amount).ToArray());
        }
    }
}