using System;
using System.Collections.Generic;
using System.Linq;
using Common.ConsoleIO;
using Common.Extensions;
using ReferencesInfo.ConsoleEditor.ConsoleUI;
using Common.Controls;
using ReferencesInfo.ConsoleEditor.Controllers;
using ReferencesInfo.Data;
using ReferencesInfo.Entities;

namespace ReferencesInfo.ConsoleEditor.Editors {
    class LinksEditor : CommandManager {
        DataContext dataContext;
        ICollection<Link> collection;
        IEnumerable<Link> sortedCollection;
        private CommandInfo[] commandsSortingInfo;
        SimpleSelector simpleSelector;

        //Конструктор
        public LinksEditor(DataContext dataContext) {
            this.dataContext = dataContext;
            collection = dataContext.Links;
            sortedCollection = collection;
        }

        //Команди
        protected override void IniCommandInfoArray() {
            commandsInfo = new CommandInfo[] {
                new CommandInfo("назад", null),
                new CommandInfo("додати запис про посилання...", Add),
                new CommandInfo("редагувати запис про посилання...", Edit),
                new CommandInfo("видалити запис про посилання...", Remove),
                new CommandInfo("видалити усі записи про посилання", Clear),
                new CommandInfo("показати повну інформацію про посилання...", FullInfoLink),
                new CommandInfo("показувати / не показувати список записів", OutDetails),
                new CommandInfo("сортування посилань ◂", Sorting),
                new CommandInfo("фільтрація книг ◂", LinksFiltration),
            };
            commandsSortingInfo = new CommandInfo[] {
                new CommandInfo("відмінити вибір команди", null),
                new CommandInfo("сортувати посилання за Id", SortById),
                new CommandInfo("сортувати посилання за темою", SortByTopic),
                new CommandInfo("сортувати посилання за назвами книг", SortByBook),
                new CommandInfo("сортувати посилання за номерами сторінок", SortByPageNumber),
            };
        }

        //PrepareScreen
        protected override void PrepareScreen() {
            Console.Clear();
            ApplyFilters();
            if (indicator == 1) {
                OutLinksData();
                Console.WriteLine();
            }
            Console.WriteLine(selectingCollection.ToLineList("  Відфільтровані посилання"));
        }

        //Вивід даних про посилання
        public void OutLinksData() {
            Console.WriteLine("  Список посилань:");
            Console.Write(new string('─', 97));
            Console.WriteLine("┐");
            Console.WriteLine("{0, 5}│{1,-40}│{2,-40}│{3} │",
                "Id", "Тема", "Книга", "Сторінка");
            Console.Write(new string('─', 97));
            Console.WriteLine("┘");
            foreach (var obj in sortedCollection) {
                Console.WriteLine(obj.ToStringMin());
                Console.Write(new string('─', 97));
                Console.WriteLine("┘");
            }
        }

        //Додання записів про посилання
        private void Add() {
            if (dataContext.Books.Count == 0) {
                Console.WriteLine("\t\tНемає книг на які можна створити посилання. Щоб створити посилання, створіть спочатку книгу");
                PressToContinue();
                return;
            }
            Link inst = new Link();
            CheckAddLink(inst);
            try {
                inst.Id = collection.Select(e => e.Id).Max() + 1;
            }
            catch (Exception ex) { inst.Id = 1; }
            collection.Add(inst);
        }
        private void CheckAddLink(Link inst) {
            string topic;
            int pageNumber;
            string note;
            while(true){
                topic = Entering.EnterString("Тема");
                if (Link.CheckTopic(topic, out string message)) break;
                Console.WriteLine("\t\t" + message);
            }
            inst.Topic = topic;

            Console.Write("\tКнига >> ");
            inst.Book = SelectAddBook();
            Console.WriteLine();

            while (true) {
                pageNumber = Entering.EnterInt("Номер сторінки");
                if (Link.CheckPageNumber(pageNumber, out string message, inst.Book)) break;
                Console.WriteLine("\t\t" + message);
            }
            inst.PageNumber = pageNumber;

            while (true) {
                note = Entering.EnterString("Примітка");
                if (Link.CheckNote(note, out string message)) break;
                Console.WriteLine("\t\t" + message);
            }
            inst.Note = note;
        }
        private Book SelectAddBook() {
            string[] items = new string[dataContext.Books.Count];
            int i = 0;
            foreach(Book el in dataContext.Books) {
                items[i] = el.Key;
                i++;
            }
            simpleSelector = new SimpleSelector(Console.CursorLeft, Console.CursorTop, 53, items);
            simpleSelector.Focus();
            string text = simpleSelector.Text;
            return dataContext.Books.First(e => e.Key == text);
        }

        //Видалення записів
        private void Clear() {
            collection.Clear();
        }
        private void Remove() {
            if (dataContext.Links.Count == 0) {
                Console.WriteLine("\t\tНемає записів, додайте записи щоб мати " +
                    "можливість видаляти їх");
                PressToContinue();
                return;
            }
            int id = Entering.EnterInt("Введіть Id запису", 1, collection.Select(e => e.Id).Max());
            try {
                Link inst = collection.First(e => e.Id == id);
                collection.Remove(inst);
            }
            catch (Exception ex) {
                Console.WriteLine("\t\tЗапису з таким Id не існує");
                PressToContinue();
            }
        }

        //Редагування записів про посилання
        private void Edit() {
            if (collection.Count == 0) {
                Console.WriteLine("\t\tНемає записів, додайте записи щоб мати " +
                    "можливість редагувати їх");
                PressToContinue();
                return;
            }
            int id = Entering.EnterInt("Введіть Id запису", 1, collection.Select(e => e.Id).Max());
            try {
                CheckEditLink(collection.First(e => e.Id == id));
            }
            catch (Exception ex) {
                Console.WriteLine("\tЗапису з таким Id не існує\n\tНажміть будь-яку клавішу");
                Console.ReadKey(true);
            }
        }
        private void CheckEditLink(Link inst) {
            int count = 0;
            while (true) {
                try {
                    switch (count) {
                        case 0:
                            Console.Write("\tТема ({0}) >> ", inst.Topic);
                            string topic = Console.ReadLine().Trim();
                            if (topic != "") inst.Topic = topic;
                            count++;
                            goto case 1;
                        case 1:
                            Console.Write("\tКнига ({0}) >> ", inst.Book.Name);
                            inst.Book = SelectEditBook(inst.Book.Key);
                            Console.WriteLine();
                            count++;
                            goto case 2;
                        case 2:
                            Console.Write("\tНомер сторінки ({0}) >> ", inst.PageNumber);
                            int pageNumber = inst.PageNumber;
                            string page = Console.ReadLine().Trim();
                            if (page != "") inst.PageNumber = int.Parse(page);
                            else inst.PageNumber = pageNumber;
                            count++;
                            goto case 3;
                        case 3:
                            Console.Write("\tПримітка >> ");
                            inst.Note = Console.ReadLine().Trim();
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
        private Book SelectEditBook(string bookKey) {
            string[] items = new string[dataContext.Books.Count];
            items[0] = bookKey;
            int i = 1;
            foreach (Book el in dataContext.Books) {
                if (bookKey == el.Key) continue;
                items[i] = el.Key;
                i++;
            }
            simpleSelector = new SimpleSelector(Console.CursorLeft, Console.CursorTop, 53, items);
            simpleSelector.Focus();
            string text = simpleSelector.Text;
            return dataContext.Books.First(e => e.Key == text);
        }

        //Показ повної інформації про книгу
        private void FullInfoLink() {
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
        private void SortByTopic() {
            sortedCollection = sortedCollection.OrderBy(e => e.Topic);
        }
        private void SortByBook() {
            sortedCollection = sortedCollection.OrderBy(e => e.Book.Name);
        }
        private void SortByPageNumber() {
            sortedCollection = sortedCollection.OrderBy(e => e.PageNumber);
        }
        private void SortById() {
            sortedCollection = sortedCollection.OrderBy(e => e.Id);
        }

        //Фільтрація
        LinksFiltrationController linksFiltrationController = new LinksFiltrationController();
        private void LinksFiltration() {
            linksFiltrationController.SelectFilterCommand();
        }
        IEnumerable<Link> selectingCollection;
        private void ApplyFilters() {
            selectingCollection = linksFiltrationController.ApplyFilters(sortedCollection);
        }

    }
}
