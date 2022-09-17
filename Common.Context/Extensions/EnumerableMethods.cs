using Common.Context.LineIndents;
using System.Collections.Generic;

namespace Common.Context.Extensions
{
    public static class EnumerableMethods
    {
        public static string ToIndentedLineList<T>(
                this IEnumerable<T> collection, string prompt) {
            string s = string.Concat(LineIndent.Current.Value, prompt, ":\n");
            LineIndent.Current.Increase();
            s += string.Concat(string.Join("\n", collection), "\n");
            LineIndent.Current.Decrease();
            return s;
        }


    }
}
