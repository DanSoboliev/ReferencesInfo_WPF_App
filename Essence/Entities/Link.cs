using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common.Context.Extensions;
using Common.Context.StringFormatters;
using Common.Entities;
using ReferencesInfo.Interfaces;

namespace ReferencesInfo.Entities {
    [Serializable]
    public partial class Link : Entity, IKeyable, INotifyPropertyChanged {
        private string topic;
        private Book book;
        private int pageNumber;

        private string note;

        public string Key {
            get {
                return string.Format("{0}: {1}, {2}", topic, book.Name, pageNumber);
            }
        }

        //Властивості
        public string Topic {
            get { return topic; }
            set {
                string message;
                if (!CheckTopic(value, out message)) {
                    throw new ArgumentException(message);
                }
                topic = value;
                OnPropertyChanged();
            }
        }
        public Book Book {
            get { return book; }
            set {
                if (value == null) {
                    throw new Exception("Потрібно вказати книгу");
                }
                book = value;
                OnPropertyChanged();
            }
        }
        public int PageNumber {
            get { return pageNumber; }
            set {
                string message;
                if (!CheckPageNumber(value, out message, book)) {
                    throw new ArgumentException(message);
                }
                pageNumber = value;
                OnPropertyChanged();
            }
        }
        public string Note {
            get { return note; }
            set {
                string message;
                if (!CheckNote(value, out message)) {
                    throw new ArgumentException(message);
                }
                note = value;
                OnPropertyChanged();
            }
        }

        //Конструктори
        public Link(string topic, Book book, int pageNumber) {
            this.topic = topic;
            this.book = book;
            this.pageNumber = pageNumber;
        }
        public Link() { }

        //ToString
        public override string ToMembersString() {
            StringFormatter.Current.LineLength = 60;
            return string.Format("{0, -40}: {1, 40} ст. {2}\n\tПримітка:" +
                " \n{3}", topic, book.Name, pageNumber,
                string.IsNullOrWhiteSpace(note) ? "" : note.ToIndentedLineBlock(10));
        }
        public string ToStringMin() {
            return string.Format("{0,5}│{1,-40}│{2,-40}│{3,8} │",
                Id, topic, book.Name, pageNumber);
        }
        public string ToStringMax() {
            return string.Format("\t►{0, 5} {1}: {2} ст. {3}\n\tПримітка:" +
                " {4}", Id, topic, book.Name, pageNumber, note);
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
