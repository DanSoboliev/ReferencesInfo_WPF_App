using System.Collections.Generic;

namespace Common.Extensions {
    public static class EnumerableMethods {
        public static string ToLineList<T>(this IEnumerable<T> collection, string prompt) {
            return string.Concat(prompt, ":\n", string.Join("\n", collection), "\n");
        }
    }
}
