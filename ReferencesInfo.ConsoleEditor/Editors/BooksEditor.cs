using System;
using System.Collections.Generic;
using System.Linq;
using Common.ConsoleIO;
using Common.Extensions;
using ReferencesInfo.ConsoleEditor.ConsoleUI;
using ReferencesInfo.ConsoleEditor.Controllers;
using ReferencesInfo.Data;
using ReferencesInfo.Entities;

namespace ReferencesInfo.ConsoleEditor.Editors {
    class BooksEditor : CommandManager {

        private DataContext dataContext;
        ICollection<Book> collection;
        IEnumerable<Book> sortedCollection;
        private CommandInfo[] commandsSortingInfo;

        //Конструктор
        public BooksEditor(DataContext dataContext) {
            this.dataContext = dataContext;
            collection = dataContext.Books;
            sortedCollection = collection;
        }

        //Команди
        protected override void IniCommandInfoArray() {
            commandsInfo = new CommandInfo[] {
                new CommandInfo("назад", null),
                new CommandInfo("додати запис про книгу...", Add),
                new CommandInfo("редагувати запис про книгу...", Edit),
                new CommandInfo("видалити запис про книгу...", Remove),
                new CommandInfo("видалити усі записи про книги", Clear),
                new CommandInfo("показати повну інформацію про книгу...", FullInfoBook),
                new CommandInfo("показувати / не показувати список записів", OutDetails),
                new CommandInfo("сортування книг ◂", Sorting),
                new CommandInfo("фільтрація книг ◂", BooksFiltration),
            };
            commandsSortingInfo = new CommandInfo[] {
                new CommandInfo("відмінити вибір команди", null),
                new CommandInfo("сортувати книги за Id", SortById),
                new CommandInfo("сортувати книги за назвою", SortByName),
                new CommandInfo("сортувати книги за роком видання", SortByYear),
                new CommandInfo("сортувати книги за кількістю сторінок", SortByNumberOfPages),
            };
        }

        //PrepareScreen
        protected override void PrepareScreen() {
            Console.Clear();
            ApplyFilters();
            if (indicator == 1) {
                OutBooksData();
                Console.WriteLine();
            }
            Console.WriteLine(selectingCollection.ToLineList("  Відфільтровані книги"));
        }

        //Вивід даних про книги
        public void OutBooksData() {
            Console.WriteLine("  Список книг:");
            Console.Write(new string('─', 77));
            Console.WriteLine("┐");
            Console.WriteLine("{0, 5}│{1, -40}│{2, 14}│{3} │",
                "Id", "Назва книги", "Рік публікації", "К-сть сторінок");
            Console.Write(new string('─', 77));
            Console.WriteLine("┘");
            foreach (var inst in sortedCollection) {
                Console.WriteLine(inst.ToStringMin());
                Console.Write(new string('─', 77));
                Console.WriteLine("┘");
            }
        }

        //Додання записів про книги
        private void Add() {
            Book inst = new Book();
            CheckAddBook(inst);
            try {
                inst.Id = collection.Select(e => e.Id).Max() + 1;
            }
            catch (Exception ex) { inst.Id = 1; }
            collection.Add(inst);
        }
        private void CheckAddBook(Book inst) {
            string name;
            int yearPublication;
            int numberOfPages;
            bool? illustration;
            string content;
            string description;

            while (true) {
                name = Entering.EnterString("Назва книги");
                if (Book.CheckName(name, out string message)) break;
                Console.WriteLine("\t\t" + message);
            }
            inst.Name = name;

            while (true) {
                yearPublication = Entering.EnterInt("Рік видання");
                if (Book.CheckYearPublication(yearPublication, out string message)) break;
                Console.WriteLine("\t\t" + message);
            }
            inst.YearPublication = yearPublication;

            while (true) {
                numberOfPages = Entering.EnterInt("Кількість сторінок");
                if (Book.CheckNumberOfPages(numberOfPages, out string message)) break;
                Console.WriteLine("\t\t" + message);
            }
            inst.NumberOfPages = numberOfPages;

            illustration = Entering.EnterNullableBool("Наявність ілюстрацій (true або false)");
            inst.Illustration = illustration;

            while (true) {
                content = Entering.EnterString("Зміст");
                if (Book.CheckContent(content, out string message)) break;
                Console.WriteLine("\t\t" + message);
            }
            inst.Content = content;

            while (true) {
                description = Entering.EnterString("Опис");
                if (Book.CheckDescription(description, out string message)) break;
                Console.WriteLine("\t\t" + message);
            }
            inst.Description = description;
        }

        //Видалення записів
        private void Clear() {
            Console.Write("\t!Всі записи про книги будуть видалені, і відповідно записи про посилання на ці книги також. Продовжити (ні - Escape)");
            if (Console.ReadKey().Key == ConsoleKey.Escape) return;
            collection.Clear();
            dataContext.Links.Clear();//?
        }
        private void Remove() {
            if (collection.Count == 0) {
                Console.WriteLine("\t\tНемає записів, додайте записи щоб мати " +
                    "можливість видаляти їх");
                PressToContinue();
                return;
            }
            Console.Write("\t!Всі посилання які пов'язані з цією книгою будуть автоматично видалені. Продовжити (ні - Escape)");
            if (Console.ReadKey().Key == ConsoleKey.Escape) return;
            Console.WriteLine();
            int id = Entering.EnterInt("Введіть Id запису", 1, collection.Select(e => e.Id).Max());
            try {
                Book inst = collection.First(e => e.Id == id);
                int n = dataContext.Links.Count(e => e.Book == inst);
                for (int i = 0; i < n; i++) {
                    dataContext.Links.Remove(dataContext.Links.First(e => e.Book == inst));
                }
                collection.Remove(inst);
            }
            catch (Exception ex) {
                Console.WriteLine("\t\tЗапису з таким Id не існує");
                PressToContinue();
            }
        }

        //Редагування записів про книги
        private void Edit() {
            if (collection.Count == 0) {
                Console.WriteLine("\t\tНемає записів, додайте записи щоб мати " +
                    "можливість редагувати їх");
                PressToContinue();
                return;
            }
            int id = Entering.EnterInt("Введіть Id запису", 1, collection.Select(e => e.Id).Max());
            try {
                CheckEditBook(collection.First(e => e.Id == id));
            }
            catch (Exception ex) {
                Console.WriteLine("\t\tЗапису з таким Id не існує");
                PressToContinue();
            }
        }
        private void CheckEditBook(Book inst) {
            int count = 0;
            while (true) {
                try {
                    switch (count) {
                        case 0:
                            Console.Write("\tНазва книги ({0}) >> ", inst.Name);
                            string name = Console.ReadLine().Trim();
                            if (name != "") inst.Name = name;
                            count++;
                            goto case 1;
                        case 1:
                            Console.Write("\tРік видання ({0}) >> ", inst.YearPublication);
                            string yearPublication = Console.ReadLine().Trim();
                            if (yearPublication != "") inst.YearPublication = int.Parse(yearPublication);
                            count++;
                            goto case 2;
                        case 2:
                            Console.Write("\tКількість сторінок ({0}) >> ", inst.NumberOfPages);
                            string numberOfPages = Console.ReadLine().Trim();
                            if (numberOfPages != "") inst.NumberOfPages = int.Parse(numberOfPages);
                            count++;
                            goto case 3;
                        case 3:
                            Console.Write("\tНаявність ілюстрацій (true або false) ({0}) >> ", inst.Illustration);
                            string illustration = Console.ReadLine().Trim();
                            if (illustration != "") inst.Illustration = bool.Parse(illustration);
                            count++;
                            goto case 4;
                        case 4:
                            Console.Write("\tЗміст >> ");
                            string content = Console.ReadLine().Trim();
                            if (content != "") inst.Content = content;
                            count++;
                            goto case 5;
                        case 5:
                            Console.Write("\tОпис >> ");
                            string description = Console.ReadLine().Trim();
                            if (description != "") inst.Description = description;
                            count = 0;
                            break;
                    }
                    break;
                }
                catch (Exception ex) {
                    Console.WriteLine("\t{0}: {1}", ex.GetType().Name, ex.Message);
                }
            }
        }

        //Показ повної інформації про книгу
        private void FullInfoBook() {
            if (collection.Count == 0) {
                Console.WriteLine("\t\tНемає записів, додайте записи щоб мати " +
                    "можливість бачити про них інформацію");
                PressToContinue();
                return;
            }
            int id = Entering.EnterInt("Введіть Id запису", 1, collection.Select(e => e.Id).Max());
            try {
                Console.WriteLine("\n" + collection.First(e => e.Id == id).ToStringMax() + "\n");
                PressToContinue();
            }
            catch (Exception ex) {
                Console.WriteLine("\t\tЗапису з таким Id не існує");
                PressToContinue();
            }
        }

        //Показ списку записів
        private void OutDetails() {
            indicator *= -1;
        }

        //Сортування
        private void Sorting() {
            ShowCommands(commandsSortingInfo, "\n  Варіанти сортування:");
            Command command = EnterCommand(commandsSortingInfo, "Введіть номер команди");
            if (command == null) {
                return;
            }
            command();
        }
        private void SortByName() {
            sortedCollection = sortedCollection.OrderBy(e => e.Name);
        }
        private void SortByYear() {
            sortedCollection = sortedCollection.OrderBy(e => e.YearPublication);
        }
        private void SortByNumberOfPages() {
            sortedCollection = sortedCollection.OrderBy(e => e.NumberOfPages);
        }
        private void SortById() {
            sortedCollection = sortedCollection.OrderBy(e => e.Id);
        }

        //Фільтрація
        BooksFiltrationController booksFiltrationController = new BooksFiltrationController();
        private void BooksFiltration() {
            booksFiltrationController.SelectFilterCommand();
        }
        IEnumerable<Book> selectingCollection;
        private void ApplyFilters() {
            selectingCollection = booksFiltrationController.ApplyFilters(sortedCollection);
        }
    }
}
