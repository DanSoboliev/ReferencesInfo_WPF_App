using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReferencesInfo.Entities;
using WpfApp1.Core;

namespace WpfApp1.MVVM.ViewModel {
    partial class BookViewModel {
        public string Name { get; set; }
        public int YearPublication { get; set; }
        public int NumberOfPages { get; set; }
        public string Illustration { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }

        public RelayCommand AddBookCommand { get; set; }
        public RelayCommand EditBookCommand { get; set; }

        bool CheckBook() {
            string message;
            bool check = true;
            check = Book.CheckName(Name, out message) && check;
            check = Book.CheckYearPublication(YearPublication, out message) && check;
            check = Book.CheckNumberOfPages(NumberOfPages, out message) && check;
            check = Book.CheckContent(Content, out message) && check;
            check = Book.CheckDescription(Description, out message) && check;
            return check;
        }

        public bool IsSelectedTrue { get; set; }
        public bool IsSelectedFalse { get; set; }
        public bool IsSelectedNull { get; set; }

        void CleanProperties() {
            Name = "";
            YearPublication = 0;
            NumberOfPages = 0;
            Illustration = "";
            Content = "";
            Description = "";
        }
    }
}
