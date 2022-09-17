using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ReferencesInfo.Data;
using ReferencesInfo.Entities;
using WpfApp1.Core;
using WpfApp1.MVVM.View;
using Common.ConsoleIO;

namespace WpfApp1.MVVM.ViewModel {
    partial class BookViewModel : ObservableObject {
        DataContext dataContext;
        //ObservableCollection<Book> collection;
        public ICollection<Book> collection { get; set; }
        private IEnumerable<Book> SortedAndFiltredCollection;
        public IEnumerable<Book> sortedAndFiltredCollection { 
            get { return SortedAndFiltredCollection; }
            set { 
                SortedAndFiltredCollection = value;
                OnPropertyChanged();
            }
        }

        BookFiltrSortViewModel bookFiltrSortViewModel;

        private object _currentView;
        public object CurrentView {
            get { return _currentView; }
            set {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private Book selectedBook;
        public Book SelectedBook {
            get { return selectedBook; }
            set {
                selectedBook = value;
                OnPropertyChanged();
            }
        }
        
        public BookViewModel(DataContext dataContext) {
            this.dataContext = dataContext;
            //this.dataContext.CreateTestingData();
            collection = dataContext.Books;
            //sortedAndFiltredCollection = collection;
            //collection.CollectionChanged += Collection_CollectionChanged;
            UserControl HomeV = new BookHomeView();
            UserControl FiltrSortV = new BookFiltrSortView();

            CurrentView = HomeV;

            RemoveBookCommand = new RelayCommand(o => {
                Book inst = o as Book;
                int n = dataContext.Links.Count(e => e.Book == inst);
                for (int i = 0; i < n; i++) {
                    dataContext.Links.Remove(dataContext.Links.First(e => e.Book == inst));
                }
                collection.Remove(inst);
                sortedAndFiltredCollection = bookFiltrSortViewModel.ApplyFilters(collection);
            });
            AddViewCommand = new RelayCommand(o => {
                UserControl AddV = new BookAddView(); 
                CurrentView = AddV;
                AddV.DataContext = this;
            });
            HomeViewCommand = new RelayCommand(o => {
                sortedAndFiltredCollection = bookFiltrSortViewModel.ApplyFilters(collection);
                CurrentView = HomeV;

                CleanProperties();
            });

            bookFiltrSortViewModel = new BookFiltrSortViewModel(HomeViewCommand);
            sortedAndFiltredCollection = bookFiltrSortViewModel.ApplyFilters(collection);

            EditViewCommand = new RelayCommand(o => {
                if (o == null) return;
                UserControl EditV = new BookEditView();
                CurrentView = EditV;
                EditV.DataContext = this;

                Book inst = o as Book;
                Name = inst.Name;
                YearPublication = inst.YearPublication;
                NumberOfPages = inst.NumberOfPages;
                if(inst.Illustration == true) {
                    IsSelectedTrue = true;
                    IsSelectedFalse = false;
                    IsSelectedNull = false;
                }
                else if (inst.Illustration == false) {
                    IsSelectedTrue = false;
                    IsSelectedFalse = true;
                    IsSelectedNull = false;
                }
                else {
                    IsSelectedTrue = false;
                    IsSelectedFalse = false;
                    IsSelectedNull = true;
                }
                Content = inst.Content;
                Description = inst.Description;
            });
            AddBookCommand = new RelayCommand(o => {
                if (!CheckBook()) {
                    return;
                }
                Book inst = new Book(Name, YearPublication, NumberOfPages, Entering.EnterNullBool(Illustration), Content, Description);
                try {
                    inst.Id = collection.Select(e => e.Id).Max() + 1;
                }
                catch (Exception ex) { inst.Id = 1; }
                collection.Add(inst);

                sortedAndFiltredCollection = bookFiltrSortViewModel.ApplyFilters(collection);

                CurrentView = HomeV;

                CleanProperties();
            });
            EditBookCommand = new RelayCommand(o => {
                if (!CheckBook()) {
                    return;
                }

                Book inst = o as Book;

                inst.Name = Name;
                inst.YearPublication = YearPublication;
                inst.NumberOfPages = NumberOfPages;
                inst.Illustration = Entering.EnterNullBool(Illustration);
                inst.Content = Content;
                inst.Description = Description;

                int n = dataContext.Links.Count(e => e.Book == inst);
                for (int i = 0; i < n; i++) {
                    try {
                        dataContext.Links.Remove(dataContext.Links.First(e => ((e.Book == inst) && (e.Book.NumberOfPages < e.PageNumber))));
                    }
                    catch(Exception ex) {
                        break;
                    }
                }

                sortedAndFiltredCollection = bookFiltrSortViewModel.ApplyFilters(collection);

                CurrentView = HomeV;

                CleanProperties();
            });
            FiltrSortViewCommand = new RelayCommand(o => {
                CurrentView = FiltrSortV;
                FiltrSortV.DataContext = bookFiltrSortViewModel;
            });

            //bookFiltrSortViewModel.ApplyFilters(collection);
        }

        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            //throw new NotImplementedException();
            sortedAndFiltredCollection = bookFiltrSortViewModel.ApplyFilters(collection);
        }

        public RelayCommand RemoveBookCommand { get; set; }
        public RelayCommand AddViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand EditViewCommand { get; set; }
        public RelayCommand FiltrSortViewCommand { get; set; }
    }
}