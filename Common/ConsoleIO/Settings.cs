using System;
using System.Text;

namespace Common.ConsoleIO {
    public static class Settings {
        public static void SetConsoleParam() {
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
        }
    }
}
