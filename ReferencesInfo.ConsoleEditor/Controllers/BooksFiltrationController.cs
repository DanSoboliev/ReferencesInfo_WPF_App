using System;
using System.Collections.Generic;
using System.Linq;
using Common.ConsoleIO;
using ReferencesInfo.Entities;

namespace ReferencesInfo.ConsoleEditor.Controllers {
    public class BooksFiltrationController {
        delegate void Filter(ref IEnumerable<Book> collection);

        Filter emptyFilter = delegate { };

        enum FilterId { None, ById, ByName, ByYearPublication, ByNumberOfPages, ByIllustration, ByContent, ByDescription };

        Filter[] filters = new Filter[Enum.GetValues(typeof(FilterId)).Length];

        private void InitializeFilters() {
            for(int i = 0; i < filters.Length; i++) {
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
                new FilerCommandInfo("назва починається з...", FilterId.ByName, FiltrNameStarts, EnterNameStarts),
                new FilerCommandInfo("назва містить...", FilterId.ByName, FiltrNameContains, EnterNameContains),
                new FilerCommandInfo("рік видання в межах від... до...", FilterId.ByYearPublication, FiltrYearPublication, EnterYear),
                new FilerCommandInfo("кількість сторінок в межах від... до...", FilterId.ByNumberOfPages, FiltrNumberOfPages, EnterPage),
                new FilerCommandInfo("наявність ілюстрацій вказана", FilterId.ByIllustration, FiltrIllustration, null),
                new FilerCommandInfo("зміст вказаний", FilterId.ByContent, FiltrContent, null),
                new FilerCommandInfo("опис вказаний", FilterId.ByDescription, FiltrDescription, null),

                new FilerCommandInfo("відмінити фільтрацію за Id", FilterId.ById, emptyFilter, null),
                new FilerCommandInfo("відмінити фільтрацію за назвою", FilterId.ByName, emptyFilter, null),
                new FilerCommandInfo("відмінити фільтрацію за роком видання", FilterId.ByYearPublication, emptyFilter, null),
                new FilerCommandInfo("відмінити фільтрацію за кількістю сторінок", FilterId.ByNumberOfPages, emptyFilter, null),
                new FilerCommandInfo("відмінити фільтрацію за наявністю ілюстрацій", FilterId.ByIllustration, emptyFilter, null),
                new FilerCommandInfo("відмінити фільтрацію за змістом", FilterId.ByContent, emptyFilter, null),
                new FilerCommandInfo("відмінити фільтрацію за приміткою", FilterId.ByDescription, emptyFilter, null),
            };
        }

        public BooksFiltrationController() {
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

        public IEnumerable<Book> ApplyFilters(IEnumerable<Book> initialCollection) {
            var selectedInstances = initialCollection;
            foreach(Filter filter in filters) {
                filter(ref selectedInstances);
            }
            return selectedInstances;
        }


        //Фільтрація за Id
        int valueId1, valueId2;
        public void EnterId() {
            Console.WriteLine("\tId");
            valueId1 = Entering.EnterInt("від", 1, 10000);
            valueId2 = Entering.EnterInt("до", valueId1, 10000);
        }
        private void FiltrId(ref IEnumerable<Book> collection) {
            collection = from e in collection
                         where e.Id >= valueId1 && e.Id <= valueId2
                         select e;
        }

        //Фільтрація за початком назви
        string nameStart = "";
        private void EnterNameStarts() {
            nameStart = Entering.EnterString("Введіть початок назви книги", 40, 1);
        }
        private void FiltrNameStarts(ref IEnumerable<Book> collection) {
            collection = from e in collection
                         where e.Name.StartsWith(nameStart)
                         select e;
        }

        //Фільтрація за фрагментом назви
        string nameContain = "";
        private void EnterNameContains() {
            nameContain = Entering.EnterString("Введіть фрагмент назви книги", 40, 1);
        }
        private void FiltrNameContains(ref IEnumerable<Book> collection) {
            collection = from e in collection
                         where e.Name.Contains(nameContain)
                         select e;
        }

        //Фільтрація за роком видання
        int valueYear1, valueYear2;
        public void EnterYear() {
            Console.WriteLine("\tРоки видання");
            valueYear1 = Entering.EnterInt("від", 1, 10000);
            valueYear2 = Entering.EnterInt("до", valueYear1, 10000);
        }
        private void FiltrYearPublication(ref IEnumerable<Book> collection) {
            collection = from e in collection
                         where e.YearPublication >= valueYear1 && e.NumberOfPages <= valueYear2
                         select e;
        }

        //Фільтрація за кількістю сторінок
        int valuePage1, valuePage2;
        public void EnterPage() {
            Console.WriteLine("\tКількість сторінок");
            valuePage1 = Entering.EnterInt("від", 1, 10000);
            valuePage2 = Entering.EnterInt("до", valuePage1, 10000);
        }
        private void FiltrNumberOfPages(ref IEnumerable<Book> collection) {
            collection = from e in collection
                         where e.NumberOfPages >= valuePage1 && e.NumberOfPages <= valuePage2
                         select e;
        }

        //Фільтрація за вказаною наявністю ілюстрацій
        private void FiltrIllustration(ref IEnumerable<Book> collection) {
            collection = from e in collection
                         where e.Illustration.HasValue == true
                         select e;
        }

        //Фільтрація за наявністю зміста
        private void FiltrContent(ref IEnumerable<Book> collection) {
            collection = from e in collection
                         where !string.IsNullOrEmpty(e.Content)
                         select e;
        }

        //Фільтрація за наявністю опису
        private void FiltrDescription(ref IEnumerable<Book> collection) {
            collection = from e in collection
                         where !string.IsNullOrEmpty(e.Description)
                         select e;
        }
    }
}
