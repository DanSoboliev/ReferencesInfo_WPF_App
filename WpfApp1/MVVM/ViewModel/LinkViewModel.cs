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

namespace WpfApp1.MVVM.ViewModel {
    partial class LinkViewModel : ObservableObject {
        DataContext dataContext;
        public ICollection<Link> collectionLink { get; set; }
        public ICollection<Book> collectionBook { get; set; }
        private IEnumerable<Link> SortedAndFiltredCollection;
        public IEnumerable<Link> sortedAndFiltredCollection {
            get { return SortedAndFiltredCollection; }
            set {
                SortedAndFiltredCollection = value;
                OnPropertyChanged();
            }
        }

        LinkFiltrSortViewModel linkFiltrSortViewModel;

        private object _currentView;
        public object CurrentView {
            get { return _currentView; }
            set {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private Link selectedLink;
        public Link SelectedLink {
            get { return selectedLink; }
            set {
                selectedLink = value;
                OnPropertyChanged();
            }
        }

        public LinkViewModel(DataContext dataContext) {
            this.dataContext = dataContext;
            collectionLink = dataContext.Links;
            collectionBook = dataContext.Books;
            dataContext.Books.CollectionChanged += Books_CollectionChanged;

            UserControl HomeV = new LinkHomeView();
            UserControl FiltrSortV = new LinkFiltrSortView();

            CurrentView = HomeV;

            RemoveLinkCommand = new RelayCommand(o => {
                Link inst = o as Link;
                collectionLink.Remove(inst);
                sortedAndFiltredCollection = linkFiltrSortViewModel.ApplyFilters(collectionLink);
            });
            AddViewCommand = new RelayCommand(o => {
                UserControl AddV = new LinkAddView();
                CurrentView = AddV;
                AddV.DataContext = this;
            });
            HomeViewCommand = new RelayCommand(o => {
                sortedAndFiltredCollection = linkFiltrSortViewModel.ApplyFilters(collectionLink);
                CurrentView = HomeV;

                CleanProperties();
            });

            linkFiltrSortViewModel = new LinkFiltrSortViewModel(HomeViewCommand);
            sortedAndFiltredCollection = linkFiltrSortViewModel.ApplyFilters(collectionLink);

            AddLinkCommand = new RelayCommand(o => {
                if (SelectedBook == null) return;
                if (!CheckLink()) return;
                Link inst = new Link(Topic, SelectedBook, PageNumber) {
                    Note = Note
                };
                try {
                    inst.Id = collectionLink.Select(e => e.Id).Max() + 1;
                }
                catch (Exception ex) { inst.Id = 1; }
                collectionLink.Add(inst);

                sortedAndFiltredCollection = linkFiltrSortViewModel.ApplyFilters(collectionLink);

                CurrentView = HomeV;
                CleanProperties();
            });
            EditViewCommand = new RelayCommand(o => {
                if (o == null) return;
                UserControl EditV = new LinkEditView();
                CurrentView = EditV;
                EditV.DataContext = this;

                Link inst = o as Link;
                Topic = inst.Topic;
                PageNumber = inst.PageNumber;
                Note = inst.Note;
                SelectedBook = inst.Book;
            });
            EditLinkCommand = new RelayCommand(o => {
                if (!CheckLink()) {
                    return;
                }

                Link inst = o as Link;

                inst.Topic = Topic;
                inst.PageNumber = PageNumber;
                inst.Note = Note;
                inst.Book = SelectedBook;

                sortedAndFiltredCollection = linkFiltrSortViewModel.ApplyFilters(collectionLink);

                CurrentView = HomeV;

                CleanProperties();
            });
            FiltrSortViewCommand = new RelayCommand(o => {
                CurrentView = FiltrSortV;
                FiltrSortV.DataContext = linkFiltrSortViewModel;
            });
        }

        private void Books_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            //hrow new NotImplementedException();
            sortedAndFiltredCollection = linkFiltrSortViewModel.ApplyFilters(collectionLink);
        }

        public RelayCommand RemoveLinkCommand { get; set; }
        public RelayCommand AddViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand EditViewCommand { get; set; }
        public RelayCommand FiltrSortViewCommand { get; set; }
    }
}
