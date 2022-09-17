using System;
using System.Text.RegularExpressions;

namespace ReferencesInfo.Entities {
    partial class Book {
        public static bool CheckName(string value, out string message) {
            if (string.IsNullOrWhiteSpace(value)) {
                message = "Потрібно вказати назву";
                return false;
            }
            else if (value.Length > 40 || value.Length < 1) {
                //throw new Exception("Кількість символів повинно бути" +
                //    " в межах від 1 до 40");
                message = "Кількість символів повинно бути в межах від 1 до 40";
                return false;
            }
            else if (!Regex.IsMatch(value, @"\A[А-ЯA-ZЇІЄҐЬ!:;,.'?""0-9’— -]{1,40}\z",
                RegexOptions.IgnoreCase)) {
                message = "Недопустимі символи";
                return false;
            }
            message = "";
            return true;
        }

        public static bool CheckContent(string value, out string message) {
            if (value == null) {
                message = "";
                return true;
            }
            if (value.Length > 500) {
                message = "Кількість символів повинно бути" +
                    " не більше 500";
                return false;
            }
            else if (!Regex.IsMatch(value, @"\A[A-ZА-ЯІЇЬЄ !."":;0-9,?'()\[\]№’— -]{0,500}\z",
                RegexOptions.IgnoreCase)) {
                message = "Недопустимі символи";
                return false;
            }
            message = "";
            return true;
        }

        public static bool CheckDescription(string value, out string message) {
            if (value == null) {
                message = "";
                return true;
            }
            if (value.Length > 500) {
                message = "Кількість символів повинно бути не більше 500";
                return false;
            }
            else if (!Regex.IsMatch(value, @"\A[A-ZА-ЯІЇЬЄ !."":;0-9,?'’— -]{0,500}\z",
                RegexOptions.IgnoreCase)) {
                message = "Недопустимі символи";
                return false;
            }
            message = "";
            return true;
        }

        public static bool CheckYearPublication(int value, out string message) {
            if (value < -5000 || value > DateTime.Now.Year) {
                message = $"Недопустимимй рік видання. Рік видання повинний бути від 5000 року д.е. до {DateTime.Now.Year} року н.е.";
                return false;
            }
            message = "";
            return true;
        }

        public static bool CheckNumberOfPages(int value, out string message) {
            if (value < 1 || value > 10000) {
                message = "Кількість сторінок повинна бути в межах віж 1 до 10000";
                return false;
            }
            message = "";
            return true;
        }
    }
}
