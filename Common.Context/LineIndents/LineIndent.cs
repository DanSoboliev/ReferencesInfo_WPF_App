namespace Common.Context.LineIndents {

    public abstract class LineIndent {

        private static LineIndent current;

        public static LineIndent Current {
            get {
                if (current == null) {
                    current = new SimpleLineIndent();
                }
                return current;
            }
            set { current = value; }
        }

        public abstract string Value { get; }
        public int Length { get; protected set; }
        public int Step { get; protected set; }

        public abstract void Increase();
        public abstract void Decrease();
        public abstract void Clear();
    }
}
