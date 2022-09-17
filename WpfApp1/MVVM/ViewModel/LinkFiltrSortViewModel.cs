using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReferencesInfo.Entities;
using WpfApp1.Core;

namespace WpfApp1.MVVM.ViewModel {
    class LinkFiltrSortViewModel {
        public LinkFiltrSortViewModel(RelayCommand exit) {
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
                if (Check6 == true) SelectFilterCommand(5);
                if (CheckNote == true) SelectFilterCommand(6);
            });

        }
        private void ClearFilter() {
            SelectFilterCommand(7);
            SelectFilterCommand(8);
            SelectFilterCommand(9);
            SelectFilterCommand(10);
            SelectFilterCommand(11);
        }

        public RelayCommand Exit { get; set; }
        public RelayCommand ApplyCommand { get; set; }

        //Для фільтрування
        public int ValueId1 { get; set; }
        public int ValueId2 { get; set; }
        public string TopicStart { get; set; }
        public string TopicContain { get; set; }
        public string NameStart { get; set; }
        public string NameContain { get; set; }
        public int ValuePage1 { get; set; }
        public int ValuePage2 { get; set; }

        public bool Check1 { get; set; }
        public bool Check2 { get; set; }
        public bool Check3 { get; set; }
        public bool Check4 { get; set; }
        public bool Check5 { get; set; }
        public bool Check6 { get; set; }
        public bool CheckNote { get; set; }

        //Для сортування
        public bool Check7 { get; set; }
        public bool Check8 { get; set; }
        public bool Check9 { get; set; }
        public bool Check10 { get; set; }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++

        delegate void Filter(ref IEnumerable<Link> collection);

        Filter emptyFilter = delegate { };
        enum FilterId { None, ById, ByTopic, ByBookName, ByPageNumber, ByNote };

        Filter[] filters = new Filter[Enum.GetValues(typeof(FilterId)).Length];
        private void InitializeFilters() {
            for (int i = 0; i < filters.Length; i++) {
                filters[i] = emptyFilter;
            }
        }
        class FilerCommandInfo {
            public FilterId filterId;
            public Filter filter;
            public Action enterCommand;

            public FilerCommandInfo(FilterId filterId, Filter filter, Action enterCommand) {
                this.filterId = filterId;
                this.filter = filter;
                this.enterCommand = enterCommand;
            }
        }

        FilerCommandInfo[] filerCommandsInfo;

        private void IniFilterCommandsInfo() {
            filerCommandsInfo = new FilerCommandInfo[] {
                new FilerCommandInfo(FilterId.ById, FiltrId, EnterId),
                new FilerCommandInfo(FilterId.ByTopic, FiltrTopicStarts, EnterTopicStarts),
                new FilerCommandInfo(FilterId.ByTopic, FiltrTopicContains, EnterTopicContains),
                new FilerCommandInfo(FilterId.ByBookName, FiltrNameStarts, EnterNameStarts),
                new FilerCommandInfo(FilterId.ByBookName, FiltrNameContains, EnterNameContains),
                new FilerCommandInfo(FilterId.ByPageNumber, FiltrPageNumber, EnterPage),
                new FilerCommandInfo(FilterId.ByNote, FiltrNote, null),

                new FilerCommandInfo(FilterId.ById, emptyFilter, null),
                new FilerCommandInfo(FilterId.ByTopic, emptyFilter, null),
                new FilerCommandInfo(FilterId.ByBookName, emptyFilter, null),
                new FilerCommandInfo(FilterId.ByPageNumber, emptyFilter, null),
                new FilerCommandInfo(FilterId.ByNote, emptyFilter, null),
            };
        }

        public void SelectFilterCommand(int number) {
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
            return Sorting(selectedInstances);
        }

        //Фільтрація за Id
        int valueId1, valueId2;
        public void EnterId() {
            valueId1 = ValueId1;
            valueId2 = ValueId2;
        }
        private void FiltrId(ref IEnumerable<Link> collection) {
            collection = from e in collection
                         where e.Id >= valueId1 && e.Id <= valueId2
                         select e;
        }
        //Фільтрація за початком теми
        string topicStart = "";
        private void EnterTopicStarts() {
            topicStart = TopicStart;
        }
        private void FiltrTopicStarts(ref IEnumerable<Link> collection) {
            if (topicStart == null) return;
            collection = from e in collection
                         where e.Topic.StartsWith(topicStart)
                         select e;
        }
        //Фільтрація за фрагментом теми
        string topicContain = "";
        private void EnterTopicContains() {
            topicContain = TopicContain;
        }
        private void FiltrTopicContains(ref IEnumerable<Link> collection) {
            if (topicContain == null) return;
            collection = from e in collection
                         where e.Topic.Contains(topicContain)
                         select e;
        }
        //Фільтрація за початком назви книги
        string nameStart = "";
        private void EnterNameStarts() {
            nameStart = NameStart;
        }
        private void FiltrNameStarts(ref IEnumerable<Link> collection) {
            if (nameStart == null) return;
            collection = from e in collection
                         where e.Book.Name.StartsWith(nameStart)
                         select e;
        }
        //Фільтрація за фрагментом назви книги
        string nameContain = "";
        private void EnterNameContains() {
            nameContain = NameContain;
        }
        private void FiltrNameContains(ref IEnumerable<Link> collection) {
            if (nameContain == null) return;
            collection = from e in collection
                         where e.Book.Name.Contains(nameContain)
                         select e;
        }
        //Фільтрація за номером сторінки
        int valuePage1, valuePage2;
        public void EnterPage() {
            valuePage1 = ValuePage1;
            valuePage2 = ValuePage2;
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

        //СОРТУВАННЯ

        private IEnumerable<Link> Sorting(IEnumerable<Link> sortedCollection) {
            if (Check7 == true) return SortById(sortedCollection);
            if (Check8 == true) return SortByTopic(sortedCollection);
            if (Check9 == true) return SortByPageNumber(sortedCollection);
            if (Check10 == true) return SortByBook(sortedCollection);
            return sortedCollection;
        }
        private IEnumerable<Link> SortByTopic(IEnumerable<Link> sortedCollection) {
            return sortedCollection.OrderBy(e => e.Topic);
        }
        private IEnumerable<Link> SortByBook(IEnumerable<Link> sortedCollection) {
            return sortedCollection.OrderBy(e => e.Book.Name);
        }
        private IEnumerable<Link> SortByPageNumber(IEnumerable<Link> sortedCollection) {
            return sortedCollection.OrderBy(e => e.PageNumber);
        }
        private IEnumerable<Link> SortById(IEnumerable<Link> sortedCollection) {
            return sortedCollection.OrderBy(e => e.Id);
        }
    }
}
