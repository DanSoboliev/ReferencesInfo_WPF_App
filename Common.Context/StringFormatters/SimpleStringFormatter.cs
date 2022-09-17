using System;
using System.Text;

namespace Common.Context.StringFormatters {

    public class SimpleStringFormatter : StringFormatter {

        public override string FormatWithLineBreaks(
                string text, int indentLength) {
            string indent = new String(' ', indentLength);
            if (text == null)
                return indent;
            StringBuilder sb = new StringBuilder(text.Length * 2);
            string[] arr = text.Split(new char[] { '\n' },
                StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in arr) {
                FormatParagraph(s + "\n", indentLength, indent, sb); 
            }
            return sb.ToString();
        }

        protected virtual void FormatParagraph(string text, 
            int indentLength, string indent, StringBuilder sb) {
            int pos = 0;
            int len = LineLength - indentLength - 1;
            while (pos < text.Length) {
                sb.Append(indent);
                if (text.Length - pos >= len) {
                    sb.AppendFormat("{0}\n", 
                        text.Substring(pos, len));
                } else {
                    sb.Append(text.Substring(pos));
                }
                pos += len;
            }
        }
    }
}
