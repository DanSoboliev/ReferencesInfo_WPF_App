using System;
using System.Collections.Generic;
using System.Linq;
using Common.ConsoleIO;
using ReferencesInfo.Entities;

namespace ReferencesInfo.ConsoleEditor.Controllers {
    public class LinksFiltrationController {
        delegate void Filter(ref IEnumerable<Link> collection);

        Filter emptyFilter = delegate { };
        enum FilterId { None, ById, ByTopic, ByBookName, ByPageNumber, ByNote};

        Filter[] filters = new Filter[Enum.GetValues(typeof(FilterId)).Length];
        private void InitializeFilters() {
            for (int i = 0; i < filters.Length; i++) {
                filters[i] = emptyFilter;
            }
        }

        class FilerCommandInfo {
            public string name;
            public FilterId filterId;
            public Filter filter;
            public Action enterCommand;

            public FilerCommandInfo(string name, FilterId filterId, Filter filter, Action enterCommand) {
                this.filterId = filterId;
                this.name = name;
                this.filter = filter;
                this.enterCommand = enterCommand;
            }
        }

        FilerCommandInfo[] filerCommandsInfo;

        private void IniFilterCommandsInfo() {
            filerCommandsInfo = new FilerCommandInfo[] {
                new FilerCommandInfo("відмінити вибір команди", FilterId.None, null, null),
                new FilerCommandInfo("видалити усі фільтри", FilterId.None, null, InitializeFilters),

                new FilerCommandInfo("Id книги в межах від... до...", FilterId.ById, FiltrId, EnterId),
                new FilerCommandInfo("тема починається з...", FilterId.ByTopic, FiltrTopicStarts, EnterTopicStarts),
                new FilerCommandInfo("тема містить...", FilterId.ByTopic, FiltrTopicContains, EnterTopicContains),
                new FilerCommandInfo("назва книги починається з...", FilterId.ByBookName, FiltrNameStarts, EnterNameStarts),
                new FilerCommandInfo("назва книги містить...", FilterId.ByBookName, FiltrNameContains, EnterNameContains),
                new FilerCommandInfo("номер сторінки в межах від... до...", FilterId.ByPageNumber, FiltrPageNumber, EnterPage),
                new FilerCommandInfo("примітка вказана", FilterId.ByNote, FiltrNote, null),

                new FilerCommandInfo("відмінити фільтрацію за Id", FilterId.ById, emptyFilter, null),
                new FilerCommandInfo("відмінити фільтрацію за темою", FilterId.ByTopic, emptyFilter, null),
                new FilerCommandInfo("відмінити фільтрацію за назвою книги", FilterId.ByBookName, emptyFilter, null),
                new FilerCommandInfo("відмінити фільтрацію за номером сторінки", FilterId.ByPageNumber, emptyFilter, null),
                new FilerCommandInfo("відмінити фільтрацію за приміткою", FilterId.ByNote, emptyFilter, null),
            };
        }

        public LinksFiltrationController() {
            InitializeFilters();
            IniFilterCommandsInfo();
        }

        private void ShowFilterCommands() {
            Console.WriteLine("\n  Список команд фільтрації:");
            for (int i = 0; i < filerCommandsInfo.Length; i++) {
                Console.WriteLine("\t{0,2} - {1}", i, filerCommandsInfo[i].name);
            }
        }

        public void SelectFilterCommand() {
            ShowFilterCommands();
            Console.WriteLine();
            int number = Entering.EnterInt("Введіть номер команди фільтрації", 0, filerCommandsInfo.Length - 1);
            var commandInfo = filerCommandsInfo[number];
            if (commandInfo.filterId != FilterId.None) {
                if (commandInfo.filter == emptyFilter) {
                    filters[(int)commandInfo.filterId] = emptyFilter;
                }
                else {
                    filters[(int)commandInfo.filterId] += commandInfo.filter;
                }
            }
            if (commandInfo.enterCommand != null) {
                commandInfo.enterCommand();
            }
        }

        public IEnumerable<Link> ApplyFilters(IEnumerable<Link> initialCollection) {
            var selectedInstances = initialCollection;
            foreach (Filter filter in filters) {
                filter(ref selectedInstances);
            }
            return selectedInstances;
        }

        //Фільтрація за Id
        int valueId1, valueId2;
        public void EnterId() {
            Console.WriteLine("\tId");
            valueId1 = Entering.EnterInt("від", 1);
            valueId2 = Entering.EnterInt("до", valueId1);
        }
        private void FiltrId(ref IEnumerable<Link> collection) {
            collection = from e in collection
                         where e.Id >= valueId1 && e.Id <= valueId2
                         select e;
        }

        //Фільтрація за початком теми
        string topicStart = "";
        private void EnterTopicStarts() {
            topicStart = Entering.EnterString("Введіть початок теми посилання", 40, 1);
        }
        private void FiltrTopicStarts(ref IEnumerable<Link> collection) {
            collection = from e in collection
                         where e.Topic.StartsWith(topicStart)
                         select e;
        }

        //Фільтрація за фрагментом теми
        string topicContain = "";
        private void EnterTopicContains() {
            topicContain = Entering.EnterString("Введіть фрагмент теми", 40, 1);
        }
        private void FiltrTopicContains(ref IEnumerable<Link> collection) {
            collection = from e in collection
                         where e.Topic.Contains(topicContain)
                         select e;
        }

        //Фільтрація за початком назви книги
        string nameStart = "";
        private void EnterNameStarts() {
            nameStart = Entering.EnterString("Введіть початок назви книги", 40, 1);
        }
        private void FiltrNameStarts(ref IEnumerable<Link> collection) {
            collection = from e in collection
                         where e.Book.Name.StartsWith(nameStart)
                         select e;
        }

        //Фільтрація за фрагментом назви книги
        string nameContain = "";
        private void EnterNameContains() {
            nameContain = Entering.EnterString("Введіть фрагмент назви книги", 40, 1);
        }
        private void FiltrNameContains(ref IEnumerable<Link> collection) {
            collection = from e in collection
                         where e.Book.Name.Contains(nameContain)
                         select e;
        }

        //Фільтрація за номером сторінки
        int valuePage1, valuePage2;
        public void EnterPage() {
            Console.WriteLine("\tНомер сторінки");
            valuePage1 = Entering.EnterInt("від", 1);
            valuePage2 = Entering.EnterInt("до", valuePage1);
        }
        private void FiltrPageNumber(ref IEnumerable<Link> collection) {
            collection = from e in collection
                         where e.PageNumber >= valuePage1 && e.PageNumber <= valuePage2
                         select e;
        }

        //Фільтрація за наявністю примітки
        private void FiltrNote(ref IEnumerable<Link> collection) {
            collection = from e in collection
                         where !string.IsNullOrEmpty(e.Note)
                         select e;
        }
    }
}
