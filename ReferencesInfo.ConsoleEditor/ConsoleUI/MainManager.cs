using System;
using System.IO;
using Common.Controls;
using ReferencesInfo.ConsoleEditor.Editors;
using ReferencesInfo.Data;
using ReferencesInfo.IO;

namespace ReferencesInfo.ConsoleEditor.ConsoleUI {
    class MainManager : CommandManager {
        private DataContext dataContext;
        private BooksEditor booksEditor;
        private LinksEditor linksEditor;
        private SimpleSelector simpleSelector;
        private static BinarySerializationController bsIoController = new BinarySerializationController();
        private static XmlFileIoController xmlFileIoController = new XmlFileIoController();
        private static TextFileIoController textFileIoController = new TextFileIoController();

        //Конструктор
        public MainManager(DataContext dataContext) {
            this.dataContext = dataContext;
            booksEditor = new BooksEditor(dataContext);
            linksEditor = new LinksEditor(dataContext);
            indicator = -1;
        }

        //Ініціалізація команд
        protected override void IniCommandInfoArray() {
            commandsInfo = new CommandInfo[] {
                new CommandInfo("вийти", null),
                new CommandInfo("створити тестові дані",  CreateTestingData),
                new CommandInfo("видалити усі записи", Clear),
                new CommandInfo("зберегти дані", Save),
                new CommandInfo("показувати / не показувати список записів", OutDetails),
                new CommandInfo("працювати з даними про книги", RunBooksEditor),
                new CommandInfo("працювати з даними про посилання", RunLinksEditor),
            };
        }

        private void RunBooksEditor() {
            booksEditor.Run();
        }
        private void RunLinksEditor() {
            linksEditor.Run();
        }

        //Видалення усіх записів
        private void Clear() {
            dataContext.Clear();
        }

        //Тестові дані
        private void CreateTestingData() {
            if (dataContext.Books.Count == 0 && dataContext.Links.Count == 0) {
                dataContext.CreateTestingData();
            }
            else {
                Console.WriteLine("\t\tТестові дані не вдалося створити");
                PressToContinue();
            }
        }

        //Підготовка консолі
        protected override void PrepareScreen() {
            Console.Clear();
            OutStatistics();
            Console.WriteLine();
            if (indicator == 1) {
                OutData();
                Console.WriteLine();
            }
        }

        //Статистична інформація
        private void OutStatistics() {
            Console.WriteLine("  Статистична інформація:\n" +
                "\t{0,-11} {1}\n\t{2,-11} {3}", 
                "Книг", dataContext.Books.Count,
                "Посилань", dataContext.Links.Count);
        }

        //Вивід таблиць даних
        private void OutData() {
            booksEditor.OutBooksData();
            Console.WriteLine();
            linksEditor.OutLinksData();
        }

        //Дозвіл на вивід даних
        private void OutDetails() {
            indicator *= -1;
        }

        //Збереження даних
        private void Save() {
            string[] items = new string[] { ".ribd", ".xml", ".txt", "*бінарний файл*", "*відмінити збереження даних*" };
            Console.Write("  Оберіть розширення файлу: ");
            simpleSelector = new SimpleSelector(Console.CursorLeft, Console.CursorTop, 32, items);
            simpleSelector.Focus();
            int index = simpleSelector.selectedIndex;
            switch (index) {
                case 0:
                    dataContext.FileIoController = bsIoController;
                    bsIoController.FileExtension = ".ribd";
                    break;
                case 1:
                    dataContext.FileIoController = xmlFileIoController;
                    break;
                case 2:
                    dataContext.FileIoController = textFileIoController;
                    break;
                case 3:
                    dataContext.FileIoController = bsIoController;
                    bsIoController.FileExtension = "";
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("\t\tСталася помилка, дані не збережено");
                    PressToContinue();
                    return;
            }
            dataContext.Save();
            Console.WriteLine("\n\tДані успішно збережено");
            PressToContinue();
        }

        //Завантаження даних
        public void Load() {
            Mark:
            if (dataContext.Books.Count != 0 || dataContext.Links.Count != 0) return;
            if (!File.Exists(@"files\ReferencesInfo.ribd") && !File.Exists(@"files\ReferencesInfo.xml")) return;
            Console.Write("Завантажити дані? Якщо ні - настиніть Esc, якщо так - натисніть будь-яку іншу клавішу... ");
            if (Console.ReadKey().Key == ConsoleKey.Escape) return;
            Console.WriteLine();
            string[] items = Directory.GetFiles("files");
            Array.Resize(ref items, items.Length + 1);
            items[items.Length - 1] = "*Відмінити завантаження даних*";
            Console.Write("Оберіть файл для завантаження даних: ");
            simpleSelector = new SimpleSelector(Console.CursorLeft, Console.CursorTop, 52, items);
            simpleSelector.Focus();
            string text = simpleSelector.Text;
            switch (text) {
                case @"files\ReferencesInfo.ribd":
                    dataContext.FileIoController = bsIoController;
                    bsIoController.FileExtension = ".ribd";
                    LoadHelp();
                    break;
                case @"files\ReferencesInfo":
                    dataContext.FileIoController = bsIoController;
                    bsIoController.FileExtension = "";
                    LoadHelp();
                    break;
                case @"files\ReferencesInfo.xml":
                    dataContext.FileIoController = xmlFileIoController;
                    LoadHelp();
                    break;
                case @"files\ReferencesInfo.txt":
                    dataContext.FileIoController = textFileIoController;
                    LoadHelp();
                    break;
                case @"*Відмінити завантаження даних*":
                    break;
                default:
                    Console.WriteLine("\n\tСталася помилка. Недопустимий файл з даними");
                    PressToContinue();
                    Console.WriteLine();
                    goto Mark;
            }
        }
        private void LoadHelp() {
            try {
                dataContext.Load();
            }
            catch(Exception ex) {
                Console.WriteLine();
                Console.WriteLine("\tПри завантажені даних сталася помилка. " +
                    "Файл може бути пошкоджений");
                PressToContinue();
                dataContext.Clear();
                return;
            }
            Console.WriteLine();
            Console.WriteLine("\tДані успішно завантажено");
            PressToContinue();
        }
    }
}
