using System;
using System.Text.RegularExpressions;

namespace Common.ConsoleIO {
    public static class Entering {

        //EnterInt
        public static int EnterInt(string prompt) {
            string str;
            int value = 0;
            while (true) {
                Console.Write("\t{0}: ", prompt);
                str = Console.ReadLine();
                try {
                    value = Convert.ToInt32(str);
                }
                catch (Exception ex) {
                    Console.WriteLine("\t\t" + ex.Message);
                    continue;
                }
                return value;
            }
        }
        public static int EnterInt(string prompt, int min, 
            int max = int.MaxValue) {
            int value = 0;
            while (true) {
                value = EnterInt(prompt);
                if(value >= min && value <= max) {
                    return value;
                }
                Console.WriteLine("\t\tЗначення повинно бути " +
                    "в межах від {0} до {1}", min, max);
            }
        }
        public static int? EnterNullableInt(string prompt) {
            Console.Write("{0}: ", prompt);
            string s = Console.ReadLine();
            return (s == "") ? (int?)null : Convert.ToInt32(s);
        }

        //EnterString
        public static string EnterString(string prompt) {
            Console.Write("\t{0}: ", prompt);
            string s = Console.ReadLine();
            s = s.Trim();
            s = RemoveDuplicateSpaces(s);
            return s;
        }
        public static string EnterString(string prompt, int maxLength, int minLength = 0){
            while (true) {
                string s = EnterString(prompt);
                if (s.Length >= minLength && s.Length <= maxLength) return s;
                else {
                    Console.WriteLine("\t\tДовжина рядка повинна бути в межах від {0} до {1}", minLength, maxLength);
                    continue;
                }
            }
        }
        public static string EnterString(string prompt, string pattern, string errorMessage, RegexOptions options = RegexOptions.None) {
            while (true) {
                string s = EnterString(prompt);
                if (Regex.IsMatch(s, pattern, options)) return s;
                Console.WriteLine("\t{0}", errorMessage); 
            }
        }
        public static string EnterString(string prompt, string current, string pattern = ".{0,}", string errorMessage = null, RegexOptions options = RegexOptions.None) {
            while (true) {
                string s = EnterString(prompt + $" ({current})");
                if (s == "") return current;
                else if (Regex.IsMatch(s, pattern, options)) return s;
                Console.WriteLine("\t{0}", errorMessage);
            }
        }

        //RemoveDuplicateSpaces
        public static string RemoveDuplicateSpaces(string s) {
            Regex regex = new Regex(@"\s+");
            s = regex.Replace(s, " ");
            return s;
        }

        //EnterBool
        public static bool? EnterNullableBool(string prompt) {
            string str;
            bool? value;
            while (true) {
                Console.Write("\t{0}: ", prompt);
                str = Console.ReadLine();
                try {
                    value = (str == "") ? (bool?)null : Convert.ToBoolean(str);
                }
                catch (Exception ex) {
                    Console.WriteLine("\t\t" + ex.Message);
                    continue;
                }
                return value;
            }
        }
        public static bool? EnterNullBool(string str) {
            bool? value = null;
            try {
                if (str == "System.Windows.Controls.ComboBoxItem: False") value = false;
                else if (str == "System.Windows.Controls.ComboBoxItem: True") value = true;
                else if (str == "System.Windows.Controls.ComboBoxItem: Невідомо") value = null;
                //value = (str == "") ? (bool?)null : Convert.ToBoolean(str);
            }
            catch (Exception ex) {
                value = null;
                return value;
            }
            return value;
        }
    }
}
