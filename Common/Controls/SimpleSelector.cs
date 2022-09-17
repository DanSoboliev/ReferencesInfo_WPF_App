using System;

namespace Common.Controls {
    public class SimpleSelector {
        int left;
        int top;
        int width;
        int maxLength;

        public string Text {
            get { return items[selectedIndex]; }
        }

        string[] items;
        public int selectedIndex = 0;

        public SimpleSelector(int left, int top, int width,
            string[] items) {
            this.left = left;
            this.top = top;
            this.width = width;
            this.items = items;
            maxLength = width - 1;
        }

        public void Focus() {
            Console.CursorVisible = false;
            Clear();
            ShowArrow();
            ShowText();
            HandleKeyPress();
            Console.CursorVisible = true;
        }

        private void Clear() {
            Console.SetCursorPosition(left, top);
            for (int i = 0; i < maxLength; i++) {
                Console.Write('\u2591');
            }
        }

        private void ShowArrow() {
            Console.SetCursorPosition(left + maxLength, top);
            Console.Write('\u2195');
        }

        private void ShowText() {
            Console.SetCursorPosition(left, top);
            Console.Write(Text);
        }

        private void HandleKeyPress() {
            while (true) {
                System.Threading.Thread.Sleep(0);
                if (!Console.KeyAvailable)
                    continue;
                var cki = Console.ReadKey(true);
                if (cki.Modifiers != 0)
                    continue;
                switch (cki.Key) {
                    case ConsoleKey.DownArrow:
                        if (selectedIndex < items.Length - 1) {
                            selectedIndex++;
                            Clear();
                            ShowText();
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0) {
                            selectedIndex--;
                            Clear();
                            ShowText();
                        }
                        break;
                    case ConsoleKey.Enter:
                        Erase();
                        ShowText();
                        return;
                }
            }
        }

        private void Erase() {
            Console.SetCursorPosition(left, top);
            for (int i = 0; i < width; i++) {
                Console.Write(' ');
            }
        }
    }
}
