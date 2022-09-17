using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ReferencesInfo.Entities;
using WpfApp1.Core;

namespace WpfApp1.MVVM.ViewModel {
    class BookFiltrSortViewModel {
        //IEnumerable<Book> sortCollection;
        public BookFiltrSortViewModel(RelayCommand exit) {
            //this.sortCollection = sortCollection;
            InitializeFilters();
            IniFilterCommandsInfo();
            Exit = exit;
            ApplyCommand = new RelayCommand(o => {
                ClearFilter();
                if (Check1 == true) SelectFilterCommand(0);
                if (Check2 == true) SelectFilterCommand(1);
                if (Check3 == true) SelectFilterCommand(2);
                if (Check4 == true) SelectFilterCommand(3);
                if (Check5 == true) SelectFilterCommand(4);
                if (CheckIllustration == true) SelectFilterCommand(5);
                if (CheckContent == true) SelectFilterCommand(6);
                if (CheckDescription == true) SelectFilterCommand(7);
            });
        }

        private void ClearFilter() {
            SelectFilterCommand(8);
            SelectFilterCommand(9);
            SelectFilterCommand(10);
            SelectFilterCommand(11);
            SelectFilterCommand(12);
            SelectFilterCommand(13);
            SelectFilterCommand(14);
        }

        public RelayCommand Exit { get; set; }
        public RelayCommand ApplyCommand { get; set; }

        //Для фільтрування
        public bool Check1 { get; set; }
        public bool Check2 { get; set; }
        public bool Check3 { get; set; }
        public bool Check4 { get; set; }
        public bool Check5 { get; set; }
        public bool CheckIllustration { get; set; }
        public bool CheckContent { get; set; }
        public bool CheckDescription { get; set; }

        //Для сортування
        public bool Check6 { get; set; }
        public bool Check7 { get; set; }
        public bool Check8 { get; set; }
        public bool Check9 { get; set; }

        public int ValueId1 { get; set; }
        public int ValueId2 { get; set; }
        public string NameStart { get; set; }
        public string NameContain { get; set; }
        public int ValueYear1 { get; set; }
        public int ValueYear2 { get; set; }
        public int ValuePage1 { get; set; }
        public int ValuePage2 { get; set; }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++

        delegate void Filter(ref IEnumerable<Book> collection);

        Filter emptyFilter = delegate { };

        enum FilterId { ById, ByName, ByYearPublication, ByNumberOfPages, ByIllustration, ByContent, ByDescription };

        Filter[] filters = new Filter[Enum.GetValues(typeof(FilterId)).Length];

        private void InitializeFilters() {
            for (int i = 0; i < filters.Length; i++) {
                filters[i] = emptyFilter;
            }
        }

        class FilterCommandInfo {
            public FilterId filterId;
            public Filter filter;
            public Action enterCommand;

            public FilterCommandInfo(FilterId filterId, Filter filter, Action enterCommand) {
                this.filterId = filterId;
                this.filter = filter;
                this.enterCommand = enterCommand;
            }
        }

        FilterCommandInfo[] filerCommandsInfo;

        private void IniFilterCommandsInfo() {
            filerCommandsInfo = new FilterCommandInfo[] {
                new FilterCommandInfo(FilterId.ById, FiltrId, EnterId),
                new FilterCommandInfo(FilterId.ByName, FiltrNameStarts, EnterNameStarts),
                new FilterCommandInfo(FilterId.ByName, FiltrNameContains, EnterNameContains),
                new FilterCommandInfo(FilterId.ByYearPublication, FiltrYearPublication, EnterYear),
                new FilterCommandInfo(FilterId.ByNumberOfPages, FiltrNumberOfPages, EnterPage),
                new FilterCommandInfo(FilterId.ByIllustration, FiltrIllustration, null),
                new FilterCommandInfo(FilterId.ByContent, FiltrContent, null),
                new FilterCommandInfo(FilterId.ByDescription, FiltrDescription, null),

                new FilterCommandInfo(FilterId.ById, emptyFilter, null),
                new FilterCommandInfo(FilterId.ByName, emptyFilter, null),
                new FilterCommandInfo(FilterId.ByYearPublication, emptyFilter, null),
                new FilterCommandInfo(FilterId.ByNumberOfPages, emptyFilter, null),
                new FilterCommandInfo(FilterId.ByIllustration, emptyFilter, null),
                new FilterCommandInfo(FilterId.ByContent, emptyFilter, null),
                new FilterCommandInfo(FilterId.ByDescription, emptyFilter, null),
            };
        }

        public void SelectFilterCommand(int number) {
            var commandInfo = filerCommandsInfo[number];
                if (commandInfo.filter == emptyFilter) {
                    filters[(int)commandInfo.filterId] = emptyFilter;
                }
                else {
                    filters[(int)commandInfo.filterId] += commandInfo.filter;
                }
            if (commandInfo.enterCommand != null) {
                commandInfo.enterCommand();
            }
        }

        public IEnumerable<Book> ApplyFilters(IEnumerable<Book> initialCollection) {
            var selectedInstances = initialCollection;
            foreach (Filter filter in filters) {
                filter(ref selectedInstances);
            }
            return Sorting(selectedInstances);
        }

        //Фільтрація за Id
        int valueId1, valueId2;
        public void EnterId() {
            valueId1 = ValueId1;
            valueId2 = ValueId2;
        }
        private void FiltrId(ref IEnumerable<Book> collection) {
            collection = from e in collection
                         where e.Id >= valueId1 && e.Id <= valueId2
                         select e;
        }

        //Фільтрація за початком назви
        string nameStart = "";
        private void EnterNameStarts() {
            nameStart = NameStart;
        }
        private void FiltrNameStarts(ref IEnumerable<Book> collection) {
            if (nameStart == null) return;
            collection = from e in collection
                         where e.Name.StartsWith(nameStart)
                         select e;
        }

        //Фільтрація за фрагментом назви
        string nameContain = "";
        private void EnterNameContains() {
            nameContain = NameContain;
        }
        private void FiltrNameContains(ref IEnumerable<Book> collection) {
            if (nameContain == null) return;
            collection = from e in collection
                         where e.Name.Contains(nameContain)
                         select e;
        }

        //Фільтрація за роком видання
        int valueYear1, valueYear2;
        public void EnterYear() {
            valueYear1 = ValueYear1;
            valueYear2 = ValueYear2;
        }
        private void FiltrYearPublication(ref IEnumerable<Book> collection) {
            collection = from e in collection
                         where e.YearPublication >= valueYear1 && e.NumberOfPages <= valueYear2
                         select e;
        }

        //Фільтрація за кількістю сторінок
        int valuePage1, valuePage2;
        public void EnterPage() {
            valuePage1 = ValuePage1;
            valuePage2 = ValuePage2;
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

        //СОРТУВАННЯ

        private IEnumerable<Book> Sorting(IEnumerable<Book> sortedCollection) {
            if (Check6 == true) return SortById(sortedCollection);
            if (Check7 == true) return SortByName(sortedCollection);
            if (Check8 == true) return SortByYear(sortedCollection);
            if (Check9 == true) return SortByNumberOfPages(sortedCollection);
            return sortedCollection;
        }
        private IEnumerable<Book> SortByName(IEnumerable<Book> sortedCollection) {
            return sortedCollection.OrderBy(e => e.Name);
        }
        private IEnumerable<Book> SortByYear(IEnumerable<Book> sortedCollection) {
            return sortedCollection.OrderBy(e => e.YearPublication);
        }
        private IEnumerable<Book> SortByNumberOfPages(IEnumerable<Book> sortedCollection) {
            return sortedCollection.OrderBy(e => e.NumberOfPages);
        }
        private IEnumerable<Book> SortById(IEnumerable<Book> sortedCollection) {
            return sortedCollection.OrderBy(e => e.Id);
        }
    }
}
