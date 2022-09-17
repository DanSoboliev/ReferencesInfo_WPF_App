using System;

namespace Common.Context.LineIndents {

    public class SimpleLineIndent : LineIndent {

        private char fillChar = ' ';

        public SimpleLineIndent(int step, char fillChar) {
            this.Step = step > 0 ? step : 2;
            this.fillChar = fillChar;
        }

        public SimpleLineIndent() : this(2, ' ') { }

        private string _value = "";

        public override string Value {
            get {
                return _value;
            }
        }

        public override void Increase() {
            _value = new String(fillChar, Length += Step);
        }

        public override void Decrease() {
            if (Length < Step) return;
            _value = new String(fillChar, Length -= Step);
        }

        public override void Clear() {
            Length = 0;
            _value = "";
        }
    }
}
