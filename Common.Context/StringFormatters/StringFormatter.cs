using Common.Context.LineIndents;

namespace Common.Context.StringFormatters {

    public abstract class StringFormatter {

        private static StringFormatter current;

        public static StringFormatter Current {
            get {
                if (current == null) {
                    current = new CorrectStringFormatter() {
                        LineLength = 79
                    };
                }
                return current;
            }
            set { current = value; }
        }

        public virtual int LineLength { get; set; }

        public abstract string FormatWithLineBreaks(
            string text, int indentLength);

        public string FormatWithLineBreaks(string text) {
            return FormatWithLineBreaks(text,
                LineIndent.Current.Length);
        }
    }
}
