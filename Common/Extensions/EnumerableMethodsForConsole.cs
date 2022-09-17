using System;
using System.Collections.Generic;

namespace Common.Extensions {
    public static class EnumerableMethodsForConsole {
        public static void OutLineList<T>(this IEnumerable<T> collection, string prompt) {
            Console.WriteLine(prompt + ":\n" + string.Join("\n", collection));
        }
        public static void OutLine<T>(this IEnumerable<T> collection, string prompt) {
            Console.WriteLine(prompt + ":\t" + string.Join(", ", collection));
        }
        public static void OutLineListTab<T>(this IEnumerable<T> collection, string prompt) {
            Console.WriteLine(prompt + ":\n\t" + string.Join("\n\t", collection));
        }
    }
}
