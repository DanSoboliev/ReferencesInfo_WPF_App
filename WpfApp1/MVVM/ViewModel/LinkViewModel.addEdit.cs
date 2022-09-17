using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReferencesInfo.Entities;
using WpfApp1.Core;

namespace WpfApp1.MVVM.ViewModel {
    partial class LinkViewModel : ObservableObject {
        private Book selectedBook;
        public Book SelectedBook {
            get { return selectedBook; }
            set {
                selectedBook = value;
                OnPropertyChanged();
            }
        }

        public string Topic { get; set; }
        public int PageNumber { get; set; }
        public string Note { get; set; }

        public RelayCommand AddLinkCommand { get; set; }
        public RelayCommand EditLinkCommand { get; set; }

        bool CheckLink() {
            string message;
            bool check = true;
            check = Link.CheckTopic(Topic, out message) && check;
            check = Link.CheckPageNumber(PageNumber, out message, SelectedBook) && check;
            check = Link.CheckNote(Note, out message) && check;
            return check;
        }

        void CleanProperties() {
            SelectedBook = null;
            Topic = "";
            PageNumber = 0;
            Note = "";
        }
    }
}