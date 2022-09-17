using System.Text.RegularExpressions;

namespace ReferencesInfo.Entities {
    partial class Link {
        public static bool CheckTopic(string value, out string message) {
            if (string.IsNullOrWhiteSpace(value)) {
                message = "Потрібно вказати тему";
                return false;
            }
            else if (value.Length > 40 || value.Length < 1) {
                message = "Кількість символів повинно бути" +
                    " в межах від 1 до 40";
                return false;
            }
            else if (!Regex.IsMatch(value, @"\A[А-ЯA-ZЇІЄҐЬ0-9!+:'?""%’— -]{1,40}\z",
                RegexOptions.IgnoreCase)) {
                message = "Недопустимі символи";
                return false;
            }
            message = "";
            return true;
        }

        public static bool CheckPageNumber(int value, out string message, Book book) {
            if (value < 1 || value > book.NumberOfPages) {
                message = $"Номер сторінки повинен бути в межах від 1 до {book.NumberOfPages}";
                return false;
            }
            message = "";
            return true;
        }

        public static bool CheckNote(string value, out string message) {
            if(value == null || value == "") {
                message = "";
                return true;
            }
            if (value.Length > 200) {
                message = "Кількість символів повинно бути" +
                    " не більше 200";
                return false;
            }
            else if (!Regex.IsMatch(value, @"\A[A-ZА-ЯІЇЬЄҐЬ !."":;0-9,?'()\[\]’— -]{0,200}\z",
                RegexOptions.IgnoreCase)) {
                message = "Недопустимі символи";
                return false;
            }
            message = "";
            return true;
        }
    }
}
