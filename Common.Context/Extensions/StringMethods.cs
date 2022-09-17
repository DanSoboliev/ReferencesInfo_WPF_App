using Common.Context.LineIndents;
using Common.Context.StringFormatters;

namespace Common.Context.Extensions {
    public static class StringMethods {
        public static string ToIndentedLineBlock(this string str) {
            string s = "";
            LineIndent.Current.Increase();
            s += StringFormatter.Current.FormatWithLineBreaks(str).TrimEnd('\n');
            LineIndent.Current.Decrease();
            return s;
        }

        public static string ToIndentedLineBlock(this string str, int indentLength) {
            string s = "";
            LineIndent.Current.Increase();
            s += StringFormatter.Current.FormatWithLineBreaks(str, indentLength).TrimEnd('\n');
            LineIndent.Current.Decrease();
            return s;
        }
    }
}
