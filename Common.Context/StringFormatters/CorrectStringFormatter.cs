using System.Text;

namespace Common.Context.StringFormatters {
    class CorrectStringFormatter : SimpleStringFormatter {
        protected override void FormatParagraph(string text,
            int indentLength, string indent, StringBuilder sb) {
            int pos = 0;
            int len = LineLength - indentLength - 1;
            int pos2;
            while (pos < text.Length) {
                pos2 = len;
                sb.Append(indent);
                if (text.Length - pos >= len) {
                    pos2 = (text.LastIndexOf(" ", pos + len, len) + 1) - pos;
                    if (pos2 < 0) pos2 = len;
                    sb.AppendFormat("{0}\n",
                        text.Substring(pos, pos2));
                }
                else {
                    sb.Append(text.Substring(pos));
                }
                pos += pos2;
            }
        }
    }
}
