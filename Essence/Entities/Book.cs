using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common.Context.Extensions;
using Common.Context.StringFormatters;
using Common.Entities;
using ReferencesInfo.Interfaces;

namespace ReferencesInfo.Entities {
    [Serializable]
    public partial class Book : Entity, IKeyable, INotifyPropertyChanged {
        private string name;
        private int yearPublication;
        private int numberOfPages;

        private bool? illustration;
        private string content;
        private string description;

        public string Key {
            get {
                return string.Format("{0} {1} {2}", name, yearPublication, numberOfPages);
            }
        }

        //Рядкові властивості
        public string Name {
            get { return name; }
            set {
                string message;
                if (!CheckName(value, out message)) {
                    throw new ArgumentException(message);
                }
                name = value;
                OnPropertyChanged();
            }
        }
        public string Content {
            get { return content; }
            set {
                string message;
                if (!CheckContent(value, out message)) {
                    throw new ArgumentException(message);
                }
                content = value;
                OnPropertyChanged();
            }
        }
        public string Description {
            get { return description; }
            set {
                string message;
                if (!CheckDescription(value, out message)) {
                    throw new ArgumentException(message);
                }
                description = value;
                OnPropertyChanged();
            }
        }

        //Властивості значимих типів
        public int YearPublication {
            get { return yearPublication; }
            set {
                string message;
                if (!CheckYearPublication(value, out message)) {
                    throw new ArgumentException(message);
                }
                yearPublication = value;
                OnPropertyChanged();
            }
        }
        public int NumberOfPages {
            get { return numberOfPages; }
            set {
                string message;
                if (!CheckNumberOfPages(value, out message)) {
                    throw new ArgumentException(message);
                }
                numberOfPages = value;
                OnPropertyChanged();
            }
        }
        public bool? Illustration {
            get { return illustration; }
            set { illustration = value; OnPropertyChanged(); }
        }

        //Конструктори
        public Book(string name, int yearPublication,
            int numberOfPages, bool? illustration,
            string content, string description) {
            this.name = name;
            this.yearPublication = yearPublication;
            this.numberOfPages = numberOfPages;
            this.illustration = illustration;
            this.content = content;
            this.description = description;
        }
        public Book() { }
        public Book(string name, int yearPublication, int numberOfPages)
            : this(name, yearPublication, numberOfPages, null, null, null) { }

        //ToString
        public override string ToMembersString() {
            StringFormatter.Current.LineLength = 60;
            return string.Format("{0, -40}, {1, 5}, к-ть сторінок:" +
                    " {2, 5}, ілюстрації: {3}\n\tзміст: \n{4}" +
                    "\n\tопис: \n{5}", name, yearPublication,
                    numberOfPages, illustration,
                    string.IsNullOrWhiteSpace(content) ? "" : content.ToIndentedLineBlock(10),
                    string.IsNullOrWhiteSpace(description) ? "" : description.ToIndentedLineBlock(10));
        }
        public string ToStringMin() {
            return string.Format("{0, 5}│{1, -40}│{2, 14}│{3, 14} │", 
                Id, name, yearPublication, numberOfPages);
        }
        public string ToStringMax() {
            return string.Format("\t►{6, 5} {0}, {1}, к-ть сторінок:" +
                " {2}, ілюстрації: {3}\n\tзміст: {4}" +
                "\n\tопис: {5}", name, yearPublication,
                numberOfPages, illustration == null ? "невідомо" : illustration.ToString(), content, description, Id);
        }

        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]
        string name = null) {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(name));
        }
    }
}
